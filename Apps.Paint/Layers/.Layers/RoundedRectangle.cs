using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class RoundedRectangleLayer : RectangleLayer
    {
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

        [Hidden]
        public override LayerType Type => LayerType.RoundedRectangle;

        public RoundedRectangleLayer(string name, int height, int width, int radiusX, int radiusY, SolidColorBrush fill, SolidColorBrush stroke, double strokeThickness) : base(name, height, width, fill, stroke, strokeThickness)
        {
            this.radiusX = radiusX;
            this.radiusY = radiusY;
        }

        public sealed override Layer Clone()
        {
            return new RoundedRectangleLayer(Name, Height, Width, RadiusX, RadiusY, Fill, Stroke, StrokeThickness)
            {
                IsLocked = IsLocked,
                IsVisible = IsVisible,
                Opacity = Opacity,
                Style = Style.Clone(),
                X = X,
                Y = Y,
            };
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(RadiusX):
                case nameof(RadiusY):
                    Render();
                    break;
            }
        }

        public override IEnumerable<System.Windows.Point> GetPoints()
        {
            var rectangle = new RectangleGeometry()
            {
                Rect = new System.Windows.Rect(X, Y, Width, Height),
                RadiusX = radiusX,
                RadiusY = radiusY
            };

            var points = new List<System.Windows.Point>();
            var segments = rectangle.GetFlattenedPathGeometry(0.01, ToleranceType.Absolute).Figures[0].Segments;

            foreach (var segment in segments)
            {
                if (segment is PolyLineSegment)
                {
                    foreach (var p in (segment as PolyLineSegment)?.Points ?? Enumerable.Empty<System.Windows.Point>())
                        points.Add(new System.Windows.Point(p.X.Round(), p.Y.Round()));
                }
            }

            var finalPoints = ShapeTool.CalculatePoints(points);
            System.Windows.Point? firstPoint = null;

            foreach (var i in finalPoints)
            {
                if (firstPoint == null)
                    firstPoint = i;

                yield return i;
            }

            if (firstPoint != null)
                yield return firstPoint.Value;
        }

        public override void Render(Graphics g)
        {
            var oldPoints = GetPoints().Select(i => new Point(i.X.Int32(), i.Y.Int32())).ToArray();
            g.FillPolygon(FillBrush, oldPoints);
        }

        public override void Render(WriteableBitmap input)
        {
            var oldPoints = GetPoints().ToList();
            var count = oldPoints.Count;

            var newPoints = new int[count * 2];
            for (var i = 0; i < count; i++)
            {
                newPoints[i * 2] = oldPoints[i].X.Int32();
                newPoints[(i * 2) + 1] = oldPoints[i].Y.Int32();
            }

            input.FillPolygon(newPoints, WithOpacity(Fill.Color).Double());
        }

        public override string ToString() => "Rounded rectangle";
    }
}