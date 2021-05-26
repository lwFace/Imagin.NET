using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that pinches a circular region.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class PinchEffect : AdjustmentEffect
    {
        public override string Name => "Pinch";

        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(PinchEffect), new UIPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(PinchEffect), new UIPropertyMetadata(((double)(0.25D)), PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty StrengthProperty = DependencyProperty.Register("Strength", typeof(double), typeof(PinchEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(2)));

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(PinchEffect), new UIPropertyMetadata(((double)(1.5D)), PixelShaderConstantCallback(3)));

        public PinchEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(PinchEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(RadiusProperty);
            UpdateShaderValue(StrengthProperty);
            UpdateShaderValue(AspectRatioProperty);
        }

        /// <summary>The center point of the pinched region.</summary>
        [Hidden(false)]
        public Point Center
        {
            get
            {
                return ((Point)(GetValue(CenterProperty)));
            }
            set
            {
                SetValue(CenterProperty, value);
            }
        }

        /// <summary>The radius of the pinched region.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Radius
        {
            get
            {
                return ((double)(GetValue(RadiusProperty)));
            }
            set
            {
                SetValue(RadiusProperty, value);
            }
        }

        /// <summary>The strength of the pinch effect.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Strength
        {
            get
            {
                return ((double)(GetValue(StrengthProperty)));
            }
            set
            {
                SetValue(StrengthProperty, value);
            }
        }

        /// <summary>The aspect ratio (width / height) of the input.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double AspectRatio
        {
            get
            {
                return ((double)(GetValue(AspectRatioProperty)));
            }
            set
            {
                SetValue(AspectRatioProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new PinchEffect()
            {
                Center = Center,
                Radius = Radius,
                Strength = Strength,
                AspectRatio = AspectRatio
            };
        }
    }
}
