using Imagin.Common.Collections.Generic;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class TreeView : System.Windows.Controls.TreeView
    {
        #region Properties

        static TreeViewItem SelectedItemOnMouseUp;

        bool SelectedIndexChangeHandled = false;

        bool SelectedItemChangeHandled = false;

        bool SelectedObjectChangeHandled = false;

        /// <summary>
        /// Occurs when one or more items are selected or unselected.
        /// </summary>
        public event EventHandler<EventArgs<IList<object>>> SelectedItemsChanged;

        public static DependencyProperty CanResizeColumnsProperty = DependencyProperty.Register(nameof(CanResizeColumns), typeof(bool), typeof(TreeView), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanResizeColumns
        {
            get => (bool)GetValue(CanResizeColumnsProperty);
            set => SetValue(CanResizeColumnsProperty, value);
        }

        public static DependencyProperty ColumnHeaderContextMenuProperty = DependencyProperty.Register(nameof(ColumnHeaderContextMenu), typeof(ContextMenu), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ContextMenu ColumnHeaderContextMenu
        {
            get => (ContextMenu)GetValue(ColumnHeaderContextMenuProperty);
            set => SetValue(ColumnHeaderContextMenuProperty, value);
        }

        public static DependencyProperty ColumnHeaderHeightProperty = DependencyProperty.Register(nameof(ColumnHeaderHeight), typeof(double), typeof(TreeView), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double ColumnHeaderHeight
        {
            get => (double)GetValue(ColumnHeaderHeightProperty);
            set => SetValue(ColumnHeaderHeightProperty, value);
        }

        public static DependencyProperty ColumnHeaderStyleProperty = DependencyProperty.Register(nameof(ColumnHeaderStyle), typeof(Style), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style ColumnHeaderStyle
        {
            get => (Style)GetValue(ColumnHeaderStyleProperty);
            set => SetValue(ColumnHeaderStyleProperty, value);
        }

        public static DependencyProperty ColumnHeaderStyleSelectorProperty = DependencyProperty.Register(nameof(ColumnHeaderStyleSelector), typeof(StyleSelector), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public StyleSelector ColumnHeaderStyleSelector
        {
            get => (StyleSelector)GetValue(ColumnHeaderStyleSelectorProperty);
            set => SetValue(ColumnHeaderStyleSelectorProperty, value);
        }

        public static DependencyProperty ColumnHeaderTemplateProperty = DependencyProperty.Register(nameof(ColumnHeaderTemplate), typeof(DataTemplate), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ColumnHeaderTemplate
        {
            get => (DataTemplate)GetValue(ColumnHeaderTemplateProperty);
            set => SetValue(ColumnHeaderTemplateProperty, value);
        }

        public static DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.Register(nameof(ColumnHeaderTemplateSelector), typeof(DataTemplateSelector), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplateSelector ColumnHeaderTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ColumnHeaderTemplateSelectorProperty);
            set => SetValue(ColumnHeaderTemplateSelectorProperty, value);
        }

        public static DependencyProperty ColumnHeaderStringFormatProperty = DependencyProperty.Register(nameof(ColumnHeaderStringFormat), typeof(string), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string ColumnHeaderStringFormat
        {
            get => (string)GetValue(ColumnHeaderStringFormatProperty);
            set => SetValue(ColumnHeaderStringFormatProperty, value);
        }

        public static DependencyProperty ColumnHeaderVisibilityProperty = DependencyProperty.Register(nameof(ColumnHeaderVisibility), typeof(Visibility), typeof(TreeView), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Visibility ColumnHeaderVisibility
        {
            get => (Visibility)GetValue(ColumnHeaderVisibilityProperty);
            set => SetValue(ColumnHeaderVisibilityProperty, value);
        }

        public static DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(TreeViewColumnCollection), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TreeViewColumnCollection Columns
        {
            get => (TreeViewColumnCollection)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public static DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(int[]), typeof(TreeView), new FrameworkPropertyMetadata(default(int[]), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIndexChanged));
        public int[] SelectedIndex
        {
            get => (int[])GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }
        static void OnSelectedIndexChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TreeView>().OnSelectedIndexChanged(new OldNew<int[]>(e));

        public static DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof(SelectedItems), typeof(Collection<object>), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Get or set list of selected items.
        /// </summary>
        public Collection<object> SelectedItems
        {
            get => (Collection<object>)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        public static DependencyProperty SelectedObjectProperty = DependencyProperty.Register(nameof(SelectedObject), typeof(object), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedObjectChanged));
        /// <summary>
        /// Get or set selected object.
        /// </summary>
        public object SelectedObject
        {
            get => GetValue(SelectedObjectProperty);
            set => SetValue(SelectedObjectProperty, value);
        }
        static void OnSelectedObjectChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TreeView>().OnSelectedObjectChanged(e.NewValue);

        public static DependencyProperty SelectedVisualProperty = DependencyProperty.Register(nameof(SelectedVisual), typeof(TreeViewItem), typeof(TreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Get or set visual associated with selected object.
        /// </summary>
        public TreeViewItem SelectedVisual
        {
            get => (TreeViewItem)GetValue(SelectedVisualProperty);
            set => SetValue(SelectedVisualProperty, value);
        }

        #endregion

        #region TreeView

        public TreeView() : base()
        {
            DefaultStyleKey = typeof(TreeView);

            Columns = new TreeViewColumnCollection();
            SelectedIndex = new int[1] { -1 };
            SelectedItems = new Collection<object>();

            GotFocus += OnGotFocus;

            SelectedItemChanged += OnSelectedItemChanged;
            SelectedItems.Changed += OnSelectedItemsChanged;

            this.Bind(TreeViewExtensions.SelectedItemsProperty, new Binding()
            {
                Path = new PropertyPath(nameof(SelectedItems)),
                Source = this 
            });
        }

        #endregion

        #region Methods

        static IEnumerable<int> GetSelectedIndex(TreeViewItem Item)
        {
            var Source = Item.To<ItemsControl>();
            var Parent = default(ItemsControl);

            while (true)
            {
                Parent = Source.As<DependencyObject>().GetParent<ItemsControl>();

                if (Parent != null)
                {
                    yield return Parent.Items.IndexOf(Source.DataContext);
                    Source = Parent;
                }

                if (Parent == null || Parent is TreeView)
                    break;
            }

            yield break;
        }

        static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            SelectedItemOnMouseUp = null;
            if (!e.OriginalSource.Is<TreeView>())
            {
                var Item = (e.OriginalSource as DependencyObject).GetVisualParent<TreeViewItem>();
                if (Item != null && Item.Is<TreeViewItem>())
                {
                    if (Mouse.LeftButton == MouseButtonState.Pressed && TreeViewItemExtensions.GetIsSelected(Item) && Keyboard.Modifiers != ModifierKeys.Control)
                    {
                        SelectedItemOnMouseUp = Item;
                    }
                    else if (sender is TreeView && TreeViewExtensions.GetSelectionMode(sender as TreeView) == TreeViewSelectionMode.Single)
                    {
                        TreeViewExtensions.SelectItem(sender as TreeView, Item);
                    }
                    else (sender as TreeView).SelectItems(Item);
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            var Item = (e.OriginalSource as DependencyObject).GetVisualParent<TreeViewItem>();
            if (TreeViewExtensions.GetSelectionMode(this) == TreeViewSelectionMode.Extended && Item != null && Item.IsFocused)
                OnGotFocus(this, e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            var Item = (e.OriginalSource as DependencyObject).GetVisualParent<TreeViewItem>();
            if (TreeViewExtensions.GetSelectionMode(this) == TreeViewSelectionMode.Extended && Item == SelectedItemOnMouseUp)
                this.SelectItems(Item);
        }

        protected virtual void OnSelectedIndexChanged(OldNew<int[]> input)
        {
            if (!SelectedIndexChangeHandled)
            {
                if (Items.Count > 0)
                {
                    var Result = default(object);
                    this.Enumerate((i, j) => Result = i, input.New);
                    SelectedObject = Result;
                }
            }
        }

        protected virtual void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!SelectedItemChangeHandled)
            {
                SelectedObjectChangeHandled = true;
                SelectedObject = e.NewValue;
                SelectedObjectChangeHandled = false;

                SelectedVisual = null;

                this.Enumerate((i, j) =>
                {
                    if (i == e.NewValue)
                    {
                        SelectedVisual = j as TreeViewItem;
                        return null;
                    }
                    return true;
                });

                if (SelectedVisual != null)
                {
                    SelectedIndexChangeHandled = true;
                    SelectedIndex = GetSelectedIndex(SelectedVisual).Reverse<int>().ToArray<int>();
                    SelectedIndexChangeHandled = false;
                }
            }
        }

        protected virtual void OnSelectedItemsChanged(object sender)
        {
            var Collection = new List<object>();
            SelectedItems.ForEach<object>(i => Collection.Add(i));
            SelectedItemsChanged?.Invoke(this, new EventArgs<IList<object>>(Collection));
        }

        protected virtual void OnSelectedObjectChanged(object Value)
        {
            if (!SelectedObjectChangeHandled)
            {
                SelectedItemChangeHandled = true;
                this.Select(Value);
                SelectedItemChangeHandled = false;
            }
        }

        ICommand collapseAllCommand;
        public ICommand CollapseAllCommand
        {
            get
            {
                collapseAllCommand = collapseAllCommand ?? new RelayCommand(() => this.CollapseAll(), () => true);
                return collapseAllCommand;
            }
        }

        #endregion
    }
}