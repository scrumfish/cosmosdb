using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.objects.Interfaces
{
    public interface IImageData
    {
        string Save(string filename, Stream stream);
        void Delete(string filename);
    }
}
