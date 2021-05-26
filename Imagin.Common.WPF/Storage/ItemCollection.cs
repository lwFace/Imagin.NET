using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Imagin.Common.Storage
{
    public class ItemCollection : ConcurrentCollection<Item>
    {
        class Automator : List<DispatcherOperation>, IPropertyChanged
        {
            [field: NonSerialized]
            public event PropertyChangedEventHandler PropertyChanged;

            readonly ItemCollection items;

            public Automator(ItemCollection items)
            {
                this.items = items;
            }

            void OnAutomated(object sender, EventArgs e)
            {
                var i = (DispatcherOperation)sender;
                i.Completed -= OnAutomated;
                Remove(i);
            }

            new public void Add(DispatcherOperation i)
            {
                base.Add(i);
                i.Completed += OnAutomated;
            }

            public void Abort()
            {
                for (var i = Count - 1; i >= 0; i--)
                {
                    this[i].Completed -= OnAutomated;
                    this[i].Abort();
                    RemoveAt(i);
                }
            }

            public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties

        readonly Automator automator;

        readonly FileSystemWatcher watcher = new FileSystemWatcher()
        {
            NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Security
        };

        CancellationTokenSource token = null;

        string pending = null;

        //----------------------------------------------------------------------------------------

        public event EventHandler<ItemChangedEventArgs> ItemChanged;

        public event EventHandler<ItemCreatedEventArgs> ItemCreated;
        
        public event EventHandler<ItemDeletedEventArgs> ItemDeleted;

        public event EventHandler<ItemRenamedEventArgs> ItemRenamed;
        
        public event EventHandler<EventArgs> Refreshed;

        public event EventHandler<EventArgs> Refreshing;

        Dispatcher Dispatcher => Application.Current.Dispatcher;

        bool isRefreshing = false;
        /// <summary>
        /// Gets whether or not the collection is in the process of refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get => isRefreshing;
            private set => this.Change(ref isRefreshing, value);
        }

        Filter filter;
        public Filter Filter
        {
            get => filter;
            set => filter = value;
        }

        string path = string.Empty;
        public string Path
        {
            get => path;
            private set => this.Change(ref path, value);
        }

        public double Progress => 0;

        #endregion

        #region ItemCollection

        public ItemCollection() : this(string.Empty, null) { }

        public ItemCollection(Filter filter) : this(string.Empty, filter) { }

        public ItemCollection(string path, Filter filter) : base()
        {
            automator = new Automator(this);

            Path = path;
            Filter = filter;
        }

        #endregion

        #region Methods

        #region Commands

        ICommand refreshAsyncCommand;
        public ICommand RefreshAsyncCommand
        {
            get
            {
                refreshAsyncCommand = refreshAsyncCommand ?? new RelayCommand(async () => await Refresh(), () => true);
                return refreshAsyncCommand;
            }
        }

        #endregion

        #region Private

        IEnumerable<Item> Query(string path, Filter filter)
        {
            filter = filter ?? new Filter(ItemType.Drive | ItemType.Folder | ItemType.File);
            if (filter.Types.HasFlag(ItemType.Drive) && (path.NullOrEmpty() || path == Folder.Long.Root))
            {
                foreach (var i in Machine.Drives)
                {
                    var drive = new Drive(i);
                    drive.Refresh();
                    yield return drive;
                }
            }
            if (filter.Types.HasFlag(ItemType.Folder))
            {
                var folders = Enumerable.Empty<string>();
                Try.Invoke(() => folders = Folder.Long.GetFolders(path).Where(j => j != ".."));
                foreach (var i in folders)
                {
                    var folder = new Folder(i);
                    folder.Refresh();
                    yield return folder;
                }
            }
            if (filter.Types.HasFlag(ItemType.File))
            {
                var files = Enumerable.Empty<string>();
                Try.Invoke(() => files = Folder.Long.GetFiles(path));
                foreach (var i in files)
                {
                    if (filter.Evaluate(i, ItemType.File))
                    {
                        File item = Shortcut.Is(i) ? new Shortcut(i) : new File(i);
                        item.Refresh();
                        yield return item;
                    }
                }
            }
        }

        void Refresh(string path, Filter filter, CancellationTokenSource token)
        {
            IEnumerable<Item> items = null;

            try
            {
                items = Query(path, filter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            foreach (var i in items)
            {
                if (token?.IsCancellationRequested == true)
                    return;

                automator.Add(Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Add(i))));
            }
        }

        bool Watch()
        {
            try
            {
                watcher.EnableRaisingEvents = false;
                watcher.Path = Path;

                watcher.Changed -= OnObjectChanged;
                watcher.Created -= OnObjectCreated;
                watcher.Deleted -= OnObjectDeleted;
                watcher.Renamed -= OnObjectRenamed;

                watcher.EnableRaisingEvents = true;
                watcher.Changed += OnObjectChanged;
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

        #endregion

        #region Public

        public async Task Refresh() => await Refresh(Path);

        public async Task Refresh(string path)
        {
            if (IsRefreshing)
            {
                pending = path;
                CancelRefresh();
                return;
            }

            IsRefreshing = true;
            Path = path;

            Watch();

            if (0 < automator.Count)
                automator.Abort();

            if (0 < Count)
                Clear();

            await Task.Run(() =>
            {
                while (0 < automator.Count && 0 < Count) { }
            });

            OnRefreshing();
            var filter = Filter;

            token = new CancellationTokenSource();
            var t = token;

            await Task.Run(() => Refresh(path, filter, t), t.Token);
            OnRefreshed();

            IsRefreshing = false;

            if (pending != null)
            {
                var p = pending;
                pending = null;
                await Refresh(p);
            }
        }

        //----------------------------------------------------------------------------------------

        public void CancelRefresh()
        {
            //Automations may exist regardless if currently refreshing due to asynchronous nature
            automator.Abort();
            token?.Cancel();
        }

        #endregion

        #region Virtual

        protected virtual void OnRefreshing()
        {
            Refreshing?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnRefreshed()
        {
            Refreshed?.Invoke(this, EventArgs.Empty);
        }

        //----------------------------------------------------------------------------------------

        protected virtual void OnObjectChanged(object sender, FileSystemEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (FileSystemEventHandler)OnObjectChanged, sender, e);
                return;
            }

            var item = this.FirstOrDefault(i => i.Path == e.FullPath);
            if (item != null)
            {
                //Get old properties
                var accessed
                    = item.Accessed;
                var created
                    = item.Created;
                var modified
                    = item.Modified;
                var size
                    = item.Size;

                //Refresh the properties
                item.Refresh();

                //Find out what properties changed
                var itemProperties = default(ItemProperty);
                if (accessed != item.Accessed)
                    itemProperties |= ItemProperty.Accessed;

                if (created != item.Created)
                    itemProperties |= ItemProperty.Created;

                if (modified != item.Modified)
                    itemProperties |= ItemProperty.Modified;

                if (size != item.Size)
                    itemProperties |= ItemProperty.Size;

                ItemChanged?.Invoke(this, new ItemChangedEventArgs(item, itemProperties));
            }
        }

        protected virtual void OnObjectCreated(object sender, FileSystemEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (FileSystemEventHandler)OnObjectCreated, sender, e);
                return;
            }

            var i = default(Item);
            if (File.Long.Exists(e.FullPath) && Filter?.Evaluate(e.FullPath, ItemType.File) != false)
            {
                i = new File(e.FullPath);
            }
            else if (Folder.Long.Exists(e.FullPath) && Filter?.Evaluate(e.FullPath, ItemType.Folder) != false)
                i = new Folder(e.FullPath);

            if (i != null)
            {
                ItemCreated?.Invoke(this, new ItemCreatedEventArgs(i));

                i.Refresh();
                Add(i);
            }
        }

        protected virtual void OnObjectDeleted(object sender, FileSystemEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (FileSystemEventHandler)OnObjectDeleted, sender, e);
                return;
            }

            var item = this.FirstOrDefault(i => i.Path == e.FullPath);
            if (item != null)
            {
                ItemDeleted?.Invoke(this, new ItemDeletedEventArgs(item.Path));
                Remove(item);
            }
        }

        protected virtual void OnObjectRenamed(object sender, RenamedEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (RenamedEventHandler)OnObjectRenamed, sender, e);
                return;
            }

            ItemRenamed?.Invoke(this, new ItemRenamedEventArgs(e.OldFullPath, e.FullPath));
            this.FirstOrDefault(i => i.Path == e.OldFullPath)?.Refresh(e.FullPath);
        }

        #endregion

        #endregion
    }
}