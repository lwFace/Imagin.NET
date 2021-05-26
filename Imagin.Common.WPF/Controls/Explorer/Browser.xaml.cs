using Imagin.Common.Analytics;
using Imagin.Common.Collections;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    #region BrowserLayout

    public enum BrowserLayout
    {
        Dynamic,
        Static
    }

    #endregion

    #region BrowserView

    public enum BrowserView
    {
        Content,
        Details,
        List,
        Thumbnails,
        Tiles
    }

    #endregion

    public partial class Browser : UserControl, IExplorer
    {
        #region Events

        public event EventHandler<EventArgs<string>> FileOpened;

        public event EventHandler<EventArgs<string>> FolderOpened;

        public event EventHandler<EventArgs<string>> PathChanged;

        public event EventHandler<EventArgs<IList<Item>>> SelectionChanged;

        #endregion

        #region Properties

        public const double ItemSizeInterval = 16;

        public const double ItemSizeMaximum = 512;

        public const double ItemSizeMinimum = 16;

        /// ......................................................................................................................

        List<System.Threading.CancellationTokenSource> sizeTokens = new List<System.Threading.CancellationTokenSource>();

        List<System.Threading.CancellationTokenSource> selectionSizeTokens = new List<System.Threading.CancellationTokenSource>();

        /// ......................................................................................................................

        public Storage.ItemCollection Items { get; private set; } = new Storage.ItemCollection();

        public IEnumerable<Item> SelectedItems => PART_ListView.SelectedItems.Cast<Item>();

        /// ......................................................................................................................

        public static DependencyProperty DropHandlerProperty = DependencyProperty.Register(nameof(DropHandler), typeof(ExplorerDropHandler), typeof(Browser), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public ExplorerDropHandler DropHandler
        {
            get => (ExplorerDropHandler)GetValue(DropHandlerProperty);
            private set => SetValue(DropHandlerProperty, value);
        }

        public static DependencyProperty FileExtensionsProperty = DependencyProperty.Register(nameof(FileExtensions), typeof(IList<string>), typeof(Browser), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnFileExtensionsChanged));
        public IList<string> FileExtensions
        {
            get => (IList<string>)GetValue(FileExtensionsProperty);
            set => SetValue(FileExtensionsProperty, value);
        }
        static void OnFileExtensionsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as Browser).OnFileExtensionsChanged(new OldNew<IList<string>>(e));
        
        public static DependencyProperty FileSizeFormatProperty = DependencyProperty.Register(nameof(FileSizeFormat), typeof(FileSizeFormat), typeof(Browser), new FrameworkPropertyMetadata(FileSizeFormat.BinaryUsingSI, FrameworkPropertyMetadataOptions.None));
        public FileSizeFormat FileSizeFormat
        {
            get => (FileSizeFormat)GetValue(FileSizeFormatProperty);
            set => SetValue(FileSizeFormatProperty, value);
        }

        public static DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof(GroupName), typeof(ItemProperty), typeof(Browser), new FrameworkPropertyMetadata(ItemProperty.None, FrameworkPropertyMetadataOptions.None, OnGroupNameChanged));
        public ItemProperty GroupName
        {
            get => (ItemProperty)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }
        static void OnGroupNameChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as Browser).OnGroupNameChanged();

        public static DependencyProperty HistoryProperty = DependencyProperty.Register(nameof(History), typeof(History), typeof(Browser), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public History History
        {
            get => (History)GetValue(HistoryProperty);
            set => SetValue(HistoryProperty, value);
        }

        public static DependencyProperty LayoutProperty = DependencyProperty.Register(nameof(Layout), typeof(BrowserLayout), typeof(Browser), new FrameworkPropertyMetadata(BrowserLayout.Static, FrameworkPropertyMetadataOptions.None));
        public BrowserLayout Layout
        {
            get => (BrowserLayout)GetValue(LayoutProperty);
            private set => SetValue(LayoutProperty, value);
        }

        public static DependencyProperty LengthProperty = DependencyProperty.Register(nameof(Length), typeof(long), typeof(Browser), new FrameworkPropertyMetadata((long)0, FrameworkPropertyMetadataOptions.None));
        public long Length
        {
            get => (long)GetValue(LengthProperty);
            private set => SetValue(LengthProperty, value);
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(Browser), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, OnPathChanged, OnPathCoerced));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static void OnPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as Browser).OnPathChanged(new OldNew<string>(e));
        static object OnPathCoerced(DependencyObject i, object value) => Explorer.Validate(i, value?.ToString());

        public static DependencyProperty ItemSizeProperty = DependencyProperty.Register(nameof(ItemSize), typeof(double), typeof(Browser), new FrameworkPropertyMetadata(32.0, FrameworkPropertyMetadataOptions.None));
        public double ItemSize
        {
            get => (double)GetValue(ItemSizeProperty);
            set => SetValue(ItemSizeProperty, value);
        }

        public static DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }
        
        public static DependencyProperty OpenFilesOnClickProperty = DependencyProperty.Register(nameof(OpenFilesOnClick), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        /// <summary>
        /// Gets or sets whether or not a <see cref="File"/> can be opened when clicking once or twice.
        /// </summary>
        public bool OpenFilesOnClick
        {
            get => (bool)GetValue(OpenFilesOnClickProperty);
            set => SetValue(OpenFilesOnClickProperty, value);
        }

        public static DependencyProperty OpenOnDoubleClickProperty = DependencyProperty.Register(nameof(OpenOnDoubleClick), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        /// <summary>
        /// Gets or sets whether or not an <see cref="Item"/> can be opened when clicked once (if <see cref="false"/>) or twice (if <see cref="true"/>).
        /// </summary>
        public bool OpenOnDoubleClick
        {
            get => (bool)GetValue(OpenOnDoubleClickProperty);
            set => SetValue(OpenOnDoubleClickProperty, value);
        }

        public static DependencyProperty SelectionProperty = DependencyProperty.Register(nameof(Selection), typeof(IList<Item>), typeof(Browser), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public IList<Item> Selection
        {
            get => (IList<Item>)GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }

        public static DependencyProperty SelectionLengthProperty = DependencyProperty.Register(nameof(SelectionLength), typeof(long), typeof(Browser), new FrameworkPropertyMetadata((long)0, FrameworkPropertyMetadataOptions.None));
        public long SelectionLength
        {
            get => (long)GetValue(SelectionLengthProperty);
            private set => SetValue(SelectionLengthProperty, value);
        }

        public static DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(System.Windows.Controls.SelectionMode), typeof(Browser), new FrameworkPropertyMetadata(System.Windows.Controls.SelectionMode.Extended, FrameworkPropertyMetadataOptions.None));
        public System.Windows.Controls.SelectionMode SelectionMode
        {
            get => (System.Windows.Controls.SelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        public static DependencyProperty ShortcutIndicatorTemplateProperty = DependencyProperty.Register(nameof(ShortcutIndicatorTemplate), typeof(DataTemplate), typeof(Browser), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate ShortcutIndicatorTemplate
        {
            get => (DataTemplate)GetValue(ShortcutIndicatorTemplateProperty);
            set => SetValue(ShortcutIndicatorTemplateProperty, value);
        }

        public static DependencyProperty ShortcutIndicatorVisibilityProperty = DependencyProperty.Register(nameof(ShortcutIndicatorVisibility), typeof(Visibility), typeof(Browser), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.None));
        public Visibility ShortcutIndicatorVisibility
        {
            get => (Visibility)GetValue(ShortcutIndicatorVisibilityProperty);
            set => SetValue(ShortcutIndicatorVisibilityProperty, value);
        }

        public static DependencyProperty SortDirectionProperty = DependencyProperty.Register(nameof(SortDirection), typeof(ListSortDirection), typeof(Browser), new FrameworkPropertyMetadata(ListSortDirection.Ascending, FrameworkPropertyMetadataOptions.None));
        public ListSortDirection SortDirection
        {
            get => (ListSortDirection)GetValue(SortDirectionProperty);
            set => SetValue(SortDirectionProperty, value);
        }

        public static DependencyProperty SortNameProperty = DependencyProperty.Register(nameof(SortName), typeof(ItemProperty), typeof(Browser), new FrameworkPropertyMetadata(ItemProperty.Name, FrameworkPropertyMetadataOptions.None));
        public ItemProperty SortName
        {
            get => (ItemProperty)GetValue(SortNameProperty);
            set => SetValue(SortNameProperty, value);
        }

        public static DependencyProperty ShowCheckBoxesProperty = DependencyProperty.Register(nameof(ShowCheckBoxes), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool ShowCheckBoxes
        {
            get => (bool)GetValue(ShowCheckBoxesProperty);
            set => SetValue(ShowCheckBoxesProperty, value);
        }

        public static DependencyProperty ViewProperty = DependencyProperty.Register(nameof(View), typeof(BrowserView), typeof(Browser), new FrameworkPropertyMetadata(BrowserView.Thumbnails, FrameworkPropertyMetadataOptions.None));
        public BrowserView View
        {
            get => (BrowserView)GetValue(ViewProperty);
            set => SetValue(ViewProperty, value);
        }

        public static DependencyProperty ViewFilesProperty = DependencyProperty.Register(nameof(ViewFiles), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool ViewFiles
        {
            get => (bool)GetValue(ViewFilesProperty);
            set => SetValue(ViewFilesProperty, value);
        }

        public static DependencyProperty ViewFileExtensionsProperty = DependencyProperty.Register(nameof(ViewFileExtensions), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool ViewFileExtensions
        {
            get => (bool)GetValue(ViewFileExtensionsProperty);
            set => SetValue(ViewFileExtensionsProperty, value);
        }

        public static DependencyProperty ViewHiddenItemsProperty = DependencyProperty.Register(nameof(ViewHiddenItems), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool ViewHiddenItems
        {
            get => (bool)GetValue(ViewHiddenItemsProperty);
            set => SetValue(ViewHiddenItemsProperty, value);
        }

        /// ......................................................................................................................

        public static DependencyProperty ItemBackgroundProperty = DependencyProperty.Register(nameof(ItemBackground), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemBackground
        {
            get => (SolidColorBrush)GetValue(ItemBackgroundProperty);
            set => SetValue(ItemBackgroundProperty, value);
        }

        public static DependencyProperty ItemBorderProperty = DependencyProperty.Register(nameof(ItemBorder), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemBorder
        {
            get => (SolidColorBrush)GetValue(ItemBorderProperty);
            set => SetValue(ItemBorderProperty, value);
        }

        public static DependencyProperty ItemForegroundProperty = DependencyProperty.Register(nameof(ItemForeground), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemForeground
        {
            get => (SolidColorBrush)GetValue(ItemForegroundProperty);
            set => SetValue(ItemForegroundProperty, value);
        }

        /// ......................................................................................................................

        public static DependencyProperty ItemMouseOverBackgroundProperty = DependencyProperty.Register(nameof(ItemMouseOverBackground), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemMouseOverBackground
        {
            get => (SolidColorBrush)GetValue(ItemMouseOverBackgroundProperty);
            set => SetValue(ItemMouseOverBackgroundProperty, value);
        }

        public static DependencyProperty ItemSelectedActiveBackgroundProperty = DependencyProperty.Register(nameof(ItemSelectedActiveBackground), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemSelectedActiveBackground
        {
            get => (SolidColorBrush)GetValue(ItemSelectedActiveBackgroundProperty);
            set => SetValue(ItemSelectedActiveBackgroundProperty, value);
        }

        public static DependencyProperty ItemSelectedInactiveBackgroundProperty = DependencyProperty.Register(nameof(ItemSelectedInactiveBackground), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemSelectedInactiveBackground
        {
            get => (SolidColorBrush)GetValue(ItemSelectedInactiveBackgroundProperty);
            set => SetValue(ItemSelectedInactiveBackgroundProperty, value);
        }

        /// ......................................................................................................................

        public static DependencyProperty ItemMouseOverBorderProperty = DependencyProperty.Register(nameof(ItemMouseOverBorder), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemMouseOverBorder
        {
            get => (SolidColorBrush)GetValue(ItemMouseOverBorderProperty);
            set => SetValue(ItemMouseOverBorderProperty, value);
        }

        public static DependencyProperty ItemSelectedActiveBorderProperty = DependencyProperty.Register(nameof(ItemSelectedActiveBorder), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemSelectedActiveBorder
        {
            get => (SolidColorBrush)GetValue(ItemSelectedActiveBorderProperty);
            set => SetValue(ItemSelectedActiveBorderProperty, value);
        }

        public static DependencyProperty ItemSelectedInactiveBorderProperty = DependencyProperty.Register(nameof(ItemSelectedInactiveBorder), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemSelectedInactiveBorder
        {
            get => (SolidColorBrush)GetValue(ItemSelectedInactiveBorderProperty);
            set => SetValue(ItemSelectedInactiveBorderProperty, value);
        }

        /// ......................................................................................................................

        public static DependencyProperty ItemMouseOverForegroundProperty = DependencyProperty.Register(nameof(ItemMouseOverForeground), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemMouseOverForeground
        {
            get => (SolidColorBrush)GetValue(ItemMouseOverForegroundProperty);
            set => SetValue(ItemMouseOverForegroundProperty, value);
        }

        public static DependencyProperty ItemSelectedActiveForegroundProperty = DependencyProperty.Register(nameof(ItemSelectedActiveForeground), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemSelectedActiveForeground
        {
            get => (SolidColorBrush)GetValue(ItemSelectedActiveForegroundProperty);
            set => SetValue(ItemSelectedActiveForegroundProperty, value);
        }

        public static DependencyProperty ItemSelectedInactiveForegroundProperty = DependencyProperty.Register(nameof(ItemSelectedInactiveForeground), typeof(SolidColorBrush), typeof(Browser), new FrameworkPropertyMetadata(default(SolidColorBrush), FrameworkPropertyMetadataOptions.None));
        public SolidColorBrush ItemSelectedInactiveForeground
        {
            get => (SolidColorBrush)GetValue(ItemSelectedInactiveForegroundProperty);
            set => SetValue(ItemSelectedInactiveForegroundProperty, value);
        }

        #endregion

        #region Browser

        public Browser() : base()
        {
            SetCurrentValue(DropHandlerProperty, new ExplorerDropHandler(this));
            SetCurrentValue(HistoryProperty, new History(Explorer.DefaultLimit));
            SetCurrentValue(SelectionProperty, (IList<Item>)Enumerable.Empty<Item>());

            InitializeComponent();
            OnGroupNameChanged();
        }

        #endregion

        #region Methods

        void Recycle()
        {
            foreach (var i in SelectedItems)
                Machine.Recycle(i.Path);
        }

        /// ......................................................................................................................

        void Open()
        {
            foreach (var item in SelectedItems)
                Open(item);
        }

        void Open(Item item)
        {
            switch (item.Type)
            {
                case ItemType.Drive:
                case ItemType.Folder:
                    if (!IsReadOnly)
                    {
                        SetCurrentValue(PathProperty, item.Path);
                    }
                    OnFolderOpened(item.Path);
                    break;

                case ItemType.File:
                    Storage.File.Long.Open(item.Path);
                    OnFileOpened(item.Path);
                    break;

                case ItemType.Shortcut:
                    var targetPath = Shortcut.TargetPath(item.Path);
                    if (Folder.Long.Exists(targetPath))
                    {
                        if (!IsReadOnly)
                        {
                            SetCurrentValue(PathProperty, targetPath);
                        }
                        OnFolderOpened(targetPath);
                    }
                    else if (Storage.File.Long.Exists(targetPath))
                    {
                        Storage.File.Long.Open(targetPath);
                        OnFileOpened(targetPath);
                    }
                    break;
            }
        }

        /// ......................................................................................................................

        //We have to be able to do this inside the Explorer and ALSO Windows Explorer

        void Copy()
        {
            SelectedItems.ForEach(i => i.IsCut = false);

            var items = new System.Collections.Specialized.StringCollection();
            foreach (var i in SelectedItems)
                items.Add(i.Path);

            Clipboard.SetFileDropList(items);
        }

        void Cut()
        {
            Copy();
            SelectedItems.ForEach(i => i.IsCut = true);
        }

        void Paste()
        {
            //To do: Do copy (or cut) operation
            foreach (var i in Clipboard.GetFileDropList())
            {
            }
            SelectedItems.ForEach(i => i.IsCut = false);
        }

        /// ......................................................................................................................

        void OnGroupNameChanged()
        {
            var groupStyle
                = GroupName == ItemProperty.None
                ? null /* (GroupStyle)Resources["GroupStyle.NoGrouping"] */
                : (GroupStyle)Resources["GroupStyle.Grouping"];

            PART_ListView.GroupStyle.Clear();

            if (groupStyle != null)
                PART_ListView.GroupStyle.Add(groupStyle);
        }

        /// ......................................................................................................................

        protected virtual void OnFileExtensionsChanged(OldNew<IList<string>> input)
        {
            Items.Filter = new Storage.Filter(ItemType.Drive | ItemType.Folder | ItemType.File | ItemType.Shortcut, input.New?.ToArray());
            Refresh();
        }

        /// ......................................................................................................................

        void OnPreviewRenamed(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != Storage.File.Long.CleanName(e.Text))
            {
                e.Handled = true;
            }
        }

        protected virtual void OnRenamed(object sender, EventArgs<string> e)
        {
            if ((sender as LabelBox).DataContext is Item i)
            {
                var a = i.Path;
                var b = $@"{e.Value}";

                if (!ViewFileExtensions)
                {
                    //Append the file extension to it!
                    b = $"{b}{System.IO.Path.GetExtension(a)}";
                }

                var result = Machine.Move(i, System.IO.Path.GetDirectoryName(i.Path), b);
                if (result is Error)
                {
                    (sender as LabelBox).GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateTarget();
                    Dialog.Show("Rename", $"{result}", DialogImage.Error, DialogButtons.Ok);
                }
            }
        }

        /// ......................................................................................................................

        protected virtual void OnFileOpened(string filePath)
        {
            FileOpened?.Invoke(this, new EventArgs<string>(filePath));
        }

        protected virtual void OnFolderOpened(string folderPath)
        {
            FolderOpened?.Invoke(this, new EventArgs<string>(folderPath));
        }

        /// ......................................................................................................................

        protected virtual void OnPathChanged(OldNew<string> input)
        {
            Refresh();
            PathChanged?.Invoke(this, new EventArgs<string>(input.New));
        }

        /// ......................................................................................................................

        async System.Threading.Tasks.Task UpdateLength(string folderPath)
        {
            foreach (var i in sizeTokens)
                i.Cancel();

            var token = new System.Threading.CancellationTokenSource();

            sizeTokens.Add(token);
            long size = await Folder.Long.GetSize(folderPath, token.Token);
            sizeTokens.Remove(token);

            if (!token.IsCancellationRequested)
                SetCurrentValue(LengthProperty, size);
        }

        async System.Threading.Tasks.Task UpdateSelectionLength(IEnumerable<Item> items)
        {
            foreach (var i in selectionSizeTokens)
                i.Cancel();

            var token = new System.Threading.CancellationTokenSource();
            selectionSizeTokens.Add(token);

            long size = 0;
            foreach (var i in items)
            {
                if (i.Type != ItemType.File)
                {
                    size += await Folder.Long.GetSize(i.Path, token.Token);
                }
                else size += i.Size;
            }

            selectionSizeTokens.Remove(token);

            if (!token.IsCancellationRequested)
                SetCurrentValue(SelectionLengthProperty, size);
        }

        /// ......................................................................................................................

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                if (e.Delta > 0)
                {
                    if (ItemSize + ItemSizeInterval <= ItemSizeMaximum)
                        SetCurrentValue(ItemSizeProperty, ItemSize + ItemSizeInterval);
                }
                else
                {
                    if (ItemSize - ItemSizeInterval >= ItemSizeMinimum)
                        SetCurrentValue(ItemSizeProperty, ItemSize - ItemSizeInterval);
                }
            }
        }

        /// ......................................................................................................................

        public void Refresh()
        {
            _ = Items.Refresh(Path);
            _ = UpdateLength(Path);
        }

        /// ......................................................................................................................

        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.Source is ListViewItem))
            {
                if (e.ClickCount == 2)
                {
                    Machine.OpenInWindowsExplorer(Path);
                }
            }
        }

        void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Try.Invoke(() =>
            {
                var point = PointToScreen(e.GetPosition(this));
                ShellContextMenu.Show(point.Int32(), new DirectoryInfo(Path));
            });
        }

        /// ......................................................................................................................

        void OnItemPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (Item)((FrameworkElement)sender).DataContext;
            if ((e.ClickCount == 1 && !OpenOnDoubleClick) || (e.ClickCount == 2 && OpenOnDoubleClick))
            {
                if (item.Type != ItemType.File || OpenFilesOnClick)
                {
                    Open(item);
                }
                else OnFileOpened(item.Path);
            }
        }

        void OnItemPreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var selectedItems = SelectedItems;
            var result = new FileSystemInfo[selectedItems.Count()];

            var j = 0;
            foreach (var i in selectedItems)
            {
                switch (i.Type)
                {
                    case ItemType.Drive:
                    case ItemType.Folder:
                        result[j] = new DirectoryInfo(i.Path);
                        break;

                    case ItemType.File:
                    case ItemType.Shortcut:
                        result[j] = new FileInfo(i.Path);
                        break;
                }
                j++;
            }
            var point = PointToScreen(e.GetPosition(this));
            ShellContextMenu.Show(point.Int32(), result);
        }

        /// ......................................................................................................................

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = (IList<Item>)Array<Item>.New(SelectedItems);
            SetCurrentValue(SelectionProperty, items);
            SelectionChanged?.Invoke(this, new EventArgs<IList<Item>>(items));
            _ = UpdateSelectionLength(items);
        }

        /// ......................................................................................................................

        void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Back:
                    History?.Undo(i => SetCurrentValue(PathProperty, i));
                    break;

                case Key.Enter:
                    Open();
                    break;

                case Key.Delete:
                    Recycle();
                    break;
            }

            if (ModifierKeys.Control.Pressed())
            {
                switch (e.Key)
                {
                    case Key.C:
                        Copy();
                        break;
                    case Key.X:
                        Cut();
                        break;
                    case Key.V:
                        Paste();
                        break;
                }
            }
        }

        /// ......................................................................................................................

        ICommand groupCommand;
        public ICommand GroupCommand
        {
            get
            {
                groupCommand = groupCommand ?? new RelayCommand<object>(i => SetCurrentValue(GroupNameProperty, (ItemProperty?)(ItemProperty)i), i => i is ItemProperty);
                return groupCommand;
            }
        }

        ICommand sortDirectionCommand;
        public ICommand SortDirectionCommand
        {
            get
            {
                sortDirectionCommand = sortDirectionCommand ?? new RelayCommand<object>(i => SetCurrentValue(SortDirectionProperty, (ListSortDirection)i), i => i is ListSortDirection);
                return sortDirectionCommand;
            }
        }

        ICommand sortNameCommand;
        public ICommand SortNameCommand
        {
            get
            {
                sortNameCommand = sortNameCommand ?? new RelayCommand<object>(i => SetCurrentValue(SortNameProperty, (ItemProperty?)(ItemProperty)i), i => i is ItemProperty);
                return sortNameCommand;
            }
        }

        /// ......................................................................................................................

        string keyFilter = null;

        System.Timers.Timer keyFilterTimer = new System.Timers.Timer()
        {
            Interval = 2000
        };

        void OnKeyFilterElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            keyFilterTimer.Elapsed -= OnKeyFilterElapsed;
            keyFilterTimer.Stop();
            keyFilter = null;
        }

        char? FromKey(Key key)
        {
            var shift = ModifierKeys.Shift.Pressed();
            switch (key)
            {
                case Key.OemCloseBrackets:
                    return shift ? '}' : ']';
                case Key.OemComma:
                    return shift ? (char?)null : ','; //<
                case Key.OemMinus:
                    return shift ? '_' : '-';
                case Key.OemOpenBrackets:
                    return shift ? '{' : '[';
                case Key.OemPeriod:
                    return shift ? '.' : (char?)null; //>
                case Key.OemPlus:
                    return shift ? '+' : '=';
                case Key.OemSemicolon:
                    return shift ? (char?)null : ';'; //:

                case Key.Space:
                    return ' ';

                case Key.A:
                case Key.B:
                case Key.C:
                case Key.D:
                case Key.E:
                case Key.F:
                case Key.G:
                case Key.H:
                case Key.I:
                case Key.J:
                case Key.K:
                case Key.L:
                case Key.M:
                case Key.N:
                case Key.O:
                case Key.P:
                case Key.Q:
                case Key.R:
                case Key.S:
                case Key.T:
                case Key.U:
                case Key.V:
                case Key.W:
                case Key.X:
                case Key.Y:
                case Key.Z:
                    return shift ? $"{key}"[0] : $"{key}".ToLower()[0];

                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    return $"{key}".Replace("NumPad", string.Empty)[0];

                case Key.Add:
                    return '+';

                case Key.Decimal:
                    return '.';

                case Key.Subtract:
                    return '-';

                case Key.D0:
                    return shift ? ')' : '0';
                case Key.D1:
                    return shift ? '!' : '1';
                case Key.D2:
                    return shift ? '@' : '2';
                case Key.D3:
                    return shift ? '#' : '3';
                case Key.D4:
                    return shift ? '$' : '4';
                case Key.D5:
                    return shift ? '%' : '5';
                case Key.D6:
                    return shift ? '^' : '6';
                case Key.D7:
                    return shift ? '&' : '7';
                case Key.D8:
                    return shift ? (char?)null : '8'; //*
                case Key.D9:
                    return shift ? '(' : '9';
            }
            return null;
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            char? character = FromKey(e.Key);
            if (character == null)
            {
                keyFilter = null;
                return;
            }

            if (keyFilter == null)
            {
                keyFilter = string.Empty;
            }

            keyFilter = $"{keyFilter}{character}";

            keyFilterTimer.Stop();
            keyFilterTimer.Start();

            keyFilterTimer.Elapsed -= OnKeyFilterElapsed;
            keyFilterTimer.Elapsed += OnKeyFilterElapsed;

            Item item = null;
            foreach (var i in Items)
            {
                if (System.IO.Path.GetFileName(i.Path).ToLower().StartsWith(keyFilter))
                {
                    item = i;
                    break;
                }
            }

            if (item == null)
            {
                keyFilter = null;
                return;
            }

            foreach (var i in Items)
            {
                if (i.Path != item.Path)
                    i.IsSelected = false;
            }

            PART_ListView.ScrollIntoView(item);
            item.IsSelected = true;
        }

        #endregion
    }
}