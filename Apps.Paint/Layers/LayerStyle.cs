using Imagin.Common;
using System;
using System.Runtime.CompilerServices;

namespace Paint
{
    [Serializable]
    public class LayerStyle : Base, Imagin.Common.ICloneable
    {
        public readonly Layer Layer;

        ColorOverlayLayerStyle colorOverlay;
        [DisplayName("Color overlay")]
        public ColorOverlayLayerStyle ColorOverlay
        {
            get => colorOverlay;
            set => this.Change(ref colorOverlay, value);
        }

        DropShadowLayerStyle dropShadow;
        [DisplayName("Drop shadow")]
        public DropShadowLayerStyle DropShadow
        {
            get => dropShadow;
            set => this.Change(ref dropShadow, value);
        }

        GradientOverlayLayerStyle gradientOverlay;
        [DisplayName("Gradient overlay")]
        public GradientOverlayLayerStyle GradientOverlay
        {
            get => gradientOverlay;
            set => this.Change(ref gradientOverlay, value);
        }

        InnerGlowLayerStyle innerGlow;
        [DisplayName("Inner glow")]
        public InnerGlowLayerStyle InnerGlow
        {
            get => innerGlow;
            set => this.Change(ref innerGlow, value);
        }

        InnerShadowLayerStyle innerShadow;
        [DisplayName("Inner shadow")]
        public InnerShadowLayerStyle InnerShadow
        {
            get => innerShadow;
            set => this.Change(ref innerShadow, value);
        }

        OuterGlowLayerStyle outerGlow;
        [DisplayName("Outer glow")]
        public OuterGlowLayerStyle OuterGlow
        {
            get => outerGlow;
            set => this.Change(ref outerGlow, value);
        }

        PatternOverlayLayerStyle patternOverlay;
        [DisplayName("Pattern Overlay")]
        public PatternOverlayLayerStyle PatternOverlay
        {
            get => patternOverlay;
            set => this.Change(ref patternOverlay, value);
        }

        SatinLayerStyle satin;
        public SatinLayerStyle Satin
        {
            get => satin;
            set => this.Change(ref satin, value);
        }

        StrokeLayerStyle stroke;
        public StrokeLayerStyle Stroke
        {
            get => stroke;
            set => this.Change(ref stroke, value);
        }

        public LayerStyle(Layer layer) : base()
        {
            Layer = layer;

            ColorOverlay
                = new ColorOverlayLayerStyle(this);
            DropShadow
                = new DropShadowLayerStyle(this);
            GradientOverlay
                = new GradientOverlayLayerStyle(this);
            InnerGlow
                = new InnerGlowLayerStyle(this);
            InnerShadow
                = new InnerShadowLayerStyle(this);
            OuterGlow
                = new OuterGlowLayerStyle(this);
            PatternOverlay
                = new PatternOverlayLayerStyle(this);
            Satin
                = new SatinLayerStyle(this);
            Stroke 
                = new StrokeLayerStyle(this);
        }

        object Imagin.Common.ICloneable.Clone()
        {
            return Clone();
        }
        public LayerStyle Clone() => new LayerStyle(Layer);

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            Layer.Changed(() => Layer.Style);
        }

        public override string ToString() => "Style";

        public void Clear()
        {
        }

        public void Rasterize()
        {
        }
    }
}