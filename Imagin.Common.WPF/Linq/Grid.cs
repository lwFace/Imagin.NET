using Imagin.Common.Converters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    public static class GridExtensions
    {
        #region Columns

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached("Columns", typeof(GridLength[]), typeof(GridExtensions), new PropertyMetadata(null, OnColumnsChanged));
        [TypeConverter(typeof(GridLengthArrayTypeConverter))]
        public static GridLength[] GetColumns(FrameworkElement d)
            => (GridLength[])d.GetValue(ColumnsProperty);
        public static void SetColumns(FrameworkElement d, GridLength[] value)
            => d.SetValue(ColumnsProperty, value);
        static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var columns = (GridLength[])e.NewValue;

            grid.ColumnDefinitions.Clear();
            if (e.NewValue != null)
            {
                for (var i = 0; i < columns.Length; i++)
                {
                    var columnDefinition = new ColumnDefinition();
                    columnDefinition.Width = columns[i];
                    grid.ColumnDefinitions.Add(columnDefinition);
                }
            }
        }

        #endregion

        #region Rows

        public static readonly DependencyProperty RowsProperty = DependencyProperty.RegisterAttached("Rows", typeof(GridLength[]), typeof(GridExtensions), new PropertyMetadata(null, OnRowsChanged));
        [TypeConverter(typeof(GridLengthArrayTypeConverter))]
        public static GridLength[] GetRows(FrameworkElement d)
            => (GridLength[])d.GetValue(RowsProperty);
        public static void SetRows(FrameworkElement d, GridLength[] value)
            => d.SetValue(RowsProperty, value);
        static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var rows = (GridLength[])e.NewValue;

            grid.RowDefinitions.Clear();
            if (e.NewValue != null)
            {
                for (var i = 0; i < rows.Length; i++)
                {
                    var rowDefinition = new RowDefinition();
                    rowDefinition.Height = rows[i];
                    grid.RowDefinitions.Add(rowDefinition);
                }
            }
        }

        #endregion
    }
}