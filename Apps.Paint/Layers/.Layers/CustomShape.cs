using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class CustomShapeLayer : RegionShapeLayer
    {
        CustomShape shape = null;
        public CustomShape Shape
        {
            get => shape;
            set => this.Change(ref shape, value);
        }

        public CustomShapeLayer(string name, int height, int width, CustomShape shape, SolidColorBrush fill, SolidColorBrush stroke, double strokeThickness) : base(LayerType.CustomShape, name, height, width, fill, stroke, strokeThickness)
        {
            this.shape = shape;
        }

        public override Layer Clone()
        {
            return new CustomShapeLayer(Name, Height, Width, Shape, Fill, Stroke, StrokeThickness)
            {
                IsLocked = IsLocked,
                IsVisible = IsVisible,
                Opacity = Opacity,
                Style = Style.Clone(),
                X = X,
                Y = Y,
            };
        }

        public override IEnumerable<System.Windows.Point> GetPoints()
        {
            var bounds = CustomShape.GetBounds(shape.Points);

            var oldPoints = CustomShape.Copy(shape.Points);
            CustomShape.Scale(oldPoints, new Point(Width.Coerce(int.MaxValue, 1), Height.Coerce(int.MaxValue, 1)));
            CustomShape.Translate(oldPoints, new Point(X, Y));

            for (var i = 0; i < oldPoints.Length; i += 2)
                yield return new System.Windows.Point(oldPoints[i], oldPoints[i + 1]);
        }

        public override void Render(Graphics g)
        {
            var region = Region;

            var oldPoints = CustomShape.Copy(shape.Points);
            CustomShape.Scale(oldPoints, new Point(Width, Height));
            CustomShape.Translate(oldPoints, new Point(X, Y));
            var newPoints = new Point[(oldPoints.Length / 2.0).Int32()];

            var i = 0;
            CustomShape.Each(oldPoints, point =>
            {
                newPoints[i] = point;
                i++;
            });

            g.FillPolygon(new SolidBrush(WithOpacity(Fill.Color)), newPoints);
        }

        public override void Render(WriteableBitmap input)
        {
            var bounds = CustomShape.GetBounds(shape.Points);
            var oldPoints = CustomShape.Copy(shape.Points);
            CustomShape.Scale(oldPoints, new Point(Width.Coerce(int.MaxValue, 1), Height.Coerce(int.MaxValue, 1)));
            CustomShape.Translate(oldPoints, new Point(X + (Width / 2), Y + (Height / 2)));
            input.FillPolygon(oldPoints, WithOpacity(Fill.Color).Double());
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Shape):
                    Render();
                    break;
            }
        }

        public override string ToString() => "Custom shape";
    }
}