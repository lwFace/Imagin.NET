using Imagin.Common;
using System;

namespace Paint
{
    [Serializable]
    public class RectangleTool : RegionShapeTool<RectangleLayer>
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Rectangle.png").OriginalString;

        protected override RectangleLayer NewLayer()
            => new RectangleLayer("Untitled", Document.Height, Document.Width, Fill, Stroke, StrokeThickness);

        public override string ToString() => "Rectangle";
    }
}