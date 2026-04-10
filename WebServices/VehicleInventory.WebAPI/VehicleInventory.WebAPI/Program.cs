using Microsoft.EntityFrameworkCore;
using VehicleInventory.Application.Interfaces;
using VehicleInventory.Application.Services;
using VehicleInventory.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Controllers + swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<VehicleService>();
// DbContext
builder.Services.AddDbContext<VehicleInventory.Infrastructure.Persistence.InventoryDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryDb"));
});

// DI
builder.Services.AddScoped<
    VehicleInventory.Application.Interfaces.IVehicleRepository,
    VehicleInventory.Infrastructure.Repositories.VehicleRepository>();

builder.Services.AddScoped<VehicleInventory.Application.Services.VehicleService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<VehicleInventory.WebAPI.Services.ExceptionHandlingMiddleware>();

app.MapControllers();
app.Run();
