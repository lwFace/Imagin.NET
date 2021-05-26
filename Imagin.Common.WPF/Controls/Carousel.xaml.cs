using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public partial class Carousel : UserControl
    {
        public event EventHandler<EventArgs<File>> DoubleClick;

        public event EventHandler<EventArgs<File>> Selected;

        /// ......................................................................................................................

        Storage.ItemCollection files = new Storage.ItemCollection(new Filter(ItemType.File));
        public Storage.ItemCollection Files => files;

        public ObservableCollection<Item> ViewFiles { get; set; } = new ObservableCollection<Item>();

        /// ......................................................................................................................

        public static DependencyProperty ActualColumnsProperty = DependencyProperty.Register(nameof(ActualColumns), typeof(int), typeof(Carousel), new FrameworkPropertyMetadata(7, FrameworkPropertyMetadataOptions.None));
        public int ActualColumns
        {
            get => (int)GetValue(ActualColumnsProperty);
            private set => SetValue(ActualColumnsProperty, value);
        }

        public static DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(int), typeof(Carousel), new FrameworkPropertyMetadata(7, FrameworkPropertyMetadataOptions.None, OnColumnsChanged, OnColumnsCoerced));
        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }
        static void OnColumnsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<Carousel>().OnColumnsChanged(new OldNew<int>(e));
        static object OnColumnsCoerced(DependencyObject i, object value) => i.As<Carousel>().OnColumnsCoerced((int)value);

        public static DependencyProperty LeftButtonTemplateProperty = DependencyProperty.Register(nameof(LeftButtonTemplate), typeof(DataTemplate), typeof(Carousel), new FrameworkPropertyMetadata(null));
        public DataTemplate LeftButtonTemplate
        {
            get => (DataTemplate)GetValue(LeftButtonTemplateProperty);
            set => SetValue(LeftButtonTemplateProperty, value);
        }

        public static DependencyProperty RightButtonTemplateProperty = DependencyProperty.Register(nameof(RightButtonTemplate), typeof(DataTemplate), typeof(Carousel), new FrameworkPropertyMetadata(null));
        public DataTemplate RightButtonTemplate
        {
            get => (DataTemplate)GetValue(RightButtonTemplateProperty);
            set => SetValue(RightButtonTemplateProperty, value);
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(Carousel), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, OnPathChanged));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static void OnPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<Carousel>().OnPathChanged(new OldNew<string>(e));

        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(File), typeof(Carousel), new FrameworkPropertyMetadata(default(File), FrameworkPropertyMetadataOptions.None, OnSelectedItemChanged));
        public File SelectedItem
        {
            get => (File)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        static void OnSelectedItemChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<Carousel>().OnSelectedItemChanged(new OldNew<File>(e));

        /// ......................................................................................................................

        public Carousel()
        {
            InitializeComponent();
            files.Refreshed += OnFilesRefreshed;
        }

        /// ......................................................................................................................

        void Move(LeftRight direction)
        {
            var first = ViewFiles.First<Item>();
            var last = ViewFiles.Last<Item>();

            switch (direction)
            {
                case LeftRight.Left:
                    ViewFiles.Remove(last);
                    break;
                case LeftRight.Right:
                    ViewFiles.Remove(first);
                    break;
            }

            var result = default(Item);

            var j = false;
            var k = default(Item);
            foreach (var i in files)
            {
                if (direction == LeftRight.Left)
                {
                    if (i == first)
                    {
                        result = k;
                        break;
                    }
                    k = i;
                }
                if (direction == LeftRight.Right)
                {
                    if (i == last)
                    {
                        j = true;
                    }
                    else if (j)
                    {
                        result = i;
                        break;
                    }
                }
            }

            if (result == null)
            {
                switch (direction)
                {
                    case LeftRight.Left:
                        result = files.Last<Item>();
                        break;
                    case LeftRight.Right:
                        result = files.First<Item>();
                        break;
                }
            }

            if (result != null)
            {
                switch (direction)
                {
                    case LeftRight.Left:
                        ViewFiles.Insert(0, result);
                        break;
                    case LeftRight.Right:
                        ViewFiles.Add(result);
                        break;
                }
            }
        }

        void Update()
        {
            SetCurrentValue(ActualColumnsProperty, files.Count < Columns ? files.Count : Columns);

            ViewFiles.Clear();
            foreach (var i in files.Take(ActualColumns))
                ViewFiles.Add(i);
        }

        /// ......................................................................................................................

        void OnFileAdded(object sender, EventArgs<Item> e) => Update();

        void OnFileRemoved(object sender, EventArgs<Item> e) => Update();

        void OnFilesRefreshed(object sender, EventArgs e)
        {
            Update();
            files.Added += OnFileAdded;
            files.Removed += OnFileRemoved;
        }

        void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnDoubleClick((sender as ListBoxItem).DataContext as File);
        }

        /// ......................................................................................................................

        protected virtual void OnColumnsChanged(OldNew<int> input) => Update();

        protected virtual object OnColumnsCoerced(int columns) => columns.Coerce(int.MaxValue, 1);

        protected virtual void OnDoubleClick(File item)
        {
            DoubleClick?.Invoke(this, new EventArgs<File>(item));
        }

        protected virtual void OnPathChanged(OldNew<string> input)
        {
            files.Added -= OnFileAdded;
            files.Removed -= OnFileRemoved;
            _ = files.Refresh(input.New);
        }

        protected virtual void OnSelectedItemChanged(OldNew<File> input)
        {
            Selected?.Invoke(this, new EventArgs<File>(input.New));
        }

        /// ......................................................................................................................

        ICommand nextCommand;
        public ICommand NextCommand => nextCommand = nextCommand ?? new RelayCommand(() => Move(LeftRight.Right), () => files.Count > Columns);

        ICommand previousCommand;
        public ICommand PreviousCommand => previousCommand = previousCommand ?? new RelayCommand(() => Move(LeftRight.Left), () => files.Count > Columns);
    }
}
