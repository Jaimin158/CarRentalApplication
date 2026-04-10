using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleInventory.Domain.Entities;

namespace VehicleInventory.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task AddAsync(Vehicle vehicle, CancellationToken ct = default);
        Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken ct = default);
        Task UpdateAsync(Vehicle vehicle, CancellationToken ct = default);
        Task DeleteAsync(Vehicle vehicle, CancellationToken ct = default);

        Task<bool> ExistsByVehicleCodeAsync(string vehicleCode, CancellationToken ct = default);
    }
}
