using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class ToggleSwitch : CheckBox
    {
        public static DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ToggleSwitch), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.None));
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static DependencyProperty SymbolSizeProperty = DependencyProperty.Register(nameof(SymbolSize), typeof(double), typeof(ToggleSwitch), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.None));
        public double SymbolSize
        {
            get => (double)GetValue(SymbolSizeProperty);
            set => SetValue(SymbolSizeProperty, value);
        }

        public static DependencyProperty ThumbStyleProperty = DependencyProperty.Register(nameof(ThumbStyle), typeof(Style), typeof(ToggleSwitch), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.None));
        public Style ThumbStyle
        {
            get => (Style)GetValue(ThumbStyleProperty);
            set => SetValue(ThumbStyleProperty, value);
        }

        public ToggleSwitch() : base() => DefaultStyleKey = typeof(ToggleSwitch);
    }
}