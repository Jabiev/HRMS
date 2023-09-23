using CareerCompassAPI.Application.DTOs.Contact_DTOs;
using FluentValidation;

namespace CareerCompassAPI.Application.Validators.ContactValidators
{
    public class ContactCreateDtoValidator : AbstractValidator<ContactCreateDto>
    {
        public ContactCreateDtoValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name cannot be longer than 50 characters");

            RuleFor(x => x.surname)
                .NotEmpty()
                .WithMessage("Surname is required")
                .MaximumLength(50)
                .WithMessage("Surname cannot be longer than 50 characters");

            RuleFor(x => x.email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email format");

            RuleFor(x => x.message)
                .NotEmpty()
                .WithMessage("Message is required")
                .MaximumLength(500)
                .WithMessage("Message cannot be longer than 500 characters");
        }
    }
}
