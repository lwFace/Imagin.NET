using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that intensifies dark regions.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class GloomEffect : AdjustmentEffect
    {
        public override string Name => "Gloom";

        public static readonly DependencyProperty GloomIntensityProperty = DependencyProperty.Register("GloomIntensity", typeof(double), typeof(GloomEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty BaseIntensityProperty = DependencyProperty.Register("BaseIntensity", typeof(double), typeof(GloomEffect), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty GloomSaturationProperty = DependencyProperty.Register("GloomSaturation", typeof(double), typeof(GloomEffect), new UIPropertyMetadata(((double)(0.2D)), PixelShaderConstantCallback(2)));

        public static readonly DependencyProperty BaseSaturationProperty = DependencyProperty.Register("BaseSaturation", typeof(double), typeof(GloomEffect), new UIPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(3)));

        public GloomEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(GloomEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(GloomIntensityProperty);
            UpdateShaderValue(BaseIntensityProperty);
            UpdateShaderValue(GloomSaturationProperty);
            UpdateShaderValue(BaseSaturationProperty);
        }

        /// <summary>Intensity of the gloom image.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double GloomIntensity
        {
            get
            {
                return ((double)(GetValue(GloomIntensityProperty)));
            }
            set
            {
                SetValue(GloomIntensityProperty, value);
            }
        }

        /// <summary>Intensity of the base image.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double BaseIntensity
        {
            get
            {
                return ((double)(GetValue(BaseIntensityProperty)));
            }
            set
            {
                SetValue(BaseIntensityProperty, value);
            }
        }

        /// <summary>Saturation of the gloom image.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double GloomSaturation
        {
            get
            {
                return ((double)(GetValue(GloomSaturationProperty)));
            }
            set
            {
                SetValue(GloomSaturationProperty, value);
            }
        }

        /// <summary>Saturation of the base image.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double BaseSaturation
        {
            get
            {
                return ((double)(GetValue(BaseSaturationProperty)));
            }
            set
            {
                SetValue(BaseSaturationProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new GloomEffect()
            {
                GloomIntensity = GloomIntensity,
                BaseIntensity = BaseIntensity,
                GloomSaturation = GloomSaturation,
                BaseSaturation = BaseSaturation
            };
        }
    }
}
