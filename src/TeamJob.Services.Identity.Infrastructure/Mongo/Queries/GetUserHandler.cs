using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using TeamJob.Services.Identity.Application.DTO;
using TeamJob.Services.Identity.Application.Queries;
using TeamJob.Services.Identity.Infrastructure.Mongo.Documents;

namespace TeamJob.Services.Identity.Infrastructure.Mongo.Queries
{
    internal sealed class GetUserHandler : IQueryHandler<GetUser, UserDto>
    {
        private readonly IMongoRepository<UserDocument, string> _userRepository;

        public GetUserHandler(IMongoRepository<UserDocument, string> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> HandleAsync(GetUser query)
        {
            var user = await _userRepository.GetAsync(query.UserId);

            return user?.AsDto();
        }
    }
}
