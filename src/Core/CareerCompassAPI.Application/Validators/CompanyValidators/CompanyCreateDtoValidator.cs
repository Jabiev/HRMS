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
                .NotEmpty().WithMessage("Company name is required")
                .MaximumLength(50).WithMessage("Company name should be max 50 characters")
                .Matches(new Regex("^[a-zA-Z0-9&!@ ]+$")).WithMessage("Company name can only contain letters, digits, and the symbols & ! @");

            RuleFor(x => x.locationId)
                .NotEmpty().WithMessage("Location is required");

            RuleFor(x => x.ceoName)
                .NotEmpty().WithMessage("CEO name is required")
                .MaximumLength(50).WithMessage("CEO name should be max 50 characters")
                .Matches(new Regex("^[A-Za-z\\s]*$")).WithMessage("CEO name cannot contain digits or symbols");

            RuleFor(x => x.dateFounded)
                .GreaterThan(1900).WithMessage("Please enter a valid year")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Date founded cannot be after the current year")
                .NotEmpty().WithMessage("Date founded is required");

            RuleFor(x => x.address)
                .NotEmpty().WithMessage("Address is required")
                .Matches(new Regex("^[a-zA-Z0-9\\s,]+$")).WithMessage("Address can only contain letters, digits, spaces");

            RuleFor(x => x.industryId)
                .NotEmpty().WithMessage("Industry is required");

            RuleFor(x => x.description)
                .MaximumLength(255).WithMessage("Description should be max 255 characters");
        }
    }
}
