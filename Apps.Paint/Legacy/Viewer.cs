using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Paint
{
    public class Viewer : UserControl
    {
        double RelativeX;

        double RelativeY;

        ScrollViewer PART_ScrollViewer;

        public static DependencyProperty CanvasAngleProperty = DependencyProperty.Register("CanvasAngle", typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double CanvasAngle
        {
            get
            {
                return (double)GetValue(CanvasAngleProperty);
            }
            set
            {
                SetValue(CanvasAngleProperty, value);
            }
        }

        public static DependencyProperty RulerLengthProperty = DependencyProperty.Register("RulerLength", typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(30.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double RulerLength
        {
            get
            {
                return (double)GetValue(RulerLengthProperty);
            }
            set
            {
                SetValue(RulerLengthProperty, value);
            }
        }

        public static DependencyProperty RulerVisibilityProperty = DependencyProperty.Register("RulerVisibility", typeof(Visibility), typeof(Viewer), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Visibility RulerVisibility
        {
            get
            {
                return (Visibility)GetValue(RulerVisibilityProperty);
            }
            set
            {
                SetValue(RulerVisibilityProperty, value);
            }
        }

        public static DependencyProperty ScrollViewerProperty = DependencyProperty.Register("ScrollViewer", typeof(ScrollViewer), typeof(Viewer), new FrameworkPropertyMetadata(default(ScrollViewer), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ScrollViewer ScrollViewer
        {
            get
            {
                return (ScrollViewer)GetValue(ScrollViewerProperty);
            }
            set
            {
                SetValue(ScrollViewerProperty, value);
            }
        }

        public static DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, OnZoomCoerced));
        public double Zoom
        {
            get
            {
                return (double)GetValue(ZoomProperty);
            }
            set
            {
                SetValue(ZoomProperty, value);
            }
        }
        static object OnZoomCoerced(DependencyObject d, object Value)
        {
            return d.As<Viewer>().OnZoomCoerced(Value);
        }

        public static DependencyProperty ZoomIncrementProperty = DependencyProperty.Register("ZoomIncrement", typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(0.01, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, OnZoomIncrementCoerced));
        public double ZoomIncrement
        {
            get
            {
                return (double)GetValue(ZoomIncrementProperty);
            }
            set
            {
                SetValue(ZoomIncrementProperty, value);
            }
        }
        static object OnZoomIncrementCoerced(DependencyObject d, object Value)
        {
            return d.As<Viewer>().OnZoomIncrementCoerced(Value);
        }

        public static DependencyProperty ZoomMaximumProperty = DependencyProperty.Register("ZoomMaximum", typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(50.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, OnZoomMaximumCoerced));
        public double ZoomMaximum
        {
            get
            {
                return (double)GetValue(ZoomMaximumProperty);
            }
            set
            {
                SetValue(ZoomMaximumProperty, value);
            }
        }
        static object OnZoomMaximumCoerced(DependencyObject d, object Value)
        {
            return d.As<Viewer>().OnZoomMaximumCoerced(Value);
        }

        public static DependencyProperty ZoomMinimumProperty = DependencyProperty.Register("ZoomMinimum", typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(0.05, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, OnZoomMinimumCoerced));
        public double ZoomMinimum
        {
            get
            {
                return (double)GetValue(ZoomMinimumProperty);
            }
            set
            {
                SetValue(ZoomMinimumProperty, value);
            }
        }
        static object OnZoomMinimumCoerced(DependencyObject d, object Value)
        {
            return d.As<Viewer>().OnZoomMinimumCoerced(Value);
        }

        public Viewer() : base()
        {
            DefaultStyleKey = typeof(Viewer);
        }

        static double CalculateOffset(double extent, double viewPort, double scrollWidth, double relBefore)
        {
            double Offset = relBefore * extent - 0.5 * viewPort;
            //If negative due to initial values, center content
            if (Offset < 0) Offset = 0.5 * scrollWidth;
            return Offset;
        }

        void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer ScrollViewer = sender.As<ScrollViewer>();
            //Check if the content size has changed
            if (e.ExtentWidthChange != 0 || e.ExtentHeightChange != 0)
            {
                //Set accordingly
                ScrollViewer.ScrollToHorizontalOffset(CalculateOffset(e.ExtentWidth, e.ViewportWidth, ScrollViewer.ScrollableWidth, RelativeX));
                ScrollViewer.ScrollToVerticalOffset(CalculateOffset(e.ExtentHeight, e.ViewportHeight, ScrollViewer.ScrollableHeight, RelativeY));
            }
            else
            {
                //Store relative values if normal scroll
                RelativeX = (e.HorizontalOffset + 0.5 * e.ViewportWidth) / e.ExtentWidth;
                RelativeY = (e.VerticalOffset + 0.5 * e.ViewportHeight) / e.ExtentHeight;
            }
        }

        void OnScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(ScrollViewerProperty, sender as ScrollViewer);
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);
            if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
            {
                if (e.Delta > 0)
                {
                    if (Zoom + ZoomIncrement <= ZoomMaximum)
                        Zoom += ZoomIncrement;
                }
                else
                {
                    if (Zoom - ZoomIncrement >= ZoomMinimum)
                        Zoom -= ZoomIncrement;
                }
            }
        }

        protected virtual object OnZoomCoerced(object Value)
        {
            return ((double)Value).Coerce(ZoomMaximum, ZoomMinimum);
        }

        protected virtual object OnZoomIncrementCoerced(object Value)
        {
            return ((double)Value).Coerce(ZoomMaximum, ZoomMinimum);
        }

        protected virtual object OnZoomMaximumCoerced(object Value)
        {
            return ((double)Value).Coerce(100.0, Zoom);
        }

        protected virtual object OnZoomMinimumCoerced(object Value)
        {
            return ((double)Value).Coerce(Zoom, 0.001);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_ScrollViewer = Template.FindName("PART_ScrollViewer", this) as ScrollViewer;

            if (PART_ScrollViewer != default(ScrollViewer))
            {
                PART_ScrollViewer.Loaded += OnScrollViewerLoaded;
                PART_ScrollViewer.ScrollChanged += OnScrollChanged;
            }
        }
    }
}