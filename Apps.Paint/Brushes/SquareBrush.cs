using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class SquareBrush : HardBrush
    {
        [Hidden]
        public override string Name => base.Name;

        [Hidden]
        public override WriteableBitmap Preview => Render(Colors.Black, 16);

        /// ---------------------------------------------------------------------------------------

        public override Matrix<byte> GetBytes(int size)
        {
            var result = new Matrix<byte>(size.UInt32(), size.UInt32());
            if (Hardness < 1)
            {
                //Hardness can indicate the lowest opacity in the entire matrix; the opacity would then increase from there to 1

                uint columns = result.Columns, rows = result.Rows;
                uint x = 0, y = 0;

                var total = 1 - Hardness;
                double a = Hardness;

                uint ring = 0;
                int rings = GetRings(columns.Int32(), rows.Int32());
                var increment = total / rings.Double();

                while (columns > 1 && rows > 1)
                {
                    for (y = ring; y < rows; y++)
                    {
                        for (x = ring; x < columns; x++)
                        {
                            result.SetValue(y, x, a.Multiply(255));
                        }
                    }

                    a += increment;

                    ring++;
                    columns -= 1;
                    rows -= 1;
                }
            }
            else
            {
                for (uint x = 0; x < result.Columns; x++)
                {
                    for (uint y = 0; y < result.Rows; y++)
                        result.SetValue(y, x, 1);
                }
            }
            return result;
        }

        /// ---------------------------------------------------------------------------------------

        public override void Draw(WriteableBitmap bitmap, Vector2<int> point, Color color, int size, BlendModes? mode)
        {
            var d = (size / 2.0).Round();
            double x1 = point.X - d, y1 = point.Y - d;
            double x2 = point.X + size - d, y2 = point.Y + size - d;

            if (Hardness < 1)
            {
                var bytes = GetBytes(size);
                bitmap.FillRectangle(x1.Int32(), y1.Int32(), bytes.Transform(i => color.A(i)), mode);
                return;
            }

            //When hardness = 1, use faster method!
            bitmap.FillRectangle(x1.Int32(), y1.Int32(), x2.Int32() - 1, y2.Int32() - 1, color, mode);
        }

        /// ---------------------------------------------------------------------------------------

        public override WriteableBitmap Render(Color color, int size)
        {
            var result = BitmapFactory.WriteableBitmap(size, size);
            Draw(result, new Vector2<int>(size / 2, size / 2), color, size, BlendModes.Normal);
            return result;
        }

        /// ---------------------------------------------------------------------------------------

        public override string ToString() => "Square brush";
    }
}