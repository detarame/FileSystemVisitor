using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitor
{
    class User
    {

        public void OnFilteredFinded(Object o, EventArgs e)
        {
            Visitor visitor = (Visitor)o;
            Console.WriteLine("Filtered file finded. Do you want to stop search? (y/-)");
            if (Console.ReadLine() == "y") visitor.toStop = true;
        }
        public void OnFilesFinded(Object o, EventArgs e)
        {
            Visitor visitor = (Visitor)o;
            Console.WriteLine("File finded. Do you want to exclude it?(y/-)");
            if (Console.ReadLine() == "y") visitor.toExclude = true;
        }
        public void OnStart(Object o, EventArgs e)
        {
            Console.WriteLine("Search started");
        }
        public void OnFinish(Object o, EventArgs e)
        {
            Console.WriteLine("Search finished");
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            var user = new User();
            var varVisitor = new Visitor(@"D:\NewDir", x => x.Length < 5);
            varVisitor.FilteredFilesFinded += user.OnFilteredFinded;
            varVisitor.Finish += user.OnFinish;
            varVisitor.Start += user.OnStart;
            varVisitor.FilesFinded += user.OnFilesFinded;

            foreach (var i in varVisitor.GetDirs(@"D:\NewDir", x => x.Length <= 35))
            {
                Console.WriteLine(i);
            }
            Console.ReadKey();
        }
    }
}
