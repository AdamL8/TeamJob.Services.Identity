using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using TeamJob.Services.Identity;

namespace Teamjob.Services.Identity.Commands.Validations
{
    public class ResetPasswordValidator : AbstractValidator<ResetPassword>
    {
        public static readonly IList<string> SupportedLanguages = new ReadOnlyCollection<string>(new List<string> { "en", "fr" });

        public ResetPasswordValidator()
        {
            RuleFor(x => x.Lang)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Language cannot be empty")
                .NotNull().WithMessage("Language cannot be empty")
                .Must(IsValidLanguage).WithMessage($"Language supplied is not supported. The supported languages are [{string.Join(" ", SupportedLanguages)}]");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Email cannot be empty")
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Invalid email address format");
        }

        private bool IsValidLanguage(string InLanguage)
        {
            return SupportedLanguages.Any(x => x.Equals(InLanguage, StringComparison.OrdinalIgnoreCase));
        }
    }
}
