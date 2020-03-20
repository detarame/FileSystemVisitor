using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace FileSystemVisitor
{
    public class Visitor
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
        IDirectory directory;
        enum ItemState
        {
            Return,
            Continue,
            Break
        }
        public Visitor(IDirectory directory)
        {
            this.directory = directory;
        }
        public IEnumerable<string> GetDirs(string root, Filter filter)
        {
            Start?.Invoke(this, new EventArgs());
            var rootsQueue = new Queue<string>(new string[]{ root });
            while (rootsQueue.Count != 0)
            {
                // Поиск в данной папке.
                root = rootsQueue.Dequeue(); 
                var dir = new List<string>(directory.GetDirectories(root));
                // Добавить папки в очередь.
                foreach (var i in dir) { 
                    rootsQueue.Enqueue(i);
                }
                dir.AddRange(directory.GetFiles(root));

                foreach (var name in dir)
                {
                    var result = ToCheck(name, filter);
                    if (result == ItemState.Return)
                    {
                            yield return name;
                    }
                    if (result == ItemState.Continue)
                    {
                       continue;
                    }
                    if (result == ItemState.Break)
                    {
                        yield break;
                    }
                }
            }
            Finish?.Invoke(this, new EventArgs());
        }
        private ItemState ToCheck(string name, Filter filter)
        {
            if (filter(name))
            {
                FilesFinded?.Invoke(this, new EventArgs());
                if (toExclude)
                {
                    toExclude = false;
                    return ItemState.Continue;
                }
                else
                {
                    return ItemState.Return;
                }
            }
            else
            {
                FilteredFilesFinded?.Invoke(this, new EventArgs());
                if (toStop)
                {
                    toStop = false;
                    Finish?.Invoke(this, new EventArgs());
                    return ItemState.Break;
                }
                else
                {
                    return ItemState.Continue;
                }
            }
            
        }
    }
}
