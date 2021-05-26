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
    public class ColorXYEffect : BaseEffect
    {
        public override string RelativePath => $"/{AssemblyData.Name};component/Effects/ColorXY.ps";

        public static readonly DependencyProperty ActualModelProperty = DependencyProperty.Register(nameof(ActualModel), typeof(Model), typeof(ColorXYEffect), new PropertyMetadata(null));
        public Model ActualModel
        {
            get => (Model)GetValue(ActualModelProperty);
            set => SetValue(ActualModelProperty, value);
        }

        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(ColorXYEffect), 0);
        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }

        internal static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(double), typeof(ColorXYEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(0)));
        internal double Model
        {
            get => (double)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        internal static readonly DependencyProperty ComponentProperty = DependencyProperty.Register(nameof(Component), typeof(double), typeof(ColorXYEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(1)));
        internal double Component
        {
            get => (double)GetValue(ComponentProperty);
            set => SetValue(ComponentProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), typeof(ColorXYEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(2)));
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public ColorXYEffect()
        {
            this.Bind(ComponentProperty, $"{nameof(ActualModel)}.{nameof(VisualModel.SelectedComponent)}", this, BindingMode.OneWay, new DefaultConverter<VisualComponent, double>(i => (int)(Components)i, null));
            this.Bind(ModelProperty, $"{nameof(ActualModel)}", this, BindingMode.OneWay, new DefaultConverter<VisualModel, double>(i => (int)(VisualModels)i, null));

            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(RelativePath, UriKind.Relative);
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ModelProperty);
            UpdateShaderValue(ComponentProperty);
            UpdateShaderValue(ValueProperty);
        }
    }
}