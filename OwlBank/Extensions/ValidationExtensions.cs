using FluentValidation;
using FluentValidation.AspNetCore;
using OwlBank.Validators;

namespace OwlBank;

public static class ValidationExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
        
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        
        return services;
    }
    
}