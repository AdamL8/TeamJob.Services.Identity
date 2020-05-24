using System;

namespace TeamJob.Services.Identity.Application.Services
{
    public interface IDateTimeProvider
    {
        public long Now { get; }
    }
}