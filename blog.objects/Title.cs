using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.objects
{
    public record Title
    {
        public string id { get; init; } = string.Empty;
        public string title { get; init; } = string.Empty;
        public DateTime publishedAt { get; init; } = DateTime.MinValue;
        public string fragment { get; init; } = string.Empty;
    }
}