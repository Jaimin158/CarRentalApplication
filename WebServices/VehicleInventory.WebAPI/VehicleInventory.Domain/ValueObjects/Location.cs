using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleInventory.Domain.Entities;
using VehicleInventory.Domain.Exceptions;

namespace VehicleInventory.Domain.ValueObjects
{
    public class Location
    {
        private Location() { }

        public Location(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new DomainException("Location Id cannot be empty.");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Location name is required.");

            Id = id;
            Name = name.Trim();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        
    }
}
