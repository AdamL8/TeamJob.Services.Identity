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
        private readonly IMongoRepository<UserDocument, string> _repository;

        public UserRepository(IMongoRepository<UserDocument, string> repository)
        {
            _repository = repository;
        }

        public async Task<User> GetAsync(string id)
        {
            var user = await _repository.GetAsync(id);

            return user?.AsEntity();
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<User> GetFromEmailAsync(string email)
        {
            var user = await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());

            return user?.AsEntity();
        }

        public async Task AddAsync(User user) => await _repository.AddAsync(user.AsDocument());
    }
}
