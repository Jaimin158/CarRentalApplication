using Maintenance.WebAPI.Services;
using CarRental.Shared.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// register fake service
builder.Services.AddScoped<IJSRepairHistoryService, JSFakeRepairHistoryService>();

// stateful in-memory usage count
var usageCounts = new Dictionary<string, int>();
builder.Services.AddSingleton(usageCounts);

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Maintenance.WebAPI",
        Version = "v1"
    });
});

var app = builder.Build();

// Swagger enabled for local + Azure
app.UseSwagger();
app.UseSwaggerUI();

// global exception handling
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

// only allow calls forwarded from API Gateway
app.UseMiddleware<GatewayOnlyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();