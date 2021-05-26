using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class ResizableGrid : Grid
    {
        #region Properties

        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(nameof(ItemContainerStyle), typeof(Style), typeof(ResizableGrid), new PropertyMetadata(default(Style), OnItemContainerStyleChanged));
        public Style ItemContainerStyle
        {
            get => (Style)GetValue(ItemContainerStyleProperty);
            set => SetValue(ItemContainerStyleProperty, value);
        }
        static void OnItemContainerStyleChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ResizableGrid>().OnItemContainerStyleChanged(new OldNew<Style>(e));

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(ResizableGrid), new PropertyMetadata(default(DataTemplate), OnItemTemplateChanged));
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }
        static void OnItemTemplateChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ResizableGrid>().OnItemTemplateChanged(new OldNew<DataTemplate>(e));

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(ResizableGrid), new PropertyMetadata(null, OnItemsSourceChanged));
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        static void OnItemsSourceChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ResizableGrid>().OnItemsSourceChange(new OldNew<IEnumerable>(e));

        public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.RegisterAttached("ColumnWidth", typeof(GridLength), typeof(ResizableGrid), new PropertyMetadata(new GridLength(1, GridUnitType.Star), OnColumnWidthChanged));
        public static void SetColumnWidth(DependencyObject i, GridLength Value) => i.SetValue(ColumnWidthProperty, Value);
        public static GridLength GetColumnWidth(DependencyObject i) => (GridLength)i.GetValue(ColumnWidthProperty);
        static void OnColumnWidthChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => UpdateColumnDefinition(i, Column => Column.Width = new OldNew<GridLength>(e).New);

        public static readonly DependencyProperty MinColumnWidthProperty = DependencyProperty.RegisterAttached("MinColumnWidth", typeof(double), typeof(ResizableGrid), new PropertyMetadata(100d, OnMinColumnWidthChanged));
        public static void SetMinColumnWidth(DependencyObject i, double Value) => i.SetValue(MinColumnWidthProperty, Value);
        public static double GetMinColumnWidth(DependencyObject i) => (double)i.GetValue(MinColumnWidthProperty);
        static void OnMinColumnWidthChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => UpdateColumnDefinition(i, Column => Column.MinWidth = new OldNew<double>(e).New);

        public static readonly DependencyProperty MaxColumnWidthProperty = DependencyProperty.RegisterAttached("MaxColumnWidth", typeof(double), typeof(ResizableGrid), new PropertyMetadata(double.MaxValue, OnMaxColumnWidthChanged));
        public static void SetMaxColumnWidth(DependencyObject i, double Value) => i.SetValue(MaxColumnWidthProperty, Value);
        public static double GetMaxColumnWidth(DependencyObject i) => (double)i.GetValue(MaxColumnWidthProperty);
        static void OnMaxColumnWidthChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => UpdateColumnDefinition(i, column => column.MaxWidth = new OldNew<double>(e).New);

        public static readonly DependencyProperty ShowSplitterProperty = DependencyProperty.Register(nameof(ShowSplitter), typeof(bool), typeof(ResizableGrid), new PropertyMetadata(true));
        public bool ShowSplitter
        {
            get => (bool)GetValue(ShowSplitterProperty);
            set => SetValue(ShowSplitterProperty, value);
        }

        public static readonly DependencyProperty SplitterWidthProperty = DependencyProperty.Register(nameof(SplitterWidth), typeof(double), typeof(ResizableGrid), new PropertyMetadata(3.0, OnSplitterWidthChanged));
        public double SplitterWidth
        {
            get => (double)GetValue(SplitterWidthProperty);
            set => SetValue(SplitterWidthProperty, value);
        }
        static void OnSplitterWidthChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ResizableGrid>().OnSplitterWidthChanged(new OldNew<double>(e));

        #endregion

        #region Methods

        ContentPresenter GenerateContainer(object Item)
        {
            return new ContentPresenter
            {
                Content = Item,
                ContentTemplate = ItemTemplate
            };
        }

        static void UpdateColumnDefinition(DependencyObject Child, Action<ColumnDefinition> UpdateAction)
        {
            var Grid = VisualTreeHelper.GetParent(Child) as ResizableGrid;
            if (Grid != null)
            {
                var Column = GetColumn((UIElement)Child);
                if (Column < Grid.ColumnDefinitions.Count)
                    Grid.Dispatcher.BeginInvoke(new Action(() => UpdateAction(Grid.ColumnDefinitions[Column])));
            }
        }

        protected virtual ColumnDefinition GetColumnDefinition(ContentPresenter Child, int Index)
        {
            return new ColumnDefinition
            {
                MaxWidth = GetMaxColumnWidth(Child),
                MinWidth = GetMinColumnWidth(Child),
                Width = GetColumnWidth(Child),
            };
        }

        protected virtual void OnItemContainerStyleChanged(OldNew<Style> input)
        {
            Children.OfType<ContentPresenter>().ForEach(i => i.Style = input.New);
        }

        protected virtual void OnItemsSourceChange(OldNew<IEnumerable> input)
        {
            Children.Clear();
            ColumnDefinitions.Clear();

            if (input.New != null)
            {
                var Columns = input.New.Cast<object>().Select(GenerateContainer).ToArray();
                if (Columns.Count() > 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                    {
                        for (int i = 0; ; i++)
                        {
                            var Child = Columns[i];
                            Child.ClipToBounds = true;
                            SetColumn(Child, i);
                            Children.Add(Child);

                            ColumnDefinitions.Add(GetColumnDefinition(Child, i));

                            if (i == Columns.Length - 1) break;

                            var s = new GridSplitter
                            {
                                HorizontalAlignment = HorizontalAlignment.Right,
                                ResizeBehavior = GridResizeBehavior.CurrentAndNext,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Visibility = ShowSplitter.Visibility(),
                                Width = SplitterWidth,
                            };

                            SetColumn(s, i);
                            Children.Add(s);
                        }
                    }));
                }
            }
        }

        protected virtual void OnItemTemplateChanged(OldNew<DataTemplate> input)
        {
            Children.OfType<ContentPresenter>().ForEach(i => i.ContentTemplate = input.New);
        }

        protected virtual void OnSplitterWidthChanged(OldNew<double> input)
        {
            Children.OfType<GridSplitter>().ForEach(i => i.Width = input.New);
        }

        #endregion
    }
}