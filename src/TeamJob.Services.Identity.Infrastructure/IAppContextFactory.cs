using System;
using System.Collections.Generic;
using System.Text;
using TeamJob.Services.Identity.Application;

namespace TeamJob.Services.Identity.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}
