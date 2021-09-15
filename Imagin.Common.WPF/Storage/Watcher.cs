using Imagin.Common.Analytics;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Imagin.Common.Storage
{
    public class Watcher : Base
    {
        public event FileSystemEventHandler Changed;

        public event FileSystemEventHandler Created;

        public event FileSystemEventHandler Deleted;

        public event RenamedEventHandler Renamed;

        ///..................................................................................................

        public event EventHandler<EventArgs<Error>> Failed;

        ///..................................................................................................

        public event EventHandler<EventArgs> Started;

        public event EventHandler<EventArgs> Stopped;

        ///..................................................................................................

        FileSystemWatcher watcher;

        List<FileSystemWatcher> parentWatchers = new List<FileSystemWatcher>();

        ///..................................................................................................

        bool Enable
        {
            get => watcher.EnableRaisingEvents;
            set => watcher.EnableRaisingEvents = value;
        }

        List<string> otherFoldersToWatch = new List<string>();

        public bool IncludeParents { get; set; }

        public bool IncludeSubfolders
        {
            get => watcher.IncludeSubdirectories;
            set => watcher.IncludeSubdirectories = value;
        }

        public NotifyFilters NotifyFilter
        {
            get => watcher.NotifyFilter;
            set => watcher.NotifyFilter = value;
        }

        public string Path
        {
            get => watcher.Path;
            private set => watcher.Path = value;
        }

        ///..................................................................................................

        public Watcher() : base()
        {
            watcher = new FileSystemWatcher();
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
        }

        ///..................................................................................................

        void Handle(FileSystemEventArgs e, Action handler, Action invoke)
        {
            var dispatcher = Application.Current?.Dispatcher;
            var checkAccess = dispatcher?.CheckAccess();

            if (checkAccess == false && handler is Action)
            {
                dispatcher?.Invoke(handler);
            }
            else if (checkAccess == true)
                invoke?.Invoke(); 
        }

        ///..................................................................................................

        protected virtual void OnChanged(object sender, FileSystemEventArgs e)
        {
            Handle(e, () => OnChanged(sender, e), () => Changed?.Invoke(this, e));
        }

        protected virtual void OnCreated(object sender, FileSystemEventArgs e)
        {
            Handle(e, () => OnCreated(sender, e), () => Created?.Invoke(this, e));
        }

        protected virtual void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Handle(e, () => OnDeleted(sender, e), () => Deleted?.Invoke(this, e));
        }

        protected virtual void OnRenamed(object sender, RenamedEventArgs e)
        {
            Handle(e, () => OnRenamed(sender, e), () => Renamed?.Invoke(this, e));
        }

        ///..................................................................................................

        protected virtual void OnParentItemDeleted(object sender, FileSystemEventArgs e)
        {
            Handle(e, () => OnParentItemDeleted(sender, e), () =>
            {
                if (parentWatchers.Select(i => i.Path).Contains(e.FullPath) || Path == e.FullPath)
                {
                    Stop();
                    OnFailed(new Error());
                }
            });
        }

        protected virtual void OnParentItemRenamed(object sender, RenamedEventArgs e)
        {
            Handle(e, () => OnParentItemRenamed(sender, e), () =>
            {
                if (e.FullPath != e.OldFullPath && (parentWatchers.Select(i => i.Path).Contains(e.OldFullPath) || Path == e.OldFullPath))
                {
                    Stop();
                    OnFailed(new Error());
                }
            });
        }

        ///..................................................................................................

        protected virtual void OnFailed(Error result)
        {
            Failed?.Invoke(this, new EventArgs<Error>(result));
        }

        ///..................................................................................................

        IEnumerable<string> GetParents()
        {
            var i = System.IO.Path.GetDirectoryName(Path);
            while (!i.NullOrEmpty())
            {
                yield return i;
                i = System.IO.Path.GetDirectoryName(i);
            }
        }

        void WatchParents(bool watch)
        {
            if (!watch)
            {
                foreach (var i in parentWatchers)
                {
                    i.Deleted -= OnParentItemDeleted;
                    i.Renamed -= OnParentItemRenamed;
                    i.EnableRaisingEvents = false;
                }

                parentWatchers.Clear();
                return;
            }

            foreach (var i in GetParents())
            {
                var j = new FileSystemWatcher()
                {
                    IncludeSubdirectories = false,
                    Path = i
                };

                j.Deleted += OnParentItemDeleted;
                j.Renamed += OnParentItemRenamed;
                parentWatchers.Add(j);
                j.EnableRaisingEvents = true;
            }
        }

        ///..................................................................................................

        public void Stop()
        {
            if (!Enable)
                return;

            Enable = false;
            WatchParents(false);

            Stopped?.Invoke(this, new EventArgs());
        }

        public void Start(string path)
        {
            if (Enable)
                Stop();

            Path = path;
            Enable = true;

            if (IncludeParents)
                WatchParents(true);

            Started?.Invoke(this, new EventArgs());
        }
    }
}