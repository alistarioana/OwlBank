using FluentValidation;
using OwlBank.DTOs.UserDTO;

namespace OwlBank.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(request => request.ConfirmPassword).NotEmpty().WithMessage("Password must be confirmed").MinimumLength(6);
        RuleFor(request => request.Password).NotEmpty().WithMessage("Password is required").MinimumLength(6);
        RuleFor(request => request.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid email address");
        RuleFor(request => request.FirstName).NotEmpty().WithMessage("First name is required");
        RuleFor(request => request.LastName).NotEmpty().WithMessage("Last name is required");
        RuleFor(request => request.DateOfBirth).NotEmpty().WithMessage("Date of birth is required");
        RuleFor(request => request.PhoneNumber).NotEmpty().MinimumLength(10).WithMessage("Phone number is required");
    }
}