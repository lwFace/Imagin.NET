using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public class TreeViewColumnHeadersPresenter : ResizableGrid
    {
        public static DependencyProperty CanResizeColumnsProperty = DependencyProperty.Register(nameof(CanResizeColumns), typeof(bool), typeof(TreeViewColumnHeadersPresenter), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanResizeColumns
        {
            get => (bool)GetValue(CanResizeColumnsProperty);
            set => SetValue(CanResizeColumnsProperty, value);
        }

        public static DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(TreeViewColumnCollection), typeof(TreeViewColumnHeadersPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColumnsChanged));
        public TreeViewColumnCollection Columns
        {
            get => (TreeViewColumnCollection)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }
        static void OnColumnsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((TreeViewColumnHeadersPresenter)i).OnColumnsChanged(new OldNew<TreeViewColumnCollection>(e));

        protected override ColumnDefinition GetColumnDefinition(ContentPresenter child, int index)
        {
            var result = base.GetColumnDefinition(child, index);
            if (child.Content != null && child.Content is TreeViewColumn)
            {
                var column = child.Content as TreeViewColumn;

                result = new ColumnDefinition();
                BindingOperations.SetBinding(result, ColumnDefinition.WidthProperty, new Binding()
                {
                    Path = new PropertyPath(nameof(TreeViewColumn.Width)),
                    Mode = BindingMode.TwoWay,
                    Source = column
                });
                BindingOperations.SetBinding(result, ColumnDefinition.MinWidthProperty, new Binding()
                {
                    Path = new PropertyPath(nameof(TreeViewColumn.MinWidth)),
                    Mode = BindingMode.TwoWay,
                    Source = column
                });
                BindingOperations.SetBinding(result, ColumnDefinition.MaxWidthProperty, new Binding()
                {
                    Path = new PropertyPath(nameof(TreeViewColumn.MaxWidth)),
                    Mode = BindingMode.TwoWay,
                    Source = column
                });
                BindingOperations.SetBinding(child, VisibilityProperty, new Binding()
                {
                    Converter = new BooleanToVisibilityConverter(),
                    Path = new PropertyPath("(0)", DependencyObjectExtensions.IsVisibleProperty),
                    Mode = BindingMode.OneWay,
                    Source = column
                });
            }
            return result;
        }

        protected virtual void OnColumnsChanged(OldNew<TreeViewColumnCollection> input)
        {
            SetCurrentValue(ItemsSourceProperty, input.New);
        }

        public TreeViewColumnHeadersPresenter() : base() { }
    }
}