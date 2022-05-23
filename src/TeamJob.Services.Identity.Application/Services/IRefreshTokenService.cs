using System;
using System.Threading.Tasks;
using TeamJob.Services.Identity.Application.DTO;

namespace TeamJob.Services.Identity.Application.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(string userId);
        Task RevokeAsync(string refreshToken);
        Task<AuthDto> UseAsync(string refreshToken);
    }
}