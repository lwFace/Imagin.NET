using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class AlphaSlider : ColorSlider
    {
        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(AlphaSlider), new FrameworkPropertyMetadata(default(Color)));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public AlphaSlider() : base()
        {
            DefaultStyleKey = typeof(AlphaSlider);
        }
    }
}