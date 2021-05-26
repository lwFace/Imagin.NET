using Imagin.Common.Math;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public partial class ColorSlider : BaseColorSelector
    {
        public static DependencyProperty ArrowForegroundProperty = DependencyProperty.Register(nameof(ArrowForeground), typeof(Brush), typeof(ColorSlider), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));
        public Brush ArrowForeground
        {
            get => (Brush)GetValue(ArrowForegroundProperty);
            set => SetValue(ArrowForegroundProperty, value);
        }

        public static DependencyProperty ArrowPositionProperty = DependencyProperty.Register(nameof(ArrowPosition), typeof(Point2D), typeof(ColorSlider), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Point2D ArrowPosition
        {
            get => (Point2D)GetValue(ArrowPositionProperty);
            set => SetValue(ArrowPositionProperty, value);
        }

        public static DependencyProperty BorderStyleProperty = DependencyProperty.Register(nameof(BorderStyle), typeof(Style), typeof(ColorSlider), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Style BorderStyle
        {
            get => (Style)GetValue(BorderStyleProperty);
            set => SetValue(BorderStyleProperty, value);
        }

        public static DependencyProperty ConverterProperty = DependencyProperty.Register(nameof(Converter), typeof(Media.ColorConverter), typeof(ColorSlider), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Media.ColorConverter Converter
        {
            get => (Media.ColorConverter)GetValue(ConverterProperty);
            set => SetValue(ConverterProperty, value);
        }

        public ColorSlider()
        {
            DefaultStyleKey = typeof(ColorSlider);
            SetCurrentValue(ArrowPositionProperty, new Point2D(0, -8));
        }

        //----------------------------------------------------------------------------------------

        protected override void Mark() => ArrowPosition.Y = ((1 - Value) * ActualHeight) - 8;

        //----------------------------------------------------------------------------------------

        protected override void OnMouseChanged(Vector2<One> input)
        {
            base.OnMouseChanged(input);
            SetCurrentValue(ValueProperty, (double)input.Y);
        }

        protected override void OnValueChanged(OldNew<double> input)
        {
            base.OnValueChanged(input);
            Mark();
        }
    }
}