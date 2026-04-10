using CarRental.Shared.Middleware;
using CustomerProfile.webapi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CustmerProfileContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomersProfile")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

// only accept requests from gateway
app.UseMiddleware<GatewayOnlyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();