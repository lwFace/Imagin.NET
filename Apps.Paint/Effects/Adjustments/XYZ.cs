using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using System.Windows;
using System.Windows.Media.Effects;

namespace Paint.Adjust
{
    [PropertyVisibility(MemberVisibility.Explicit)]
    public class XYZEffect : AdjustmentEffect
    {
        public override string Name => "XYZ";

        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(XYZEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(XYZEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty ZProperty = DependencyProperty.Register("Z", typeof(double), typeof(XYZEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(2)));

        public XYZEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = Resource(nameof(XYZEffect));
            PixelShader = pixelShader;

            UpdateShaderValue(XProperty);
            UpdateShaderValue(YProperty);
            UpdateShaderValue(ZProperty);
        }

        public XYZEffect(double x, double y, double z) : this()
        {
            SetCurrentValue(XProperty, x);
            SetCurrentValue(YProperty, y);
            SetCurrentValue(ZProperty, z);
        }

        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double X
        {
            get => (double)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }
        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Y
        {
            get => (double)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }
        [Hidden(false)]
        [Range(-100.0, 100.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Z
        {
            get => (double)GetValue(ZProperty);
            set => SetValue(ZProperty, value);
        }

        public override AdjustmentEffect Copy()
        {
            return new XYZEffect()
            {
                X = X,
                Y = Y,
                Z = Z
            };
        }
    }
}