﻿using System;
using System.Threading.Tasks;
using TeamJob.Services.Identity.Core.Entities;

namespace TeamJob.Services.Identity.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}