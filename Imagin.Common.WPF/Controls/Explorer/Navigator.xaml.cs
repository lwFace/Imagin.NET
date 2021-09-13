using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Imagin.Common.Threading;
namespace Imagin.Common.Controls
{
    public partial class Navigator : UserControl
    {
        public Storage.ItemCollection Items { get; private set; } = new Storage.ItemCollection(string.Empty, new Filter());

        public static DependencyProperty RootProperty = DependencyProperty.Register(nameof(Root), typeof(object), typeof(Navigator), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnRootChanged));
        public object Root
        {
            get => (object)GetValue(RootProperty);
            set => SetValue(RootProperty, value);
        }
        static void OnRootChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<Navigator>().OnRootChanged(new OldNew(e));

        public static DependencyProperty DropHandlerProperty = DependencyProperty.Register(nameof(DropHandler), typeof(ExplorerDropHandler), typeof(Navigator), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public ExplorerDropHandler DropHandler
        {
            get => (ExplorerDropHandler)GetValue(DropHandlerProperty);
            private set => SetValue(DropHandlerProperty, value);
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(Navigator), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, null, OnPathCoerced));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static object OnPathCoerced(DependencyObject i, object value) => Explorer.Validate(i, value?.ToString());

        public static DependencyProperty RefreshOnClickProperty = DependencyProperty.Register(nameof(RefreshOnClick), typeof(bool), typeof(Navigator), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool RefreshOnClick
        {
            get => (bool)GetValue(RefreshOnClickProperty);
            set => SetValue(RefreshOnClickProperty, value);
        }

        public static DependencyProperty ShowCheckBoxesProperty = DependencyProperty.Register(nameof(ShowCheckBoxes), typeof(bool), typeof(Navigator), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool ShowCheckBoxes
        {
            get => (bool)GetValue(ShowCheckBoxesProperty);
            set => SetValue(ShowCheckBoxesProperty, value);
        }

        public static DependencyProperty ViewFilesProperty = DependencyProperty.Register(nameof(ViewFiles), typeof(bool), typeof(Navigator), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool ViewFiles
        {
            get => (bool)GetValue(ViewFilesProperty);
            set => SetValue(ViewFilesProperty, value);
        }

        public static DependencyProperty ViewFileExtensionsProperty = DependencyProperty.Register(nameof(ViewFileExtensions), typeof(bool), typeof(Navigator), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool ViewFileExtensions
        {
            get => (bool)GetValue(ViewFileExtensionsProperty);
            set => SetValue(ViewFileExtensionsProperty, value);
        }

        public static DependencyProperty ViewHiddenItemsProperty = DependencyProperty.Register(nameof(ViewHiddenItems), typeof(bool), typeof(Navigator), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool ViewHiddenItems
        {
            get => (bool)GetValue(ViewHiddenItemsProperty);
            set => SetValue(ViewHiddenItemsProperty, value);
        }

        public Navigator()
        {
            SetCurrentValue(DropHandlerProperty, new ExplorerDropHandler(this));
            //SetCurrentValue(RootProperty, Folder.Long.Root);

            InitializeComponent();
        }

        /// <summary>
        /// //We fill when 
        /// 
        /// a) RefreshOnClick = true, OR 
        /// b) RefreshOnClick = false & no items are present.
        /// 
        /// A lack of items indicates a refresh has never been performed.
        /// 
        /// Ultimately, this ensures items are obtained at least once, 
        /// leaving all subsequent attempts conditional upon RefreshOnClick.
        /// </summary>
        protected void OnExpanded(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var item = (sender as TreeViewItem).DataContext.As<Item>();
            if (item != null)
            {
                var result = string.Empty;
                if (item is Container)
                {
                    result = item.Path;
                    if (RefreshOnClick || !item.As<Container>().Items.Any<Item>())
                    {
                        item.As<Container>().Items.Filter = new Filter();
                        _ = item.As<Container>().Items.RefreshAsync(item.Path);
                    }
                }
                else if (item is File)
                {
                    if (item is Shortcut)
                    {
                        var s = Storage.Shortcut.TargetPath(item.Path);
                        if (Folder.Long.Exists(s))
                        {
                            result = s;
                            if (RefreshOnClick || !item.As<Container>().Items.Any<Item>())
                            {
                                item.As<Shortcut>().Items.Filter = new Filter();
                                _ = item.As<Shortcut>().Items.RefreshAsync(result);
                            }
                        }
                    }
                }
                if (!result.NullOrEmpty())
                {
                    SetCurrentValue(PathProperty, result);
                }
            }
        }

        protected void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as FrameworkElement).DataContext is File)
                Open();
        }

        BackgroundQueue queue = new BackgroundQueue();

        void Update(OldNew input)
        {
            if (input.Old is Favorites fa)
            {
                fa.Added -= OnItemAdded;
                fa.Removed -= OnItemRemoved;
            }

            Items.Clear();

            if (input.New is string i)
            {
                Items.Refresh(i);
            }
            else if (input.New is Favorites fb)
            {
                foreach (var favorite in fb)
                    Items.Add(new Folder(favorite.Path));

                fb.Added -= OnItemAdded;
                fb.Added += OnItemAdded;

                fb.Removed -= OnItemRemoved;
                fb.Removed += OnItemRemoved;
            }
        }

        protected virtual void OnRootChanged(OldNew input)
        {
            queue.Add(() => Update(input));
        }

        void OnItemRemoved(object sender, Input.EventArgs<Favorite> e)
        {
            for (var i = Items.Count - 1; i >= 0; i--)
            {
                if (Items[i].Path == e.Value.Path)
                    Items.RemoveAt(i);
            }
        }

        void OnItemAdded(object sender, Input.EventArgs<Favorite> e)
        {
            Items.Add(new Folder(e.Value.Path));
        }

        public virtual void Open()
        {
            /*
            var First = Source.SelectedItems.First();
            switch (First.Type)
            {
                case ItemType.Drive:
                case ItemType.Folder:
                    Source.OnFolderOpened(First.Path);
                    break;

                case ItemType.File:
                    if (Shortcut.Is(First.Path))
                    {
                        var i = Shortcut.GetTargetName(First.Path);

                        if (i.DirectoryExists())
                        {
                            Source.OnFolderOpened(i);
                        }
                        else if (i.FileExists())
                        {
                            LocalFile.TryOpen(i);
                        }
                        else Imagin.Common.Dialog.ShowError("Open", "Shortcut target \"{0}\" no longer exists.".F(i));
                    }
                    else
                    {
                        var e = 0;
                        var Result = Source.SelectedItems.TryForEach<StorageItem>(i =>
                        {
                            if (i.Type == ItemType.File && !LocalFile.TryOpen(i.Path))
                                e++;
                        });
                        if (!Result || e > 0)
                            Imagin.Common.Dialog.ShowError("Open", "One or more items could not be opened.");
                    }
                    break;
            }
            */
        }
    }
}