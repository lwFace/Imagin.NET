using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public static class FrameworkElementExtensions
    {
        #region ScrollAnimationBehavior

        public static class FindVisualChildHelper
        {
            public static T GetFirstChildOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
            {
                if (dependencyObject == null)
                {
                    return null;
                }

                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyObject, i);

                    var result = (child as T) ?? GetFirstChildOfType<T>(child);

                    if (result != null)
                    {
                        return result;
                    }
                }

                return null;
            }
        }

        public static class ScrollAnimationBehavior
        {
            static ScrollViewer _listBoxScroller = new ScrollViewer();

            #region VerticalOffset

            public static DependencyProperty VerticalOffsetProperty = DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollAnimationBehavior), new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));
            public static void SetVerticalOffset(FrameworkElement target, double value)
            {
                target.SetValue(VerticalOffsetProperty, value);
            }
            public static double GetVerticalOffset(FrameworkElement target)
            {
                return (double)target.GetValue(VerticalOffsetProperty);
            }
            static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
            {
                var ScrollViewer = target as ScrollViewer;

                if (ScrollViewer != null)
                    ScrollViewer.ScrollToVerticalOffset((double)e.NewValue);
            }

            #endregion

            #region TimeDuration

            public static DependencyProperty TimeDurationProperty = DependencyProperty.RegisterAttached("TimeDuration", typeof(TimeSpan), typeof(ScrollAnimationBehavior), new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 0)));
            public static void SetTimeDuration(FrameworkElement target, TimeSpan value)
            {
                target.SetValue(TimeDurationProperty, value);
            }
            public static TimeSpan GetTimeDuration(FrameworkElement target)
            {
                return (TimeSpan)target.GetValue(TimeDurationProperty);
            }

            #endregion

            #region PointsToScroll

            public static DependencyProperty PointsToScrollProperty = DependencyProperty.RegisterAttached("PointsToScroll", typeof(double), typeof(ScrollAnimationBehavior), new PropertyMetadata(0.0));
            public static void SetPointsToScroll(FrameworkElement target, double value)
            {
                target.SetValue(PointsToScrollProperty, value);
            }
            public static double GetPointsToScroll(FrameworkElement target)
            {
                return (double)target.GetValue(PointsToScrollProperty);
            }

            #endregion

            #region IsEnabled

            public static DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ScrollAnimationBehavior), new UIPropertyMetadata(false, OnIsEnabledChanged));
            public static void SetIsEnabled(FrameworkElement target, bool value)
            {
                target.SetValue(IsEnabledProperty, value);
            }
            public static bool GetIsEnabled(FrameworkElement target)
            {
                return (bool)target.GetValue(IsEnabledProperty);
            }
            static void OnIsEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            {
                var target = sender;

                if (target != null && target is ScrollViewer)
                {
                    ScrollViewer scroller = target as ScrollViewer;
                    scroller.Loaded += new RoutedEventHandler(scrollerLoaded);
                }

                if (target != null && target is ListBox)
                {
                    ListBox listbox = target as ListBox;
                    listbox.Loaded += new RoutedEventHandler(listboxLoaded);
                }
            }

            #endregion

            #region AnimateScroll Helper

            private static void AnimateScroll(ScrollViewer scrollViewer, double ToValue)
            {
                DoubleAnimation verticalAnimation = new DoubleAnimation();

                verticalAnimation.From = scrollViewer.VerticalOffset;
                verticalAnimation.To = ToValue;
                verticalAnimation.Duration = new Duration(GetTimeDuration(scrollViewer));

                Storyboard storyboard = new Storyboard();

                storyboard.Children.Add(verticalAnimation);
                Storyboard.SetTarget(verticalAnimation, scrollViewer);
                Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(ScrollAnimationBehavior.VerticalOffsetProperty));
                storyboard.Begin();
            }

            #endregion

            #region NormalizeScrollPos Helper

            private static double NormalizeScrollPos(ScrollViewer scroll, double scrollChange, Orientation o)
            {
                double returnValue = scrollChange;

                if (scrollChange < 0)
                {
                    returnValue = 0;
                }

                if (o == Orientation.Vertical && scrollChange > scroll.ScrollableHeight)
                {
                    returnValue = scroll.ScrollableHeight;
                }
                else if (o == Orientation.Horizontal && scrollChange > scroll.ScrollableWidth)
                {
                    returnValue = scroll.ScrollableWidth;
                }

                return returnValue;
            }

            #endregion

            #region UpdateScrollPosition Helper

            private static void UpdateScrollPosition(object sender)
            {
                ListBox listbox = sender as ListBox;

                if (listbox != null)
                {
                    double scrollTo = 0;

                    for (int i = 0; i < (listbox.SelectedIndex); i++)
                    {
                        ListBoxItem tempItem = listbox.ItemContainerGenerator.ContainerFromItem(listbox.Items[i]) as ListBoxItem;

                        if (tempItem != null)
                        {
                            scrollTo += tempItem.ActualHeight;
                        }
                    }

                    AnimateScroll(_listBoxScroller, scrollTo);
                }
            }

            #endregion

            #region SetEventHandlersForScrollViewer Helper

            private static void SetEventHandlersForScrollViewer(ScrollViewer scroller)
            {
                scroller.PreviewMouseWheel += new MouseWheelEventHandler(ScrollViewerPreviewMouseWheel);
                scroller.PreviewKeyDown += new KeyEventHandler(ScrollViewerPreviewKeyDown);
            }

            #endregion

            #region scrollerLoaded Event Handler

            private static void scrollerLoaded(object sender, RoutedEventArgs e)
            {
                ScrollViewer scroller = sender as ScrollViewer;

                SetEventHandlersForScrollViewer(scroller);
            }

            #endregion

            #region listboxLoaded Event Handler

            private static void listboxLoaded(object sender, RoutedEventArgs e)
            {
                ListBox listbox = sender as ListBox;

                _listBoxScroller = FindVisualChildHelper.GetFirstChildOfType<ScrollViewer>(listbox);
                SetEventHandlersForScrollViewer(_listBoxScroller);

                SetTimeDuration(_listBoxScroller, new TimeSpan(0, 0, 0, 0, 200));
                SetPointsToScroll(_listBoxScroller, 16.0);

                listbox.SelectionChanged += new SelectionChangedEventHandler(ListBoxSelectionChanged);
                listbox.Loaded += new RoutedEventHandler(ListBoxLoaded);
                listbox.LayoutUpdated += new EventHandler(ListBoxLayoutUpdated);
            }

            #endregion

            #region ScrollViewerPreviewMouseWheel Event Handler

            private static void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
            {
                double mouseWheelChange = (double)e.Delta;
                ScrollViewer scroller = (ScrollViewer)sender;
                double newVOffset = GetVerticalOffset(scroller) - (mouseWheelChange / 3);

                if (newVOffset < 0)
                {
                    AnimateScroll(scroller, 0);
                }
                else if (newVOffset > scroller.ScrollableHeight)
                {
                    AnimateScroll(scroller, scroller.ScrollableHeight);
                }
                else
                {
                    AnimateScroll(scroller, newVOffset);
                }

                e.Handled = true;
            }

            #endregion

            #region ScrollViewerPreviewKeyDown Handler

            private static void ScrollViewerPreviewKeyDown(object sender, KeyEventArgs e)
            {
                ScrollViewer scroller = (ScrollViewer)sender;

                Key keyPressed = e.Key;
                double newVerticalPos = GetVerticalOffset(scroller);
                bool isKeyHandled = false;

                if (keyPressed == Key.Down)
                {
                    newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos + GetPointsToScroll(scroller)), Orientation.Vertical);
                    isKeyHandled = true;
                }
                else if (keyPressed == Key.PageDown)
                {
                    newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos + scroller.ViewportHeight), Orientation.Vertical);
                    isKeyHandled = true;
                }
                else if (keyPressed == Key.Up)
                {
                    newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos - GetPointsToScroll(scroller)), Orientation.Vertical);
                    isKeyHandled = true;
                }
                else if (keyPressed == Key.PageUp)
                {
                    newVerticalPos = NormalizeScrollPos(scroller, (newVerticalPos - scroller.ViewportHeight), Orientation.Vertical);
                    isKeyHandled = true;
                }

                if (newVerticalPos != GetVerticalOffset(scroller))
                {
                    AnimateScroll(scroller, newVerticalPos);
                }

                e.Handled = isKeyHandled;
            }

            #endregion

            #region ListBox Event Handlers

            static void ListBoxLayoutUpdated(object sender, EventArgs e)
            {
                UpdateScrollPosition(sender);
            }

            static void ListBoxLoaded(object sender, RoutedEventArgs e)
            {
                UpdateScrollPosition(sender);
            }

            static void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                UpdateScrollPosition(sender);
            }

            #endregion
        }

        #endregion

        #region Properties

        #region Extends

        public static readonly DependencyProperty ExtendsProperty = DependencyProperty.RegisterAttached("Extends", typeof(bool), typeof(FrameworkElementExtensions), new PropertyMetadata(false, OnExtendsChanged));
        public static bool GetExtends(FrameworkElement d)
            => (bool)d.GetValue(ExtendsProperty);
        public static void SetExtends(FrameworkElement d, bool value)
            => d.SetValue(ExtendsProperty, value);
        static void OnExtendsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            frameworkElement.SizeChanged -= Extends_OnSizeChanged;

            if ((bool)e.NewValue)
                frameworkElement.SizeChanged += Extends_OnSizeChanged;
        }

        static void Extends_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            SetActualHeight(frameworkElement, frameworkElement.ActualHeight);
            SetActualWidth(frameworkElement, frameworkElement.ActualWidth);
        }

        #endregion

        #region ActualHeight

        public static readonly DependencyProperty ActualHeightProperty = DependencyProperty.RegisterAttached("ActualHeight", typeof(double), typeof(FrameworkElementExtensions), new PropertyMetadata(0.0));
        public static double GetActualHeight(FrameworkElement d)
            => (double)d.GetValue(ActualHeightProperty);
        public static void SetActualHeight(FrameworkElement d, double value)
            => d.SetValue(ActualHeightProperty, value);

        #endregion

        #region ActualWidth

        public static readonly DependencyProperty ActualWidthProperty = DependencyProperty.RegisterAttached("ActualWidth", typeof(double), typeof(FrameworkElementExtensions), new PropertyMetadata(0.0));
        public static double GetActualWidth(FrameworkElement d)
            => (double)d.GetValue(ActualWidthProperty);
        public static void SetActualWidth(FrameworkElement d, double value)
            => d.SetValue(ActualWidthProperty, value);

        #endregion

        #region Content

        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached("Content", typeof(object), typeof(FrameworkElementExtensions), new PropertyMetadata(null, OnContentChanged));
        public static object GetContent(FrameworkElement i) => i.GetValue(ContentProperty);
        public static void SetContent(FrameworkElement i, object value) => i.SetValue(ContentProperty, value);
        static void OnContentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement i)
            {
                if (e.NewValue is FrameworkElement j)
                    j.DataContext = i.DataContext;
            }
        }

        #endregion

        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(FrameworkElementExtensions), new PropertyMetadata(default(CornerRadius)));
        public static CornerRadius GetCornerRadius(FrameworkElement d)
            => (CornerRadius)d.GetValue(CornerRadiusProperty);
        public static void SetCornerRadius(FrameworkElement d, CornerRadius value)
            => d.SetValue(CornerRadiusProperty, value);

        #endregion

        #region DragMoveWindow

        public static readonly DependencyProperty DragMoveWindowProperty = DependencyProperty.RegisterAttached("DragMoveWindow", typeof(bool), typeof(FrameworkElementExtensions), new PropertyMetadata(false, OnDragMoveWindowChanged));
        public static bool GetDragMoveWindow(FrameworkElement d)
            => (bool)d.GetValue(DragMoveWindowProperty);
        public static void SetDragMoveWindow(FrameworkElement d, bool value)
            => d.SetValue(DragMoveWindowProperty, value);
        static void OnDragMoveWindowChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as FrameworkElement).MouseDown -= DragMoveWindow_MouseDown;

            if ((bool)e.NewValue)
                (sender as FrameworkElement).MouseDown += DragMoveWindow_MouseDown;
        }

        static void DragMoveWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                sender.As<FrameworkElement>().GetParent<Window>()?.DragMove();
        }

        #endregion

        #region EnableContextMenu

        static readonly DependencyProperty contextMenuProperty = DependencyProperty.RegisterAttached("contextMenu", typeof(ContextMenu), typeof(FrameworkElementExtensions), new PropertyMetadata(default(ContextMenu)));
        static ContextMenu GetContextMenu(FrameworkElement d)
            => (ContextMenu)d.GetValue(contextMenuProperty);
        static void SetContextMenu(FrameworkElement d, ContextMenu value)
            => d.SetValue(contextMenuProperty, value);

        public static readonly DependencyProperty EnableContextMenuProperty = DependencyProperty.RegisterAttached("EnableContextMenu", typeof(bool), typeof(FrameworkElementExtensions), new PropertyMetadata(true, OnEnableContextMenuChanged));
        public static bool GetEnableContextMenu(FrameworkElement d)
            => (bool)d.GetValue(EnableContextMenuProperty);
        public static void SetEnableContextMenu(FrameworkElement d, bool value)
            => d.SetValue(EnableContextMenuProperty, value);
        static void OnEnableContextMenuChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement != null)
            {
                if ((bool)e.NewValue)
                {
                    frameworkElement.ContextMenu = frameworkElement.ContextMenu ?? GetContextMenu(frameworkElement);
                    SetContextMenu(frameworkElement, null);
                }
                else
                {
                    SetContextMenu(frameworkElement, frameworkElement.ContextMenu);
                    frameworkElement.ContextMenu = null;
                }
            }
        }

        #endregion

        #region HorizontalAlignment

        public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.RegisterAttached("HorizontalAlignment", typeof(HorizontalAlignment?), typeof(FrameworkElementExtensions), new PropertyMetadata(null));
        public static HorizontalAlignment? GetHorizontalAlignment(FrameworkElement d)
            => (HorizontalAlignment?)d.GetValue(HorizontalAlignmentProperty);
        public static void SetHorizontalAlignment(FrameworkElement d, HorizontalAlignment? value)
            => d.SetValue(HorizontalAlignmentProperty, value);

        #endregion

        #region Margin

        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin", typeof(Thickness?), typeof(FrameworkElementExtensions), new PropertyMetadata(null));
        public static Thickness? GetMargin(FrameworkElement d)
            => (Thickness?)d.GetValue(MarginProperty);
        public static void SetMargin(FrameworkElement d, Thickness? value)
            => d.SetValue(MarginProperty, value);

        #endregion

        #region ShellContextMenu

        public static readonly DependencyProperty ShellContextMenuProperty = DependencyProperty.RegisterAttached("ShellContextMenu", typeof(string), typeof(FrameworkElementExtensions), new PropertyMetadata(null, OnShellContextMenuChanged));
        public static string GetShellContextMenu(FrameworkElement d)
            => (string)d.GetValue(ShellContextMenuProperty);
        public static void SetShellContextMenu(FrameworkElement d, string value)
            => d.SetValue(ShellContextMenuProperty, value);
        static void OnShellContextMenuChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            frameworkElement.PreviewMouseRightButtonUp -= ShellContextMenu_PreviewMouseRightButtonUp;
            if (e.NewValue?.ToString().NullOrEmpty() == false)
                frameworkElement.PreviewMouseRightButtonUp += ShellContextMenu_PreviewMouseRightButtonUp;
        }

        static void ShellContextMenu_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;

            var path = GetShellContextMenu(frameworkElement);
            FileSystemInfo result = null;

            Try.Invoke(() =>
            {
                result = Storage.File.Long.Exists(path)
                ? (FileSystemInfo)new FileInfo(path)
                : new DirectoryInfo(path);
            });

            result.If(i => i != null, i =>
            {
                var point = frameworkElement.PointToScreen(e.GetPosition(frameworkElement));
                Storage.ShellContextMenu.Show(point.Int32(), i);
            });
        }

        #endregion

        #region VerticalAlignment

        public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.RegisterAttached("VerticalAlignment", typeof(VerticalAlignment?), typeof(FrameworkElementExtensions), new PropertyMetadata(null));
        public static VerticalAlignment? GetVerticalAlignment(FrameworkElement d)
            => (VerticalAlignment?)d.GetValue(VerticalAlignmentProperty);
        public static void SetVerticalAlignment(FrameworkElement d, VerticalAlignment? value)
            => d.SetValue(VerticalAlignmentProperty, value);

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Helper method to determine if the given <see cref="FrameworkElement"/> has the mouse over it or not.
        /// </summary>
        /// <param name="input">The <see cref="FrameworkElement"/> to test for mouse containment.</param>
        /// <returns>True, if the mouse is over the FrameworkElement; false, otherwise.</returns>
        public static bool ContainsMouse(this FrameworkElement input)
        {
            var point = Mouse.GetPosition(input);
            return
            (
                point.X >= 0
                &&
                point.X <= input.ActualWidth
                &&
                point.Y >= 0
                &&
                point.Y <= input.ActualHeight
            );
        }

        public static Style FindStyle<Element>(this Element input) where Element : FrameworkElement
            => (Style)input.FindResource(typeof(Element));

        public static int Index(this FrameworkElement input, uint origin = 0)
        {
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(input);
            var index = itemsControl.ItemContainerGenerator.IndexFromContainer(input);
            return origin.Int32() + index;
        }

        public static System.Drawing.Bitmap Render(this FrameworkElement input)
        {
            try
            {
                input.Measure(new Size(input.ActualWidth, input.ActualHeight));
                input.Arrange(new Rect(new Size(input.ActualWidth, input.ActualHeight)));

                var bmp = new RenderTargetBitmap((int)input.ActualWidth, (int)input.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(input);

                var stream = new MemoryStream();

                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(stream);

                return new System.Drawing.Bitmap(stream);
            }
            catch
            {
                return null;
            }
        }

        public static bool TryFindStyle<Element>(this Element input, out Style style) where Element : FrameworkElement
        {
            try
            {
                style = input.FindStyle();
                return true;
            }
            catch
            {
                style = default(Style);
                return false;
            }
        }

        #endregion
    }
}