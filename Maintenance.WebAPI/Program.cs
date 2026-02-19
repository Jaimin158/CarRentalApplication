using JSMainteanance.WebAPI.Middleware;
using Maintenance.WebAPI.Services;
using MLMaintenance.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register fake service
builder.Services.AddScoped<IJSRepairHistoryService, JSFakeRepairHistoryService>();

var usageCounts = new Dictionary<string, int>();
builder.Services.AddSingleton(usageCounts);

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. Example: MY_SECRET_KEY_123",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Name = "X-Api-Key",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] { }
        }
    });
});
var app = builder.Build();


// Swagger enabled for Azure too (rubric requirement)
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();
if (!app.Environment.IsDevelopment())
{
    app.UseMiddleware<ApiKeyMiddleware>();
}

app.UseMiddleware<GlobalExceptionMiddleware>();


app.MapControllers();

app.Run();
