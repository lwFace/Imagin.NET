using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class EllipseLayer : RegionShapeLayer
    {
        public EllipseLayer(string name, int height, int width, SolidColorBrush fill, SolidColorBrush stroke, double strokeThickness) : base(LayerType.Ellipse, name, height, width, fill, stroke, strokeThickness) { }

        public sealed override Layer Clone()
        {
            return new EllipseLayer(Name, Height, Width, Fill, Stroke, StrokeThickness)
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
            var ellipse = new EllipseGeometry()
            {
                Center = new System.Windows.Point(Width / 2.0, Height / 2.0),
                RadiusX = Width / 2.0,
                RadiusY = Height / 2.0
            };

            var points = new List<System.Windows.Point>();
            var segments = ellipse.GetFlattenedPathGeometry(0.01, ToleranceType.Absolute).Figures[0].Segments;
            foreach (var segment in segments)
            {
                if (segment is PolyLineSegment)
                {
                    foreach (var p in (segment as PolyLineSegment)?.Points ?? Enumerable.Empty<System.Windows.Point>())
                        points.Add(new System.Windows.Point((p.X + X).Round(), (p.Y + Y).Round()));
                }
            }

            foreach (var i in ShapeTool.CalculatePoints(points))
                yield return i;
        }

        public override void Render(Graphics g)
        {
            var region = Region;
            g.DrawEllipse(StrokePen, region.X, region.Y, region.Width, region.Height);
            g.FillEllipse(FillBrush, region.X + StrokeThickness.Int32(), region.Y + StrokeThickness.Int32(), region.Width - (StrokeThickness.Int32() * 2), region.Height - (StrokeThickness.Int32() * 2));
        }

        public override void Render(WriteableBitmap input)
        {
            var x1 = Region.X;
            var y1 = Region.Y;

            var x2 = Region.X + Region.Width;
            var y2 = Region.Y + Region.Height;

            if (Style.ColorOverlay.IsEnabled)
            {
                input.FillEllipse(x1, y1, x2, y2, Style.ColorOverlay.Color, null);
                return;
            }

            input.FillEllipse(x1, y1, x2, y2, Stroke.Color, null);

            x1 += StrokeThickness.Int32();
            y1 += StrokeThickness.Int32();
            x2 -= StrokeThickness.Int32();
            y2 -= StrokeThickness.Int32();

            x2 = x2 <= x1 ? x1 + 1 : x2;
            y2 = y2 <= y1 ? y1 + 1 : y2;

            input.FillEllipse(x1, y1, x2, y2, Fill.Color, null);

            if (Transform is ScaleTransform)
            {

            }
            if (Transform is SkewTransform)
            {
                if ((Transform as SkewTransform).TopLeft != null)
                {
                    Preview = TransformTool.Shear(input.Bitmap(), (Transform as SkewTransform).TopLeft.X, (Transform as SkewTransform).TopLeft.Y).WriteableBitmap();
                }
            }
        }

        public override string ToString() => "Ellipse";
    }
}
