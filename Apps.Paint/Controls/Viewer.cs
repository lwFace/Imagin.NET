using Imagin.Common.Linq;
using Imagin.Common.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Paint
{
    public class Viewer : UserControl
    {
        double RelativeX;

        double RelativeY;

        ContentPresenter PART_Cursor;

        ScrollViewer PART_ScrollViewer;
        
        public static DependencyProperty CanvasAngleProperty = DependencyProperty.Register("CanvasAngle", typeof(double), typeof(Viewer), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.None));
        public double CanvasAngle
        {
            get => (double)GetValue(CanvasAngleProperty);
            set => SetValue(CanvasAngleProperty, value);
        }

        public static DependencyProperty CompassVisibilityProperty = DependencyProperty.Register("CompassVisibility", typeof(Visibility), typeof(Viewer), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.None));
        public Visibility CompassVisibility
        {
            get => (Visibility)GetValue(CompassVisibilityProperty);
            set => SetValue(CompassVisibilityProperty, value);
        }

        public static DependencyProperty CursorTemplateProperty = DependencyProperty.Register("CursorTemplate", typeof(DataTemplate), typeof(Viewer), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.None));
        public DataTemplate CursorTemplate
        {
            get => (DataTemplate)GetValue(CursorTemplateProperty);
            set => SetValue(CursorTemplateProperty, value);
        }

        public static readonly DependencyProperty ResolutionProperty = DependencyProperty.Register("Resolution", typeof(float), typeof(Viewer), new FrameworkPropertyMetadata(72f, FrameworkPropertyMetadataOptions.None));
        public float Resolution
        {
            get => (float)GetValue(ResolutionProperty);
            set => SetValue(ResolutionProperty, value);
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
            get => (ScrollViewer)GetValue(ScrollViewerProperty);
            set => SetValue(ScrollViewerProperty, value);
        }

        public static DependencyProperty SurfaceProperty = DependencyProperty.Register("Surface", typeof(DataTemplate), typeof(Viewer), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.None));
        public DataTemplate Surface
        {
            get => (DataTemplate)GetValue(SurfaceProperty);
            set => SetValue(SurfaceProperty, value);
        }

        public static DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(GraphicalUnit), typeof(Viewer), new FrameworkPropertyMetadata(GraphicalUnit.Pixel, FrameworkPropertyMetadataOptions.None));
        public GraphicalUnit Unit
        {
            get => (GraphicalUnit)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
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

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            PART_Cursor.Visibility = Visibility.Visible;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            PART_Cursor.Visibility = Visibility.Collapsed;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var point = e.GetPosition(this);
            var x = point.X;
            var y = point.Y;

            var center = new Point(x, y);
            x = center.X - (PART_Cursor.ActualWidth / 2.0);
            y = center.Y - (PART_Cursor.ActualHeight / 2.0);

            Canvas.SetLeft(PART_Cursor, x);
            Canvas.SetTop(PART_Cursor, y);
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

            PART_Cursor = Template.FindName("PART_Cursor", this) as ContentPresenter;
            
            PART_ScrollViewer = Template.FindName("PART_ScrollViewer", this) as ScrollViewer;
            if (PART_ScrollViewer != default(ScrollViewer))
            {
                PART_ScrollViewer.Loaded += OnScrollViewerLoaded;
                PART_ScrollViewer.ScrollChanged += OnScrollChanged;
            }
        }
    }
}
