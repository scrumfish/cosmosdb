using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.objects
{
    public record BlogEntry
    {
        public string title { get; init; } = string.Empty;
        public string article { get; init; } = string.Empty;
        public string fragment { get; init; } = string.Empty;
        public IList<string> images { get; init; } = new List<string>();
    }
}