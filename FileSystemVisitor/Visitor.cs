using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace FileSystemVisitor
{
    class Visitor
    {
        // Флаг для прекращения поиска.
        public bool toStop { get; set; } 
        // Флаг для исключения файла/папки.
        public bool toExclude { get; set; } 

        public event EventHandler Start;
        public event EventHandler Finish;
        public event EventHandler FilesFinded;
        public event EventHandler FilteredFilesFinded;

        public delegate bool Filter(string x);

        public IEnumerable GetDirs(string root, Filter filter)
        {
            Start?.Invoke(this, new EventArgs());
            var rootsQueue = new Queue<string>(new string[]{ root });
            while (rootsQueue.Count != 0)
            {
                // Поиск в данной папке.
                root = rootsQueue.Dequeue(); 
                var dir = new List<string>(Directory.GetDirectories(root));
                // Добавить папки в очередь.
                foreach (var i in dir) { 
                    rootsQueue.Enqueue(i);
                }
                dir.AddRange(Directory.GetFiles(root));

                foreach (var name in dir)
                {
                    var result = ToCheck(name, filter);
                    if (result == 1)
                    {
                            yield return name;
                    }
                    else if (result == 0)
                    {
                       continue;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
            Finish?.Invoke(this, new EventArgs());
        }
        private int ToCheck(string name, Filter filter)
        {
            if (filter(name))
            {
                FilesFinded?.Invoke(this, new EventArgs());
                if (toExclude)
                {
                    toExclude = false;
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                FilteredFilesFinded?.Invoke(this, new EventArgs());
                if (toStop)
                {
                    toStop = false;
                    Finish?.Invoke(this, new EventArgs());
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            
        }
    }
}
