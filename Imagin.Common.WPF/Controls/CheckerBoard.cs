using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class CheckerBoard : ClippedBorder
    {
        public DrawingBrush DrawingBrush
        {
            get
            {
                var result = new DrawingBrush();
                result.TileMode = TileMode.Tile;
                result.Viewport = new Rect(0.0, 0.0, CheckerSize, CheckerSize);
                result.ViewportUnits = BrushMappingMode.Absolute;

                var drawingGroup = new DrawingGroup();
                drawingGroup.Children.Add(new GeometryDrawing()
                {
                    Brush = CheckerForeground,
                    Geometry = Geometry.Parse("M5,5 L0,5 0,10 5,10 5,5 10,5 10,0 5,0 Z")
                });
                drawingGroup.Children.Add(new GeometryDrawing()
                {
                    Brush = CheckerBackground,
                    Geometry = Geometry.Parse("M0,0 L0,5 0,10 0,5, 10,5 10,10 5,10 5,0 Z")
                });

                result.Drawing = drawingGroup;
                return result;
            }
        }

        public static DependencyProperty CheckerForegroundProperty = DependencyProperty.Register(nameof(CheckerForeground), typeof(Brush), typeof(CheckerBoard), new PropertyMetadata(Brushes.LightGray, OnCheckerPropertyChanged));
        public Brush CheckerForeground
        {
            get => (Brush)GetValue(CheckerForegroundProperty);
            set => SetValue(CheckerForegroundProperty, value);
        }

        public static DependencyProperty CheckerBackgroundProperty = DependencyProperty.Register(nameof(CheckerBackground), typeof(Brush), typeof(CheckerBoard), new PropertyMetadata(Brushes.White, OnCheckerPropertyChanged));
        public Brush CheckerBackground
        {
            get => (Brush)GetValue(CheckerBackgroundProperty);
            set => SetValue(CheckerBackgroundProperty, value);
        }

        public static DependencyProperty CheckerSizeProperty = DependencyProperty.Register(nameof(CheckerSize), typeof(double), typeof(CheckerBoard), new PropertyMetadata(10.0, OnCheckerPropertyChanged));
        public double CheckerSize
        {
            get => (double)GetValue(CheckerSizeProperty);
            set => SetValue(CheckerSizeProperty, value);
        }

        static void OnCheckerPropertyChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<CheckerBoard>().Background = i.As<CheckerBoard>().DrawingBrush;

        public CheckerBoard() : base()
        {
            SetCurrentValue(BackgroundProperty, DrawingBrush);
            SetCurrentValue(SnapsToDevicePixelsProperty, true);
        }
    }
}