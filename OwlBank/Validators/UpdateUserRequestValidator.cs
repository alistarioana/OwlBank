using FluentValidation;
using Microsoft.EntityFrameworkCore.TestUtilities;
using OwlBank.DTOs.UserDTO;

namespace OwlBank.Validators
{
    public class UpdateUserRequestValidator: AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Length(8).Custom((Content, Context) =>
            {
                bool hasUpperCase = false;
                bool hasDigit = false;
                foreach(char  c in Content)
                {
                    if (char.IsUpper(c) )
                    {
                        hasUpperCase = true;
                        return;
                    }
                    
                }
                foreach (char c in Content)
                {
                    
                    if (char.IsDigit(c))
                    {
                        hasDigit = true;
                        return;
                    }
                }
                if (!hasUpperCase || !hasDigit)
                {
                    Context.AddFailure("Your password must have one upper case letter and one digit");
                }
 
            } );
        }
    }
}
