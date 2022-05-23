using System;
using System.Collections.Generic;
using System.Text;
using Convey.Types;

namespace TeamJob.Services.Identity.Infrastructure.Mongo.Documents
{
    internal sealed class RefreshTokenDocument : IIdentifiable<string>
    {
        public string Id             { get; set; }
        public string UserId         { get; set; }
        public string Token        { get; set; }
        public DateTime CreatedAt  { get; set; }
        public DateTime? RevokedAt { get; set; }
    }
}
