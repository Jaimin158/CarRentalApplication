using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VehicleInventory.Application.DTOs;
using VehicleInventory.Application.Interfaces;
using VehicleInventory.Domain.Entities;
using VehicleInventory.Domain.Enums;
using VehicleInventory.Domain.Exceptions;
using VehicleInventory.Domain.ValueObjects;

namespace VehicleInventory.Application.Services
{
    //
    public class VehicleService
    {
        private readonly IVehicleRepository _repo;

        public VehicleService(IVehicleRepository repo)
        {
            _repo = repo;
        }

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken ct = default)
        {
            var code = request.VehicleCode?.Trim() ?? "";

            if (await _repo.ExistsByVehicleCodeAsync(code, ct))
                throw new DomainException("VehicleCode must be unique.");

            var vehicle = new Vehicle(
                id: Guid.NewGuid(),
                vehicleCode: new VehicleCode(code),
                locationId: request.LocationId,
                vehicleType: new VehicleType(request.VehicleType?.Trim() ?? "")
            );

            await _repo.AddAsync(vehicle, ct);

            return ToDto(vehicle);
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id, CancellationToken ct = default)
        {
            var vehicle = await _repo.GetByIdAsync(id, ct);

            if (vehicle == null)
                return null;

            return ToDto(vehicle);
        }

        public async Task<IReadOnlyList<VehicleDto>> GetAllVehiclesAsync(CancellationToken ct = default)
        {
            var vehicles = await _repo.GetAllAsync(ct);

            return vehicles.Select(v => ToDto(v)).ToList();
        }

        public async Task<VehicleDto?> UpdateVehicleStatusAsync(Guid id, UpdateVehicleStatusRequest request, CancellationToken ct = default)
        {
            var vehicle = await _repo.GetByIdAsync(id, ct);

            if (vehicle == null)
                return null;

            switch (request.NewStatus)
            {
                case VehicleStatus.Available:
                    if (request.ReleaseReservation)
                        vehicle.ReleaseReservation();
                    else
                        vehicle.MarkAvailable();
                    break;

                case VehicleStatus.Rented:
                    vehicle.MarkRented();
                    break;

                case VehicleStatus.Reserved:
                    vehicle.MarkReserved();
                    break;

                case VehicleStatus.Serviced:
                    vehicle.MarkServiced();
                    break;

                default:
                    throw new DomainException("Invalid vehicle status transition request.");
            }

            await _repo.UpdateAsync(vehicle, ct);
            return ToDto(vehicle);
        }

        public async Task<bool> DeleteVehicleAsync(Guid id, CancellationToken ct = default)
        {
            var vehicle = await _repo.GetByIdAsync(id, ct);

            if (vehicle == null)
                return false;

            await _repo.DeleteAsync(vehicle, ct);
            return true;
        }

        private static VehicleDto ToDto(Vehicle v)
        {
            return new VehicleDto(v.Id, v.VehicleCode.Value, v.LocationId, v.VehicleType.Value, v.Status);
        }
    }
}