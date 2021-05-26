using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>Pixel shader which produces random scratches, noise and other FX like an old projector</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class SepiaEffect : AdjustmentEffect
    {
        public override string Name => "Sepia";

        public static readonly DependencyProperty ScratchAmountProperty = DependencyProperty.Register("ScratchAmount", typeof(double), typeof(SepiaEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty NoiseAmountProperty = DependencyProperty.Register("NoiseAmount", typeof(double), typeof(SepiaEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty RandomCoord1Property = DependencyProperty.Register("RandomCoord1", typeof(Point), typeof(SepiaEffect), new UIPropertyMetadata(new Point(0D, 0D), PixelShaderConstantCallback(2)));

        public static readonly DependencyProperty RandomCoord2Property = DependencyProperty.Register("RandomCoord2", typeof(Point), typeof(SepiaEffect), new UIPropertyMetadata(new Point(0D, 0D), PixelShaderConstantCallback(3)));

        public static readonly DependencyProperty FrameProperty = DependencyProperty.Register("Frame", typeof(double), typeof(SepiaEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(4)));

        public static readonly DependencyProperty NoiseSamplerProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("NoiseSampler", typeof(SepiaEffect), 1);

        public SepiaEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(SepiaEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(ScratchAmountProperty);
            UpdateShaderValue(NoiseAmountProperty);
            UpdateShaderValue(RandomCoord1Property);
            UpdateShaderValue(RandomCoord2Property);
            UpdateShaderValue(FrameProperty);
            UpdateShaderValue(NoiseSamplerProperty);
        }

        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double ScratchAmount
        {
            get
            {
                return ((double)(GetValue(ScratchAmountProperty)));
            }
            set
            {
                SetValue(ScratchAmountProperty, value);
            }
        }

        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double NoiseAmount
        {
            get
            {
                return ((double)(GetValue(NoiseAmountProperty)));
            }
            set
            {
                SetValue(NoiseAmountProperty, value);
            }
        }

        /// <summary>The random coordinate 1 that is used for lookup in the noise texture.</summary>
        [Hidden(false)]
        public Point RandomCoord1
        {
            get
            {
                return ((Point)(GetValue(RandomCoord1Property)));
            }
            set
            {
                SetValue(RandomCoord1Property, value);
            }
        }
        /// <summary>The random coordinate 2 that is used for lookup in the noise texture.</summary>
        [Hidden(false)]
        public Point RandomCoord2
        {
            get
            {
                return ((Point)(GetValue(RandomCoord2Property)));
            }
            set
            {
                SetValue(RandomCoord2Property, value);
            }
        }

        /// <summary>The current frame.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Frame
        {
            get
            {
                return ((double)(GetValue(FrameProperty)));
            }
            set
            {
                SetValue(FrameProperty, value);
            }
        }

        [Hidden(false)]
        public Brush NoiseSampler
        {
            get
            {
                return ((Brush)(GetValue(NoiseSamplerProperty)));
            }
            set
            {
                SetValue(NoiseSamplerProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new SepiaEffect()
            {
                ScratchAmount = ScratchAmount,
                NoiseAmount = NoiseAmount,
                RandomCoord1 = RandomCoord1,
                RandomCoord2 = RandomCoord2,
                Frame = Frame,
                NoiseSampler = NoiseSampler
            };
        }
    }
}
