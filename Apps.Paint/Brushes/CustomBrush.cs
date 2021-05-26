using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Math;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class CustomBrush : BaseBrush
    {
        [Hidden]
        public override WriteableBitmap Preview => Render(Colors.Black, 16);

        [Hidden]
        public WriteableBitmap CurrentPixels => Render(Current, Colors.Black);

        Matrix<byte> current = null;
        [Hidden]
        public Matrix<byte> Current
        {
            get => current;
            set
            {
                this.Change(ref current, value);
                this.Changed(() => CurrentPixels);
            }
        }

        Matrix<byte> original = null;
        [Hidden]
        public Matrix<byte> Original
        {
            get => original;
            set
            {
                this.Change(ref original, value);
                this.Changed(() => Preview);
            }
        }

        CustomBrush(string name) : base(name) { }

        static Matrix<byte> GetBytes(WriteableBitmap bitmap)
        {
            var result = new Matrix<byte>(bitmap.PixelHeight.UInt32(), bitmap.PixelWidth.UInt32());
            for (var x = 0; x < result.Columns; x++)
            {
                for (var y = 0; y < result.Rows; y++)
                    result.SetValue(y.UInt32(), x.UInt32(), bitmap.GetPixel(x, y).A);
            }
            return result;
        }

        public override Matrix<byte> GetBytes(int size) => Current;

        public override void Draw(WriteableBitmap bitmap, Vector2<int> point, Color color, int size, BlendModes? mode)
        {
            var colors = new Matrix<Color>(Current.Rows, Current.Columns);
            Current.Each((y, x, i) =>
            {
                colors.SetValue(y.UInt32(), x.UInt32(), color.A(i));
                return i;
            });

            var x1 = point.X - (Current.Columns.Double() / 2.0).Int32();
            var y1 = point.Y - (Current.Rows.Double() / 2.0).Int32();

            bitmap.FillRectangle(x1, y1, colors, mode);
        }

        public override WriteableBitmap Render(Color color, int size)
        {
            var oldBitmap = Render(Original, color);
            var oldBitmapResized = oldBitmap.Resize(size, size, Interpolations.Bilinear);

            var newBitmap = GetBytes(oldBitmapResized);
            return Render(newBitmap, color);
        }

        async public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Size):

                    await App.Current.Dispatcher.BeginInvoke(() =>
                    {
                        var current = Render(Original, Colors.Black).Resize(Size, Size, Interpolations.Bilinear);
                        Current = GetBytes(current);
                    });
                    break;
            }
        }

        async public static Task<CustomBrush> New(string filePath)
        {
            var bitmap = await Document.Open(filePath);

            var result = new CustomBrush(System.IO.Path.GetFileNameWithoutExtension(filePath));
            result.Original = GetBytes(bitmap);
            result.Size = Math.Max(bitmap.PixelHeight, bitmap.PixelWidth);
            return result;
        }

        public override string ToString() => Name;

        public static WriteableBitmap Render(Matrix<byte> input, Color color)
        {
            var result = BitmapFactory.WriteableBitmap(input.Rows.Int32(), input.Columns.Int32());
            for (uint x = 0; x < input.Columns; x++)
            {
                for (uint y = 0; y < input.Rows; y++)
                    result.SetPixel(x.Int32(), y.Int32(), color.A(input[y, x]));
            }
            return result;
        }
    }
}