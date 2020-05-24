using System;
using System.Collections.Generic;
using System.Text;
using Convey.Types;

namespace TeamJob.Services.Identity.Infrastructure.Mongo.Documents
{
    internal sealed class UserDocument : IIdentifiable<Guid>
    {
        public Guid Id                         { get; set; }
        public string Email                    { get; set; }
        public string Role                     { get; set; }
        public string Password                 { get; set; }
        public long CreatedAt                  { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
