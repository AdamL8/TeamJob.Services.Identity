using System;
using System.Collections.Generic;
using System.Text;
using Convey.CQRS.Queries;
using TeamJob.Services.Identity.Application.DTO;

namespace TeamJob.Services.Identity.Application.Queries
{
    public class GetUser : IQuery<UserDto>
    {
        public string UserId { get; set; }
    }
}
