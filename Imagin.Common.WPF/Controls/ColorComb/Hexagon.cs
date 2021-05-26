using Imagin.Common.Math;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public sealed class Hexagon : RadioButton
    {
        /// <summary>
        /// Corresponds to path geometry in XAML.
        /// </summary>
        public const double Radius = 12.0;

        public static readonly double Offset = Radius * 2 * System.Math.Cos(30d * Numbers.PI / 180d) + 1.5;

        internal bool Visited;

        Hexagon[] _neighbors = new Hexagon[6];
        internal Hexagon[] Neighbors => _neighbors;

        Color _nominalColor = Color.FromScRgb(1f, 1f, 1f, 1f);
        public Color NominalColor
        {
            get => _nominalColor;
            set
            {
                _nominalColor = value;
                Background = new SolidColorBrush(_nominalColor);
            }
        }

        public static DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(Hexagon), new FrameworkPropertyMetadata(0.15));
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        internal Hexagon() : base() => DefaultStyleKey = typeof(Hexagon);

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            Background = new SolidColorBrush(_nominalColor);
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            Background = new SolidColorBrush(_nominalColor);
        }

        protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsPressedChanged(e);
            if (IsPressed)
            {
                Background = new SolidColorBrush(_nominalColor);
            }
            else Background = new SolidColorBrush(_nominalColor);
        }
    }
}