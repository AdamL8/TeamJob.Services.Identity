using FluentValidation;
using System.Text.RegularExpressions;

namespace Teamjob.Services.Identity.Requests.Validations
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Password cannot be empty")
                .NotEmpty().WithMessage("Password cannot be empty");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Email cannot be empty")
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Invalid email address format");
        }
    }
}
