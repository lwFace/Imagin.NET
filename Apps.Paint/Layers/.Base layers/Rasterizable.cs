using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Math;
using System;

namespace Paint
{
    [Serializable]
    public abstract class RasterizableLayer : VisualLayer
    {
        protected RasterizableLayer(LayerType type) : this(type, string.Empty) { }

        protected RasterizableLayer(LayerType type, string name) : base(type, name) { }

        public void Rasterize(LayerCollection layers, System.Drawing.Size size)
        {
            var result = new PixelLayer(Name, size);
            result.Pixels = Render(size).WriteableBitmap();
            result.Parent = Parent;

            layers
            = Parent != null
            ? Parent.Layers
            : layers;

            var index = layers.IndexOf(this);
            layers.Remove(this);
            layers.Insert(index, result);
        }

        public override void Resize(Int32Size oldSize, Int32Size newSize, Interpolations interpolation) { }
    }
}