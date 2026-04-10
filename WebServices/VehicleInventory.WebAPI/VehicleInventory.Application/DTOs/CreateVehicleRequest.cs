using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleInventory.Application.DTOs
{
    public sealed record CreateVehicleRequest
    (
         string VehicleCode,
        Guid LocationId,
        string VehicleType
        
        );
    
}
