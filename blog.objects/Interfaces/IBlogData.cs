using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.objects.Interfaces
{
    public interface IBlogData
    {
        string Add(BlogEntry entry);
        IList<Title> GetTitles();
        Blog? Get(string id);
        void Delete(string id);
    }
}