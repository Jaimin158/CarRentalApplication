using Maintenance.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register fake service
builder.Services.AddScoped<IJSRepairHistoryService, JSFakeRepairHistoryService>();

var app = builder.Build();

// Swagger enabled for Azure too (rubric requirement)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();



app.Run();
