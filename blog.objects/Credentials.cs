using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.objects
{
    public record Credentials
    {
        public string email { get; init; } = string.Empty;
        public string password { get; init; } = string.Empty;
    }
}
