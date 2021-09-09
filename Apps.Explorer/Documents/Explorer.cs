using Imagin.Common;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Models;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;

namespace Explorer
{
    [Serializable]
    public class ExplorerDocument : Document
    {
        public delegate void ChangedEventHandler(ExplorerDocument sender);

        [field: NonSerialized]
        public event ChangedEventHandler PathChanged;

        [field: NonSerialized]
        public event ChangedEventHandler SelectionChanged;

        History history = new History(Imagin.Common.Controls.Explorer.DefaultLimit);
        [Hidden]
        public History History
        {
            get => history;
            set => this.Change(ref history, value);
        }

        string path;
        public string Path
        {
            get => path;
            set
            {
                this.Change(ref path, value);

                Get.Current<MainViewModel>().Changed(() => Get.Current<MainViewModel>().Title);
                this.Changed(() => Title);
                this.Changed(() => ToolTip);

                PathChanged?.Invoke(this);
            }
        }

        [field: NonSerialized]
        IList<Item> selection = null;
        public IList<Item> Selection
        {
            get => selection;
            set
            {
                this.Change(ref selection, value);
                SelectionChanged?.Invoke(this);
            }
        }

        public override string Title => Machine.FriendlyName(path);

        public override object ToolTip => path;

        public ExplorerDocument() : base()
        {
            Path = Get.Current<Options>().DefaultFolderPath;
        }

        public ExplorerDocument(string path) : base()
        {
            Path = path;
        }

        public override void Save() { }
    }
}