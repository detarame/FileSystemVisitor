using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

using System.Threading.Tasks;



namespace FileSystemVisitor
{
    class Visitor: IEnumerable
    {
        string RootPath { get; set; } //начальная точка

        public bool toStop { get; set; } //флаг для прекращения 
        public bool toExclude { get; set; } //флаг для исключения файла/папки

        List<string> allFiltered; //папки и файлы фильтр
        List<string> all; //папки и файлы вместе
        List<string> dir;
        List<string> files;
       

        public event EventHandler Start;
        public event EventHandler Finish;
        public event EventHandler FilesFinded;
        public event EventHandler FilteredFilesFinded;


        public delegate bool Filter(string x);
        Filter FilterMethod;

        public Visitor(string path, Filter filter)
        {
            RootPath = path;
            FilterMethod = filter;
            all = new List<string>();
            dir = new List<string>();
            files = new List<string>();

            List<string> root_path = new List<string> { RootPath };
            Start?.Invoke(this, new EventArgs());
            FindFilesAndDirs(root_path);
            Finish?.Invoke(this, new EventArgs());
            FilesFinded?.Invoke(this, new EventArgs());
            FilterFiles();
            FilteredFilesFinded?.Invoke(this, new EventArgs());
        }


        public void FindFilesAndDirs(List<string> paths)//найти все вложенные папки и файлы 
        {
            foreach (var i in paths)
            {
                var buf = new List<string>(Directory.GetDirectories(i));//добавить папки в буфер
                files.AddRange(new List<string>(Directory.GetFiles(i)));//добавить файлы в список
                if (buf.Count == 0) return;//при отсутствии вложенных папок выйти
                dir.AddRange(buf); //добавить папки в список
                FindFilesAndDirs(buf); //обработать вложенные папки
            }
        }
        public void FilterFiles() //отфильтровать всё найденное
        {
            foreach (var name in all)
            {
                if (FilterMethod(name)) allFiltered.Add(name);
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < files.Count; i++)
                yield return files[i];
            for (int i = 0; i < dir.Count; i++)
                yield return dir[i];
        }

        //version 3
        public IEnumerable GetDirs(string root, Filter filter)
        {
            Start?.Invoke(this, new EventArgs());
            Queue<string> rootsQueue = new Queue<string>(new string[]{ root });
            while (rootsQueue.Count != 0)
            {
                root = rootsQueue.Dequeue(); //поиск в данной папке
                files = new List<string>(Directory.GetFiles(root));
                dir = new List<string>(Directory.GetDirectories(root));
                foreach (var i in dir) { //добавить папки в очередь
                    rootsQueue.Enqueue(i);
                }
                for (int i = 0; i < files.Count; i++)
                { //поиск и фильтр файлов
                    if (filter(files[i]))
                    {
                        FilesFinded?.Invoke(this, new EventArgs());
                        if (toExclude) //если файл исключается
                        {
                            toExclude = false;
                            continue;
                        }
                        else { yield return files[i]; }
                    }
                    else
                    {
                        FilteredFilesFinded?.Invoke(this, new EventArgs());
                        if (toStop) { toStop = false; yield break; }; //остановка поиска
                    }
                }
                for (int i = 0; i < dir.Count; i++) //поиск и фильтр папок
                {
                    if (filter(dir[i]))
                    {
                        FilesFinded?.Invoke(this, new EventArgs());
                        if (toExclude)
                        {
                            toExclude = false;
                            continue;
                        }
                        else { yield return dir[i]; }
                    }
                    else
                    {
                        FilteredFilesFinded?.Invoke(this, new EventArgs());
                        if (toStop) { toStop = false; yield break; };
                    }
                }
            }
            Finish?.Invoke(this, new EventArgs());
        }

    }
}
