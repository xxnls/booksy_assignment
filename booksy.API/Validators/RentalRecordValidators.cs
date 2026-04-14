using FluentValidation;
using booksy.API.Models.DTOs;

namespace booksy.API.Validators
{
    public class CreateRentalRecordDtoValidator : AbstractValidator<CreateRentalRecordDto>
    {
        public CreateRentalRecordDtoValidator()
        {
            RuleFor(x => x.HardwareId)
                .GreaterThan(0).WithMessage("Hardware ID is required");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID is required");
        }
    }
}
