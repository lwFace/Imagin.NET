using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using Imagin.Common.Text;
using Imagin.Common.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Vault
{
    [DisplayName(nameof(Task))]
    [Serializable]
    [XmlType(TypeName = nameof(Task))]
    public class CopyTask : Base, ILockable
    {
        #region Enums

        enum Category
        {
            Action,
            Files,
            Folders,
            Location,
            Status
        }

        public enum Statuses
        {
            Disabled,
            Synchronizing,
            Synchronized
        }

        #endregion

        #region Events

        [field: NonSerialized]
        public event EventHandler<EventArgs> Locked;

        public delegate void SynchronizedEventHandler();

        [field: NonSerialized]
        public event SynchronizedEventHandler Synchronized;

        [field: NonSerialized]
        public event EventHandler<EventArgs> Unlocked;

        #endregion

        #region Properties

        [field: NonSerialized]
        CancellableTask task;

        [field: NonSerialized]
        Log log;
        [Hidden]
        [XmlIgnore]
        public Log Log
        {
            get
            {
                if (log != null)
                    return log;

                log = new Log(this);
                return log;
            }
        }

        [field: NonSerialized]
        Queue queue;
        [Hidden]
        [XmlIgnore]
        public Queue Queue
        {
            get
            {
                if (queue != null)
                    return queue;

                queue = new Queue(this);
                return queue;
            }
        }

        [field: NonSerialized]
        Watcher watcher;
        [XmlIgnore]
        Watcher Watcher
        {
            get
            {
                if (watcher != null)
                    return watcher;

                watcher = new Watcher()
                {
                    IncludeParents = true,
                    IncludeSubfolders = true,
                    NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Security
                };

                watcher.Changed += OnItemChanged;
                watcher.Created += OnItemCreated;
                watcher.Deleted += OnItemDeleted;
                watcher.Renamed += OnItemRenamed;

                watcher.Failed += OnWatchingFailed;
                return watcher;
            }
        }

        [field: NonSerialized]
        bool enable = false;
        [Category(nameof(Category.Action))]
        [Description("Whether or not to enable the task.")]
        [Featured]
        [XmlIgnore]
        public bool Enable
        {
            get => enable;
            set
            {
                //If the task is already enabled
                if (enable)
                {
                    //If the task should be disabled
                    if (!value)
                    {
                        OnDisabled();
                    }
                }                
                //If the task is already disabled
                else
                {
                    //If the task should be enabled
                    if (value)
                    {
                        OnEnabled();
                    }
                }
            }
        }

        bool enabled = false;
        [Hidden]
        public bool Enabled
        {
            get => enabled;
            set => this.Change(ref enabled, value);
        }

        string source = string.Empty;
        [Category(nameof(Category.Location))]
        [Description("The path to the folder where stuff should be written from.")]
        [Locked]
        [StringFormat(StringFormat.FolderPath)]
        public string Source
        {
            get => source;
            set => this.Change(ref source, value);
        }

        string destination = string.Empty;
        [Category(nameof(Category.Location))]
        [Description("The path to the folder where stuff should be written to.")]
        [Locked]
        [StringFormat(StringFormat.FolderPath)]
        [Validate(typeof(DestinationValidateHandler))]
        public string Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
                OnPropertyChanged("Destination");
            }
        }

        [Hidden]
        [XmlIgnore]
        public IValidate DestinationValidateHandler => new DestinationValidateHandler();
        
        bool logs = true;
        [Description("Whether or not messages are logged.")]
        [Locked]
        public bool Logs
        {
            get => logs;
            set => this.Change(ref logs, value);
        }

        Converter.Action action = new Converter.Action();
        [Category(nameof(Category.Action))]
        [Description("How you want to encrypt or decrypt stuff.")]
        [Locked]
        public Converter.Action Action
        {
            get => action;
            set => this.Change(ref action, value);
        }

        [field: NonSerialized]
        bool isLocked = false;
        [Hidden]
        [XmlIgnore]
        public bool IsLocked
        {
            get => isLocked;
            set
            {
                this.Change(ref isLocked, value);
                action.IsLocked = value;

                if (value)
                {
                    OnLocked();
                }
                else OnUnlocked();
            }
        }

        Attributes folderAttributes = Attributes.All;
        [Category(nameof(Category.Folders))]
        [Description("Folder attributes", "Whether or not to include or exclude folders with the given attributes.")]
        [DisplayName("Attributes")]
        [EnumFormat(EnumFormat.Flags)]
        [Locked]
        public Attributes FolderAttributes
        {
            get => folderAttributes;
            set => this.Change(ref folderAttributes, value);
        }

        Attributes fileAttributes = Attributes.All;
        [Category(nameof(Category.Files))]
        [Description("File attributes", "Whether or not to include or exclude files with the given attributes.")]
        [DisplayName("Attributes")]
        [EnumFormat(EnumFormat.Flags)]
        [Locked]
        public Attributes FileAttributes
        {
            get => fileAttributes;
            set => this.Change(ref fileAttributes, value);
        }

        FileExtensions fileExtensions = new FileExtensions();
        [Category(nameof(Category.Files))]
        [Description("File extensions", "The file extensions to include or exclude.")]
        [DisplayName("Extensions")]
        [Locked]
        public FileExtensions FileExtensions
        {
            get => fileExtensions;
            set => this.Change(ref fileExtensions, value);
        }

        OverwriteCondition overwriteFiles = OverwriteCondition.Always;
        [Category(nameof(Category.Action))]
        [Description("When files should be overwritten.")]
        [DisplayName("Overwrite files")]
        [Locked]
        public OverwriteCondition OverwriteFiles
        {
            get => overwriteFiles;
            set => this.Change(ref overwriteFiles, value);
        }

        DateTime lastActive = DateTime.Now;
        [Category(nameof(Category.Status))]
        [DateTimeFormat(DateTimeFormat.Relative)]
        [Description("When the task was last active.")]
        [DisplayName("Last active")]
        [ReadOnly]
        public DateTime LastActive
        {
            get => lastActive;
            set => this.Change(ref lastActive, value);
        }

        [field: NonSerialized]
        Statuses status = Statuses.Disabled;
        [Category(nameof(Category.Status))]
        [Description("The status of the task.")]
        [ReadOnly]
        [XmlIgnore]
        public Statuses Status
        {
            get => status;
            set => this.Change(ref status, value);
        }

        #endregion

        #region CopyTask

        public CopyTask() : base() { }

        #endregion

        #region Methods

        /// <summary>
        /// Occurs when the <see cref="CopyTask"/> should be enabled.
        /// </summary>
        async void OnEnabled()
        {
            var doubleCheck = Dialog.Show(nameof(Enable), $"Are you sure '{destination}' is the intended destination? Double check to avoid any unintentional loss of files or folders.", DialogImage.Warning, DialogButtons.ContinueCancel);
            if (doubleCheck == 1)
                goto Stop;

            var result = Try(() =>
            {
                if (Destination.StartsWith(Source))
                    throw new InvalidDataException($"'{nameof(Destination)}' cannot equal (or derive from) '{nameof(Source)}'.");

                if (!Folder.Long.Exists(Destination))
                    throw new DirectoryNotFoundException($"'{nameof(Destination)}' does not exist.");

                if (!DestinationValidateHandler.Validate(ExplorerWindow.Modes.OpenFolder, Destination))
                    throw new InvalidDataException($"'{nameof(Destination)}' must start with (but not equal) '{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}'.");

                if
                (
                    action.Target.HasAny
                    (
                        Converter.Action.Targets.FileContents,
                        Converter.Action.Targets.FileNames,
                        Converter.Action.Targets.FolderNames
                    )
                    &&
                    action.Password.NullOrEmpty()
                )
                {
                    throw new InvalidDataException($"{nameof(Action.Password)} not specified for {(action.Type.ToString().ToLower())}ion.");
                }
            });

            if (!result)
                goto Stop;

            try
            {
                Watcher.Start(Source);
            }
            catch (Exception e)
            {
                log.Write(Log.Types.Error, $"Watching failed: {e.Message}.");
                goto Stop;
            }

            this.Change(ref enable, true, () => Enable);
            IsLocked = true;

            log.Write(Log.Types.Enable, $"Enabled");
            log.Write(Log.Types.Watch, $"Watching");

            Status = Statuses.Synchronizing;

            task = new CancellableTask(token => Try(() => Synchronize(Source, token)));
            await task.Invoke();

            Status = Statuses.Synchronized;
            task = null;

            Synchronized?.Invoke();
            return;

            Stop: this.Changed(() => Enable);
        }

        /// <summary>
        /// Occurs when the <see cref="CopyTask"/> should be disabled.
        /// </summary>
        void OnDisabled()
        {
            if (Status == Statuses.Synchronizing)
            {
                Synchronized -= OnSynchronized;
                Synchronized += OnSynchronized;
                task.Complete();
                return;
            }

            Queue.Clear();

            Status = Statuses.Disabled;
            Watcher.Stop();

            log.Write(Log.Types.Disable, $"Disabled");

            IsLocked = false;
            this.Change(ref enable, false, () => Enable);
        }

        ///..................................................................................................

        void Synchronize(string source, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;

            if (!Directory.Exists(source))
                throw new DirectoryNotFoundException($"'{source}' does not exist.");

            log.Write(Log.Types.Synchronize, source);

            var result = default(Result);
            List<string> sItems = new List<string>(), dItems = new List<string>();

            result = Try(() => sItems = Imagin.Common.Storage.Folder.Long.GetItems(source).ToList());

            if (!result)
                return;

            var destination = source.Replace(Source, Destination);
            destination = Converter.FormatUri(Destination, destination.Replace(Destination, string.Empty), ItemType.Folder, action);

            result = Try(false, () => dItems = Folder.Long.GetItems(destination).ToList());
            foreach (var i in sItems)
            {
                if (token.IsCancellationRequested)
                    return;

                ItemType itemType = default;
                result = Try(() => itemType = Type(i));

                var oldDestination = i.Replace(Source, Destination);
                var newDestination = Converter.FormatUri(Destination, oldDestination.Replace(Destination, string.Empty), itemType, action);

                if (result)
                {
                    if (itemType == ItemType.Folder)
                    {
                        if (Writable(i, newDestination))
                        {
                            Queue.Add(Operation.Types.Create, itemType, i, newDestination, t => Try(() => CreateFolder(newDestination)));
                            Synchronize(i, token);
                            goto Remove;
                        }
                    }
                    else if (itemType == ItemType.File)
                    {
                        if (Writable(i, newDestination))
                        {
                            Queue.Add(Operation.Types.Create, itemType, i, newDestination, t => Try(() => CreateFile("Creating", i, newDestination, t)));
                        }
                        goto Remove;
                    }
                }

                continue;
                Remove: dItems.Remove(newDestination);
            }

            if (token.IsCancellationRequested)
                return;

            if (dItems.Count > 0)
            {
                dItems.ForEach<string>(i =>
                {
                    log.Write(Log.Types.Delete, i);
                    Delete(i);
                });
            }
        }

        ///..................................................................................................

        void OnWatchingFailed(object sender, EventArgs<Error> e)
        {
            log.Write(Log.Types.Error, $"Watching failed: The source folder (or one of it's parents) has moved or was deleted.");
            Enable = false;
        }

        void OnSynchronized()
        {
            Synchronized -= OnSynchronized;
            OnDisabled();
        }

        ///..................................................................................................

        void CreateFile(string prefix, string source, string destination, CancellationToken token)
        {
            log.Write(Log.Types.Create, destination);

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            Try(() =>
            {
                var callback = new Converter.Callback((sizeRead, size) =>
                {
                    Queue.Current.Duration = stopWatch.Elapsed;
                    Queue.Current.SizeRead = sizeRead.Int64();
                    Queue.Current.Progress = sizeRead / size;
                    return token.IsCancellationRequested;
                });
                if (action.Target.HasAll(Converter.Action.Targets.FileContents))
                {
                    if (action.Type == Converter.Action.Types.Decrypt)
                        Converter.DecryptFile(source, destination, action.Password, action.Algorithm, action.Encoding, true, callback);

                    if (action.Type == Converter.Action.Types.Encrypt)
                        Converter.EncryptFile(source, destination, action.Password, action.Algorithm, action.Encoding, true, callback);
                }
                else Converter.CopyFile(source, destination, action.Encoding, true, callback);
            });

            stopWatch.Stop();
            Queue.Remove(Queue.Current);
        }

        void CreateFolder(string destination)
        {
            log.Write(Log.Types.Create, destination);
            Imagin.Common.Storage.Folder.Long.Create(destination);
        }

        ///..................................................................................................

        void Delete(string i)
        {
            Try(() =>
            {
                ItemType itemType = Type(i);
                switch (itemType)
                {
                    case ItemType.File:
                        Imagin.Common.Storage.File.Long.Delete(i);
                        break;

                    case ItemType.Folder:
                        Imagin.Common.Storage.Folder.Long.Delete(i, true);
                        break;
                }
            });
        }

        ///..................................................................................................

        public Result Try(Action action)
        {
            return Try(true, action);
        }

        public Result Try(bool log, Action action)
        {
            try
            {
                action();
                return new Success();
            }
            catch (Exception e)
            {
                var error = new Error(e);

                if (log)
                    Log.Write(error);

                return error;
            }
        }

        //---------------------------------------------------------------------------

        bool DoAttributes(string source)
        {
            FileAttributes attributes = Imagin.Common.Storage.File.Long.Attributes(this.source);
            var h = attributes.HasAny(System.IO.FileAttributes.Hidden);
            var r = attributes.HasAny(System.IO.FileAttributes.ReadOnly);

            var type = Type(source);
            switch (type)
            {
                case ItemType.File:
                    if (!FileAttributes.HasAny(Attributes.Hidden))
                    {
                        if (h)
                        {
                            return false;
                        }
                    }
                    if (!FileAttributes.HasAny(Attributes.ReadOnly))
                    {
                        if (r)
                        {
                            return false;
                        }
                    }
                    break;

                case ItemType.Folder:
                    if (!FolderAttributes.HasAny(Attributes.Hidden))
                    {
                        if (h)
                        {
                            return false;
                        }
                    }
                    if (!FolderAttributes.HasAny(Attributes.ReadOnly))
                    {
                        if (r)
                        {
                            return false;
                        }
                    }
                    break;
            }
            return true;
        }

        bool DoExtensions(string source)
        {
            if (FileExtensions == null || FileExtensions.Extensions.NullOrEmpty())
                return true;

            if (FileExtensions.Extensions.Contains(Imagin.Common.Storage.Path.GetExtension(source).Replace(".", string.Empty)))
                return FileExtensions.Filter == Imagin.Common.Data.Filter.Include;

            return false;
        }

        bool DoOverwrite(string source, string destination)
        {
            if (overwriteFiles == OverwriteCondition.Always)
                return true;

            try
            {
                var a = new FileInfo(source);
                var b = new FileInfo(destination);

                switch (overwriteFiles)
                {
                    case OverwriteCondition.IfNewer:
                        return a.LastWriteTime > b.LastWriteTime;

                    case OverwriteCondition.IfNewerOrSizeDifferent:
                        return (a.LastWriteTime > b.LastWriteTime) || (a.Length != b.Length);

                    case OverwriteCondition.IfSizeDifferent:
                        return a.Length != b.Length;
                }

                return false;
            }
            catch
            {
                return true;
            }
        }

        //---------------------------------------------------------------------------

        static ItemType Type(string path)
        {
            if (Imagin.Common.Storage.Folder.Long.Exists(path))
                return ItemType.Folder;

            if (Imagin.Common.Storage.File.Long.Exists(path))
                return ItemType.File;

            throw new NotSupportedException();
        }

        bool Writable(string source, string destination)
        {
            bool r()
            {
                if (Imagin.Common.Storage.File.Long.Exists(source))
                {
                    return DoAttributes(source) && DoExtensions(source) && DoOverwrite(source, destination);
                }
                else if (Imagin.Common.Storage.Folder.Long.Exists(source))
                    return DoAttributes(source);

                return false;
            }

            var result = r();
            if (!result)
            {
                log.Write(Log.Types.Skip, source);
            }
            return result;
        }

        //---------------------------------------------------------------------------

        void OnItemChanged(object sender, FileSystemEventArgs e)
        {
            var source = e.FullPath;
            var itemType = Type(source);

            if (itemType == ItemType.File)
            {
                var oldDestination = source.Replace(Source, Destination);
                var newDestination = Converter.FormatUri(Destination, oldDestination.Replace(Destination, string.Empty), ItemType.File, action);

                if (Writable(source, newDestination))
                {
                    Queue.Add(Operation.Types.Create, ItemType.File, source, newDestination, token => Try(() => CreateFile("Updated", source, newDestination, token)));
                }
            }
        }

        void OnItemCreated(object sender, FileSystemEventArgs e)
        {
            var source = e.FullPath;
            var itemType = Type(source);

            var oldDestination = source.Replace(Source, Destination);
            var newDestination = Converter.FormatUri(Destination, oldDestination.Replace(Destination, string.Empty), itemType, action);

            if (Writable(source, newDestination))
            {
                if (itemType == ItemType.File)
                {
                    Queue.Add(Operation.Types.Create, itemType, source, newDestination, token => Try(() => CreateFile("Created", source, newDestination, token)));
                }
                else if (itemType == ItemType.Folder)
                    Queue.Add(Operation.Types.Create, itemType, source, newDestination, token => Try(() => CreateFolder(newDestination)));
            }
        }

        void OnItemDeleted(object sender, FileSystemEventArgs e)
        {
            var head = Destination;
            var tail = e.FullPath.Replace(Source, Destination).Replace(Destination, string.Empty);

            var filePath = Converter.FormatUri(head, tail, ItemType.File, action);
            var folderPath = Converter.FormatUri(head, tail, ItemType.Folder, action);

            ItemType? itemType = null;

            var result = Try(() => itemType = Type(filePath));
            result = !result ? Try(() => itemType = Type(folderPath)) : result;

            if (itemType == ItemType.File)
            {
                Queue.Add(Operation.Types.Delete, itemType.Value, filePath, string.Empty, token => { /*Imagin.Common.Storage.File.Long.Delete(filePath)*/ });
            }
            else if (itemType == ItemType.Folder)
                Queue.Add(Operation.Types.Delete, itemType.Value, folderPath, string.Empty, token => { /*Imagin.Common.Storage.Folder.Long.Delete(folderPath, true)*/ });
        }

        void OnItemRenamed(object sender, RenamedEventArgs e)
        {
            var source = e.FullPath;
            var destination = e.FullPath.Replace(Source, Destination);

            var oldPath = e.OldFullPath;
            var newPath = e.FullPath;

            string a = string.Empty, b = string.Empty;
            //We know this will always be in destination scope
            a = oldPath.Replace(Source, Destination);
            //We won't know that this will always be in destination scope
            b = newPath.Replace(Source, Destination);

            var aTail = a.Replace(Destination, string.Empty);
            var bTail = b.Replace(Destination, string.Empty);

            if (b.StartsWith(Destination))
            {
                var itemType = Type(newPath);
                if (itemType == ItemType.Folder)
                {
                    var dA = Converter.FormatUri(Destination, aTail, ItemType.Folder, action);
                    var dB = Converter.FormatUri(Destination, bTail, ItemType.Folder, action);
                    Queue.Add(Operation.Types.Move, itemType, dA, dB, token => Try(() => Imagin.Common.Storage.Folder.Long.Move(dA, dB)));
                }
                else if (itemType == ItemType.File)
                {
                    var dA = Converter.FormatUri(Destination, aTail, ItemType.File, action);
                    var dB = Converter.FormatUri(Destination, bTail, ItemType.File, action);
                    Queue.Add(Operation.Types.Move, itemType, dA, dB, token => Try(() => Imagin.Common.Storage.File.Long.Move(dA, dB)));
                }
            }
            //Delete object at path, a: Since the original object has moved outside scope of destination folder, it must no longer be present there!
            else
            {
                var dA = Converter.FormatUri(Destination, aTail, Type(a), action);
                Queue.Add(Operation.Types.Delete, Type(dA), dA, string.Empty, token => Try(() => { /*Delete(dA)*/ }));
            }
        }

        //---------------------------------------------------------------------------

        [field: NonSerialized]
        ICommand deleteCommand;
        [Hidden]
        [XmlIgnore]
        public ICommand DeleteCommand
        {
            get
            {
                deleteCommand = deleteCommand ?? new RelayCommand(() =>
                {
                    var result = Dialog.Show("Delete", "Are you sure you want to delete this?", DialogImage.Warning, DialogButtons.YesNo);
                    if (result == 0)
                        Get.Current<Options>().Tasks.Remove(this);

                }, () => true);
                return deleteCommand;
            }
        }

        #region Overrides

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Enable):
                    Enabled = enable;
                    break;
            }
            Get.Current<Options>().Save();
        }

        public override string ToString() => nameof(Task);

        #endregion

        #region Virtual

        protected virtual void OnLocked()
        {
            Locked?.Invoke(this, new EventArgs());
        }

        protected virtual void OnUnlocked()
        {
            Unlocked?.Invoke(this, new EventArgs());
        }

        #endregion

        /*
        async Task TryCompressAsync()
        {
            await Task.Run(() =>
            {
                var outName = compressionOptions.OutputName;

                if (outName.IsNullOrEmpty())
                    outName = Source.GetFileName();

                var outExtension = string.Empty;
                switch (compressionOptions.Type)
                {
                    case CompressionFormat.BZip2:
                        outExtension = "tar.bz2";
                        break;
                    case CompressionFormat.GZip:
                        outExtension = "tar.gz";
                        break;
                    case CompressionFormat.Tar:
                        outExtension = "tar";
                        break;
                    case CompressionFormat.Zip:
                        outExtension = "zip";
                        break;
                }

                var outPath = @"{0}\{1}.{2}".F(Destination, outName, outExtension);

                try
                {
                    var fileStream = Imagin.Common.Storage.File.Long.Create(outPath);

                    var stream = default(Stream);
                    switch (compressionOptions.Type)
                    {
                        case CompressionFormat.BZip2:
                            stream = new BZip2OutputStream(fileStream);
                            break;
                        case CompressionFormat.GZip:
                            stream = new GZipOutputStream(fileStream);
                            break;
                        case CompressionFormat.Tar:
                            stream = new TarOutputStream(fileStream);
                            break;
                        case CompressionFormat.Zip:
                            stream = new ZipOutputStream(fileStream);

                            var zipStream = stream as ZipOutputStream;
                            zipStream.SetLevel(compressionOptions.Level);
                            zipStream.Password = compressionOptions.Password;
                            break;
                    }

                    switch (compressionOptions.Type)
                    {
                        case CompressionFormat.BZip2:
                        case CompressionFormat.GZip:
                        case CompressionFormat.Tar:
                            var tarArchive = TarArchive.CreateOutputTarArchive(stream);

                            tarArchive.RootPath = Destination.Replace('\\', '/');
                            if (tarArchive.RootPath.EndsWith("/"))
                                tarArchive.RootPath = tarArchive.RootPath.Remove(tarArchive.RootPath.Length - 1);

                            var tarEntry = TarEntry.CreateEntryFromFile(Destination);
                            tarArchive.WriteEntry(tarEntry, false);

                            CompressTar(tarArchive, Destination, outPath);
                            break;
                        case CompressionFormat.Zip:
                            int folderOffset = Destination.Length + (Destination.EndsWith("\\") ? 0 : 1);
                            CompressZip(stream as ZipOutputStream, Destination, folderOffset, outPath);
                            break;
                    }

                    switch (compressionOptions.Type)
                    {
                        case CompressionFormat.BZip2:
                            var bZip2Stream = stream as BZip2OutputStream;
                            bZip2Stream.IsStreamOwner = true;
                            bZip2Stream.Close();
                            break;
                        case CompressionFormat.GZip:
                            var gZipStream = stream as GZipOutputStream;
                            gZipStream.IsStreamOwner = true;
                            gZipStream.Close();
                            break;
                        case CompressionFormat.Tar:
                            var tarStream = stream as TarOutputStream;
                            tarStream.IsStreamOwner = true;
                            tarStream.Close();
                            break;
                        case CompressionFormat.Zip:
                            var zipStream = stream as ZipOutputStream;
                            zipStream.IsStreamOwner = true;
                            zipStream.Close();
                            break;
                    }

                    fileStream?.Close();
                    fileStream?.Dispose();
                }
                catch
                {

                }
            });
        }

        void CompressFile(ZipOutputStream ZipStream, string Path, int FolderOffset)
        {
            var FileInfo = new FileInfo(Path);

            //Makes the name in zip based on the folder
            var EntryName = Path.Substring(FolderOffset);
            //Removes drive from name and fixes slash direction 
            EntryName = ZipEntry.CleanName(EntryName);

            var NewEntry = new ZipEntry(EntryName);
            NewEntry.DateTime = FileInfo.LastWriteTime;
            NewEntry.Size = FileInfo.Length;

            ZipStream.PutNextEntry(NewEntry);

            var Buffer = new byte[4096];
            using (var StreamReader = Imagin.Common.Storage.File.Long.OpenRead(Path))
                StreamUtils.Copy(StreamReader, ZipStream, Buffer);

            ZipStream.CloseEntry();
        }

        void CompressFile(TarArchive Archive, string Path)
        {
            var tarEntry = TarEntry.CreateEntryFromFile(Path);
            Archive.WriteEntry(tarEntry, true);
        }

        void CompressTar(TarArchive archive, string folderPath, string outPath)
        {
            var Files = Enumerable.Empty<string>();
            try
            {
                Files = Folder.GetFiles(folderPath);
            }
            catch (Exception e)
            {
                Write(e.Message, LogEntryType.Error);
            }

            foreach (var i in Files)
            {
                if (i != outPath)
                    CompressFile(archive, i);
            }

            var Folders = Enumerable.Empty<string>();
            try
            {
                Folders = Folder.GetFolders(Destination);
            }
            catch (Exception e)
            {
                Write(e.Message, LogEntryType.Error);
            }

            foreach (var i in Folders)
                CompressTar(archive, i, outPath);
        }

        void CompressZip(ZipOutputStream stream, string folderPath, int folderOffset, string outPath)
        {
            var Files = Enumerable.Empty<string>();
            try
            {
                Files = Folder.GetFiles(folderPath);
            }
            catch (Exception e)
            {
                Write(e.Message, LogEntryType.Error);
            }

            foreach (var i in Files)
            {
                if (i != outPath)
                {
                    try
                    {
                        CompressFile(stream, i, folderOffset);
                        Imagin.Common.Storage.File.Long.Delete(i);
                    }
                    catch (Exception e)
                    {
                        Write(e.Message, LogEntryType.Error);
                    }
                }
            }

            var Folders = Enumerable.Empty<string>();
            try
            {
                Folders = Folder.GetFolders(Destination);
            }
            catch (Exception e)
            {
                Write(e.Message, LogEntryType.Error);
            }

            foreach (var i in Folders)
            {
                CompressZip(stream, i, folderOffset, outPath);

                try
                {
                    Folder.Delete(i);
                }
                catch (Exception e)
                {
                    Write(e.Message, LogEntryType.Error);
                }
            }
        }
        */

        /*
        static string GetExtension(string Path, TaskAction Options)
        {
            var Result = string.Empty;
            switch (Options.Action)
            {
                case SecurityAction.Decrypt:
                    var FileName = Path.GetFileName();
                    FileName = Cryptography.EscapeUri(FileName, SecurityAction.Decrypt);
                    FileName = Cryptography.DecryptText(FileName, Options.Password, Options.Algorithm);

                    Result = FileName.GetExtension(true);
                    break;
                case SecurityAction.Encrypt:
                    Result = Path.GetExtension(true);
                    break;
            }
            return Result;
        }
        */

        #endregion
    }
}