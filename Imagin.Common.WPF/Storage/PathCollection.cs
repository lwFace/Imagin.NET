using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace Imagin.Common.Storage
{
    public class PathCollection : ConcurrentCollection<string>
    {
        Dispatcher dispatcher => Application.Current.Dispatcher;

        readonly FileSystemWatcher watcher = new FileSystemWatcher()
        {
            NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Security
        };

        Filter filter;
        public Filter Filter
        {
            get => filter;
            set => filter = value;
        }

        public string Path { get; private set; }

        public PathCollection(Filter filter) : base()
        {
            Filter = filter;
        }

        bool Watch()
        {
            if (Folder.Long.Exists(Path))
            {
                watcher.Path = Path;

                watcher.Created -= OnObjectCreated;
                watcher.Deleted -= OnObjectDeleted;
                watcher.Renamed -= OnObjectRenamed;

                try
                {
                    watcher.EnableRaisingEvents = true;
                    watcher.Created += OnObjectCreated;
                    watcher.Deleted += OnObjectDeleted;
                    watcher.Renamed += OnObjectRenamed;
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            return false;
        }

        protected virtual void OnObjectCreated(object sender, FileSystemEventArgs e)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(DispatcherPriority.Normal, (FileSystemEventHandler)OnObjectCreated, sender, e);
                return;
            }

            if (File.Long.Exists(e.FullPath))
            {
                if (Filter.Evaluate(e.FullPath, ItemType.File))
                {
                    Add(e.FullPath);
                }
            }
            else if (Folder.Long.Exists(e.FullPath))
            {
                if (Filter.Evaluate(e.FullPath, ItemType.Folder))
                {
                    Add(e.FullPath);
                }
            }
        }

        protected virtual void OnObjectDeleted(object sender, FileSystemEventArgs e)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(DispatcherPriority.Normal, (FileSystemEventHandler)OnObjectDeleted, sender, e);
                return;
            }
            Remove(this.FirstOrDefault(i => i == e.FullPath));
        }

        protected virtual void OnObjectRenamed(object sender, RenamedEventArgs e)
        {
            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(DispatcherPriority.Normal, (RenamedEventHandler)OnObjectRenamed, sender, e);
                return;
            }
            var index = IndexOf(this.FirstOrDefault(i => i == e.OldFullPath));
            RemoveAt(index);
            Insert(index, e.FullPath);
        }

        IEnumerable<string> Query(string path)
        {
            if (filter.Types.HasFlag(ItemType.Drive) && (Path.NullOrEmpty() || Path == @"\"))
            {
                foreach (var i in Machine.Drives)
                    yield return i.Name;
            }
            if (filter.Types.HasFlag(ItemType.Folder))
            {
                var folders = Folder.Long.GetFolders(Path);
                foreach (var i in folders.Where(j => j != ".."))
                    yield return i;
            }
            if (filter.Types.HasFlag(ItemType.File))
            {
                var files = Folder.Long.GetFiles(Path);
                foreach (var i in files)
                {
                    if (filter.Evaluate(i, ItemType.File))
                        yield return i;
                }
            }
        }

        public void Refresh(string path)
        {
            Path = path;
            Watch();

            if (0 < Count)
            {
                Clear();
            }

            try
            {
                var result = Query(Path);
                result.ForEach(i => Add(i));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}