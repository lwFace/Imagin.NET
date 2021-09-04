using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public abstract class LayerStyleComponent : NamedObject
    {
        public readonly LayerStyle Style;

        [Hidden]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        bool isEnabled;
        [DisplayName("Enable")]
        [Featured]
        public bool IsEnabled
        {
            get => isEnabled;
            set => this.Change(ref isEnabled, value);
        }

        public LayerStyleComponent(LayerStyle style, string name) : base(name)
        {
            Style = style;
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            Style?.Changed();
        }
    }

    [Serializable]
    public class ColorOverlayLayerStyle : LayerStyleComponent
    {
        BlendModes blend = BlendModes.Normal;
        public BlendModes Blend
        {
            get => blend;
            set => this.Change(ref blend, value);
        }

        string color = $"255,0,0,0";
        public Color Color
        {
            get
            {
                var result = color.Split(',');
                return System.Windows.Media.Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte());
            }
            set => this.Change(ref color, $"{value.A},{value.R},{value.G},{value.B}");
        }

        public ColorOverlayLayerStyle(LayerStyle style) : base(style, "Color overlay") { }
    }

    [Serializable]
    public class DropShadowLayerStyle : LayerStyleComponent
    {
        public DropShadowLayerStyle(LayerStyle style) : base(style, "Drop shadow") { }
    }

    [Serializable]
    public class GradientOverlayLayerStyle : LayerStyleComponent
    {
        public GradientOverlayLayerStyle(LayerStyle style) : base(style, "Gradient overlay") { }
    }

    [Serializable]
    public class InnerGlowLayerStyle : LayerStyleComponent
    {
        public InnerGlowLayerStyle(LayerStyle style) : base(style, "Inner glow") { }
    }

    [Serializable]
    public class InnerShadowLayerStyle : LayerStyleComponent
    {
        public InnerShadowLayerStyle(LayerStyle style) : base(style, "Inner shadow") { }
    }

    [Serializable]
    public class OuterGlowLayerStyle : LayerStyleComponent
    {
        public OuterGlowLayerStyle(LayerStyle style) : base(style, "Outer glow") { }
    }

    [Serializable]
    public class PatternOverlayLayerStyle : LayerStyleComponent
    {
        public PatternOverlayLayerStyle(LayerStyle style) : base(style, "Pattern overlay") { }
    }

    [Serializable]
    public class SatinLayerStyle : LayerStyleComponent
    {
        public SatinLayerStyle(LayerStyle style) : base(style, "Satin") { }
    }

    [Serializable]
    public class StrokeLayerStyle : LayerStyleComponent
    {
        public StrokeLayerStyle(LayerStyle style) : base(style, "Stroke") { }
    }
}