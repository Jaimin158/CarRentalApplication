using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleInventory.Domain.Enums;

namespace VehicleInventory.Application.DTOs
{
    public sealed record UpdateVehicleStatusRequest
    (
        VehicleStatus NewStatus,
        bool ReleaseReservation = false
        );
    
}
