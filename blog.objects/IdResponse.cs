using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.objects
{
    public record IdResponse
    {
        public string id { get; init; } = string.Empty;
    }
}