using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleInventory.Domain.Exceptions;

namespace VehicleInventory.Domain.ValueObjects
{
    public sealed class VehicleType
    {
        public string Value { get; }

        private VehicleType() { } 

        public VehicleType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("VehicleType is required.");

            Value = value.Trim();
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            if (obj is not VehicleType other)
                return false;

            return Value == other.Value;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator string(VehicleType vehicleType) => vehicleType.Value;
        public static explicit operator VehicleType(string value) => new VehicleType(value);
    }
}
