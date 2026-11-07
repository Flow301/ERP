using ERP.Application.Implementation;
using ERP.Application.Interfaces;
using ERP.Application.Mapper;
using ERP.Domain.DTO.PatternResult;
using ERP.Infraestructure.Data;
using ERP.Infraestructure.Data.Seed;
using ERP.Infraestructure.Implementation;
using ERP.Infraestructure.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ERP.Domain.DTO.PatternResult;
using Microsoft.AspNetCore.Mvc;
using ERP.WebAPI.Handlers;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

// NUEVO: Kestrel no anuncia su nombre en el header Server
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

// Serilog reemplaza al proveedor de logging por defecto (clase 6-1)
builder.Services.AddSerilog((services, logConfig) => logConfig
 .MinimumLevel.Information() // nivel general
 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // menos ruido de framework
 .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information) //"Now listening on..."
 .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command",
 builder.Environment.IsDevelopment() ? LogEventLevel.Information :
LogEventLevel.Warning) // SQL de EF (5-1)
 .Enrich.FromLogContext()
 .WriteTo.Console()
 .WriteTo.File(
 path: "logs/serilog-.txt",
 rollingInterval: RollingInterval.Day, // un archivo por día: serilog-20261003.txt
 retainedFileCountLimit: 14, // conserva 14 archivos
 fileSizeLimitBytes: 10_485_760, // 10 MB por archivo
 rollOnFileSizeLimit: true, // al llegar a 10 MB abre serilog-20261003_001.txt
 shared: true, // varios procesos pueden escribir el mismo archivo
 outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] ({SourceContext}){ Message:lj}{NewLine}{Exception}"));


// Add services to the container.

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
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
    options.AddPolicy("Frontend", policy => policy
        .WithOrigins(allowedOrigins)                      // orígenes desde la configuración, no desde el código
        .WithMethods("GET", "POST", "PUT", "DELETE")      // solo los verbos que usa la API
        .WithHeaders("Content-Type")                      // en 9-1 se agregará "Authorization"
        .WithExposedHeaders("Location")                   // React podrá leer el Location de un 201
        .SetPreflightMaxAge(TimeSpan.FromMinutes(10))));  // el navegador recuerda el permiso 10 min

// Manejo global de errores (clase 6-1)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // obligatorio para usar UseExceptionHandler() sin parámetros

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
 ?? throw new InvalidOperationException("Falta la configuración Seed:AdminPassword (User Secrets).");
var isDevelopment = builder.Environment.IsDevelopment();
builder.Services.AddDbContext<AppDbContext>(options => options
 .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
 // Las herramientas de EF (dotnet ef database update) usan la versión síncrona
 .UseSeeding((context, _) =>
 {
     MasterSeeder.Seed(context, adminPassword);
     if (isDevelopment) DevelopmentSeeder.Seed(context);
 })
 // La aplicación (Database.MigrateAsync) usa la versión asíncrona
 .UseAsyncSeeding(async (context, _, ct) =>
 {
     await MasterSeeder.SeedAsync(context, adminPassword, ct);
     if (isDevelopment) await DevelopmentSeeder.SeedAsync(context, ct);
 }));

// Mapster: Pattern Mapper
// https://code-maze.com/mapster-aspnetcore-introduction/
// 1. Trigger custom configurations 
MapsterConfig.RegisterMaps();
// 2. Grab global settings or create a new config instance
var config = TypeAdapterConfig.GlobalSettings;
// 3. Register Mapster into the Service Collection. It can use it with D.I.
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddMapster();

// Inyección de dependencias: repositorios (Infraestructure) y servicios (Application)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();


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

app.UseSerilogRequestLogging(); // 1. Una línea de log por petición: método, ruta, código y duración
app.UseExceptionHandler(); // 2. Todo lo que viene después queda protegido por el manejador global
app.UseCors("Frontend");    // NUEVO: aplica la política "Frontend"

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => options
 .WithTitle("ERP API")
 .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch)); // ejemplos de consumo en JavaScript
}

app.UseAuthorization();

app.MapControllers();

app.Run();
