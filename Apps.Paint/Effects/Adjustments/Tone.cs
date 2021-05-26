using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that blends between partial desaturation and a two-color ramp.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class ToneEffect : AdjustmentEffect
    {
        public override string Name => "Tone";

        public static readonly DependencyProperty DesaturationProperty = DependencyProperty.Register("Desaturation", typeof(double), typeof(ToneEffect), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(0)));
        /// <summary>The amount of desaturation to apply.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double Desaturation
        {
            get
            {
                return ((double)(GetValue(DesaturationProperty)));
            }
            set
            {
                SetValue(DesaturationProperty, value);
            }
        }

        public static readonly DependencyProperty TonedProperty = DependencyProperty.Register("Toned", typeof(double), typeof(ToneEffect), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(1)));
        /// <summary>The amount of color toning to apply.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double Toned
        {
            get
            {
                return ((double)(GetValue(TonedProperty)));
            }
            set
            {
                SetValue(TonedProperty, value);
            }
        }

        public static readonly DependencyProperty LightColorProperty = DependencyProperty.Register("LightColor", typeof(Color), typeof(ToneEffect), new UIPropertyMetadata(Color.FromArgb(255, 255, 255, 0), PixelShaderConstantCallback(2)));
        /// <summary>The first color to apply to input. This is usually a light tone.</summary>
        [Hidden(false)]
        public Color LightColor
        {
            get
            {
                return ((Color)(GetValue(LightColorProperty)));
            }
            set
            {
                SetValue(LightColorProperty, value);
            }
        }

        public static readonly DependencyProperty DarkColorProperty = DependencyProperty.Register("DarkColor", typeof(Color), typeof(ToneEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 128), PixelShaderConstantCallback(3)));
        /// <summary>The second color to apply to the input. This is usuall a dark tone.</summary>
        [Hidden(false)]
        public Color DarkColor
        {
            get
            {
                return ((Color)(GetValue(DarkColorProperty)));
            }
            set
            {
                SetValue(DarkColorProperty, value);
            }
        }

        public ToneEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(ToneEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(DesaturationProperty);
            UpdateShaderValue(TonedProperty);
            UpdateShaderValue(LightColorProperty);
            UpdateShaderValue(DarkColorProperty);
        }

        public override AdjustmentEffect Copy()
        {
            return new ToneEffect()
            {
                Desaturation = Desaturation,
                Toned = Toned,
                LightColor = LightColor,
                DarkColor = DarkColor
            };
        }
    }
}
