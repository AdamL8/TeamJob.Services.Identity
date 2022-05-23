using System;
using System.Threading.Tasks;
using TeamJob.Services.Identity.Core.Entities;

namespace TeamJob.Services.Identity.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string id);
        Task DeleteAsync(string id);
        Task<User> GetFromEmailAsync(string email);
        Task AddAsync(User user);
    }
}