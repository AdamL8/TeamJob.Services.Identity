using System;
using System.Threading.Tasks;
using TeamJob.Services.Identity.Core.Entities;

namespace TeamJob.Services.Identity.Core.Repositories
{

    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string id);
        Task<RefreshToken> GetFromTokenAsync(string token);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
        Task DeleteAsync(string id);
    }
}