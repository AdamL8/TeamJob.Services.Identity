using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using TeamJob.Services.Identity.Application.Services;

namespace TeamJob.Services.Identity.Application.Commands.Handler
{
    // Simple wrapper
    internal sealed class RegisterHandler : ICommandHandler<Register>
    {
        private readonly IIdentityService _identityService;

        public RegisterHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task HandleAsync(Register command) => await _identityService.RegisterAsync(command);
    }
}
