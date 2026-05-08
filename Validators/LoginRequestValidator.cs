using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace OwlBank.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(request => request.Password).NotEmpty().MinimumLength(8).WithMessage("Password is required");
    }
}