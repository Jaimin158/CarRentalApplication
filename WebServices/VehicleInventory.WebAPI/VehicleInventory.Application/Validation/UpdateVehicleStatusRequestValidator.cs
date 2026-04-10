using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using VehicleInventory.Application.DTOs;

namespace VehicleInventory.Application.Validation
{
    public class UpdateVehicleStatusRequestValidator : AbstractValidator<UpdateVehicleStatusRequest>
    {
        public UpdateVehicleStatusRequestValidator()
        {
            RuleFor(x => x.NewStatus).IsInEnum();
        }
    }
}
