using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleInventory.Domain.Exceptions;

namespace VehicleInventory.Domain.ValueObjects
{
    public sealed class VehicleCode
    {
        public string Value { get; }

        
        private VehicleCode() { } 

        public VehicleCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("VehicleCode is required.");

            Value = value.Trim();
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            if (obj is not VehicleCode other)
                return false;

            return Value == other.Value;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator string(VehicleCode vehicleCode) => vehicleCode.Value;
        public static explicit operator VehicleCode(string value) => new VehicleCode(value);
    }
}
