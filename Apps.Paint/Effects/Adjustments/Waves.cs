using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that applies a wave pattern to the input.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class WavesEffect : AdjustmentEffect
    {
        public override string Name => "Wave warper";

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(double), typeof(WavesEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty WaveSizeProperty = DependencyProperty.Register("WaveSize", typeof(double), typeof(WavesEffect), new UIPropertyMetadata(((double)(64D)), PixelShaderConstantCallback(1)));

        public WavesEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(WavesEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(TimeProperty);
            UpdateShaderValue(WaveSizeProperty);
        }

        [Hidden(false)]
        [Range(0.0, 2048.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Time
        {
            get
            {
                return ((double)(GetValue(TimeProperty)));
            }
            set
            {
                SetValue(TimeProperty, value);
            }
        }

        /// <summary>The distance between waves. (the higher the value the closer the waves are to their neighbor).</summary>
        [Hidden(false)]
        [Range(32.0, 256.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double WaveSize
        {
            get
            {
                return ((double)(GetValue(WaveSizeProperty)));
            }
            set
            {
                SetValue(WaveSizeProperty, value);
            }
        }

        public override AdjustmentEffect Copy()
        {
            return new WavesEffect()
            {
                Time = Time,
                WaveSize = WaveSize
            };
        }
    }
}
