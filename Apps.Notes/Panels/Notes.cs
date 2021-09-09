using Imagin.Common;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using Imagin.Common.Storage;
using Imagin.Common.Threading;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Notes
{
    public class NotesPanel : Panel
    {
        public event EventHandler<EventArgs<File>> FileSelected;

        readonly BackgroundQueue queue = new BackgroundQueue();

        public string LastImage;

        public ItemCollection Storage { get; private set; } = new ItemCollection(new Imagin.Common.Storage.Filter(ItemType.File, MainViewModel.FileExtensions.Select(i => i.Value).ToArray()));

        ListCollectionView storageView = null;
        public ListCollectionView StorageView
        {
            get => storageView;
            private set => this.Change(ref storageView, value);
        }

        public override string Title => $"Notes ({Storage.Count})";

        public NotesPanel() : base(Resources.Uri(nameof(Notes), "Images/File.png"))
        {
            Get.Current<Options>().PropertyChanged += OnOptionsChanged;

            Storage.ItemChanged += OnItemChanged;
            Storage.ItemDeleted += OnItemDeleted;
            Storage.ItemRenamed += OnItemRenamed;
            Storage.Changed += OnItemsChanged;
            _ = Storage.RefreshAsync(Get.Current<Options>().Folder);

            StorageView = new ListCollectionView(Storage);
            OnSort();
        }

        async void OnOptionsChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Options.Folder):
                    Get.Current<MainViewModel>().Documents.Clear();
                    await Storage.RefreshAsync(Get.Current<Options>().Folder);
                    break;

                case nameof(Options.SortDirection):
                case nameof(Options.SortName):
                    OnSort();
                    break;
            }
        }

        void OnFileChanged(File file)
        {
            FileSelected?.Invoke(this, new EventArgs<File>(file));
        }

        void OnItemChanged(object sender, ItemChangedEventArgs e)
        {
            if (e.Parameter.HasFlag(ItemProperty.Modified))
                StorageView.Refresh();

            /*
            //The only issue with this is we'll be doing a lot of reading each time a change is made!
            foreach (var i in Get.Current<MainViewModel>().Documents)
            {
                var document = (TextDocument)i;
                if (document.Path == e.Value.Path)
                {
                    queue.Add(async () =>
                    {
                        var result = Imagin.Common.Storage.File.Long.ReadAllText(document.Path, Get.Current<Options>().Encoding.Complement());
                        await App.Current.Dispatcher.BeginInvoke(() => document.Text = result);
                    });
                    return;
                }
            }
            */
        }

        void OnItemDeleted(object sender, ItemDeletedEventArgs e)
        {
            foreach (var i in Get.Current<MainViewModel>().Documents)
            {
                var document = (TextDocument)i;
                if (document.Path == e.Path)
                {
                    Get.Current<MainViewModel>().Documents.Remove(document);
                    return;
                }
            }
        }

        void OnItemRenamed(object sender, ItemRenamedEventArgs e)
        {
            TextDocument result = null;
            foreach (var i in Get.Current<MainViewModel>().Documents)
            {
                var document = (TextDocument)i;
                if (document.Path == e.OldPath)
                {
                    result = document;
                    break;
                }
            }

            if (result == null)
                return;

            var fileExtension = System.IO.Path.GetExtension(result.Path).Replace(".", string.Empty);

            //If different file extension
            if (fileExtension != result.Extension)
            {
                Get.Current<MainViewModel>().Documents.Remove(result);
                foreach (var j in MainViewModel.FileExtensions)
                {
                    //If valid file extension
                    if (fileExtension == j.Value)
                    {
                        //Reopen it
                        Get.Current<MainViewModel>().Open(e.NewPath);
                        return;
                    }
                }
            }
            else
            {
                var newName = System.IO.Path.GetFileNameWithoutExtension(e.NewPath);
                result.Rename(newName);
            }
        }

        void OnItemsChanged(object sender)
        {
            this.Changed(() => Title);
        }

        void OnSort()
        {
            StorageView.SortDescriptions.Clear();
            StorageView.SortDescriptions.Add(new SortDescription($"{Get.Current<Options>().SortName}", Get.Current<Options>().SortDirection == SortDirection.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending));
        }

        ICommand newCommand;
        public ICommand NewCommand
        {
            get
            {
                newCommand = newCommand ?? new RelayCommand<Type>(i =>
                {
                    var fileExtension = MainViewModel.FileExtensions[i];

                    var fileName = File.Long.ClonePath($"{Get.Current<Options>().Folder}\\{TextDocument.DefaultName}.{fileExtension}");
                    fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);

                    var document = TextDocument.Create(fileName, fileExtension, string.Empty);
                    document.Save();

                    if (document is List list)
                        list.Validate();

                    Get.Current<MainViewModel>().Documents.Add(document);
                }, 
                i =>
                {
                    try
                    {
                        return Folder.Long.Exists(Get.Current<Options>().Folder);
                    }
                    catch
                    {
                        return false;
                    }
                });
                return newCommand;
            }
        }

        ICommand selectCommand;
        public ICommand SelectCommand
        {
            get
            {
                selectCommand = selectCommand ?? new RelayCommand<object>(parameter => OnFileChanged((File)parameter), parameter => parameter is File);
                return selectCommand;
            }
        }
    }
}