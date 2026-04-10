using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using VehicleInventory.Application.DTOs;

namespace VehicleInventory.Application.Validation
{
    public class CreateVehicleRequestValidator : AbstractValidator<CreateVehicleRequest>
    {
        public CreateVehicleRequestValidator()
        {
            RuleFor(x => x.VehicleCode)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.VehicleType)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.LocationId)
                .NotEmpty();
        }
    }
}
