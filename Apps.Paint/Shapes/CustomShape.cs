using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class CustomShape : NamedObject
    {
        Int32Region bounds;
        public Int32Region Bounds
        {
            get => bounds;
            private set => this.Change(ref bounds, value);
        }

        int[] points = null;
        public int[] Points
        {
            get => points;
            set
            {
                this.Change(ref points, value);
                Bounds = GetBounds(value);
            }
        }

        [field: NonSerialized]
        WriteableBitmap preview = null;
        public WriteableBitmap Preview
        {
            get => preview;
            set => this.Change(ref preview, value);
        }

        //---------------------------------------------------------------------------

        public CustomShape(string name, params int[] points) : base(name)
        {
            Points = points;
        }

        //---------------------------------------------------------------------------

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Points):
                    Preview = new WriteableBitmap(40, 40, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);
                    Preview.Clear(Colors.Transparent);

                    var newPoints = Copy(Points);
                    Translate(newPoints, new System.Drawing.Point(20, 20));
                    Preview.FillPolygon(newPoints, Colors.Black);
                    break;
            }
        }

        //---------------------------------------------------------------------------

        #region static void FindIntersection

        // Find the point of intersection between
        // the lines p1 --> p2 and p3 --> p4.
        static void FindIntersection
        (
            Point p1, Point p2, Point p3, Point p4,
            out bool lines_intersect, out bool segments_intersect,
            out Point intersection, out Point close_p1, out Point close_p2
        )
        {
            // Get the segments' parameters.
            double dx12 = p2.X - p1.X;
            double dy12 = p2.Y - p1.Y;
            double dx34 = p4.X - p3.X;
            double dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            double denominator = (dy12 * dx34 - dx12 * dy34);

            double t1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;
            if (double.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                segments_intersect = false;
                intersection = new Point(double.NaN, double.NaN);
                close_p1 = new Point(double.NaN, double.NaN);
                close_p2 = new Point(double.NaN, double.NaN);
                return;
            }
            lines_intersect = true;

            double t2 = ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12) / -denominator;

            // Find the point of intersection.
            intersection = new Point(p1.X + dx12 * t1, p1.Y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect =
                ((t1 >= 0) && (t1 <= 1) &&
                 (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            close_p1 = new Point(p1.X + dx12 * t1, p1.Y + dy12 * t1);
            close_p2 = new Point(p3.X + dx34 * t2, p3.Y + dy34 * t2);
        }

        #endregion

        /// <summary>
        /// Calculate the inner star radius.
        /// </summary>
        /// <param name="num_points"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        static double CalculateConcaveRadius(int num_points, int skip)
        {
            // For really small numbers of points.
            if (num_points < 5) return 0.33f;

            // Calculate angles to key points.
            double dtheta = 2 * Math.PI / num_points;
            double theta00 = -Math.PI / 2;
            double theta01 = theta00 + dtheta * skip;
            double theta10 = theta00 + dtheta;
            double theta11 = theta10 - dtheta * skip;

            // Find the key points.
            Point pt00 = new Point(Math.Cos(theta00), Math.Sin(theta00));
            Point pt01 = new Point(Math.Cos(theta01), Math.Sin(theta01));
            Point pt10 = new Point(Math.Cos(theta10), Math.Sin(theta10));
            Point pt11 = new Point(Math.Cos(theta11), Math.Sin(theta11));

            // See where the segments connecting the points intersect.
            bool lines_intersect, segments_intersect;
            Point intersection, close_p1, close_p2;
            FindIntersection(pt00, pt01, pt10, pt11,
                out lines_intersect, out segments_intersect,
                out intersection, out close_p1, out close_p2);

            // Calculate the distance between the
            // point of intersection and the center.
            return Math.Sqrt(intersection.X * intersection.X + intersection.Y * intersection.Y);
        }

        //---------------------------------------------------------------------------

        public static System.Drawing.Point[] GetPolygon(double startAngle, Point center, Size size, int sides)
        {
            sides.Coerce(32, 3);

            var a = 2.0 * Math.PI / sides.Double();
            var b = startAngle;
            double x = 0, y = 0;

            var points = new System.Drawing.Point[sides];
            for (int i = 0; i < sides; i++)
            {
                x = center.X + size.Width * Math.Cos(b);
                y = center.Y + size.Height * Math.Sin(b);
                points[i] = new System.Drawing.Point(x.Round().Int32(), y.Round().Int32());
                b += a;
            }

            return points;
        }

        /// <summary>
        /// Generate the points for a star.
        /// </summary>
        /// <param name="startAngle"></param>
        /// <param name="sides"></param>
        /// <param name="skip"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static System.Drawing.Point[] GetStar(double startAngle, int sides, int skip, Rect rect)
        {
            sides.Coerce(32, 2);

            double theta, dtheta;
            System.Drawing.Point[] result;

            double rx = rect.Width.Single() / 2f;
            double ry = rect.Height.Single() / 2f;
            double cx = rect.X.Single() + rx;
            double cy = rect.Y.Single() + ry;

            // If this is a polygon, don't bother with concave points.
            if (skip == 1)
            {
                result = new System.Drawing.Point[sides];
                theta = startAngle;
                dtheta = 2 * Math.PI / sides;
                for (int i = 0; i < sides; i++)
                {
                    var x = (cx + rx * Math.Cos(theta)).Int32();
                    var y = (cy + ry * Math.Sin(theta)).Int32();
                    result[i] = new System.Drawing.Point(x, y);
                    theta += dtheta;
                }
                return result;
            }

            // Find the radius for the concave vertices.
            double concave_radius = CalculateConcaveRadius(sides, skip);

            // Make the points.
            result = new System.Drawing.Point[2 * sides];
            theta = startAngle;
            dtheta = Math.PI / sides;
            for (int i = 0; i < sides; i++)
            {
                var x = (cx + rx * Math.Cos(theta)).Int32();
                var y = (cy + ry * Math.Sin(theta)).Int32();
                result[2 * i] = new System.Drawing.Point(x, y);
                theta += dtheta;

                x = (cx + rx * Math.Cos(theta) * concave_radius).Int32();
                y = (cy + ry * Math.Sin(theta) * concave_radius).Int32();
                result[2 * i + 1] = new System.Drawing.Point(x, y);
                theta += dtheta;
            }
            return result;
        }

        //---------------------------------------------------------------------------

        public static int[] From(System.Drawing.Point[] input)
        {
            var count = input.Length * 2;
            var result = new int[count + 2];

            for (var i = 0; i < input.Length; i++)
            {
                result[i * 2]
                    = input[i].X;
                result[(i * 2) + 1]
                    = input[i].Y;
            }

            result[count] = result[0];
            result[count + 1] = result[1];
            return result;
        }

        public static int[] From(IList<Point> input)
        {
            var count = input.Count * 2;
            var result = new int[count];

            for (var i = 0; i < input.Count; i++)
            {
                result[i * 2]
                    = input[i].X.Int32();
                result[(i * 2) + 1]
                    = input[i].Y.Int32();
            }
            return result;
        }

        //---------------------------------------------------------------------------

        public static void Each(int[] input, Action<System.Drawing.Point> action)
        {
            for (var i = 0; i < input.Length; i += 2)
                action(new System.Drawing.Point(input[i], input[i + 1]));
        }

        public static void Each(int[] input, Func<System.Drawing.Point, System.Drawing.Point> action)
        {
            for (var i = 0; i < input.Length; i += 2)
            {
                var result = action(new System.Drawing.Point(input[i], input[i + 1]));
                input[i] = result.X;
                input[i + 1] = result.Y;
            }
        }

        //---------------------------------------------------------------------------

        public static int[] Copy(int[] input)
        {
            var result = new int[input.Length];
            for (var i = 0; i < input.Length; i++)
                result[i] = input[i];

            return result;
        }

        //---------------------------------------------------------------------------

        public static void Scale(int[] input, System.Drawing.Point scale)
        {
            var bounds = GetBounds(input);
            Each(input, point => new System.Drawing.Point((point.X * scale.X / bounds.Width), (point.Y * scale.Y / bounds.Height)));
        }

        public static void Translate(int[] input, System.Drawing.Point newCenter)
        {
            var bounds = GetBounds(input);
            var oldCenter = bounds.Center;

            var x = (newCenter.X - oldCenter.X);
            var y = (newCenter.Y - oldCenter.Y);

            Each(input, point => new System.Drawing.Point(point.X + x, point.Y + y));
        }

        //---------------------------------------------------------------------------

        public static Int32Region GetBounds(int[] points)
        {
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            Each(points, point =>
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);

                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            });

            //bottom left = minX, minY
            //bottom right = maxX, minY
            //top left = minX, maxY <------------------ What we want!
            //top right = maxX, maxY
            return new Int32Region(minX, maxY, maxX - minX, maxY - minY);
        }

        //---------------------------------------------------------------------------

        /// <summary>
        /// Moves all points in the direction of the origin until each lives in quadrant I.
        /// </summary>
        /// <param name="points"></param>
        public static void Normalize(int[] points)
        {
            var bounds = GetBounds(points);

            var x = (bounds.Width.Double() / 2.0).Int32();
            var y = (bounds.Height.Double() / 2.0).Int32();

            Translate(points, new System.Drawing.Point(x, y));
        }
    }
}