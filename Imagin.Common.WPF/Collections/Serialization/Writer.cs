using Imagin.Common.Analytics;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Imagin.Common.Collections.Serialization
{
    public abstract class Writer<T> : Generic.Collection<T>, ILimit, ISerialize
    {
        public string FilePath { get; private set; }

        Limit limit = default;
        public Limit Limit
        {
            get => limit;
            set
            {
                limit = value;
                limit.Coerce(this);
            }
        }

        /// <summary>
        /// Unreadable files that get preserved avoids unintentional losses. This is only useful if the file contains XML.
        /// </summary>
        public bool Preserve { get; set; } = true;

        //..............................................................................................

        public Writer(string filePath, Limit limit) : base()
        {
            FilePath = filePath;
            Limit = limit;
        }

        //..............................................................................................

        protected override void OnAdded(T i)
        {
            base.OnAdded(i);
            limit.Coerce(this);
        }

        protected override void OnInserted(T i, int index)
        {
            base.OnAdded(i);
            limit.Coerce(this);
        }

        //..............................................................................................

        public abstract Result Deserialize(string filePath, out object result);

        public Result Deserialize(string filePath, out IEnumerable<T> items)
        {
            items = null;

            var result = Deserialize(filePath, out object i);
            if (i is IEnumerable<T> j)
            {
                items = j;
                return new Success();
            }

            return result;
        }

        //..............................................................................................

        public Result Serialize(object input) => Serialize(FilePath, input);

        public abstract Result Serialize(string filePath, object input);

        //..............................................................................................

        public Result Load()
        {
            var result = Deserialize(FilePath, out IEnumerable<T> i);
            result.If(true, () =>
            {
                Clear();
                i.ForEach(j => Add(j));
            });

            //If loading fails (for any reason)
            if (!result)
            {
                //If unreadable files should be preserved
                if (Preserve)
                {
                    //Preserve the unreadable file by renaming it to a "clone" that does not yet exist
                    if (Storage.File.Long.Exists(FilePath))
                        Try.Invoke(() => Storage.File.Long.Move(FilePath, Storage.Path.Clone(FilePath, Storage.Path.DefaultCloneFormat, j => Storage.File.Long.Exists(j))));
                }
            }
            return result;
        }

        public Result Save() => Serialize(this);

        //..............................................................................................

        ICommand clearCommand;
        public ICommand ClearCommand => clearCommand = clearCommand ?? new RelayCommand(() => Clear(), () => Count > 0);

        ICommand exportCommand;
        public ICommand ExportCommand => exportCommand = exportCommand ?? new RelayCommand(() => _ = Export(), () => Count > 0);

        ICommand importCommand;
        public ICommand ImportCommand => importCommand = importCommand ?? new RelayCommand(() => Import(), () => true);

        //..............................................................................................

        public async Task<Result> Export() => await Export(this);

        public async Task<Result> Export(T i) => await Export(Array<T>.New(i));

        public async Task<Result> Export(IEnumerable<T> items)
        {
            var path = string.Empty;
            if (ExplorerWindow.Show(out path, nameof(Export), ExplorerWindow.Modes.SaveFile, new [] { "xml" }, FilePath))
                return await Task.Run(() => Serialize(path, items));

            return new Error(new OperationCanceledException());
        }

        //..............................................................................................

        public Result Import()
        {
            var e = 0;

            string[] paths = null;
            if (ExplorerWindow.Show(out paths, nameof(Import), ExplorerWindow.Modes.OpenFile, new[] { "xml" }, FilePath))
            {
                foreach (var i in paths)
                {
                    var result = Deserialize(i, out IEnumerable<T> items);
                    if (result)
                        items.ForEach(j => Add(j));

                    if (!result)
                        e++;
                }
            }

            return e == 0 ? new Success() : new Error(new InvalidResultException()) as Result;
        }
    }
}