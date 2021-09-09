using Imagin.Common;
using System;
using System.Collections.ObjectModel;
using Imagin.Common.Linq;

namespace Paint
{
    [Serializable]
    public class CustomShapeTool : RegionShapeTool<CustomShapeLayer>
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/CustomShape.png").OriginalString;

        CustomShape shape = null;
        public CustomShape Shape
        {
            get => shape;
            set => this.Change(ref shape, value);
        }

        ObservableCollection<CustomShape> shapes = new ObservableCollection<CustomShape>();
        public ObservableCollection<CustomShape> Shapes
        {
            get => shapes;
            set => this.Change(ref shapes, value);
        }

        public CustomShapeTool() : base()
        {
            var size = new System.Windows.Size(20, 20);
            var origin = new System.Windows.Point(size.Width, size.Height);

            var polygonNames = new string[9]
            {
                "Triangle",
                "Rectangle",
                "Pentagon",
                "Hexagon",
                "Heptagon",
                "Octagon",
                "Nonagon",
                "Decagon",
                "Hexadecagon",
            };

            for (var i = 3; i <= 11; i++)
            {
                var n = i;
                i = i == 11 ? 16 : i;

                var points = CustomShape.From(CustomShape.GetPolygon(270.0.FromDegreeToRadian(), origin, size, i));
                CustomShape.Normalize(points);
                Shapes.Add(new CustomShape(polygonNames[n - 3], points));
            }
            for (var i = 3; i <= 16; i++)
            {
                var points = CustomShape.From(CustomShape.GetStar(270.0.FromDegreeToRadian(), i, 2, new System.Windows.Rect(origin, size)));
                CustomShape.Normalize(points);
                Shapes.Add(new CustomShape($"{i}-Star", points));
            }
            Shape = Shapes[0];
        }

        protected override CustomShapeLayer NewLayer()
            => new CustomShapeLayer("Untitled", Document.Height, Document.Width, Shape, Fill, Stroke, StrokeThickness);

        public override string ToString() => "Custom shape";
    }
}