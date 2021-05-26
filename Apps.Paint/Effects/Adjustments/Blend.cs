using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class BlendEffect : AdjustmentEffect
    {
        public override string Name => "Blend";

        public static DependencyProperty BlendProperty = DependencyProperty.Register(nameof(Blend), typeof(BlendModes), typeof(BlendEffect), new FrameworkPropertyMetadata(BlendModes.Normal, FrameworkPropertyMetadataOptions.None, OnBlendChanged));
        [Hidden(false)]
        public BlendModes Blend
        {
            get => (BlendModes)GetValue(BlendProperty);
            set => SetValue(BlendProperty, value);
        }
        protected static void OnBlendChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BlendEffect).OnBlendChanged((BlendModes)e.NewValue);
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(BlendEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register("Alpha", typeof(double), typeof(BlendEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(1)));
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double Alpha
        {
            get => (double)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(double), typeof(BlendEffect), new UIPropertyMetadata((double)(int)BlendModes.Normal, PixelShaderConstantCallback(2)));
        public double Mode
        {
            get => (double)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public BlendEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(BlendEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(ColorProperty);
            UpdateShaderValue(AlphaProperty);
            UpdateShaderValue(ModeProperty);
        }

        protected virtual void OnBlendChanged(BlendModes input)
        {
            SetCurrentValue(ModeProperty, (double)(int)input);
        }

        public override AdjustmentEffect Copy()
        {
            return new BlendEffect()
            {
                Color = Color,
                Alpha = Alpha,
                Mode = Mode
            };
        }
    }
}