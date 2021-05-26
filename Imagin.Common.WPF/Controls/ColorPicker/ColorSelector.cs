using Imagin.Common.Linq;
using Imagin.Common.Math;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class ColorSelector : BaseColorSelector
    {
        Handle handle = false;

        ///-------------------------------------------------------------------------------------------------------------

        public static DependencyProperty EllipsePositionProperty = DependencyProperty.Register(nameof(EllipsePosition), typeof(Point2D), typeof(ColorSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Point2D EllipsePosition
        {
            get => (Point2D)GetValue(EllipsePositionProperty);
            private set => SetValue(EllipsePositionProperty, value);
        }

        public static DependencyProperty EllipseSizeProperty = DependencyProperty.Register(nameof(EllipseSize), typeof(double), typeof(ColorSelector), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.None));
        public double EllipseSize
        {
            get => (double)GetValue(EllipseSizeProperty);
            set => SetValue(EllipseSizeProperty, value);
        }

        public static DependencyProperty BorderStyleProperty = DependencyProperty.Register(nameof(BorderStyle), typeof(Style), typeof(ColorSelector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public Style BorderStyle
        {
            get => (Style)GetValue(BorderStyleProperty);
            set => SetValue(BorderStyleProperty, value);
        }
        
        ///-------------------------------------------------------------------------------------------------------------

        public ColorSelector() : base()
        {
            SetCurrentValue(EllipsePositionProperty, new Point2D(0, 0));
            DefaultStyleKey = typeof(ColorSelector);
        }

        ///-------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Note: Rounding avoids "shaking" when changing quickly between two really close values. This is particularly evident with <see cref="Media.Models.HSB"/> and similarly complex models.
        /// </summary>
        protected override void Mark()
        {
            var result = Model.SelectedComponent.PointFrom(Model.Converter.Color);
            EllipsePosition.X = ((result.X * ActualWidth) - (EllipseSize / 2.0)).Round();
            EllipsePosition.Y = ((ActualHeight - (result.Y * ActualHeight)) - (EllipseSize / 2.0)).Round();
        }

        protected override void OnMouseChanged(Vector2<One> input)
        {
            base.OnMouseChanged(input);
            Model.Update(input, Value);
            Mark();
        }

        protected override void OnValueChanged(OldNew<double> input)
        {
            base.OnValueChanged(input);
            if (Model != null)
                Mark();
        }
    }
}