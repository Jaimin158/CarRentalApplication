using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleInventory.Domain.Entities;
using VehicleInventory.Domain.ValueObjects;

namespace VehicleInventory.Infrastructure.Persistence
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        // Vehicles and location table
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        public DbSet<Location> Locations => Set<Location>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Locations");

                entity.HasKey(l => l.Id);

                entity.Property(l => l.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasData(
                    new Location(new Guid("11111111-1111-1111-1111-111111111111"), "Toronto Branch"),
                    new Location(new Guid("22222222-2222-2222-2222-222222222222"), "Mississauga Branch")
                );
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("Vehicles");

                entity.HasKey(v => v.Id);

                entity.OwnsOne(v => v.VehicleCode, vehicleCode =>
                {
                    vehicleCode.Property(vc => vc.Value)
                               .HasColumnName("VehicleCode")
                               .IsRequired()
                               .HasMaxLength(30);
                });

                entity.OwnsOne(v => v.VehicleType, vehicleType =>
                {
                    vehicleType.Property(vt => vt.Value)
                               .HasColumnName("VehicleType")
                               .IsRequired()
                               .HasMaxLength(50);
                });

                entity.Property(v => v.LocationId)
                      .IsRequired();

                entity.Property(v => v.Status)
                      .IsRequired();

         
            });
        }
    }
}