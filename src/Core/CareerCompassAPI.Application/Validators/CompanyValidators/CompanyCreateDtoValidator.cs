using CareerCompassAPI.Application.DTOs.Company_DTOs;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace CareerCompassAPI.Application.Validators.CompanyValidators
{
    public class CompanyCreateDtoValidator : AbstractValidator<CompanyCreateDto>
    {
        public CompanyCreateDtoValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .MaximumLength(50)
                .Matches(new Regex("^[a-zA-Z0-9]+$"))
                .WithMessage("Company name can only contain letters and digits and its length should be max 50 characters.");

            RuleFor(x => x.ceoName)
                .NotEmpty()
                .MaximumLength(50)
                .Matches(new Regex("^[a-zA-Z ]+$"))
                .WithMessage("CEO name can only contain letters and its length should be max 50 characters.");

            RuleFor(x => x.dateFounded)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage("Date founded cannot be after the current year.");

            RuleFor(x => x.companySize)
              .GreaterThan(0)
              .WithMessage("Company size must be greater than 0.");
        }
    }
}
