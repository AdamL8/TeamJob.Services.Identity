using FluentValidation;
using System.Text.RegularExpressions;

namespace Teamjob.Services.Identity.Commands.Validations
{
    public class ChangePasswordValidator : AbstractValidator<ChangePassword>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotNull().WithMessage("User ID cannot be empty");

            RuleFor(x => x.CurrentPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Current Password cannot be empty")
                .NotEmpty().WithMessage("Current Password cannot be empty")
                .Must(IsValidPassword).WithMessage("Current Password must be between 8 and 20 alphanumeric characters");

            RuleFor(x => x.NewPassword)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("New Password cannot be empty")
                .NotEmpty().WithMessage("New Password cannot be empty")
                .Must(IsValidPassword).WithMessage("New Password must be between 8 and 20 alphanumeric characters");
        }

        private bool IsValidPassword(string password)
        {
            const string regex = @"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,20})$";

            return Regex.Match(password, regex).Success;
        }

    }
}
