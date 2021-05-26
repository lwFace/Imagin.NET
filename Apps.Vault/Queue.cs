using Imagin.Common;
using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using Imagin.Common.Threading;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Vault
{
    public class Operation : CancellableTask
    {
        public enum Types
        {
            Create,
            Delete,
            Move
        }

        public enum Statuses
        {
            Active,
            Inactive
        }

        Types type;
        public Types Type
        {
            get => type;
            set => this.Change(ref type, value);
        }

        string source;
        public string Source
        {
            get => source;
            set => this.Change(ref source, value);
        }

        string destination;
        public string Destination
        {
            get => destination;
            set => this.Change(ref destination, value);
        }

        long sizeRead;
        public long SizeRead
        {
            get => sizeRead;
            set
            {
                this.Change(ref sizeRead, value);
                this.Changed(() => Speed);
            }
        }

        long size;
        public long Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        public double Speed
            => Duration.TotalSeconds == 0 ? 0 : sizeRead.Double() / Duration.TotalSeconds;

        double progress;
        public double Progress
        {
            get => progress;
            set => this.Change(ref progress, value);
        }

        TimeSpan duration = TimeSpan.Zero;
        public TimeSpan Duration
        {
            get => duration;
            set
            {
                this.Change(ref duration, TimeSpan.FromSeconds(value.TotalSeconds.Round()));
                this.Changed(() => Speed);
            }
        }

        Statuses status = Statuses.Inactive;
        public Statuses Status
        {
            get => status;
            set => this.Change(ref status, value);
        }

        ItemType itemType;
        public ItemType ItemType
        {
            get => itemType;
            set => this.Change(ref itemType, value);
        }

        public Operation(Types type, ItemType itemType, string source, string destination, CancellableDelegate action) : base(action)
        {
            Type = type;
            ItemType = itemType;
            Source = source;

            if (ItemType == ItemType.File)
            {
                var fileInfo = new System.IO.FileInfo(source);
                Size = fileInfo.Length;
            }

            Destination = destination;
        }

        new public async Task Invoke()
        {
            Status = Statuses.Active;
            await base.Invoke();
            Status = Statuses.Inactive;
        }
    }

    public class Queue : ConcurrentCollection<Operation>
    {
        readonly CopyTask Task;

        public Operation Current { get; private set; }

        public Queue(CopyTask task)
        {
            Task = task;
        }

        async void Assign(Operation operation)
        {
            Current = operation;
            await Current.Invoke();

            Remove(Current);
            Current = null;

            await Application.Current.Dispatcher.BeginInvoke(() => Task.LastActive = DateTime.Now);
            Scan();
        }

        void Scan()
        {
            if (Current == null)
            {
                if (Count > 0)
                {
                    Assign(this[0]);
                }
            }
        }

        protected override void OnAdded(Operation Item)
        {
            base.OnAdded(Item);
            Scan();
        }

        public void Add(Operation.Types type, ItemType itemType, string source, string destination, CancellableDelegate action)
        {
            Add(new Operation(type, itemType, source, destination, action));
        }

        new public void Clear()
        {
            foreach (var i in this)
                i.Complete();

            base.Clear();
        }
    }
}