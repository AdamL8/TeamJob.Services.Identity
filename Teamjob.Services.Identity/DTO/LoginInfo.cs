using System;

namespace Teamjob.Services.Identity.DTO
{
    public class LoginInfo
    {
        public Guid Id            { get; set; }
        public string AccessToken { get; set; }
        public string Role        { get; set; }
    }
}
