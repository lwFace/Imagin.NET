using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class EllipseSelectionTool : SelectionTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/EllipseSelection.png").OriginalString;

        protected override IEnumerable<Point> GetPoints(double x, double y, double height, double width)
        {
            var ellipse = new EllipseGeometry()
            {
                Center = new Point(width / 2.0, height / 2.0),
                RadiusX = width / 2.0,
                RadiusY = height / 2.0
            };

            var points = new List<Point>();
            var segments = ellipse.GetFlattenedPathGeometry(0.01, ToleranceType.Absolute).Figures[0].Segments;
            foreach (var segment in segments)
            {
                if (segment is PolyLineSegment)
                {
                    foreach (var p in (segment as PolyLineSegment)?.Points ?? Enumerable.Empty<Point>())
                        points.Add(new Point((p.X + x).Round(), (p.Y + y).Round()));
                }
            }

            foreach (var i in ShapeTool.CalculatePoints(points))
                yield return i;
        }

        public override string ToString() => "Ellipse selection";
    }
}