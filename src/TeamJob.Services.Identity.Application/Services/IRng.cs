using System;
using System.Collections.Generic;
using System.Text;

namespace TeamJob.Services.Identity.Application.Services
{
    public interface IRng
    {
        string Generate(int length = 50, bool removeSpecialChars = false);
    }
}
