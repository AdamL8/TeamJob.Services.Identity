using FluentValidation;
using System.Text.RegularExpressions;

namespace Teamjob.Services.Identity.Requests.Validations
{
    public class RegisterValidator : AbstractValidator<Register>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Password cannot be empty")
                .NotEmpty().WithMessage("Password cannot be empty")
                .Must(IsValidPassword).WithMessage("Password must be between 8 and 20 alphanumeric characters");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Email cannot be empty")
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Invalid email address format");
        }

        private bool IsValidPassword(string password)
        {
            const string regex = @"^(?=.*?[a-z])(?=.*?[0-9]).{8,20}$";

            return Regex.Match(password, regex).Success;
        }

    }
}
