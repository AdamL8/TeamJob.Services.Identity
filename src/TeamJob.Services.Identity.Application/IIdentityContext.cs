using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Application
{
    public interface IIdentityContext
    {
        Guid Id                            { get; }
        string Role                        { get; }
        bool IsAuthenticated               { get; }
        bool IsAdmin                       { get; }
        IDictionary<string, string> Claims { get; }
    }
}
