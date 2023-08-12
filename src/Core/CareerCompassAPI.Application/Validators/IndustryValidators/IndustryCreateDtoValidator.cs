using CareerCompassAPI.Application.DTOs.Industry_DTOs;
using FluentValidation;

namespace CareerCompassAPI.Application.Validators.IndustryValidators
{
    public class IndustryCreateDtoValidator : AbstractValidator<IndustryCreateDto>
    {
        public IndustryCreateDtoValidator()
        {
            RuleFor(i => i.industryName).NotNull().NotEmpty().MaximumLength(50)
            .Matches("^[a-zA-Z ]+$").WithMessage("Industry name can only contain letters.");
        }
    }
}
