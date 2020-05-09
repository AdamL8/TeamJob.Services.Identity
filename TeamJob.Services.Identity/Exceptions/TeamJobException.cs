using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teamjob.Services.Identity.Exceptions
{
    public class TeamJobException : Exception
    {
        public virtual string Code { get; }

        public TeamJobException()
        {
        }

        public TeamJobException(string InCode)
        {
            Code = InCode;
        }

        public TeamJobException(string InMessage, params object[] InArgs)
            : this(string.Empty, InMessage, InArgs)
        {
        }

        public TeamJobException(string InCode, string InMessage, params object[] InArgs)
            : this(null, InCode, InMessage, InArgs)
        {
        }

        public TeamJobException(Exception InnerException, string InMessage, params object[] InArgs)
            : this(InnerException, string.Empty, InMessage, InArgs)
        {
        }

        public TeamJobException(Exception InnerException, string InCode, string InMessage, params object[] InArgs)
            : base(string.Format(InMessage, InArgs), InnerException)
        {
            Code = InCode;
        }
    }
}
