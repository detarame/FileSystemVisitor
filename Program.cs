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
            Console.WriteLine("filter");
            foreach (var i in varVisitor.GetDirs(@"D:\NewDir", x => x.Length <= 20))
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("end filter");
            Console.ReadKey();
        }
    }
}
