using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Common.Controls
{
    public partial class Clock : UserControl
    {
        const double height = 100;

        const double width = 100;

        /// ----------------------------------------------------------------------------------------

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        /// ----------------------------------------------------------------------------------------

        List<System.Windows.Shapes.Line> path = new List<System.Windows.Shapes.Line>();

        List<System.Windows.Shapes.Line> intermediateTicks = new List<System.Windows.Shapes.Line>();

        List<System.Windows.Shapes.Line> majorTicks = new List<System.Windows.Shapes.Line>();

        List<Ellipse> minorTicks = new List<Ellipse>();

        /// ----------------------------------------------------------------------------------------

        public static DependencyProperty DateTimeProperty = DependencyProperty.Register(nameof(DateTime), typeof(DateTime), typeof(Clock), new FrameworkPropertyMetadata(default(DateTime), FrameworkPropertyMetadataOptions.None));
        public DateTime DateTime
        {
            get => (DateTime)GetValue(DateTimeProperty);
            private set => SetValue(DateTimeProperty, value);
        }

        public static DependencyProperty AProperty = DependencyProperty.Register(nameof(A), typeof(DateTime), typeof(Clock), new FrameworkPropertyMetadata(DateTime.Now.Date.AddHours(3), FrameworkPropertyMetadataOptions.None, OnAChanged));
        public DateTime A
        {
            get => (DateTime)GetValue(AProperty);
            set => SetValue(AProperty, value);
        }
        static void OnAChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as Clock).OnTravelChanged((i as Clock).EllipseA, new OldNew<DateTime>(e).New);

        public static DependencyProperty AStrokeProperty = DependencyProperty.Register(nameof(AStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));
        public Brush AStroke
        {
            get => (Brush)GetValue(AStrokeProperty);
            set => SetValue(AStrokeProperty, value);
        }

        public static DependencyProperty AStrokeThicknessProperty = DependencyProperty.Register(nameof(AStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(6.0, FrameworkPropertyMetadataOptions.None));
        public double AStrokeThickness
        {
            get => (double)GetValue(AStrokeThicknessProperty);
            set => SetValue(AStrokeThicknessProperty, value);
        }

        public static DependencyProperty AVisibilityProperty = DependencyProperty.Register(nameof(AVisibility), typeof(Visibility), typeof(Clock), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.None));
        public Visibility AVisibility
        {
            get => (Visibility)GetValue(AVisibilityProperty);
            set => SetValue(AVisibilityProperty, value);
        }
        
        /// ----------------------------------------------------------------------------------------

        public static DependencyProperty BProperty = DependencyProperty.Register(nameof(B), typeof(DateTime), typeof(Clock), new FrameworkPropertyMetadata(DateTime.Now.Date.AddHours(9), FrameworkPropertyMetadataOptions.None, OnBChanged));
        public DateTime B
        {
            get => (DateTime)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }
        static void OnBChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as Clock).OnTravelChanged((i as Clock).EllipseB, new OldNew<DateTime>(e).New);

        public static DependencyProperty BStrokeProperty = DependencyProperty.Register(nameof(BStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));
        public Brush BStroke
        {
            get => (Brush)GetValue(BStrokeProperty);
            set => SetValue(BStrokeProperty, value);
        }

        public static DependencyProperty BStrokeThicknessProperty = DependencyProperty.Register(nameof(BStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(6.0, FrameworkPropertyMetadataOptions.None));
        public double BStrokeThickness
        {
            get => (double)GetValue(BStrokeThicknessProperty);
            set => SetValue(BStrokeThicknessProperty, value);
        }

        public static DependencyProperty BVisibilityProperty = DependencyProperty.Register(nameof(BVisibility), typeof(Visibility), typeof(Clock), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.None));
        public Visibility BVisibility
        {
            get => (Visibility)GetValue(BVisibilityProperty);
            set => SetValue(BVisibilityProperty, value);
        }

        /// ----------------------------------------------------------------------------------------

        public static DependencyProperty CenterStrokeProperty = DependencyProperty.Register(nameof(CenterStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));
        public Brush CenterStroke
        {
            get => (Brush)GetValue(CenterStrokeProperty);
            set => SetValue(CenterStrokeProperty, value);
        }

        /// ----------------------------------------------------------------------------------------

        public static DependencyProperty HourStrokeProperty = DependencyProperty.Register(nameof(HourStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));
        public Brush HourStroke
        {
            get => (Brush)GetValue(HourStrokeProperty);
            set => SetValue(HourStrokeProperty, value);
        }

        public static DependencyProperty MinuteStrokeProperty = DependencyProperty.Register(nameof(MinuteStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));
        public Brush MinuteStroke
        {
            get => (Brush)GetValue(MinuteStrokeProperty);
            set => SetValue(MinuteStrokeProperty, value);
        }

        public static DependencyProperty SecondStrokeProperty = DependencyProperty.Register(nameof(SecondStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));
        public Brush SecondStroke
        {
            get => (Brush)GetValue(SecondStrokeProperty);
            set => SetValue(SecondStrokeProperty, value);
        }

        public static DependencyProperty HourStrokeThicknessProperty = DependencyProperty.Register(nameof(HourStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(3.0, FrameworkPropertyMetadataOptions.None));
        public double HourStrokeThickness
        {
            get => (double)GetValue(HourStrokeThicknessProperty);
            set => SetValue(HourStrokeThicknessProperty, value);
        }

        public static DependencyProperty MinuteStrokeThicknessProperty = DependencyProperty.Register(nameof(MinuteStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.None));
        public double MinuteStrokeThickness
        {
            get => (double)GetValue(MinuteStrokeThicknessProperty);
            set => SetValue(MinuteStrokeThicknessProperty, value);
        }

        public static DependencyProperty SecondStrokeThicknessProperty = DependencyProperty.Register(nameof(SecondStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None));
        public double SecondStrokeThickness
        {
            get => (double)GetValue(SecondStrokeThicknessProperty);
            set => SetValue(SecondStrokeThicknessProperty, value);
        }

        /// ----------------------------------------------------------------------------------------

        public static DependencyProperty PathStrokeProperty = DependencyProperty.Register(nameof(PathStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Green, FrameworkPropertyMetadataOptions.None, OnPathChanged));
        public Brush PathStroke
        {
            get => (Brush)GetValue(PathStrokeProperty);
            set => SetValue(PathStrokeProperty, value);
        }

        public static DependencyProperty PathStrokeThicknessProperty = DependencyProperty.Register(nameof(PathStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None, OnPathChanged));
        public double PathStrokeThickness
        {
            get => (double)GetValue(PathStrokeThicknessProperty);
            set => SetValue(PathStrokeThicknessProperty, value);
        }

        static void OnPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as Clock).DrawPath();

        /// ----------------------------------------------------------------------------------------

        public static DependencyProperty MajorTickStrokeProperty = DependencyProperty.Register(nameof(MajorTickStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None, OnTicksChanged));
        public Brush MajorTickStroke
        {
            get => (Brush)GetValue(MajorTickStrokeProperty);
            set => SetValue(MajorTickStrokeProperty, value);
        }

        public static DependencyProperty MajorTickStrokeThicknessProperty = DependencyProperty.Register(nameof(MajorTickStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.None, OnTicksChanged));
        public double MajorTickStrokeThickness
        {
            get => (double)GetValue(MajorTickStrokeThicknessProperty);
            set => SetValue(MajorTickStrokeThicknessProperty, value);
        }

        public static DependencyProperty MajorTickLengthProperty = DependencyProperty.Register(nameof(MajorTickLength), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(8.0, FrameworkPropertyMetadataOptions.None, OnTicksChanged));
        public double MajorTickLength
        {
            get => (double)GetValue(MajorTickLengthProperty);
            set => SetValue(MajorTickLengthProperty, value);
        }

        /// ----------------------------------------------------------------------------------------

        public static DependencyProperty MinorTickStrokeProperty = DependencyProperty.Register(nameof(MinorTickStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None, OnTicksChanged));
        public Brush MinorTickStroke
        {
            get => (Brush)GetValue(MinorTickStrokeProperty);
            set => SetValue(MinorTickStrokeProperty, value);
        }

        public static DependencyProperty MinorTickStrokeThicknessProperty = DependencyProperty.Register(nameof(MinorTickStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.None, OnTicksChanged));
        public double MinorTickStrokeThickness
        {
            get => (double)GetValue(MinorTickStrokeThicknessProperty);
            set => SetValue(MinorTickStrokeThicknessProperty, value);
        }

        /// ----------------------------------------------------------------------------------------

        public static DependencyProperty IntermediateTickStrokeProperty = DependencyProperty.Register(nameof(IntermediateTickStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None, OnTicksChanged));
        public Brush IntermediateTickStroke
        {
            get => (Brush)GetValue(IntermediateTickStrokeProperty);
            set => SetValue(IntermediateTickStrokeProperty, value);
        }

        public static DependencyProperty IntermediateTickStrokeThicknessProperty = DependencyProperty.Register(nameof(IntermediateTickStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.None, OnTicksChanged));
        public double IntermediateTickStrokeThickness
        {
            get => (double)GetValue(IntermediateTickStrokeThicknessProperty);
            set => SetValue(IntermediateTickStrokeThicknessProperty, value);
        }

        public static DependencyProperty IntermediateTickLengthProperty = DependencyProperty.Register(nameof(IntermediateTickLength), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(4.0, FrameworkPropertyMetadataOptions.None, OnTicksChanged));
        public double IntermediateTickLength
        {
            get => (double)GetValue(IntermediateTickLengthProperty);
            set => SetValue(IntermediateTickLengthProperty, value);
        }

        /// ----------------------------------------------------------------------------------------

        static void OnTicksChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as Clock).DrawTicks();

        /// ----------------------------------------------------------------------------------------

        public Clock()
        {
            InitializeComponent();
            timer.Tick += OnTick;
            timer.Start();

            DrawTicks();
        }

        /// ----------------------------------------------------------------------------------------

        double AngleFrom(DateTime input) => ((input.Hour > 11 ? input.Hour - 12 : input.Hour) * 30.0) + (input.Minute * 0.5) - 90;

        /// ----------------------------------------------------------------------------------------

        void OnTick(object sender, EventArgs e)
        {
            Hour.RenderTransform = new RotateTransform()
            {
                CenterX = width / 2.0,
                CenterY = height / 2.0,
                Angle = (DateTime.Now.Hour * 30.0) + (DateTime.Now.Minute * 0.5)
            };
            Minute.RenderTransform = new RotateTransform()
            {
                CenterX = width / 2.0,
                CenterY = height / 2.0,
                Angle = DateTime.Now.Minute * 6.0
            };
            Second.RenderTransform = new RotateTransform()
            {
                CenterX = width / 2.0,
                CenterY = height / 2.0,
                Angle = DateTime.Now.Second * 6.0
            };

            SetCurrentValue(DateTimeProperty, DateTime.Now);
        }

        /// ----------------------------------------------------------------------------------------

        void ClearPath()
        {
            for (var i = path.Count - 1; i >= 0; i--)
            {
                PART_Canvas.Children.Remove(path[i]);
                path.RemoveAt(i);
            }
        }

        void ClearTicks()
        {
            for (var i = majorTicks.Count - 1; i >= 0; i--)
            {
                PART_Canvas.Children.Remove(majorTicks[i]);
                majorTicks.RemoveAt(i);
            }
            for (var i = minorTicks.Count - 1; i >= 0; i--)
            {
                PART_Canvas.Children.Remove(minorTicks[i]);
                minorTicks.RemoveAt(i);
            }
            for (var i = intermediateTicks.Count - 1; i >= 0; i--)
            {
                PART_Canvas.Children.Remove(intermediateTicks[i]);
                intermediateTicks.RemoveAt(i);
            }
        }

        /// ----------------------------------------------------------------------------------------

        void DrawPath()
        {
            ClearPath();

            double? x = null, y = null;

            var a = AngleFrom(A);
            var b = AngleFrom(B);

            var end = 359;
            for (var i = a.NearestFactor(6); i <= end; i += 6)
            {
                if (i < b)
                {
                    if (i + 6 > b)
                    {
                        AddPath(x.Value, y.Value, X(b), Y(b));
                        break;
                    }
                }

                if (x != null && y != null)
                    AddPath(x.Value, y.Value, X(i), Y(i));

                x = X(i);
                y = Y(i);
            }
        }

        void DrawTicks()
        {
            ClearTicks();

            var x = width / 2.0;
            var y = 0.0;

            for (var i = 0; i < 4; i++)
            {
                var line = new System.Windows.Shapes.Line();
                line.Fill = MajorTickStroke;
                line.Stroke = MajorTickStroke;
                line.StrokeThickness = MajorTickStrokeThickness;

                line.X1 = x;
                line.Y1 = y;

                switch (i)
                {
                    case 0:
                        line.X2 = x;
                        line.Y2 = y + MajorTickLength;
                        x += width / 2.0;
                        y += height / 2.0;
                        break;
                    case 1:
                        line.X2 = x - MajorTickLength;
                        line.Y2 = y;
                        x -= width / 2.0;
                        y += height / 2.0;
                        break;
                    case 2:
                        line.X2 = x;
                        line.Y2 = y - MajorTickLength;
                        x -= width / 2.0;
                        y -= height / 2.0;
                        break;
                    case 3:
                        line.X2 = x + MajorTickLength;
                        line.Y2 = y;
                        x += width / 2.0;
                        y -= height / 2.0;
                        break;
                }

                PART_Canvas.Children.Add(line);
                majorTicks.Add(line);

                x = x > width ? 0 : x;
                y = y > height ? 0 : y;
            }

            for (var i = 0; i < 360; i += 6)
            {
                var l = new Ellipse();
                l.Fill = MinorTickStroke;
                l.Height = MinorTickStrokeThickness;
                l.Width = MinorTickStrokeThickness;

                x = System.Math.Cos(Imagin.Common.Math.Angle.GetRadian(i)) * 50.0;
                y = System.Math.Sin(Imagin.Common.Math.Angle.GetRadian(i)) * 50.0;

                Canvas.SetLeft(l, x.Round() + (width / 2.0) - (MinorTickStrokeThickness / 2.0));
                Canvas.SetTop(l, y.Round() + (height / 2.0) - (MinorTickStrokeThickness / 2.0));

                PART_Canvas.Children.Add(l);
                minorTicks.Add(l);
            }

            var k = 0;
            for (var i = 0; i < 360; i += (6 * 5))
            {
                if (k == 0)
                {
                    k++;
                    continue;
                }
                else if (k == 3)
                {
                    k = 1;
                    continue;
                }
                else k++;

                var l = new System.Windows.Shapes.Line();
                l.Fill = IntermediateTickStroke;
                l.Stroke = IntermediateTickStroke;
                l.StrokeThickness = IntermediateTickStrokeThickness;

                var angle = Imagin.Common.Math.Angle.GetRadian(i);

                x = System.Math.Cos(angle) * 50.0;
                y = System.Math.Sin(angle) * 50.0;

                l.X1 = x.Round() + (width / 2.0);
                l.Y1 = y.Round() + (height / 2.0);

                l.X2 = l.X1.Round() - (IntermediateTickLength * System.Math.Cos(angle));
                l.Y2 = l.Y1.Round() - (IntermediateTickLength * System.Math.Sin(angle));

                PART_Canvas.Children.Add(l);
                intermediateTicks.Add(l);
            }
        }

        /// ----------------------------------------------------------------------------------------

        double X(double i) => (System.Math.Cos(Imagin.Common.Math.Angle.GetRadian(i)) * 50.0) + (width / 2.0);

        double Y(double i) => (System.Math.Sin(Imagin.Common.Math.Angle.GetRadian(i)) * 50.0) + (height / 2.0);

        /// ----------------------------------------------------------------------------------------

        System.Windows.Shapes.Line AddPath(double x1, double y1, double x2, double y2)
        {
            var result = new System.Windows.Shapes.Line();

            Panel.SetZIndex(result, 1);

            result.Fill = PathStroke;
            result.Stroke = PathStroke;
            result.StrokeThickness = PathStrokeThickness;

            result.X1 = x1;
            result.Y1 = y1;

            result.X2 = x2;
            result.Y2 = y2;

            PART_Canvas.Children.Add(result);
            path.Add(result);
            return result;
        }

        /// ----------------------------------------------------------------------------------------

        void OnTravelChanged(Ellipse ellipse, DateTime input)
        {
            var x = System.Math.Cos(Imagin.Common.Math.Angle.GetRadian((input.Hour * 30.0) + (input.Minute * 0.5) - 90)) * 50.0;
            var y = System.Math.Sin(Imagin.Common.Math.Angle.GetRadian((input.Hour * 30.0) + (input.Minute * 0.5) - 90)) * 50.0;
            Canvas.SetLeft(ellipse, x + (width / 2.0) - 3);
            Canvas.SetTop(ellipse, y + (height / 2.0) - 3);
            DrawPath();
        }
    }
}