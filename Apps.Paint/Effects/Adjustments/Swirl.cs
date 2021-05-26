using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that swirls the input in a spiral.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class SwirlEffect : AdjustmentEffect
    {
        public override string Name => "Swirl";

        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(SwirlEffect), new UIPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty SpiralStrengthProperty = DependencyProperty.Register("SpiralStrength", typeof(double), typeof(SwirlEffect), new UIPropertyMetadata(((double)(10D)), PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(SwirlEffect), new UIPropertyMetadata(((double)(1.5D)), PixelShaderConstantCallback(2)));

        public SwirlEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(SwirlEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(SpiralStrengthProperty);
            UpdateShaderValue(AspectRatioProperty);
        }

        /// <summary>The center point of the spiral. (1,1) is lower right corner</summary>
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

        /// <summary>The amount of twist to the spiral.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double SpiralStrength
        {
            get
            {
                return ((double)(GetValue(SpiralStrengthProperty)));
            }
            set
            {
                SetValue(SpiralStrengthProperty, value);
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
            return new SwirlEffect()
            {
                Center = Center,
                SpiralStrength = SpiralStrength,
                AspectRatio = AspectRatio
            };
        }
    }
}
