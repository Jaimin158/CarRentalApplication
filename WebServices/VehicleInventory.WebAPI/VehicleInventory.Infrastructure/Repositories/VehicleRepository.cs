using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleInventory.Application.Interfaces;
using VehicleInventory.Domain.Entities;
using VehicleInventory.Infrastructure.Persistence;


namespace VehicleInventory.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly InventoryDbContext _db;

        public VehicleRepository(InventoryDbContext db)
        {
            _db = db;
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            await _db.Vehicles.AddAsync(vehicle, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            _db.Vehicles.Update(vehicle);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            _db.Vehicles.Remove(vehicle);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsByVehicleCodeAsync(string vehicleCode, CancellationToken cancellationToken)
        {
            return await _db.Vehicles
                .AnyAsync(v => v.VehicleCode.Value == vehicleCode, cancellationToken);
        }

        public async Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _db.Vehicles
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}