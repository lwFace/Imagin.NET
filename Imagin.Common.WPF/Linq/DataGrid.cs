using Imagin.Common.Collections;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    public static class DataGridExtensions
    {
        #region Extends

        public static readonly DependencyProperty ExtendsProperty = DependencyProperty.RegisterAttached("Extends", typeof(bool), typeof(DataGridExtensions), new PropertyMetadata(false, OnExtendsChanged));
        public static bool GetExtends(DataGrid i) => (bool)i.GetValue(ExtendsProperty);
        public static void SetExtends(DataGrid i, bool value) => i.SetValue(ExtendsProperty, value);
        static void OnExtendsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid)
            {
                var d = sender as DataGrid;
                d.SelectionChanged -= OnSelectionChanged;

                if ((bool)e.NewValue)
                    d.SelectionChanged += OnSelectionChanged;
            }
        }

        static void OnSelectionChanged(object sender, SelectionChangedEventArgs e) => SetSelectedItems((DataGrid)sender, e.AddedItems);

        #endregion

        #region AddCommand

        /// <summary>
        ///  The following will be kept indefinitely for reference purposes; demonstrates how
        ///  to subscribe commands to controls via attached properties. You would register
        ///  the command in another attached property with the following line: 
        ///  dataGrid.CommandBindings.Add(new CommandBinding(AddCommand, OnAddCommandExecuted, OnAddCommandCanExecute));
        ///  This particular command allows you to add a new item to an existing collection that 
        ///  is bound to the DataGrid instance. It is not ideal to use a command like this because
        ///  generally you want more control over how an object ultimately gets created in your app!
        ///  This one assumes your object has a blank constructor and uses reflection, which is probably
        ///  slower!
        /// </summary>
        public static readonly RoutedUICommand AddCommand = new RoutedUICommand("AddCommand", "AddCommand", typeof(DataGridExtensions));
        static void OnAddCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid.ItemsSource is IList list)
            {
                Type result = default;
                foreach (var i in list.GetType().Inheritance())
                {
                    var j = i.GetGenericArguments();
                    if (j.Length == 1)
                    {
                        result = j[0];
                        break;
                    }
                }

                if (result != null)
                {
                    object instance = null;
                    Try.Invoke(() => instance = Activator.CreateInstance(result));

                    if (instance != null)
                        list.Add(instance);
                }
            }
        }
        static void OnAddCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = (sender as DataGrid).CanUserAddRows;

        #endregion

        #region DisplayRowNumber

        public static DependencyProperty DisplayRowNumberProperty = DependencyProperty.RegisterAttached("DisplayRowNumber", typeof(bool), typeof(DataGridExtensions), new FrameworkPropertyMetadata(false, OnDisplayRowNumberChanged));
        public static bool GetDisplayRowNumber(DataGrid i) => (bool)i.GetValue(DisplayRowNumberProperty);
        public static void SetDisplayRowNumber(DataGrid i, bool value) => i.SetValue(DisplayRowNumberProperty, value);
        static void OnDisplayRowNumberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            dataGrid.LayoutUpdated -= DisplayRowNumber_OnLayoutUpdated;
            dataGrid.SizeChanged -= DisplayRowNumber_OnSizeChanged;

            dataGrid.SelectionChanged -= DisplayRowNumber_OnSelectionChanged;

            dataGrid.LoadingRow -= DisplayRowNumber_OnLoadingRow;
            dataGrid.UnloadingRow -= DisplayRowNumber_OnLoadingRow;

            if ((bool)e.NewValue)
            {
                dataGrid.LayoutUpdated += DisplayRowNumber_OnLayoutUpdated;
                dataGrid.SizeChanged += DisplayRowNumber_OnSizeChanged;

                dataGrid.SelectionChanged += DisplayRowNumber_OnSelectionChanged;

                dataGrid.LoadingRow += DisplayRowNumber_OnLoadingRow;
                dataGrid.UnloadingRow += DisplayRowNumber_OnLoadingRow;

                if (dataGrid.IsLoaded)
                    UpdateAll(dataGrid);
            }
            else dataGrid.GetVisualChildren<DataGridRow>().ToList().ForEach(i => i.Header = string.Empty);
        }

        static void UpdateAll(DataGrid dataGrid) => dataGrid.GetVisualChildren<DataGridRow>().ForEach(i => UpdateRow(dataGrid, i));

        static void UpdateRow(DataGrid dataGrid, DataGridRow dataGridRow) => dataGridRow.Header = dataGridRow.GetIndex() + GetDisplayRowNumberOffset(dataGrid);

        static void DisplayRowNumber_OnSelectionChanged(object sender, EventArgs e) => UpdateAll((DataGrid)sender);

        static void DisplayRowNumber_OnLayoutUpdated(object sender, EventArgs e) => UpdateAll((DataGrid)sender);

        static void DisplayRowNumber_OnSizeChanged(object sender, SizeChangedEventArgs e) => UpdateAll((DataGrid)sender);

        static void DisplayRowNumber_OnLoadingRow(object sender, DataGridRowEventArgs e) => UpdateRow(sender as DataGrid, e.Row);

        #endregion

        #region DisplayRowNumberOffset

        public static DependencyProperty DisplayRowNumberOffsetProperty = DependencyProperty.RegisterAttached("DisplayRowNumberOffset", typeof(int), typeof(DataGridExtensions), new FrameworkPropertyMetadata(0, OnDisplayRowNumberOffsetChanged));
        public static int GetDisplayRowNumberOffset(DependencyObject i) => (int)i.GetValue(DisplayRowNumberOffsetProperty);
        public static void SetDisplayRowNumberOffset(DependencyObject i, int value) => i.SetValue(DisplayRowNumberOffsetProperty, value);
        static void OnDisplayRowNumberOffsetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var offset = (int)e.NewValue;

            if (GetDisplayRowNumber(dataGrid))
                dataGrid.GetVisualChildren<DataGridRow>().ForEach(i => i.Header = i.GetIndex() + offset);
        }

        #endregion

        #region ScrollAddedIntoView

        static Dictionary<int, DataGrid> ScrollAddedIntoView_Instances = new Dictionary<int, DataGrid>();

        public static readonly DependencyProperty ScrollAddedIntoViewProperty = DependencyProperty.RegisterAttached("ScrollAddedIntoView", typeof(bool), typeof(DataGridExtensions), new PropertyMetadata(false, OnScrollAddedIntoViewChanged));
        public static bool GetScrollAddedIntoView(DataGrid i) => (bool)i.GetValue(ScrollAddedIntoViewProperty);
        public static void SetScrollAddedIntoView(DataGrid i, bool value) => i.SetValue(ScrollAddedIntoViewProperty, value);
        static void OnScrollAddedIntoViewChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            dataGrid.Loaded -= ScrollAddedIntoView_OnLoaded;

            if (dataGrid?.ItemsSource is ICollect i)
            {
                ScrollAddedIntoView_Subscribe(dataGrid, i, (bool)e.NewValue);
            }
            else dataGrid.Loaded += ScrollAddedIntoView_OnLoaded;
        }

        static void ScrollAddedIntoView_Subscribe(DataGrid sender, ICollect i, bool subscribe)
        {
            var h = i.GetHashCode();
            if (subscribe)
            {
                ScrollAddedIntoView_Instances.Add(h, sender);
                i.Added += ScrollAddedIntoView_OnItemAdded;
            }
            else
            {
                i.Added -= ScrollAddedIntoView_OnItemAdded;
                ScrollAddedIntoView_Instances.Remove(h);
            }
        }

        static void ScrollAddedIntoView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid?.ItemsSource is ICollect i)
            {
                dataGrid.Loaded -= ScrollAddedIntoView_OnLoaded;
                ScrollAddedIntoView_Subscribe(dataGrid, i, GetScrollAddedIntoView(dataGrid));
            }
        }

        static void ScrollAddedIntoView_OnItemAdded(object sender, EventArgs<object> e)
        {
            ScrollAddedIntoView_Instances[sender.As<ICollect>().GetHashCode()]?.As<DataGrid>()?.ScrollIntoView(e.Value);
        }

        #endregion

        #region SelectedItems

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(DataGridExtensions), new PropertyMetadata(null));
        public static IList GetSelectedItems(DataGrid i) => (IList)i.GetValue(SelectedItemsProperty);
        public static void SetSelectedItems(DataGrid i, IList value) => i.SetValue(SelectedItemsProperty, value);

        #endregion
    }
}