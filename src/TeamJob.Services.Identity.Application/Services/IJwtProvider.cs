using System;
using System.Collections.Generic;
using TeamJob.Services.Identity.Application.DTO;

namespace TeamJob.Services.Identity.Application.Services
{
    public interface IJwtProvider
    {
        AuthDto Create(string                                     userId,
                       string                                   role, 
                       string                                   audience = null,
                       IDictionary<string, IEnumerable<string>> claims = null);
    }
}