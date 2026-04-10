using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleInventory.Domain.Enums;
using VehicleInventory.Domain.Exceptions;
using VehicleInventory.Domain.ValueObjects;


namespace VehicleInventory.Domain.Entities
{
    public class Vehicle
    {

        private Vehicle() { }

        public Vehicle(Guid id, VehicleCode vehicleCode, Guid locationId, VehicleType vehicleType)
        {
            //Basic validation
            if (id == Guid.Empty)
                throw new DomainException("Id cannot be empty.");

            if (vehicleCode is null)
                throw new DomainException("VehicleCode is required.");

            if (locationId == Guid.Empty)
                throw new DomainException("LocationId cannot be empty.");

            if (vehicleType is null)
                throw new DomainException("VehicleType is required.");

            Id = id;
            VehicleCode = vehicleCode;
            LocationId = locationId;
            VehicleType = vehicleType;

            // New vehicles are available
            Status = VehicleStatus.Available;
        }

        public Guid Id { get; private set; }
        public VehicleCode VehicleCode { get; private set; } = null!;
        public Guid LocationId { get; private set; }
        public VehicleType VehicleType { get; private set; } = null!;
        public VehicleStatus Status { get; private set; }

        // Change vehicle location through domain behavior
        public void ChangeLocation(Guid newLocationId)
        {
            if (newLocationId == Guid.Empty)
                throw new DomainException("LocationId cannot be empty.");

            LocationId = newLocationId;
        }

        public void MarkAvailable()
        {

            if (Status == VehicleStatus.Reserved)
                throw new DomainException("Reserved vehicle cannot be marked Available without explicit release.");

            Status = VehicleStatus.Available;
        }

        public void MarkRented()
        {
            if (Status == VehicleStatus.Rented)
                throw new DomainException("Vehicle cannot be rented because it is already rented.");

            if (Status == VehicleStatus.Reserved)
                throw new DomainException("Vehicle cannot be rented because it is reserved.");

            if (Status == VehicleStatus.Serviced)
                throw new DomainException("Vehicle cannot be rented because it is under service.");

            Status = VehicleStatus.Rented;
        }

        public void MarkReserved()
        {
            if (Status == VehicleStatus.Rented)
                throw new DomainException("Rented vehicle cannot be reserved.");

            if (Status == VehicleStatus.Serviced)
                throw new DomainException("Serviced vehicle cannot be reserved.");

            Status = VehicleStatus.Reserved;
        }

        public void MarkServiced()
        {
            if (Status == VehicleStatus.Rented)
                throw new DomainException("Rented vehicle cannot be put into service.");

            Status = VehicleStatus.Serviced;
        }

        // Explicit command method
        public void ReleaseReservation()
        {
            if (Status != VehicleStatus.Reserved)
                throw new DomainException("Vehicle is not reserved, so reservation cannot be released.");

            Status = VehicleStatus.Available;
        }
    }
}
