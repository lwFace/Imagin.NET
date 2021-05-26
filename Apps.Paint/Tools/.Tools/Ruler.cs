using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class RulerTool : Tool
    {
        public override Cursor Cursor => Cursors.Cross;

        double snap = 30;
        public double Snap
        {
            get => snap;
            set => this.Change(ref snap, value);
        }

        double? x = null;
        public double? X
        {
            get => x;
            set => this.Change(ref x, value);
        }

        double? y = null;
        public double? Y
        {
            get => y;
            set => this.Change(ref y, value);
        }

        double x2;
        public double X2
        {
            get => x2;
            set => this.Change(ref x2, value);
        }

        double y2;
        public double Y2
        {
            get => y2;
            set => this.Change(ref y2, value);
        }

        double? height = null;
        public double? Height
        {
            get => height;
            set => this.Change(ref height, value);
        }

        double? width = null;
        public double? Width
        {
            get => width;
            set => this.Change(ref width, value);
        }

        double? length = null;
        public double? Length
        {
            get => length;
            set => this.Change(ref length, value);
        }

        double? angle = null;
        public double? Angle
        {
            get => angle;
            set => this.Change(ref angle, value);
        }

        public override string Icon => Resources.Uri(nameof(Paint), "Images/Ruler.png").OriginalString;

        public override string ToString() => "Ruler";

        protected void Draw()
        {
            X = MouseDown.Value.X.Int32();
            Y = MouseDown.Value.Y.Int32();

            if (Angle != null && Length != null)
            {
                var newAngle = -(Angle.Value - 90).FromDegreeToRadian();
                var x = X.Value + Math.Sin(newAngle) * Length.Value;
                var y = Y.Value + Math.Cos(newAngle) * Length.Value;

                X2 = x;
                Y2 = y;
            }
            else
            {
                X2 = X.Value;
                Y2 = Y.Value;
            }
        }

        public override void OnPreviewRendered(DrawingContext input, double zoom)
        {
            base.OnPreviewRendered(input, zoom);
            if (x != null && y != null)
            {
                input.DrawLine(new Pen(Brushes.Black, 1 / zoom), new Point(x.Value, y.Value), new Point(x2, y2));
                input.DrawLine(new Pen(Brushes.White, 0.5 / zoom), new Point(x.Value, y.Value), new Point(x2, y2));
            }
        }

        public override bool OnMouseDown(Point point)
        {
            if (base.OnMouseDown(point))
            {
                Height = Width = Angle = Length = 0;
                Draw();
                return true;
            }
            return false;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                Height = point.Y - y;
                Width = point.X - x;

                Angle = Math.Atan2(point.Y - y.Value, point.X - X.Value).FromRadianToDegree().NearestFactor(Snap);
                Length = point.Distance(new Point(x.Value, y.Value));

                Draw();
            }
        }
    }
}