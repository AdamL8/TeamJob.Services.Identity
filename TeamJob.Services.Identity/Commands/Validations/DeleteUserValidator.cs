using FluentValidation;
using System.Text.RegularExpressions;
using TeamJob.Services.Identity.Commands;

namespace Teamjob.Services.Identity.Commands.Validations
{
    public class DeleteUserValidator : AbstractValidator<DeleteUser>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("User ID cannot be empty");
        }
    }
}
