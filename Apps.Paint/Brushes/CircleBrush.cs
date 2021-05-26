using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class CircleBrush : HardBrush
    {
        [Hidden]
        public override string Name => base.Name;

        [Hidden]
        public override WriteableBitmap Preview => Render(Colors.Black, 16);

        /// ---------------------------------------------------------------------------------------

        public override Matrix<byte> GetBytes(int size)
        {
            var result = new Matrix<byte>(size.UInt32(), size.UInt32());
            WriteableBitmap bitmap = null;

            if (Hardness < 1)
            {
                bitmap = BitmapFactory.WriteableBitmap(size, size);

                int columns = size, rows = size;
                int x1 = 0, y1 = 0;

                double a = Hardness;

                uint ring = 0;
                int rings = GetRings(columns, rows);

                var total = 1 - Hardness;
                var i = total / rings.Double();

                while (columns > 1 && rows > 1)
                {
                    bitmap.FillEllipse(x1, y1, x1 + columns, y1 + rows, Colors.White.A(a.Multiply(255)), null);
                    a += i;

                    x1++;
                    y1++;

                    ring++;
                    columns -= 2;
                    rows -= 2;
                }

                bitmap.ForEach((x2, y2, color) =>
                {
                    result.SetValue(y2.UInt32(), x2.UInt32(), color.A);
                    return color;
                });
            }
            else
            {
                bitmap = BitmapFactory.WriteableBitmap(result.Rows.Int32(), result.Columns.Int32());
                bitmap.FillEllipse(0, 0, result.Columns.Int32() - 1, result.Rows.Int32() - 1, Colors.Black);

                for (uint x = 0; x < result.Columns; x++)
                {
                    for (uint y = 0; y < result.Rows; y++)
                        result.SetValue(y, x, bitmap.GetPixel(x.Int32(), y.Int32()).A);
                }
            }
            return result;
        }

        /// ---------------------------------------------------------------------------------------

        public override void Draw(WriteableBitmap bitmap, Vector2<int> point, Color color, int size, BlendModes? mode)
        {
            var d = (size.Double() / 2.0).Round();
            double x1 = point.X - d, y1 = point.Y - d;
            double x2 = point.X + size - d, y2 = point.Y + size - d;

            if (Hardness < 1)
            {
                var bytes = GetBytes(size);
                bitmap.FillRectangle(x1.Int32(), y1.Int32(), bytes.Transform(i => color.A(i)), mode);
                return;
            }

            bitmap.FillEllipse(x1.Int32(), y1.Int32(), x2.Int32() - 1, y2.Int32() - 1, color, mode);
        }

        /// ---------------------------------------------------------------------------------------

        public override WriteableBitmap Render(Color color, int size)
        {
            var result = BitmapFactory.WriteableBitmap(size, size);
            Draw(result, new Vector2<int>(size / 2, size / 2), color, size, BlendModes.Normal);
            return result;
        }

        /// ---------------------------------------------------------------------------------------

        public override string ToString() => "Circle brush";
    }
}