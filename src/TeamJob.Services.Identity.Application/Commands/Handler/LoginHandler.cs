using System.Threading.Tasks;
using Convey.CQRS.Commands;
using TeamJob.Services.Identity.Application.Services;

namespace TeamJob.Services.Identity.Application.Commands.Handler
{
    // Simple wrapper
    internal sealed class LoginHandler : ICommandHandler<Login>
    {
        private readonly IIdentityService _identityService;

        public LoginHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task HandleAsync(Login command) => await _identityService.LoginAsync(command);
    }
}
