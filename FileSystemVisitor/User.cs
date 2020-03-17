using System;

namespace FileSystemVisitor
{
    class User
    {
        public void OnFilteredFinded(Object o, EventArgs e)
        {
            var visitor = (Visitor)o;
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
}
