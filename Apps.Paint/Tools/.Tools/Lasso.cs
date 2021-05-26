﻿using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class LassoTool : PreviewSelectionTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Lasso.png").OriginalString;

        public override void OnPreviewRendered(DrawingContext input, double zoom)
        {
            base.OnPreviewRendered(input, zoom);
            if (Preview?.Count > 1)
            {
                var geometry = new CustomPath(Preview).Geometry();
                input.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Black, 1 / zoom), geometry);
                input.DrawGeometry(Brushes.Transparent, new Pen(Brushes.White, 0.5 / zoom), geometry);
            }
        }

        public override bool OnMouseDown(Point point)
        {
            //Move the selection
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                return false;
            }
            //Start a new selection (otherwise, add to the selection)
            else if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                Document.Selections.Clear();
            }

            base.OnMouseDown(point);
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                Points.Add(new Point(point.X, point.Y));
                Preview = new PointCollection(Points);
            }
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);

            var newSelection = new CustomPath();
            ShapeTool.CalculatePoints(Points).ForEach(i => newSelection.Points.Add(i));

            for (var i = 0; i < newSelection.Points.Count; i++)
            {
                var current = newSelection.Points[i];
                newSelection.Points[i] = current.Coerce(new Point(Document.Width, Document.Height), new Point(0, 0));
            }

            Document.Selections.Add(newSelection);
            Points.Clear();
            Preview = null;
        }

        public override string ToString() => "Lasso";
    }
}