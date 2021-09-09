using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class BucketTool : Tool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Bucket.png").OriginalString;

        byte tolerance = 32;
        public byte Tolerance
        {
            get => tolerance;
            set => this.Change(ref tolerance, value);
        }

        bool Similar(Color a, Color b)
        {
            return a == b;
            /*
            if (tolerance == 0)
                return a == b;

            var ab = a.Drawing().GetBrightness();
            var bb = b.Drawing().GetBrightness();

            var cb = (ab - bb).Absolute() * 255f;
            return cb <= tolerance;
            */
        }

        void Draw(System.Drawing.Point point, System.Drawing.Color fill, System.Drawing.Color old)
        {
            var height = Bitmap.PixelHeight.UInt32();
            var width = Bitmap.PixelWidth.UInt32();

            if (fill == old)
                return;

            if (point.X < 0 || point.X >= Bitmap.PixelWidth)
                return;

            if (point.Y < 0 || point.Y >= Bitmap.PixelHeight)
                return;

            if (Bitmap.GetPixel(point.X, point.Y) != old.Double())
                return;

            Queue<System.Drawing.Point> queue = new Queue<System.Drawing.Point>();
            Bitmap.SetPixel(point.X, point.Y, fill.Double());
            queue.Enqueue(point);

            while (queue.Count > 0)
            {
                var next = queue.Dequeue();

                //Left
                var p = new System.Drawing.Point(next.X - 1, next.Y);
                if (p.X >= 0 && Similar(Bitmap.GetPixel(p.X, p.Y), old.Double()))
                {
                    Bitmap.SetPixel(p.X, p.Y, fill.Double());
                    queue.Enqueue(p);
                }

                //Right
                p = new System.Drawing.Point(next.X + 1, next.Y);
                if (p.X < width && Similar(Bitmap.GetPixel(p.X, p.Y), old.Double()))
                {
                    Bitmap.SetPixel(p.X, p.Y, fill.Double());
                    queue.Enqueue(p);
                }

                //Top
                p = new System.Drawing.Point(next.X, next.Y + 1);
                if (p.Y < height && Similar(Bitmap.GetPixel(p.X, p.Y), old.Double()))
                {
                    Bitmap.SetPixel(p.X, p.Y, fill.Double());
                    queue.Enqueue(p);
                }

                //Bottom
                p = new System.Drawing.Point(next.X, next.Y - 1);
                if (p.Y >= 0 && Similar(Bitmap.GetPixel(p.X, p.Y), old.Double()))
                {
                    Bitmap.SetPixel(p.X, p.Y, fill.Double());
                    queue.Enqueue(p);
                }
            }
        }

        protected override bool AssertLayer() => AssertPixelLayer();

        public override bool OnMouseDown(Point point)
        {
            if (base.OnMouseDown(point))
            {
                int x = point.X.Int32(), y = point.Y.Int32();

                var color1 = Get.Current<Options>().ForegroundColor.Int32();
                var color2 = Bitmap.GetPixel(x, y).Int32();

                Draw(point.Int32(), color1, color2);
                return true;
            }
            return false;
        }

        public override string ToString() => "Bucket";
    }
}