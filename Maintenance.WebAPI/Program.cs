using JSMainteanance.WebAPI.Middleware;
using Maintenance.WebAPI.Services;
using MLMaintenance.WebAPI.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// register fake service
builder.Services.AddScoped<IJSRepairHistoryService, JSFakeRepairHistoryService>();

// stateful in-memory usage count
var usageCounts = new Dictionary<string, int>();
builder.Services.AddSingleton(usageCounts);

// Swagger + API key support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Maintenance.WebAPI",
        Version = "v1"
    });

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints.",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-Api-Key",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Swagger enabled for local + Azure
app.UseSwagger();
app.UseSwaggerUI();

// global exception handling
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

// API key middleware should run in all environments
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
