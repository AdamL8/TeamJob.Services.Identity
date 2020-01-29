using Convey.CQRS.Events;

namespace Teamjob.Services.Identity.Events
{
    public class LoginRejected : IEvent
    {
        public string Email  { get; }
        public string Reason { get; }

        public LoginRejected(string email, string reason)
        {
            Email  = email;
            Reason = reason;
        }
    }
}
