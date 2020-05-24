using System;
using System.Threading.Tasks;
using TeamJob.Services.Identity.Core.Entities;

namespace TeamJob.Services.Identity.Core.Repositories
{

    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(Guid id);
        Task<RefreshToken> GetAsync(string token);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
        Task DeleteAsync(Guid id);
    }
}