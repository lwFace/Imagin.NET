using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class ThresholdEffect : AdjustmentEffect
    {
        public override string Name => "Threshold";

        public static readonly DependencyProperty Color1Property = DependencyProperty.Register("Color1", typeof(Color), typeof(ThresholdEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(0)));
        [Hidden(false)]
        public Color Color1
        {
            get => (Color)GetValue(Color1Property);
            set => SetValue(Color1Property, value);
        }

        public static readonly DependencyProperty Color2Property = DependencyProperty.Register("Color2", typeof(Color), typeof(ThresholdEffect), new UIPropertyMetadata(Color.FromArgb(255, 1, 1, 1), PixelShaderConstantCallback(1)));
        [Hidden(false)]
        public Color Color2
        {
            get => (Color)GetValue(Color2Property);
            set => SetValue(Color2Property, value);
        }

        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register("Level", typeof(double), typeof(ThresholdEffect), new UIPropertyMetadata(100.0, PixelShaderConstantCallback(2)));
        [Hidden(false)]
        [Range(1.0, 255.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Level
        {
            get => (double)GetValue(LevelProperty);
            set => SetValue(LevelProperty, value);
        }

        public ThresholdEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(ThresholdEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(Color1Property);
            UpdateShaderValue(Color2Property);
            UpdateShaderValue(LevelProperty);
        }

        public override AdjustmentEffect Copy()
        {
            return new ThresholdEffect()
            {
                Color1 = Color1,
                Color2 = Color2,
                Level = Level
            };
        }
    }
}