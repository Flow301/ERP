using ERP.Application.Mapper;
using ERP.Infraestructure.Data;
using ERP.Infraestructure.Data.Seed;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
