using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_Ellipse), Type = typeof(Ellipse))]
    [TemplatePart(Name = nameof(PART_Line), Type = typeof(Line))]
    public class AnglePicker : UserControl
    {
        #region Properties

        Ellipse PART_Ellipse = null;

        System.Windows.Shapes.Line PART_Line = null;

        public static DependencyProperty DegreesProperty = DependencyProperty.Register(nameof(Degrees), typeof(double), typeof(AnglePicker), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDegreesChanged));
        public double Degrees
        {
            get => (double)GetValue(DegreesProperty);
            set => SetValue(DegreesProperty, value);
        }
        static void OnDegreesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.As<AnglePicker>().OnDegreesChanged(new OldNew<double>(e));

        public static DependencyProperty OriginFillProperty = DependencyProperty.Register(nameof(OriginFill), typeof(Brush), typeof(AnglePicker), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush OriginFill
        {
            get => (Brush)GetValue(OriginFillProperty);
            set => SetValue(OriginFillProperty, value);
        }

        public static DependencyProperty OriginStrokeProperty = DependencyProperty.Register(nameof(OriginStroke), typeof(Brush), typeof(AnglePicker), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush OriginStroke
        {
            get => (Brush)GetValue(OriginStrokeProperty);
            set => SetValue(OriginStrokeProperty, value);
        }

        public static DependencyProperty OriginStrokeThicknessProperty = DependencyProperty.Register(nameof(OriginStrokeThickness), typeof(double), typeof(AnglePicker), new FrameworkPropertyMetadata(8d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double OriginStrokeThickness
        {
            get => (double)GetValue(OriginStrokeThicknessProperty);
            set => SetValue(OriginStrokeThicknessProperty, value);
        }

        public static DependencyProperty OriginVisibilityProperty = DependencyProperty.Register(nameof(OriginVisibility), typeof(Visibility), typeof(AnglePicker), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Visibility OriginVisibility
        {
            get => (Visibility)GetValue(OriginVisibilityProperty);
            set => SetValue(OriginVisibilityProperty, value);
        }
        
        public static DependencyProperty NeedleStrokeProperty = DependencyProperty.Register(nameof(NeedleStroke), typeof(Brush), typeof(AnglePicker), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush NeedleStroke
        {
            get => (Brush)GetValue(NeedleStrokeProperty);
            set => SetValue(NeedleStrokeProperty, value);
        }

        public static DependencyProperty NeedleStrokeThicknessProperty = DependencyProperty.Register(nameof(NeedleStrokeThickness), typeof(double), typeof(AnglePicker), new FrameworkPropertyMetadata(2d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double NeedleStrokeThickness
        {
            get => (double)GetValue(NeedleStrokeThicknessProperty);
            set => SetValue(NeedleStrokeThicknessProperty, value);
        }
        
        public static DependencyProperty RadiansProperty = DependencyProperty.Register(nameof(Radians), typeof(double), typeof(AnglePicker), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRadiansChanged));
        public double Radians
        {
            get => (double)GetValue(RadiansProperty);
            set => SetValue(RadiansProperty, value);
        }
        static void OnRadiansChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.As<AnglePicker>().OnRadiansChanged(new OldNew<double>(e));

        #endregion

        #region AnglePicker

        BindingExpressionBase j = null;

        public AnglePicker()
        {
            DefaultStyleKey = typeof(AnglePicker);
            this.Bind(HeightProperty, nameof(Width), this, BindingMode.TwoWay);
            j = this.Bind(RadiansProperty, nameof(Degrees), this, BindingMode.TwoWay, new DefaultConverter<double, double>
            (
                i => Math.Angle.GetRadian(i),
                i => Math.Angle.GetDegree(i)
            ));
        }

        #endregion

        #region Methods

        void Center(System.Windows.Shapes.Line input) => PART_Line.If(i => i != null, i =>
        {
            i.X1 = i.X2 = ActualWidth / 2d;
            i.Y2 = ActualHeight / 2d;
        });

        void Update(System.Windows.Shapes.Line input) => PART_Line.If(i => i != null, i =>
        {
            i.RenderTransform = new RotateTransform(Degrees + 90d);
        });

        double RadiansFromPoint(Point point)
        {
            var center = new Point(ActualWidth / 2, ActualHeight / 2);
            point.X = point.X.Coerce(ActualWidth);
            point.Y = point.Y.Coerce(ActualHeight);
            return System.Math.Atan2(point.Y - center.Y, point.X - center.X);
        }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is Ellipse i)
                {
                    i.CaptureMouse();
                    SetCurrentValue(RadiansProperty, RadiansFromPoint(e.GetPosition(i)));
                }
            }
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is Ellipse i)
                    SetCurrentValue(RadiansProperty, RadiansFromPoint(e.GetPosition(i)));
            }
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (sender is Ellipse i)
                {
                    if (i.IsMouseCaptured)
                        i.ReleaseMouseCapture();
                }
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Center(PART_Line);
            j?.UpdateTarget();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Line = Template.FindName(nameof(PART_Line), this) as System.Windows.Shapes.Line;
            Center(PART_Line);

            PART_Ellipse = Template.FindName(nameof(PART_Ellipse), this) as Ellipse;
            PART_Ellipse.MouseDown += OnMouseDown;
            PART_Ellipse.MouseMove += OnMouseMove;
            PART_Ellipse.MouseUp += OnMouseUp;
        }

        protected virtual void OnDegreesChanged(OldNew<double> input)
        {
            Update(PART_Line);
        }

        protected virtual void OnRadiansChanged(OldNew<double> input)
        {
            Update(PART_Line);
        }

        #endregion
    }
}