using System;
using System.Collections.Generic;
using System.Text;
using TeamJob.Services.Identity.Application.Services;

namespace TeamJob.Services.Identity.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public long Now => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
