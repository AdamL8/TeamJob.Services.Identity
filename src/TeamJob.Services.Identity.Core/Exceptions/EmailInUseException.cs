﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Core.Exceptions
{
    public class EmailInUseException : DomainException
    {
        public override string Code { get; } = "service.identity.exception.email_in_use";
        public string Email { get; }

        public EmailInUseException(string email)
            : base($"Email {email} is already in use.")
        {
            Email = email;
        }
    }
}
