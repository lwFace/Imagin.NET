using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Media.Models;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Imagin.Common.Effects
{
    public class ColorZEffect : BaseEffect
    {
        public override string RelativePath => $"/{AssemblyData.Name};component/Effects/ColorZ.ps";

        public static readonly DependencyProperty ActualModelProperty = DependencyProperty.Register(nameof(ActualModel), typeof(Model), typeof(ColorZEffect), new PropertyMetadata(null));
        public Model ActualModel
        {
            get => (Model)GetValue(ActualModelProperty);
            set => SetValue(ActualModelProperty, value);
        }

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ColorZEffect), 0);
        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        internal static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(double), typeof(ColorZEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(0)));
        internal double Model
        {
            get => (double)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        internal static readonly DependencyProperty ComponentProperty = DependencyProperty.Register("Component", typeof(double), typeof(ColorZEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(1)));
        internal double Component
        {
            get => (double)GetValue(ComponentProperty);
            set => SetValue(ComponentProperty, value);
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorZEffect), new UIPropertyMetadata(Colors.Red, PixelShaderConstantCallback(2)));
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public ColorZEffect()
        {
            this.Bind(ComponentProperty, $"{nameof(ActualModel)}.{nameof(VisualModel.SelectedComponent)}", this, BindingMode.OneWay, new DefaultConverter<VisualComponent, double>(i => (int)(Components)i, null));
            this.Bind(ModelProperty, $"{nameof(ActualModel)}", this, BindingMode.OneWay, new DefaultConverter<VisualModel, double>(i => (int)(VisualModels)i, null));

            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(RelativePath, UriKind.Relative);
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ModelProperty);
            UpdateShaderValue(ComponentProperty);
            UpdateShaderValue(ColorProperty);
        }
    }
}