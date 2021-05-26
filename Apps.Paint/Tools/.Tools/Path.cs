using Imagin.Common;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class PathTool : FreePathTool { }

    [Serializable]
    public class FreePathTool : Tool
    {
        public override Cursor Cursor => Cursors.Cross;

        public override string Icon => Resources.Uri(nameof(Paint), "Images/FreePath.png").OriginalString;

        PointCollection points = new PointCollection();

        public override void OnPreviewRendered(DrawingContext input, double zoom)
        {
            base.OnPreviewRendered(input, zoom);
            if (points.Count > 1)
            {
                var geometry = new CustomPath(points).Geometry();
                input.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Black, 1 / zoom), geometry);
                input.DrawGeometry(Brushes.Transparent, new Pen(Brushes.White, 0.5 / zoom), geometry);
            }
        }

        public override bool OnMouseDown(Point point)
        {
            points.Add(point);
            return base.OnMouseDown(point);
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
                points.Add(point);
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);

            var layer = new PathLayer("Path");
            layer.Points = points;

            Document.Layers.Insert(0, layer);
            points = new PointCollection();
        }
    }
}