using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.objects
{
    public class Blog
    {
        public string? id { get; set; }
        public string? title { get; set; }
        public string? article { get; set; }
        public string? fragment { get; set; }
        public DateTime publishedAt { get; set; }
    }
}