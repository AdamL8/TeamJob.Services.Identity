using System;
using System.Threading.Tasks;
using TeamJob.Services.Identity.Application.Commands;
using TeamJob.Services.Identity.Application.DTO;

namespace TeamJob.Services.Identity.Application.Services
{
    public interface IIdentityService
    {
        Task<UserDto> GetAsync(string id);
        Task<AuthDto> LoginAsync(Login command);
        Task<UserDto> RegisterAsync(Register command);
    }
}