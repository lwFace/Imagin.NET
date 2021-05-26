using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Paint
{
    public static class UIElementExtensions
    {
        public static void Set(this UIElement input, Point point)
        {
            Canvas.SetLeft(input, point.X);
            Canvas.SetTop(input, point.Y);
        }
    }

    public partial class TransformView : UserControl
    {
        Thumb tl = null, t = null, tr = null, l = null, r = null, bl = null, b = null, br = null;

        public static DependencyProperty BoundsProperty = DependencyProperty.Register(nameof(Bounds), typeof(PointCollection), typeof(TransformView), new FrameworkPropertyMetadata(default(PointCollection), FrameworkPropertyMetadataOptions.None, OnBoundsChanged));
        public PointCollection Bounds
        {
            get => (PointCollection)GetValue(BoundsProperty);
            set => SetValue(BoundsProperty, value);
        }
        protected static void OnBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TransformView).OnBoundsChanged((PointCollection)e.NewValue);
        }
        
        public static DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(VisualLayer), typeof(TransformView), new FrameworkPropertyMetadata(default(VisualLayer), FrameworkPropertyMetadataOptions.None, OnLayerChanged));
        public VisualLayer Layer
        {
            get => (VisualLayer)GetValue(LayerProperty);
            set => SetValue(LayerProperty, value);
        }
        protected static void OnLayerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TransformView).OnLayerChanged((VisualLayer)e.NewValue);
        }

        public static DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(Transform.Modes), typeof(TransformView), new FrameworkPropertyMetadata(Transform.Modes.None, FrameworkPropertyMetadataOptions.None, OnModeChanged));
        public Transform.Modes Mode
        {
            get => (Transform.Modes)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        protected static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TransformView).OnModeChanged((Transform.Modes)e.NewValue);
        }

        public static DependencyProperty ShapeBoundsProperty = DependencyProperty.Register(nameof(ShapeBounds), typeof(PointCollection), typeof(TransformView), new FrameworkPropertyMetadata(default(PointCollection), FrameworkPropertyMetadataOptions.None));
        public PointCollection ShapeBounds
        {
            get => (PointCollection)GetValue(ShapeBoundsProperty);
            set => SetValue(ShapeBoundsProperty, value);
        }

        public static DependencyProperty ThumbStyleProperty = DependencyProperty.Register(nameof(ThumbStyle), typeof(Style), typeof(TransformView), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.None, OnThumbStyleChanged));
        public Style ThumbStyle
        {
            get => (Style)GetValue(ThumbStyleProperty);
            set => SetValue(ThumbStyleProperty, value);
        }
        protected static void OnThumbStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TransformView).OnThumbStyleChanged((Style)e.NewValue);
        }

        //-------------------------------------------------------------------------------

        public TransformView()
        {
            InitializeComponent();
            tl = new Thumb();
            t = new Thumb();
            tr = new Thumb();
            l = new Thumb();
            r = new Thumb();
            bl = new Thumb();
            b = new Thumb();
            br = new Thumb();

            Thumbs.Children.Add(tl);
            Thumbs.Children.Add(t);
            Thumbs.Children.Add(tr);
            Thumbs.Children.Add(l);
            Thumbs.Children.Add(r);
            Thumbs.Children.Add(bl);
            Thumbs.Children.Add(b);
            Thumbs.Children.Add(br);

            SubscribeThumbs();
            foreach (Thumb i in Thumbs.Children)
            {
                i.DragStarted += OnDragStarted;
                i.SetCurrentValue(StyleProperty, ThumbStyle);
            }
        }

        //-------------------------------------------------------------------------------

        void Update(VisualLayer layer)
        {
            SetCurrentValue(BoundsProperty, layer.Bounds);
            //SetCurrentValue(ShapeBoundsProperty, layer is RegionShapeLayer ? (layer as RegionShapeLayer).ShapeBounds : null);
        }

        //-------------------------------------------------------------------------------

        void OnBoundsChanged(PointCollection bounds)
        {
            if (bounds.Count == 4)
            {
                tl.Set(Bounds[0]);
                t.Set(Bounds[0].Between(Bounds[1]));
                tr.Set(Bounds[1]);
                l.Set(Bounds[0].Between(Bounds[3]));
                r.Set(Bounds[1].Between(Bounds[2]));
                bl.Set(Bounds[3]);
                b.Set(Bounds[3].Between(Bounds[2]));
                br.Set(Bounds[2]);
            }
        }

        void OnLayerChanged(VisualLayer layer)
        {
            Update(layer);
        }

        void OnModeChanged(Transform.Modes mode)
        {
            Update(Layer);
        }

        void OnThumbStyleChanged(Style style)
        {
            foreach (Thumb i in Thumbs.Children)
                i.Style = style;
        }

        //-------------------------------------------------------------------------------

        void SubscribeThumbs()
        {
            tl.DragDelta += OnTopLeftDragDelta;
            t.DragDelta += OnTopDragDelta;
            tr.DragDelta += OnTopRightDragDelta;
            l.DragDelta += OnLeftDragDelta;
            r.DragDelta += OnRightDragDelta;
            bl.DragDelta += OnBottomLeftDragDelta;
            b.DragDelta += OnBottomDragDelta;
            br.DragDelta += OnBottomRightDragDelta;

            tl.DragStarted += OnTopLeftDragStarted;
            t.DragStarted += OnTopDragStarted;
            tr.DragStarted += OnTopRightDragStarted;
            l.DragStarted += OnLeftDragStarted;
            r.DragStarted += OnRightDragStarted;
            bl.DragStarted += OnBottomLeftDragStarted;
            b.DragStarted += OnBottomDragStarted;
            br.DragStarted += OnBottomRightDragStarted;
        }

        void UnsubscribeThumbs()
        {
            tl.DragDelta -= OnTopLeftDragDelta;
            t.DragDelta -= OnTopDragDelta;
            tr.DragDelta -= OnTopRightDragDelta;
            l.DragDelta -= OnLeftDragDelta;
            r.DragDelta -= OnRightDragDelta;
            bl.DragDelta -= OnBottomLeftDragDelta;
            b.DragDelta -= OnBottomDragDelta;
            br.DragDelta -= OnBottomRightDragDelta;

            tl.DragStarted -= OnTopLeftDragStarted;
            t.DragStarted -= OnTopDragStarted;
            tr.DragStarted -= OnTopRightDragStarted;
            l.DragStarted -= OnLeftDragStarted;
            r.DragStarted -= OnRightDragStarted;
            bl.DragStarted -= OnBottomLeftDragStarted;
            b.DragStarted -= OnBottomDragStarted;
            br.DragStarted -= OnBottomRightDragStarted;
        }

        //-------------------------------------------------------------------------------

        PointCollection Clone(PointCollection points)
        {
            var result = new PointCollection();
            points.ForEach(i => result.Add(i));
            return result;
        }

        double GetRadians()
        {
            var currentPoint = System.Windows.Input.Mouse.GetPosition(Thumbs);
            System.Windows.Vector deltaVector = Point.Subtract(currentPoint, centerPoint);
            double angle = System.Windows.Vector.AngleBetween(startVector, deltaVector);
            return angle.FromDegreeToRadian();
        }

        void DoRotate(PointCollection points)
        {
            var radians = GetRadians();

            double x(Point p) => Math.Cos(radians) * (p.X - centerPoint.X) - Math.Sin(radians) * (p.Y - centerPoint.Y) + centerPoint.X;
            double y(Point p) => Math.Sin(radians) * (p.X - centerPoint.X) + Math.Cos(radians) * (p.Y - centerPoint.Y) + centerPoint.X;

            points[0] = new Point(x(startPoints[0]), y(startPoints[0]));
            points[1] = new Point(x(startPoints[1]), y(startPoints[1]));
            points[2] = new Point(x(startPoints[2]), y(startPoints[2]));
            points[3] = new Point(x(startPoints[3]), y(startPoints[3]));
        }

        private void OnTopLeftDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case Transform.Modes.Distort:
                    break;
                case Transform.Modes.Perspective:
                    break;
                case Transform.Modes.Rotate:
                    DoRotate(result);
                    break;
                case Transform.Modes.Scale:
                    result[3] = new Point(startPoints[3].X + e.HorizontalChange, startPoints[3].Y);
                    result[0] = new Point(startPoints[0].X + e.HorizontalChange, startPoints[0].Y + e.VerticalChange);
                    result[1] = new Point(startPoints[1].X, startPoints[1].Y + e.VerticalChange);
                    break;
                case Transform.Modes.Skew:
                    /*
                    //First, figure out if we want to skew up/down or left/right
                    var newBounds = new PointCollection();
                    Bounds.ForEach(i => newBounds.Add(i));
                    newBounds[0] = new Point(startPoint.X + e.HorizontalChange, startPoint.Y + e.VerticalChange);
                    SetCurrentValue(BoundsProperty, newBounds);
                    */
                    break;
                case Transform.Modes.Warp:
                    break;
            }
            SetCurrentValue(BoundsProperty, result);
        }

        private void OnTopDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case Transform.Modes.Distort:
                    break;
                case Transform.Modes.Perspective:
                    break;
                case Transform.Modes.Rotate:
                    DoRotate(result);
                    break;
                case Transform.Modes.Scale:
                    result[0] = new Point(startPoints[0].X, startPoints[0].Y + e.VerticalChange);
                    result[1] = new Point(startPoints[1].X, startPoints[1].Y + e.VerticalChange);
                    break;
                case Transform.Modes.Skew:
                    break;
                case Transform.Modes.Warp:
                    break;
            }
            SetCurrentValue(BoundsProperty, result);
        }

        private void OnTopRightDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case Transform.Modes.Distort:
                    break;
                case Transform.Modes.Perspective:
                    break;
                case Transform.Modes.Rotate:
                    DoRotate(result);
                    break;
                case Transform.Modes.Scale:
                    result[0] = new Point(startPoints[0].X, startPoints[0].Y + e.VerticalChange);
                    result[1] = new Point(startPoints[1].X + e.HorizontalChange, startPoints[1].Y + e.VerticalChange);
                    result[2] = new Point(startPoints[2].X + e.HorizontalChange, startPoints[2].Y);
                    break;
                case Transform.Modes.Skew:
                    /*
                    var newBounds = new PointCollection();
                    Bounds.ForEach(i => newBounds.Add(i));
                    newBounds[1] = new Point(startPoint.X + e.HorizontalChange, startPoint.Y + e.VerticalChange);
                    SetCurrentValue(BoundsProperty, newBounds);
                    */
                    break;
                case Transform.Modes.Warp:
                    break;
            }
            SetCurrentValue(BoundsProperty, result);
        }

        private void OnLeftDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case Transform.Modes.Distort:
                    break;
                case Transform.Modes.Perspective:
                    break;
                case Transform.Modes.Rotate:
                    DoRotate(result);
                    break;
                case Transform.Modes.Scale:
                    result[0] = new Point(startPoints[0].X + e.HorizontalChange, startPoints[0].Y);
                    result[3] = new Point(startPoints[3].X + e.HorizontalChange, startPoints[3].Y);
                    break;
                case Transform.Modes.Skew:
                    break;
                case Transform.Modes.Warp:
                    break;
            }
            SetCurrentValue(BoundsProperty, result);
        }

        private void OnRightDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case Transform.Modes.Distort:
                    break;
                case Transform.Modes.Perspective:
                    break;
                case Transform.Modes.Rotate:
                    DoRotate(result);
                    break;
                case Transform.Modes.Scale:
                    result[1] = new Point(startPoints[1].X + e.HorizontalChange, startPoints[1].Y);
                    result[2] = new Point(startPoints[2].X + e.HorizontalChange, startPoints[2].Y);
                    break;
                case Transform.Modes.Skew:
                    break;
                case Transform.Modes.Warp:
                    break;
            }
            SetCurrentValue(BoundsProperty, result);
        }

        private void OnBottomLeftDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case Transform.Modes.Distort:
                    break;
                case Transform.Modes.Perspective:
                    break;
                case Transform.Modes.Rotate:
                    DoRotate(result);
                    break;
                case Transform.Modes.Scale:
                    result[0] = new Point(startPoints[0].X + e.HorizontalChange, startPoints[0].Y);
                    result[2] = new Point(startPoints[2].X , startPoints[2].Y + e.VerticalChange);
                    result[3] = new Point(startPoints[3].X + e.HorizontalChange, startPoints[3].Y + e.VerticalChange);
                    break;
                case Transform.Modes.Skew:
                    /*
                    var newBounds = new PointCollection();
                    Bounds.ForEach(i => newBounds.Add(i));
                    newBounds[3] = new Point(startPoint.X + e.HorizontalChange, startPoint.Y + e.VerticalChange);
                    SetCurrentValue(BoundsProperty, newBounds);
                    */
                    break;
                case Transform.Modes.Warp:
                    break;
            }
            SetCurrentValue(BoundsProperty, result);
        }

        private void OnBottomDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case Transform.Modes.Distort:
                    break;
                case Transform.Modes.Perspective:
                    break;
                case Transform.Modes.Rotate:
                    DoRotate(result);
                    break;
                case Transform.Modes.Scale:
                    result[3] = new Point(startPoints[3].X, startPoints[3].Y + e.VerticalChange);
                    result[2] = new Point(startPoints[2].X, startPoints[2].Y + e.VerticalChange);
                    break;
                case Transform.Modes.Skew:
                    break;
                case Transform.Modes.Warp:
                    break;
            }
            SetCurrentValue(BoundsProperty, result);
        }

        private void OnBottomRightDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case Transform.Modes.Distort:
                    break;
                case Transform.Modes.Perspective:
                    DoRotate(result);
                    break;
                case Transform.Modes.Rotate:
                    break;
                case Transform.Modes.Scale:
                    result[1] = new Point(startPoints[1].X + e.HorizontalChange, startPoints[1].Y);
                    result[2] = new Point(startPoints[2].X + e.HorizontalChange, startPoints[2].Y + e.VerticalChange);
                    result[3] = new Point(startPoints[3].X, startPoints[3].Y + e.VerticalChange);
                    break;
                case Transform.Modes.Skew:
                    /*
                    var newBounds = new PointCollection();
                    Bounds.ForEach(i => newBounds.Add(i));
                    newBounds[2] = new Point(startPoint.X + e.HorizontalChange, startPoint.Y + e.VerticalChange);
                    SetCurrentValue(BoundsProperty, newBounds);
                    */
                    break;
                case Transform.Modes.Warp:
                    break;
            }
            SetCurrentValue(BoundsProperty, result);
        }

        //-------------------------------------------------------------------------------

        Point centerPoint;

        Point startPoint;

        Vector startVector;

        PointCollection startPoints;
        
        private void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            centerPoint = new Point(Bounds[0].X + ((Bounds[1].X - Bounds[0].X) / 2), Bounds[0].Y + ((Bounds[2].Y - Bounds[1].Y) / 2));
            startVector = Point.Subtract(startPoint, centerPoint);
            startPoints = Clone(Bounds);
            startPoint = System.Windows.Input.Mouse.GetPosition(Thumbs);
        }

        //-------------------------------------------------------------------------------

        private void OnTopLeftDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        private void OnTopDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        private void OnTopRightDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        private void OnLeftDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        private void OnRightDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        private void OnBottomLeftDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        private void OnBottomDragStarted(object sender, DragStartedEventArgs e)
        {
        }

        private void OnBottomRightDragStarted(object sender, DragStartedEventArgs e)
        {
        }
    }
}