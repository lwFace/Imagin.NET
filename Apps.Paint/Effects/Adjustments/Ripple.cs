using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    /// <summary>An effect that superimposes rippling waves upon the input.</summary>
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class RippleEffect : AdjustmentEffect
    {
        public override string Name => "Ripple";

        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(RippleEffect), new UIPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(0)));

        public static readonly DependencyProperty AmplitudeProperty = DependencyProperty.Register("Amplitude", typeof(double), typeof(RippleEffect), new UIPropertyMetadata(((double)(0.1D)), PixelShaderConstantCallback(1)));

        public static readonly DependencyProperty FrequencyProperty = DependencyProperty.Register("Frequency", typeof(double), typeof(RippleEffect), new UIPropertyMetadata(((double)(70D)), PixelShaderConstantCallback(2)));

        public static readonly DependencyProperty PhaseProperty = DependencyProperty.Register("Phase", typeof(double), typeof(RippleEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(3)));

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(RippleEffect), new UIPropertyMetadata(((double)(1.5D)), PixelShaderConstantCallback(4)));

        public RippleEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(RippleEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(AmplitudeProperty);
            UpdateShaderValue(FrequencyProperty);
            UpdateShaderValue(PhaseProperty);
            UpdateShaderValue(AspectRatioProperty);
        }

        /// <summary>The center point of the ripples.</summary>
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

        /// <summary>The amplitude of the ripples.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double Amplitude
        {
            get
            {
                return ((double)(GetValue(AmplitudeProperty)));
            }
            set
            {
                SetValue(AmplitudeProperty, value);
            }
        }

        /// <summary>The frequency of the ripples.</summary>
        [Hidden(false)]
        [Range(0.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Frequency
        {
            get
            {
                return ((double)(GetValue(FrequencyProperty)));
            }
            set
            {
                SetValue(FrequencyProperty, value);
            }
        }

        /// <summary>The phase of the ripples.</summary>
        [Hidden(false)]
        [Range(-20.0, 20.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Phase
        {
            get
            {
                return ((double)(GetValue(PhaseProperty)));
            }
            set
            {
                SetValue(PhaseProperty, value);
            }
        }

        /// <summary>The aspect ratio (width / height) of the input.</summary>
        [Hidden(false)]
        [Range(0.5, 2.0, 0.01)]
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
            return new RippleEffect()
            {
                Center = Center,
                Amplitude = Amplitude,
                Frequency = Frequency,
                Phase = Phase,
                AspectRatio = AspectRatio,
            };
        }
    }
}
