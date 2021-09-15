using Imagin.Common.Controls;
using Imagin.Common.Converters;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    public static class ItemsControlExtensions
    {
        #region Properties

        #region AutoSizeColumns

        /// <summary>
        /// Applies GridUnit.Star GridLength to all columns.
        /// </summary>
        public static readonly DependencyProperty AutoSizeColumnsProperty = DependencyProperty.RegisterAttached("AutoSizeColumns", typeof(bool), typeof(ItemsControlExtensions), new PropertyMetadata(false, OnAutoSizeColumnsChanged));
        public static bool GetAutoSizeColumns(ItemsControl d)
        {
            return (bool)d.GetValue(AutoSizeColumnsProperty);
        }
        public static void SetAutoSizeColumns(ItemsControl d, bool value)
        {
            d.SetValue(AutoSizeColumnsProperty, value);
        }
        static void OnAutoSizeColumnsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid)
            {
                var d = sender as DataGrid;
                var l = (bool)e.NewValue ? new DataGridLength(1.0, DataGridLengthUnitType.Star) : new DataGridLength(1.0, DataGridLengthUnitType.Auto);
                d.Columns.ForEach(i => i.Width = l);
            }
            else if (sender is Controls.TreeView t)
            {
                var l = (bool)e.NewValue ? new GridLength(1.0, GridUnitType.Star) : new GridLength(1.0, GridUnitType.Auto);
                t.Columns.ForEach(i => i.Width = l);
            }
        }

        #endregion

        #region CanDragSelect

        /// <summary>
        /// Gets or sets value indicating whether ItemsControl should allow drag selecting items.
        /// </summary>
        public static readonly DependencyProperty CanDragSelectProperty = DependencyProperty.RegisterAttached("CanDragSelect", typeof(bool), typeof(ItemsControlExtensions), new PropertyMetadata(false, OnCanDragSelectChanged));
        public static bool GetCanDragSelect(DependencyObject d)
            => (bool)d.GetValue(CanDragSelectProperty);
        public static void SetCanDragSelect(DependencyObject d, bool value)
            => d.SetValue(CanDragSelectProperty, value);
        static void OnCanDragSelectChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            if (itemsControl != null)
            {
                itemsControl.Loaded -= CanDragSelect_Loaded;
                if ((bool)e.NewValue)
                {
                    itemsControl.Loaded += CanDragSelect_Loaded;
                }
                else
                {
                    if (GetDragSelector(itemsControl) != null)
                        GetDragSelector(itemsControl).Deinitialize();

                    SetDragSelector(itemsControl, null);
                }
            }
        }

        static void CanDragSelect_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            if (GetDragSelector(itemsControl) == null)
            {
                itemsControl.ApplyTemplate();

                var dragSelector = new DragSelector(itemsControl);
                dragSelector.Initialize();

                SetDragSelector(itemsControl, dragSelector);
            }
        }

        #endregion

        #region DragSelector

        internal static readonly DependencyProperty DragSelectorProperty = DependencyProperty.RegisterAttached("DragSelector", typeof(DragSelector), typeof(ItemsControlExtensions), new PropertyMetadata(null));
        internal static DragSelector GetDragSelector(ItemsControl d)
            => (DragSelector)d.GetValue(DragSelectorProperty);
        internal static void SetDragSelector(ItemsControl d, DragSelector value) => d.SetValue(DragSelectorProperty, value);

        #endregion

        #region IsColumnMenuEnabled

        /// <summary>
        /// Determines whether or not to add a ContextMenu to the column header for toggling column visibility.
        /// </summary>
        public static readonly DependencyProperty IsColumnMenuEnabledProperty = DependencyProperty.RegisterAttached("IsColumnMenuEnabled", typeof(bool), typeof(ItemsControlExtensions), new PropertyMetadata(false, IsColumnMenuEnabledChanged));
        public static bool GetIsColumnMenuEnabled(ItemsControl d)
        {
            return (bool)d.GetValue(IsColumnMenuEnabledProperty);
        }
        public static void SetIsColumnMenuEnabled(ItemsControl d, bool value)
        {
            d.SetValue(IsColumnMenuEnabledProperty, value);
        }

        static void IsColumnMenuEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataGrid || sender is System.Windows.Controls.TreeView)
            {
                var i = sender as ItemsControl;
                var h = i.GetHashCode();

                i.Loaded -= RegisterIsColumnMenuEnabled;

                if ((bool)e.NewValue)
                {
                    if (!i.IsLoaded)
                    {
                        i.Loaded += RegisterIsColumnMenuEnabled;
                    }
                    else RegisterIsColumnMenuEnabled(i, true);
                }
                else
                {
                    //TO DO: Remove menu
                    RegisterIsColumnMenuEnabled(i, false);
                }
            }
        }

        static void RegisterIsColumnMenuEnabled(ItemsControl Control, bool IsEnabled)
        {
            if (Control is DataGrid)
            {
                var d = Control as DataGrid;

                var t = new Style(typeof(DataGridColumnHeader));
                t.BasedOn = d.ColumnHeaderStyle;
                t.Setters.Add(new Setter(DataGridColumnHeader.ContextMenuProperty,
                    IsEnabled && d?.Columns.Count >= 0
                    ? GetContextMenu(d)
                    : null));

                d.ColumnHeaderStyle = t;
            }
            else if (Control is Controls.TreeView t)
            {
                t.ColumnHeaderContextMenu = IsEnabled ? GetContextMenu(t) : null;
            }
        }

        static void RegisterIsColumnMenuEnabled(object sender, RoutedEventArgs e)
        {
            var Control = sender as ItemsControl;
            RegisterIsColumnMenuEnabled(Control, GetIsColumnMenuEnabled(Control));
        }

        static MenuItem GetMenuItem(DependencyObject Column)
        {
            var Result = GetMenuItem();

            //Bind model boolean to menu item's check state
            BindingOperations.SetBinding(Result, MenuItem.IsCheckedProperty, new Binding()
            {
                Path = new PropertyPath("(0)", DependencyObjectExtensions.IsVisibleProperty),
                Mode = BindingMode.TwoWay,
                Source = Column
            });

            //Bind model boolean to column visibility
            BindingOperations.SetBinding(Column, DataGridColumn.VisibilityProperty, new Binding()
            {
                Path = new PropertyPath("IsChecked"),
                Mode = BindingMode.OneWay,
                Source = Result,
                Converter = new Imagin.Common.Converters.BooleanToVisibilityConverter()
            });

            return Result;
        }

        static ContextMenu GetContextMenu(Controls.TreeView TreeView)
        {
            var Result = new ContextMenu();
            foreach (var Column in TreeView.Columns)
            {
                if (Column is TreeViewTextColumn || Column is TreeViewTemplateColumn && (Column.Header != null && !Column.Header.ToString().Empty()))
                {
                    if (Column is TreeViewTextColumn && (Column.As<TreeViewTextColumn>().MemberPath.NullOrEmpty()))
                        continue;

                    //Result.Items.Add(GetMenuItem(Column, () => Column.Header == null || Column.Header.ToString().IsEmpty() ? (Column as TreeViewTextColumn).MemberPath : Column.Header.ToString()));
                }
            }
            return Result;
        }

        static ContextMenu GetContextMenu(DataGrid DataGrid)
        {
            var Result = new ContextMenu();
            foreach (var Column in DataGrid.Columns)
            {
                if (Column is DataGridTextColumn || Column is DataGridTemplateColumn)
                {
                    if (Column.Header != null && !Column.Header.ToString().NullOrEmpty() && !Column.SortMemberPath.NullOrEmpty())
                    {
                        var m = GetMenuItem(Column);

                        if (Column.Header != null)
                        {
                            m.SetBinding(MenuItem.HeaderProperty, new Binding()
                            {
                                Mode = BindingMode.OneWay,
                                Path = new PropertyPath("Header"),
                                Source = Column
                            });
                        }
                        else m.Header = Column.SortMemberPath;

                        Result.Items.Add(m);
                    }
                }
            }
            return Result;
        }

        static MenuItem GetMenuItem()
        {
            return new MenuItem()
            {
                IsCheckable = true,
                IsChecked = true,
                StaysOpenOnClick = true
            };
        }

        #endregion

        #region DragScrollOffset

        public static readonly DependencyProperty DragScrollOffsetProperty = DependencyProperty.RegisterAttached("DragScrollOffset", typeof(double), typeof(ItemsControlExtensions), new PropertyMetadata(10.0));
        public static double GetDragScrollOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(DragScrollOffsetProperty);
        }
        public static void SetDragScrollOffset(DependencyObject obj, double value)
        {
            obj.SetValue(DragScrollOffsetProperty, value);
        }

        #endregion

        #region DragScrollTolerance

        public static readonly DependencyProperty DragScrollToleranceProperty = DependencyProperty.RegisterAttached("DragScrollTolerance", typeof(double), typeof(ItemsControlExtensions), new PropertyMetadata(5.0));
        public static double GetDragScrollTolerance(DependencyObject obj)
        {
            return (double)obj.GetValue(DragScrollToleranceProperty);
        }
        public static void SetDragScrollTolerance(DependencyObject obj, double value)
        {
            obj.SetValue(DragScrollToleranceProperty, value);
        }

        #endregion

        /// ----------------------------------------------------------------------------------------------------

        static void GroupSort(ItemsControl itemsControl)
        {
            if (itemsControl.ItemsSource is ListCollectionView || itemsControl.ItemsSource is IList)
            {
                GroupSort_Update(itemsControl);
            }
            else
            {
                itemsControl.Loaded -= GroupSort_OnLoaded;
                itemsControl.Loaded += GroupSort_OnLoaded;
            }
        }

        static void GroupSort(ItemsControl itemsControl, ListCollectionView listCollectionView)
        {
            listCollectionView.GroupDescriptions.Clear();
            listCollectionView.SortDescriptions.Clear();

            if (!GetGroupName(itemsControl).NullOrEmpty())
                listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(GetGroupName(itemsControl), new DefaultConverter<object, object>(i => i, i => i)));

            if (!GetSortName(itemsControl).NullOrEmpty())
                listCollectionView.SortDescriptions.Add(new SortDescription(GetSortName(itemsControl), GetSortDirection(itemsControl)));
        }

        /// ----------------------------------------------------------------------------------------------------

        static void GroupSort_OnLoaded(object sender, RoutedEventArgs e)
        {
            var itemsControl = (ItemsControl)sender;
            itemsControl.Loaded -= GroupSort_OnLoaded;

            if (itemsControl.ItemsSource is ListCollectionView || itemsControl.ItemsSource is IList)
            {
                GroupSort_Update(itemsControl);
                return;
            }

            DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl)).AddValueChanged(itemsControl, GroupSort_OnItemsSourceChanged);
        }

        static void GroupSort_OnItemsSourceChanged(object sender, EventArgs e)
        {
            var itemsControl = (ItemsControl)sender;
            DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl)).RemoveValueChanged(itemsControl, GroupSort_OnItemsSourceChanged);
            GroupSort_Update(itemsControl);
        }

        static void GroupSort_Update(ItemsControl itemsControl)
        {
            if (itemsControl.ItemsSource is ListCollectionView)
            {
                GroupSort(itemsControl, (ListCollectionView)itemsControl.ItemsSource);
            }
            else if (itemsControl.ItemsSource is IList)
            {
                var listCollectionView = new ListCollectionView((IList)itemsControl.ItemsSource);

                itemsControl.SetCurrentValue(ItemsControl.ItemsSourceProperty, listCollectionView);
                GroupSort(itemsControl, listCollectionView);
            }
        }

        /// ----------------------------------------------------------------------------------------------------

        #region GroupName

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached("GroupName", typeof(string), typeof(ItemsControlExtensions), new PropertyMetadata(string.Empty, OnGroupNameChanged));
        public static string GetGroupName(ItemsControl d)
        {
            return (string)d.GetValue(GroupNameProperty);
        }
        public static void SetGroupName(ItemsControl d, string value)
        {
            d.SetValue(GroupNameProperty, value);
        }
        static void OnGroupNameChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GroupSort((ItemsControl)sender);
        }

        #endregion

        #region GroupStyle

        public static readonly DependencyProperty GroupStyleProperty = DependencyProperty.RegisterAttached("GroupStyle", typeof(GroupStyle), typeof(ItemsControlExtensions), new PropertyMetadata(null, OnGroupStyleChanged));
        public static GroupStyle GetGroupStyle(ItemsControl d)
        {
            return (GroupStyle)d.GetValue(GroupStyleProperty);
        }
        public static void SetGroupStyle(ItemsControl d, GroupStyle value)
        {
            d.SetValue(GroupStyleProperty, value);
        }
        static void OnGroupStyleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var groupStyle = ((ItemsControl)sender).GroupStyle;
            groupStyle.Clear();
            groupStyle.Add((GroupStyle)e.NewValue);
        }

        #endregion

        /// ----------------------------------------------------------------------------------------------------

        #region SortName

        public static readonly DependencyProperty SortNameProperty = DependencyProperty.RegisterAttached("SortName", typeof(string), typeof(ItemsControlExtensions), new PropertyMetadata(string.Empty, OnSortNameChanged));
        public static string GetSortName(ItemsControl d)
        {
            return (string)d.GetValue(SortNameProperty);
        }
        public static void SetSortName(ItemsControl d, string value)
        {
            d.SetValue(SortNameProperty, value);
        }
        static void OnSortNameChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GroupSort((ItemsControl)sender);
        }

        #endregion

        #region SortDirection

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.RegisterAttached("SortDirection", typeof(ListSortDirection), typeof(ItemsControlExtensions), new PropertyMetadata(ListSortDirection.Ascending, OnSortDirectionChanged));
        public static ListSortDirection GetSortDirection(ItemsControl d)
        {
            return (ListSortDirection)d.GetValue(SortDirectionProperty);
        }
        public static void SetSortDirection(ItemsControl d, ListSortDirection value)
        {
            d.SetValue(SortDirectionProperty, value);
        }
        static void OnSortDirectionChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GroupSort((ItemsControl)sender);
        }

        #endregion

        /// ----------------------------------------------------------------------------------------------------

        #region SelectNoneOnEmptySpaceClick

        public static readonly DependencyProperty SelectNoneOnEmptySpaceClickProperty = DependencyProperty.RegisterAttached("SelectNoneOnEmptySpaceClick", typeof(bool), typeof(ItemsControlExtensions), new PropertyMetadata(false, OnSelectNoneOnEmptySpaceClickChanged));
        public static bool GetSelectNoneOnEmptySpaceClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectNoneOnEmptySpaceClickProperty);
        }
        public static void SetSelectNoneOnEmptySpaceClick(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectNoneOnEmptySpaceClickProperty, value);
        }
        static void OnSelectNoneOnEmptySpaceClickChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            itemsControl.PreviewMouseDown -= SelectNoneOnEmptySpaceClick_PreviewMouseDown;

            if ((bool)e.NewValue)
                itemsControl.PreviewMouseDown += SelectNoneOnEmptySpaceClick_PreviewMouseDown;
        }

        static void SelectNoneOnEmptySpaceClick_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if ((itemsControl is System.Windows.Controls.TreeView && !e.OriginalSource.Is<TreeViewItem>())
                 || (itemsControl is DataGrid && !e.OriginalSource.Is<DataGridRow>())
                 || (itemsControl is ListBox && !e.OriginalSource.Is<ListBoxItem>()))
                {
                    if ((e.OriginalSource as DependencyObject).GetVisualParent<ScrollBar>() == null)
                    {
                        //This prevents all items from getting unselected when trying to drag select with a modifier key pressed.
                        //The operation would proceed normally anyway, but this is annoying!
                        if (GetCanDragSelect(itemsControl))
                        {
                            if (ModifierKeys.Control.Pressed() || ModifierKeys.Shift.Pressed())
                                return;
                        }

                        itemsControl.TryClearSelection();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Collapse all items in ItemsControl (siblings of <see langword="Source"/>).
        /// </summary>
        /// <param name="itemsControl"></param>
        /// <param name="source"></param>
        internal static void CollapseSiblings(this ItemsControl itemsControl, TreeViewItem source)
        {
            if (itemsControl == null || itemsControl.Items == null) return;
            foreach (var i in itemsControl.Items)
            {
                var c = itemsControl.ItemContainerGenerator.ContainerFromItem(i);
                if (c == null) continue;
                var Child = c.As<TreeViewItem>();
                if (Child != null && !Child.Equals(source))
                    Child.IsExpanded = false;
            }
        }

        //----------------------------------------------------------------------------------------------------

        public static void Select(this ItemsControl itemsControl, object item)
        {
            if (itemsControl != null)
            {
                foreach (var i in itemsControl.Items)
                {
                    var j = (TreeViewItem)itemsControl.ItemContainerGenerator.ContainerFromItem(i);

                    if (item == i)
                    {
                        if (j is TreeViewItem)
                        {
                            TreeViewItemExtensions.SetIsSelected(j as TreeViewItem, true);
                        }
                    }
                    else 
                    {
                        TreeViewItemExtensions.SetIsSelected(j as TreeViewItem, false);
                        Select(j, item);
                    }
                }
            }
        }

        //----------------------------------------------------------------------------------------------------

        public static void ClearSelection(this ItemsControl itemsControl)
        {
            if (itemsControl is ListBox)
            {
                (itemsControl as ListBox).SelectedIndex = -1;
            }
            else if (itemsControl is DataGrid)
            {
                (itemsControl as DataGrid).SelectedIndex = -1;
            }
            else if (itemsControl is System.Windows.Controls.TreeView)
            {
                var treeView = itemsControl.As<System.Windows.Controls.TreeView>();

                var item = treeView.ItemContainerGenerator.ContainerFromItem(treeView.SelectedItem);
                if (item != null)
                    item.As<TreeViewItem>().IsSelected = false;
            }
            else if (itemsControl is System.Windows.Controls.TreeView)
                itemsControl.As<System.Windows.Controls.TreeView>().SelectNone();
        }

        public static bool TryClearSelection(this ItemsControl itemsControl) => Try.Invoke(() => itemsControl.ClearSelection());

        #endregion
    }
}