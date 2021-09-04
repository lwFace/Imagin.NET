using Imagin.Common;
using System;

namespace Paint
{
    [Serializable]
    public class PolygonTool : RegionShapeTool<PolygonLayer>
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Polygon.png").OriginalString;

        double angle = 0;
        public double Angle
        {
            get => angle;
            set => this.Change(ref angle, value);
        }

        int indent = 2;
        public int Indent
        {
            get => indent;
            set => this.Change(ref indent, value);
        }

        uint sides = 5;
        public uint Sides
        {
            get => sides;
            set => this.Change(ref sides, value);
        }

        bool star = false;
        public bool Star
        {
            get => star;
            set => this.Change(ref star, value);
        }

        protected override PolygonLayer NewLayer()
            => new PolygonLayer("Untitled", Document.Height, Document.Width, Angle, Sides, Star, Indent, Fill, Stroke, StrokeThickness);

        public override string ToString() => "Polygon";
    }
}