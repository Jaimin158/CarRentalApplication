using CarRentalApiGateway.Middleware;
using CarRental.Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

// API key authentication happens here
app.UseMiddleware<ApiKeyMiddleware>();

app.MapReverseProxy();

app.Run();