﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using TeamJob.Services.Identity.Core.Entities;
using TeamJob.Services.Identity.Core.Repositories;
using TeamJob.Services.Identity.Infrastructure.Mongo.Documents;

namespace TeamJob.Services.Identity.Infrastructure.Mongo.Repositories
{
    internal sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoRepository<RefreshTokenDocument, Guid> _repository;

        public RefreshTokenRepository(IMongoRepository<RefreshTokenDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<RefreshToken> GetAsync(Guid id)
        {
            var refreshToken = await _repository.GetAsync(x => x.Id == id);

            return refreshToken?.AsEntity();
        }

        public async Task<RefreshToken> GetAsync(string token)
        {
            var refreshToken = await _repository.GetAsync(x => x.Token == token);

            return refreshToken?.AsEntity();
        }

        public async Task AddAsync(RefreshToken token) => await _repository.AddAsync(token.AsDocument());

        public async Task UpdateAsync(RefreshToken token) => await _repository.UpdateAsync(token.AsDocument());
        public async Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);
    }
}
