using CareerCompassAPI.Application.Validators.IndustryValidators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CareerCompassAPI.Persistence.ExtensionMethods
{
    public static class FluentValidationExtensions
    {
        public static void AddFluentValidationValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<IndustryCreateDtoValidator>();
        }
    }
}
