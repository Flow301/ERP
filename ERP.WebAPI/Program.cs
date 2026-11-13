using ERP.Application.Implementation;
using ERP.Application.Interfaces;
using ERP.Application.Mapper;
using ERP.Domain.DTO.PatternResult;
using ERP.Infraestructure.Data;
using ERP.Infraestructure.Data.Seed;
using ERP.Infraestructure.Implementation;
using ERP.Infraestructure.Interfaces;
using ERP.WebAPI;   // AuthorizationPolicies (la clase está en la raíz de ERP.WebAPI)
using ERP.WebAPI.Handlers;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using System.Text;




var builder = WebApplication.CreateBuilder(args);
// NUEVO: Kestrel no anuncia su nombre en el header Server
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader =
false);


// Serilog reemplaza al proveedor de logging por defecto (clase 6-1)
builder.Services.AddSerilog((services, logConfig) => logConfig
    .MinimumLevel.Information()                                              // nivel general
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)               // menos ruido del framework
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)   // "Now listening on..."
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command",
        builder.Environment.IsDevelopment() ? LogEventLevel.Information : LogEventLevel.Warning) // SQL de EF (5-1)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/serilog-.txt",
        rollingInterval: RollingInterval.Day,       // un archivo por día: serilog-20261003.txt
        retainedFileCountLimit: 14,                 // conserva 14 archivos
        fileSizeLimitBytes: 10_485_760,             // 10 MB por archivo
        rollOnFileSizeLimit: true,                  // al llegar a 10 MB abre serilog-20261003_001.txt
        shared: true,                               // varios procesos pueden escribir el mismo archivo
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] ({SourceContext}) {Message:lj}{NewLine}{Exception}"));


// =====================================================
// Add services to the container
// =====================================================

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // El 400 automático de [ApiController] usa la misma forma Result<T> que el resto de la API
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(entry => entry.Value!.Errors.Count > 0)
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

            var result = Result<Dictionary<string, string[]>>.Failure(400, "Bad Request",
                "Uno o más datos no son válidos.");
            result.Data = errors;

            return new BadRequestObjectResult(result);
        };
    });

// CORS: solo el FrontEnd autorizado, leído desde appsettings.json
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
?? [];
builder.Services.AddCors(options =>
 options.AddPolicy("Frontend", policy => policy
 .WithOrigins(allowedOrigins) // orígenes desde la configuración, no desde el código
 .WithMethods("GET", "POST", "PUT", "DELETE") // solo los verbos que usa la API
 .WithHeaders("Content-Type", "Authorization") //React enviará el token en este header
 .WithExposedHeaders("Location") // React podrá leer el Location de un 201
 .SetPreflightMaxAge(TimeSpan.FromMinutes(10)))); // el navegador recuerda el permiso 10 min

// Configuración del JWT: la llave es un secreto (user-secrets), el resto va en appsettings.json
var jwtKey = builder.Configuration["Jwt:Key"]
 ?? throw new InvalidOperationException("Falta el secreto Jwt:Key. Configúrelo con dotnet user-secrets.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;
var jwtMinutes = builder.Configuration.GetValue<int>("Jwt:ExpirationMinutes");

// Servicios del login
builder.Services.AddSingleton<IJwtTokenService>(new JwtTokenService(jwtKey, jwtIssuer,
jwtAudience, jwtMinutes));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
// Authentication: cómo se valida el token que llega en cada petición
builder.Services
 .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
 options.MapInboundClaims = false; // los claims se leen con su nombre corto: sub, name, role
 options.TokenValidationParameters = new TokenValidationParameters
 {
     ValidateIssuer = true,
     ValidIssuer = jwtIssuer,
     ValidateAudience = true,
     ValidAudience = jwtAudience,
     ValidateIssuerSigningKey = true,
     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
     ValidateLifetime = true,
     ClockSkew = TimeSpan.Zero, // vence exactamente en exp (por defecto tolera 5 minutos)
     NameClaimType = "name",
     RoleClaimType = "role" // la siguiente clase lo usa en [Authorize(Roles = "...")]
    };

    // 401 y 403 con el mismo formato Result<T> que el resto de la API
    options.Events = new JwtBearerEvents
    {
        // 401: no hay token, o es inválido o está vencido
        OnChallenge = async context =>
        {
            context.HandleResponse();                                  // reemplaza la respuesta vacía por defecto
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.Headers.WWWAuthenticate = "Bearer";      // indica qué tipo de credencial se espera
            var result = Result<object>.Failure(401, "Unauthorized",
                "Debe iniciar sesión: el token no se envió, es inválido o está vencido.");
            await context.Response.WriteAsJsonAsync(result);
        },

        // 403: el token es válido, pero el rol no tiene permiso
        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            var result = Result<object>.Failure(403, "Forbidden",
                "Su rol no tiene permiso para realizar esta operación.");
            await context.Response.WriteAsJsonAsync(result);
        }
    };
 });

// Autorización: las reglas de la matriz de permisos
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.AdminOnly,
        policy => policy.RequireRole("Administrador"));

    options.AddPolicy(AuthorizationPolicies.ProductManagers,
        policy => policy.RequireRole("Administrador", "Vendedor"));

    options.AddPolicy(AuthorizationPolicies.SellerOnly,
        policy => policy.RequireRole("Vendedor"));

    // Protección por defecto: un endpoint sin [Authorize] ni [AllowAnonymous] igual exige token
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// RETO 9-1 -- Máximo 5 intentos de login por minuto
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        var result = Result<object>.Failure(429, "Too Many Requests", "Demasiados intentos. Espere un minuto.");
        await context.HttpContext.Response.WriteAsJsonAsync(result, token);
    };
    options.AddFixedWindowLimiter("login", limiter =>
    {
        limiter.PermitLimit = 5;
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.QueueLimit = 0;
    });
});

// Manejo global de errores (clase 6-1)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();   // obligatorio para usar UseExceptionHandler() sin parámetros


// Documento OpenAPI con título, versión y descripción (clase 6-2)
builder.Services.AddOpenApi(options =>
{
options.AddDocumentTransformer((document, context, cancellationToken) =>
{
document.Info = new()
{
    Title = "ERP API",
    Version = "v1",
    Description = "API del sistema ERP (ISW-621, UTN). Todas las respuestas usan Result<T>: isSuccess, status, title, detail y data."
    };
    return Task.CompletedTask;
    });
});


var adminPassword = builder.Configuration["Seed:AdminPassword"]
    ?? throw new InvalidOperationException(
        "Falta la configuración Seed:AdminPassword (User Secrets).");

var isDevelopment = builder.Environment.IsDevelopment();

builder.Services.AddDbContext<AppDbContext>(options => options
    .UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))


    // EF Tools
    .UseSeeding((context, _) =>
    {
        MasterSeeder.Seed(context, adminPassword);

        if (isDevelopment)
            DevelopmentSeeder.Seed(context);
    })

    // Aplicación
    .UseAsyncSeeding(async (context, _, ct) =>
    {
        await MasterSeeder.SeedAsync(
            context,
            adminPassword,
            ct);

        if (isDevelopment)
            await DevelopmentSeeder.SeedAsync(
                context,
                ct);
    }));


// =====================================================
// Mapster
// =====================================================

// 1. Registrar configuraciones personalizadas
MapsterConfig.RegisterMaps();

// 2. Configuración global
var config = TypeAdapterConfig.GlobalSettings;

// 3. Registrar Mapster en DI
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddMapster();


// =====================================================
// Repositories
// =====================================================

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Reto Invoice
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

//Reto Warehouse
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();


// =====================================================
// Services
// =====================================================

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();


// Reto Invoice
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

//Reto Warehouse
builder.Services.AddScoped<IWarehouseService, WarehouseService>();


// =====================================================
// Build
// =====================================================

var app = builder.Build();

// NUEVO: Headers de seguridad en TODAS las respuestas (también en las de error)
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        var headers = context.Response.Headers;
        headers["X-Content-Type-Options"] = "nosniff";
        headers["X-Frame-Options"] = "DENY";
        headers["Referrer-Policy"] = "no-referrer";
        headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
        // La API solo entrega JSON e imágenes: no necesita cargar scripts, estilos ni marcos.
        // Scalar sí es una página con scripts, por eso queda fuera de esta política.
        if (!context.Request.Path.StartsWithSegments("/scalar"))
            headers["Content-Security-Policy"] = "default-src 'none'; frame-ancestors 'none'";
        return Task.CompletedTask;
    });
    await next();
});

app.UseSerilogRequestLogging();   // 2. Una línea de log por petición
app.UseExceptionHandler();        // 3. Excepciones con GlobalExceptionHandler
app.UseStaticFiles();             // 4. Imágenes de productos (wwwroot)
app.UseCors("Frontend");          // 5. NUEVO: aplica la política "Frontend"
app.UseRateLimiter();             // 6. RETO 9-1: aplica el límite "login"

// =====================================================
// Configure HTTP request pipeline
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();                      // sin token
    app.MapScalarApiReference(options => options
        .WithTitle("ERP API")
        .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch))
        .AllowAnonymous();                                  // sin token
}

app.UseHttpsRedirection();
app.UseAuthentication(); // NUEVO: lee el header Authorization, valida el token y llena HttpContext.User
app.UseAuthorization();  // ya existía; en la siguiente clase decide quién puede entrar a cada endpoint
app.MapControllers();

app.Run();