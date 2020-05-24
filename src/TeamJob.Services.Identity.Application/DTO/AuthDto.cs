using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Application.DTO
{
    public class AuthDto
    {
        public Guid Id             { get; set; }
        public string AccessToken  { get; set; }
        public string RefreshToken { get; set; }
        public string Role         { get; set; }
        public long Expires        { get; set; }
    }
}
