using Imagin.Common;
using System;

namespace Paint
{
    [Serializable]
    public class RoundedRectangleTool : RegionShapeTool<RoundedRectangleLayer>
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/RoundedRectangle.png").OriginalString;

        int radiusX = 5;
        public int RadiusX
        {
            get => radiusX;
            set => this.Change(ref radiusX, value);
        }

        int radiusY = 5;
        public int RadiusY
        {
            get => radiusY;
            set => this.Change(ref radiusY, value);
        }

        protected override RoundedRectangleLayer NewLayer()
            => new RoundedRectangleLayer("Untitled", Document.Height, Document.Width, RadiusX, RadiusY, Fill, Stroke, StrokeThickness);

        public override string ToString() => "Rounded rectangle";
    }
}