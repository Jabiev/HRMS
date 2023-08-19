using CareerCompassAPI.Application.DTOs.Vacancy_DTOs;
using FluentValidation;

namespace CareerCompassAPI.Application.Validators.VacancyValidators
{
    public class VacancyCreateDtoValidator : AbstractValidator<VacancyCreateDto>
    {
        public VacancyCreateDtoValidator()
        {
            RuleFor(v => v.jobTitle)
            .NotEmpty().WithMessage("Job title is required.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Job title can only contain letters and spaces.")
            .MaximumLength(30).WithMessage("Job title can have a maximum of 30 characters, including spaces.");

            RuleFor(v => v.salary)
             .NotEmpty().WithMessage("Salary is required.")
             .Must(value => value.ToString().All(char.IsDigit)).WithMessage("Salary can only contain digits.");

            RuleFor(v => v.experienceLevelId)
                .NotEmpty().WithMessage("Experience level ID is required.");

            RuleFor(v => v.jobTypeIds)
                .NotEmpty().WithMessage("Job type IDs are required.")
                .Must(ids => ids.Any()).WithMessage("At least one job type ID is required.");

            RuleFor(v => v.locationId)
                .NotEmpty().WithMessage("Job location ID is required.");

            RuleFor(v => v.description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(30).WithMessage("Description should contain a minimum of 30 characters.");

            RuleFor(v => v.shiftIds)
                .NotEmpty().WithMessage("Shift IDs are required.")
                .Must(ids => ids.Any()).WithMessage("At least one shift ID is required.");
        }
    }
}
