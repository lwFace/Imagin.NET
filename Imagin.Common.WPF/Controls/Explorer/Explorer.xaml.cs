using Imagin.Common.Collections;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Converters;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public partial class Explorer : UserControl
    {
        public static readonly Limit DefaultLimit = new Limit(50);

        public static string DefaultPath => Folder.Long.Root; //System.Environment.SpecialFolder.Desktop.Path();

        public event EventHandler<EventArgs<string>> FileOpened;

        public static DependencyProperty FavoritesProperty = DependencyProperty.Register(nameof(Favorites), typeof(Favorites), typeof(Explorer), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Favorites Favorites
        {
            get => (Favorites)GetValue(FavoritesProperty);
            set => SetValue(FavoritesProperty, value);
        }

        public static DependencyProperty FileExtensionsProperty = DependencyProperty.Register(nameof(FileExtensions), typeof(IList<string>), typeof(Explorer), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public IList<string> FileExtensions
        {
            get => (IList<string>)GetValue(FileExtensionsProperty);
            set => SetValue(FileExtensionsProperty, value);
        }

        public static DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof(GroupName), typeof(ItemProperty), typeof(Explorer), new FrameworkPropertyMetadata(ItemProperty.None, FrameworkPropertyMetadataOptions.None));
        public ItemProperty GroupName
        {
            get => (ItemProperty)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }

        public static DependencyProperty HistoryProperty = DependencyProperty.Register(nameof(History), typeof(History), typeof(Explorer), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public History History
        {
            get => (History)GetValue(HistoryProperty);
            set => SetValue(HistoryProperty, value);
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(Explorer), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, null, OnPathCoerced));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static object OnPathCoerced(DependencyObject d, object value) => Validate(d, value?.ToString());

        public static DependencyProperty RootProperty = DependencyProperty.Register(nameof(Root), typeof(string), typeof(Explorer), new FrameworkPropertyMetadata(Folder.Long.Root, FrameworkPropertyMetadataOptions.None));
        public string Root
        {
            get => (string)GetValue(RootProperty);
            set => SetValue(RootProperty, value);
        }

        public static DependencyProperty SelectionProperty = DependencyProperty.Register(nameof(Selection), typeof(IList<Item>), typeof(Explorer), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public IList<Item> Selection
        {
            get => (IList<Item>)GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }

        public static DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(SelectionMode), typeof(Explorer), new FrameworkPropertyMetadata(SelectionMode.Multiple, FrameworkPropertyMetadataOptions.None));
        public SelectionMode SelectionMode
        {
            get => (SelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        public static DependencyProperty SortDirectionProperty = DependencyProperty.Register(nameof(SortDirection), typeof(ListSortDirection), typeof(Explorer), new FrameworkPropertyMetadata(ListSortDirection.Ascending, FrameworkPropertyMetadataOptions.None));
        public ListSortDirection SortDirection
        {
            get => (ListSortDirection)GetValue(SortDirectionProperty);
            set => SetValue(SortDirectionProperty, value);
        }

        public static DependencyProperty SortNameProperty = DependencyProperty.Register(nameof(SortName), typeof(ItemProperty), typeof(Explorer), new FrameworkPropertyMetadata(ItemProperty.Name, FrameworkPropertyMetadataOptions.None));
        public ItemProperty SortName
        {
            get => (ItemProperty)GetValue(SortNameProperty);
            set => SetValue(SortNameProperty, value);
        }

        public static DependencyProperty ViewProperty = DependencyProperty.Register(nameof(View), typeof(BrowserView), typeof(Explorer), new FrameworkPropertyMetadata(BrowserView.Thumbnails, FrameworkPropertyMetadataOptions.None));
        public BrowserView View
        {
            get => (BrowserView)GetValue(ViewProperty);
            set => SetValue(ViewProperty, value);
        }

        public static DependencyProperty ViewFileExtensionsProperty = DependencyProperty.Register(nameof(ViewFileExtensions), typeof(bool), typeof(Explorer), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool ViewFileExtensions
        {
            get => (bool)GetValue(ViewFileExtensionsProperty);
            set => SetValue(ViewFileExtensionsProperty, value);
        }

        public static DependencyProperty ViewFilesProperty = DependencyProperty.Register(nameof(ViewFiles), typeof(bool), typeof(Explorer), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool ViewFiles
        {
            get => (bool)GetValue(ViewFilesProperty);
            set => SetValue(ViewFilesProperty, value);
        }
        
        public static DependencyProperty ViewHiddenItemsProperty = DependencyProperty.Register(nameof(ViewHiddenItems), typeof(bool), typeof(Explorer), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool ViewHiddenItems
        {
            get => (bool)GetValue(ViewHiddenItemsProperty);
            set => SetValue(ViewHiddenItemsProperty, value);
        }

        public static DependencyProperty ViewSizeProperty = DependencyProperty.Register(nameof(ViewSize), typeof(double), typeof(Explorer), new FrameworkPropertyMetadata(32.0, FrameworkPropertyMetadataOptions.None));
        public double ViewSize
        {
            get => (double)GetValue(ViewSizeProperty);
            set => SetValue(ViewSizeProperty, value);
        }

        public Explorer()
        {
            InitializeComponent();

            SetCurrentValue(HistoryProperty, new History(DefaultLimit));
            SetCurrentValue(PathProperty, Environment.SpecialFolder.Desktop.Path());
            SetCurrentValue(SelectionProperty, (IList<Item>)Enumerable.Empty<Item>());

            PART_Browser.Bind(Browser.SelectionModeProperty, nameof(SelectionMode), this, BindingMode.OneWay, new DefaultConverter<SelectionMode, System.Windows.Controls.SelectionMode>(i =>
            {
                switch (i)
                {
                    case SelectionMode.Multiple:
                        return System.Windows.Controls.SelectionMode.Extended;

                    case SelectionMode.Single:
                    case SelectionMode.SingleOrNone:
                        return System.Windows.Controls.SelectionMode.Single;
                }
                throw new NotSupportedException();
            }, null));
        }

        void OnFileOpened(object sender, EventArgs<string> e) => FileOpened?.Invoke(sender, e);

        void OnRefreshed(object sender, RoutedEventArgs e)
        {
            PART_Browser.Refresh();
        }

        public static string Validate(DependencyObject dependencyObject, string folderPath)
        {
            if (folderPath.NullOrEmpty())
                return DefaultPath;

            if (!DesignerProperties.GetIsInDesignMode(dependencyObject))
            {
                if (folderPath != Folder.Long.RecycleBin && !Folder.Long.Exists(folderPath))
                    return DefaultPath;
            }

            return folderPath;
        }
    }
}