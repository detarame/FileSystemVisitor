using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitor
{
    public interface IDirectory
    {
        string[] GetFiles(string root);
        string[] GetDirectories(string root);
    }
}
