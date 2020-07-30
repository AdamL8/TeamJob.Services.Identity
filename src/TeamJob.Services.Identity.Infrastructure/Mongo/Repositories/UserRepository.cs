using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using TeamJob.Services.Identity.Core.Entities;
using TeamJob.Services.Identity.Core.Repositories;
using TeamJob.Services.Identity.Infrastructure.Mongo.Documents;

namespace TeamJob.Services.Identity.Infrastructure.Mongo.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public UserRepository(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<User> GetAsync(Guid id)
        {
            var user = await _repository.GetAsync(id);

            return user?.AsEntity();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<User> GetAsync(string email)
        {
            var user = await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());

            return user?.AsEntity();
        }

        public async Task AddAsync(User user) => await _repository.AddAsync(user.AsDocument());

        public async Task UpdateAsync(User user)
        {
            if (user is null)
            { return; }
            await _repository.UpdateAsync(user.AsDocument());
        }
    }
}
