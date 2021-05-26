using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class SelectionTool : Tool
    {
        protected CustomPath newSelection;

        /// ----------------------------------------------------------------------------------

        public override Cursor Cursor => Cursors.Cross;

        public override string Icon => Resources.Uri(nameof(Paint), "Images/Selection.png").OriginalString;

        /// ----------------------------------------------------------------------------------

        [field: NonSerialized]
        WriteableBitmap preview;
        public WriteableBitmap Preview
        {
            get => preview;
            set => this.Change(ref preview, value);
        }

        /// ----------------------------------------------------------------------------------

        protected virtual IEnumerable<Point> GetPoints(double x, double y, double height, double width)
        {
            yield return new Point(x, y);
            yield return new Point(x, y + height);
            yield return new Point(x + width, y + height);
            yield return new Point(x + width, y);
        }

        /// ----------------------------------------------------------------------------------

        int Orientation(Vector2<int> p, Vector2<int> q, Vector2<int> r)
        {
            int result = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            return result == 0 ? 0 : result > 0 ? 1 : 2;
        }

        bool OnSegment(Vector2<int> p, Vector2<int> q, Vector2<int> r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) && q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }

        bool Intersects(Stroke<int> line1, Stroke<int> line2)
        {
            var p1 = new Vector2<int>(line1.X1, line1.Y1);
            var q1 = new Vector2<int>(line1.X2, line1.Y2);

            var p2 = new Vector2<int>(line2.X1, line2.Y1);
            var q2 = new Vector2<int>(line2.X2, line2.Y2);

            int o1 = 0, o2 = 0, o3 = 0, o4 = 0;
            o1 = Orientation(p1, q1, p2);
            o2 = Orientation(p1, q1, q2);
            o3 = Orientation(p2, q2, p1);
            o4 = Orientation(p2, q2, q1);

            if (o1 != o2 && o3 != o4)
                return true;

            if (o1 == 0 && OnSegment(p1, p2, q1))
                return true;

            if (o2 == 0 && OnSegment(p1, q2, q1))
                return true;

            if (o3 == 0 && OnSegment(p2, p1, q2))
                return true;

            if (o4 == 0 && OnSegment(p2, q1, q2))
                return true;

            return false;
        }

        bool Intersects(System.Windows.Media.PointCollection a, System.Windows.Media.PointCollection b)
        {
            var m = GetLines(a);
            var n = GetLines(b);
            foreach (var i in m)
            {
                foreach (var j in n)
                {
                    if (Intersects(i, j))
                        return true;
                }
            }
            return false;
        }

        IEnumerable<Stroke<int>> GetLines(System.Windows.Media.PointCollection points)
        {
            var previous = default(Point);
            foreach (var i in points)
            {
                if (previous != null)
                    yield return new Stroke<int>(previous.X.Int32(), previous.Y.Int32(), i.X.Int32(), i.Y.Int32());

                previous = i;
            }
        }

        /// ----------------------------------------------------------------------------------

        protected virtual Int32Region CalculateRegion(Point point) => ShapeTool.CalculateRegion(MouseDown.Value, point);

        /// ----------------------------------------------------------------------------------

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

            newSelection = new CustomPath();
            Document.Selections.Add(newSelection);

            base.OnMouseDown(point);
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                var region = CalculateRegion(point);
                newSelection.Refresh(GetPoints(region.X, region.Y, region.Height, region.Width));
            }
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            for (var i = 0; i < newSelection.Points.Count; i++)
            {
                var current = newSelection.Points[i];
                newSelection.Points[i] = current.Coerce(new Point(Document.Width, Document.Height), new Point(0, 0));
            }

            System.Windows.Media.PointCollection a = null, b = null;

            CustomPath previous = null;
            for (var i = Document.Selections.Count - 1; i >= 0; i--)
            {
                if (previous != null)
                {
                    a = previous.Points;
                    b = Document.Selections[i].Points;

                    //if (Intersects(a, b))
                        //Document.Selections.Remove(previous);
                }
                previous = Document.Selections[i];
            }
        }

        /// ----------------------------------------------------------------------------------

        public override string ToString() => "Selection";
    }
}