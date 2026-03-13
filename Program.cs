using CarRentalApplication.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CustmerProfileContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomersProfile")));

builder.Services.AddHttpClient("MaintenanceApi", (sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(config["MaintenanceApi:BaseUrl"]!);
    client.DefaultRequestHeaders.Add("X-Api-Key", "5214eee0-0f8c-407b-9bc4-3b33db2030a2");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
