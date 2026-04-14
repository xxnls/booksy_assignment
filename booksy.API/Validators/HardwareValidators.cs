using FluentValidation;
using booksy.API.Models.DTOs;
using booksy.API.Models.Enums;

namespace booksy.API.Validators
{
    public class CreateHardwareDtoValidator : AbstractValidator<CreateHardwareDto>
    {
        public CreateHardwareDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required")
                .Length(2, 100).WithMessage("Brand must be between 2 and 100 characters");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");

            RuleFor(x => x.History)
                .MaximumLength(2000).WithMessage("History cannot exceed 2000 characters");
        }
    }

    public class UpdateHardwareDtoValidator : AbstractValidator<UpdateHardwareDto>
    {
        public UpdateHardwareDtoValidator()
        {
            RuleFor(x => x.Name)
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Brand)
                .Length(2, 100).WithMessage("Brand must be between 2 and 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Brand));

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status")
                .When(x => x.Status.HasValue);

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));

            RuleFor(x => x.History)
                .MaximumLength(2000).WithMessage("History cannot exceed 2000 characters")
                .When(x => !string.IsNullOrEmpty(x.History));
        }
    }
}
