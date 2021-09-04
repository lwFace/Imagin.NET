using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class RectangleLayer : RegionShapeLayer
    {
        public RectangleLayer(string name, int height, int width, SolidColorBrush fill, SolidColorBrush stroke, double strokeThickness) : base(LayerType.Rectangle, name, height, width, fill, stroke, strokeThickness) { }

        public override Layer Clone()
        {
            return new RectangleLayer(Name, Height, Width, Fill, Stroke, StrokeThickness)
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
            yield return new System.Windows.Point(Region.X, Region.Y);
            yield return new System.Windows.Point(Region.X, Region.Y + Region.Height);
            yield return new System.Windows.Point(Region.X + Region.Width, Region.Y + Region.Height);
            yield return new System.Windows.Point(Region.X + Region.Width, Region.Y);
        }

        public override void Render(Graphics g)
        {
            var region = Region;
            g.DrawRectangle(StrokePen, region.X, region.Y, region.Width, region.Height);
            g.FillRectangle(FillBrush, region.X + StrokeThickness.Int32(), region.Y + StrokeThickness.Int32(), region.Width - (StrokeThickness.Int32() * 2), region.Height - (StrokeThickness.Int32() * 2));
        }

        public override void Render(WriteableBitmap input)
        {
            var x1 = Region.X;
            var y1 = Region.Y;

            var x2 = Region.X + Region.Width;
            var y2 = Region.Y + Region.Height;

            input.FillRectangle(x1, y1, x2, y2, Stroke.Color);

            x1 += StrokeThickness.Int32();
            y1 += StrokeThickness.Int32();
            x2 -= StrokeThickness.Int32();
            y2 -= StrokeThickness.Int32();

            x2 = x2 <= x1 ? x1 + 1 : x2;
            y2 = y2 <= y1 ? y1 + 1 : y2;

            input.FillRectangle(x1, y1, x2, y2, Fill.Color);
        }

        public override string ToString() => "Rectangle";
    }
}