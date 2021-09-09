using Imagin.Common;
using System;

namespace Paint
{
    [Serializable]
    public class EllipseTool : RegionShapeTool<EllipseLayer>
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Ellipse.png").OriginalString;

        protected override EllipseLayer NewLayer() 
            => new EllipseLayer("Untitled", Document.Height, Document.Width, Fill, Stroke, StrokeThickness);

        public override string ToString() => "Ellipse";
    }
}