using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VehicleInventory.Infrastructure.Persistence;


public class InventoryDbContextFactory : IDesignTimeDbContextFactory<InventoryDbContext>
{
    public InventoryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();

        
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=VehicleInventoryDb;Trusted_Connection=True;TrustServerCertificate=True"
        );

        return new InventoryDbContext(optionsBuilder.Options);
    }
}
