using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Application.Exceptions
{
    public abstract class AppException : Exception
    {
        public virtual string Code { get; }

        protected AppException(string message) : base(message)
        {
        }
    }
}
