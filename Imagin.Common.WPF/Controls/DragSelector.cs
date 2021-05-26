using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// Provides logic for drag selecting over an ItemsControl.
    /// </summary>
    internal sealed class DragSelector : Base
    {
        #region Properties

        /// <summary>
        /// Element defined in template that wraps <see cref="ItemsPresenter"/>; used for mouse events.
        /// </summary>
        Panel panel;

        ItemsControl itemsControl;

        /// <summary>
        /// <see cref="System.Windows.Controls.ScrollContentPresenter"/> associated with <see cref="scrollViewer"/>.
        /// </summary>
        ScrollContentPresenter scrollContentPresenter;

        /// <summary>
        /// Scrolling element defined in template.
        /// </summary>
        ScrollViewer scrollViewer { get; set; } = default(ScrollViewer);

        /// ..........................................................................

        /// <summary>
        /// Indicates if we're currently dragging.
        /// </summary>
        bool isDragging = false;

        /// <summary>
        /// Stores reference to previously selected area.
        /// </summary>
        Rect previousSelection { get; set; } = new Rect();

        SelectionMode selectionMode
        {
            get
            {
                bool result()
                {
                    if (itemsControl is ListBox)
                        return itemsControl.As<ListBox>().SelectionMode == System.Windows.Controls.SelectionMode.Single;

                    if (itemsControl is DataGrid)
                        return itemsControl.As<DataGrid>().SelectionMode == DataGridSelectionMode.Single;

                    if (itemsControl is TreeView)
                        return TreeViewExtensions.GetSelectionMode(itemsControl.As<TreeView>()) == TreeViewSelectionMode.Single;

                    return false;
                }
                return result() ? SelectionMode.Single : SelectionMode.Multiple;
            }
        }

        List<Rect> selections { get; set; } = new List<Rect>();

        /// <summary>
        /// Point indicating where the drag started.
        /// </summary>
        Point startPoint
        {
            get; set;
        }

        DoubleRegion selection = new DoubleRegion(0, 0, 0, 0);
        public DoubleRegion Selection
        {
            get
            {
                return selection;
            }
            set
            {
                selection = value;
                OnPropertyChanged("Selection");
            }
        }

        #endregion

        #region DragSelector

        public DragSelector(ItemsControl itemsControl) : base()
        {
            this.itemsControl = itemsControl;
            foreach (var e in this.itemsControl.GetVisualChildren<FrameworkElement>())
            {
                if (e.Is<ScrollViewer>())
                {
                    scrollViewer = e as ScrollViewer;
                    break;
                }
            }

            if (scrollViewer == null)
                throw new NullReferenceException($"{nameof(ScrollViewer)}");
        }

        #endregion

        #region Methods

        #region Handlers

        void Initialize(object sender, EventArgs e) => Initialize(GetScrollContentPresenter());

        //....................................................................

        /// <summary>
        /// Occurs when mouse is down; starts drag.
        /// </summary>
        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && selectionMode == SelectionMode.Multiple)
            {
                isDragging = true;

                panel.CaptureMouse();
                startPoint = e.GetPosition(panel);

                if (!ModifierKeys.Control.Pressed() && !ModifierKeys.Shift.Pressed())
                    Try.Invoke(() => selections.Clear());

                previousSelection = new Rect();
            }
        }

        /// <summary>
        /// Ocurrs when mouse moves; drag is evaluated.
        /// </summary>
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var selection = GetSelection(startPoint, e.GetPosition(panel));
                if (isDragging)
                {
                    Selection.X = selection.X;
                    Selection.Y = selection.Y;
                    Selection.Height = selection.Height;
                    Selection.Width = selection.Width;

                    Select(itemsControl, new Rect(scrollContentPresenter.TranslatePoint(new Point(Selection.TopLeft.X, Selection.TopLeft.Y), scrollContentPresenter), scrollContentPresenter.TranslatePoint(new Point(Selection.BottomRight.X, Selection.BottomRight.Y), scrollContentPresenter)));
                    Scroll(e.GetPosition(itemsControl));
                }
            }
        }

        /// <summary>
        /// Occurs when mouse is up; ends drag.
        /// </summary>
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && isDragging)
            {
                isDragging = false;

                if (panel.IsMouseCaptured)
                    panel.ReleaseMouseCapture();

                if (!Try.Invoke(() => selections.Add(previousSelection)))
                    Try.Invoke(() => selections.Clear());

                Selection.X = 0;
                Selection.Y = 0;
                Selection.Height = 0;
                Selection.Width = 0;

                startPoint = default(Point);
            }
        }

        #endregion

        #region Private

        Rect GetSelection(Point a, Point b)
        {
            b = new Point(b.X.Coerce(panel.ActualWidth), b.Y.Coerce(panel.ActualHeight));

            double
                x = a.X < b.X ? a.X : b.X,
                y = a.Y < b.Y ? a.Y : b.Y;

            //Now, the point is precisely what it should be
            var width = (a.X > b.X ? a.X - b.X : b.X - a.X).Absolute();
            var height = (a.Y > b.Y ? a.Y - b.Y : b.Y - a.Y).Absolute();
            return new Rect(new Point(x, y), new Size(width, height));
        }

        //....................................................................

        /// <summary>
        /// Find and store reference to <see cref="ScrollContentPresenter"/> by searching <see cref="scrollViewer"/> template.
        /// </summary>
        ScrollContentPresenter GetScrollContentPresenter()
        {
            if (scrollContentPresenter == null)
            {
                foreach (var e in scrollViewer.GetVisualChildren<FrameworkElement>())
                {
                    if (e is ScrollContentPresenter)
                        return e as ScrollContentPresenter;
                }
            }
            return null;
        }

        //....................................................................

        Rect GetItemBounds(FrameworkElement Item)
        {
            var topLeft = Item.TranslatePoint(new Point(0, 0), panel);
            return new Rect(topLeft.X, topLeft.Y, Item.ActualWidth, Item.ActualHeight);
        }

        //....................................................................

        /// <summary>
        /// Scroll based on current position.
        /// </summary>
        /// <param name="point"></param>
        void Scroll(Point point)
        {
            double x = point.X, y = point.Y;

            var scrollOffset = ItemsControlExtensions.GetDragScrollOffset(itemsControl);
            var scrollTolerance = ItemsControlExtensions.GetDragScrollTolerance(itemsControl);

            //Cursor is at top, scroll up.
            if (y < scrollTolerance)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - scrollOffset);
            }
            //Cursor is at bottom, scroll down.  
            else if (y > itemsControl.ActualHeight - scrollTolerance) //Bottom of visible list?
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + scrollOffset);
            }
            //Cursor is at left, scroll left.  
            else if (x < scrollTolerance)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - scrollOffset);
            }
            //Cursor is at right, scroll right.  
            else if (x > itemsControl.ActualWidth - scrollTolerance)
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + scrollOffset);
        }

        //....................................................................

        /// <summary>
        /// Gets whether or not the given <see cref="Rect"/> intersects with either of the other two given <see cref="Rect"/>s: True if first, false if second, null if neither.
        /// </summary>
        /// <param name="Rect1"></param>
        /// <param name="Rect2"></param>
        /// <param name="Rect3"></param>
        /// <returns></returns>
        bool? IntersectsWith(Rect Rect1, Rect Rect2, Rect Rect3)
        {
            if (Rect1.IntersectsWith(Rect2))
                return true;

            if (Rect1.IntersectsWith(Rect3))
                return false;

            return null;
        }

        /// <summary>
        /// Gets whether or not the given <see cref="Rect"/> intersects with any previous selection.
        /// </summary>
        /// <param name="Bounds"></param>
        /// <returns></returns>
        bool? IntersectedWith(Rect Bounds)
        {
            var u = 0;
            var v = false;

            Try.Invoke(() =>
            {
                foreach (var y in selections)
                {
                    if (Bounds.IntersectsWith(y))
                    {
                        v = u % 2 == 0;
                        u++;
                    }
                }
            }, e => u = 0);
            return u == 0 ? null : (bool?)v;
        }

        //....................................................................

        /// <summary>
        /// Select items in control based on given area.
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Area"></param>
        void Select(ItemsControl Control, Rect Area)
        {
            foreach (var i in Control.Items)
            {
                //Get visual object from data object
                var item = i is FrameworkElement ? i as FrameworkElement : Control.ItemContainerGenerator.ContainerFromItem(i) as FrameworkElement;

                if (item == null || item.Visibility != Visibility.Visible)
                    continue;

                //Get item bounds
                var itemBounds = GetItemBounds(item);

                //Check if current (or previous) selection intersects with item bounds
                var intersectsWith = IntersectsWith(itemBounds, Area, previousSelection);

                var result = default(bool?);
                if ((ModifierKeys.Control.Pressed() || ModifierKeys.Shift.Pressed()))
                {
                    //Check whether or not the current item intersects with any previous selection
                    var intersectedWith = IntersectedWith(itemBounds);

                    //If current item has never insected with a previous selection...
                    if (intersectedWith == null)
                    {
                        result = intersectsWith;
                    }
                    else
                    {
                        result = intersectedWith.Value;
                        //If current item also intersects with current (or previous) selection, flip it once more
                        if (intersectsWith != null && intersectsWith.Value)
                            result = !result;
                    }
                }
                else result = intersectsWith;

                //If we are allowed to make a selection, make it
                if (result != null)
                    item.TrySelect(result.Value);

                //If visible TreeViewItem, enumerate its children
                if (item is TreeViewItem)
                    Select(item as ItemsControl, Area);
            }
            previousSelection = Area;
        }

        #endregion

        #region Public

        void Initialize(ScrollContentPresenter input)
        {
            if (scrollContentPresenter != null || input == null)
                return;

            scrollContentPresenter = input;
            foreach (var e in scrollContentPresenter.GetVisualChildren<FrameworkElement>())
            {
                if (e is Panel)
                {
                    panel = e as Panel;
                    break;
                }
            }

            if (panel == null)
            {
                //throw new NullReferenceException($"{nameof(Panel)}");
                //Using this instead seems to prevent designer crash, but it is unclear if this comes with implications
                return;
            }

            var dragSelection = new DragSelection();

            try
            {
                panel.Children.Add(dragSelection);
            }
            catch
            {
                //Sometimes, we're not allowed to add anything :(
                return;
            }

            dragSelection.Bind(DragSelection.SelectionProperty, new Binding()
            {
                Path = new PropertyPath(nameof(Selection)),
                Source = this
            });

            //Required for mouse events
            panel.Background = Brushes.Transparent;

            panel.MouseDown += OnMouseDown;
            panel.MouseMove += OnMouseMove;
            panel.MouseUp += OnMouseUp;
        }

        //....................................................................

        public void Initialize()
        {
            scrollViewer.LayoutUpdated += Initialize;
            scrollViewer.Loaded += Initialize;
        }

        public void Deinitialize()
        {
            if (panel != null)
            {
                panel.MouseDown -= OnMouseDown;
                panel.MouseMove -= OnMouseMove;
                panel.MouseUp -= OnMouseUp;
            }

            scrollViewer.LayoutUpdated -= Initialize;
            scrollViewer.Loaded -= Initialize;
        }

        #endregion

        #endregion
    }
}