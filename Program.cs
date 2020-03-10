using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var varVisitor = new Visitor(@"D:\NewDir", x => x.Length < 5);

            foreach (var Dir in varVisitor)
            {
                Console.WriteLine(Dir);
            }
            Console.ReadKey();
        }
    }
}
