using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Media.Models;
using Imagin.Common.Math;
using Paint.Adjust;
using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public abstract class VisualLayer : Layer
    {
        [Hidden]
        public bool IsRasterizable => this is RasterizableLayer;

        bool transforming = false;
        [Hidden]
        public bool Transforming
        {
            get => transforming;
            set => this.Change(ref transforming, value);
        }

        protected int x = 0;
        [Hidden]
        public virtual int X
        {
            get => x;
            set => this.Change(ref x, value);
        }

        protected int y = 0;
        [Hidden]
        public virtual int Y
        {
            get => y;
            set => this.Change(ref y, value);
        }

        [Hidden]
        public abstract System.Windows.Media.PointCollection Bounds { get; }

        public abstract WriteableBitmap Pixels { get; set; }

        [Hidden]
        public abstract Int32Region Region { get; }

        [Hidden]
        public System.Windows.Point Position => new System.Windows.Point(x, y);

        Transform transform;
        [Hidden]
        public Transform Transform
        {
            get => transform;
            set => this.Change(ref transform, value);
        }

        /// ---------------------------------------------------------------------------

        public VisualLayer(LayerType type, string name) : base(type, name)
        {
        }

        /// ---------------------------------------------------------------------------

        protected Matrix<Argb> From(WriteableBitmap input)
        {
            if (input != null)
            {
                var result = new Matrix<Argb>(input.PixelHeight.UInt32(), input.PixelWidth.UInt32());
                input.ForEach((x, y, color) =>
                {
                    result.SetValue(y.UInt32(), x.UInt32(), new Argb(color.A, color.R, color.G, color.B));
                    return color;
                });
                return result;
            }
            return null;
        }

        protected WriteableBitmap From(Matrix<Argb> input)
        {
            if (input != null)
            {
                var result = BitmapFactory.WriteableBitmap(input.Rows.Int32(), input.Columns.Int32());
                input.Each((y, x, i) =>
                {
                    result.SetPixel(x, y, System.Windows.Media.Color.FromArgb(i.A, i.R, i.G, i.B));
                    return i;
                });
                return result;
            }
            return null;
        }

        /// ---------------------------------------------------------------------------

        public abstract void Crop(int x, int y, int height, int width);

        public abstract void Crop(Int32Size oldSize, Int32Size newSize, CardinalDirection direction);

        /// ---------------------------------------------------------------------------

        public abstract void Resize(Int32Size oldSize, Int32Size newSize, Interpolations interpolation);

        /// ---------------------------------------------------------------------------

        public Bitmap Render(Size size)
        {
            var result = BitmapFactory.Bitmap(size);
            using (var g = Graphics.FromImage(result))
                Render(g);

            return result;
        }

        public abstract void Render(Graphics g);

        /// ---------------------------------------------------------------------------

        public PixelLayer Merge(VisualLayer layer, Size size)
        {
            var a = Render(size);
            var b = layer.Render(size);

            using (var g = Graphics.FromImage(a))
                g.DrawImage(b, 0, 0);

            return new PixelLayer(Name, a.WriteableBitmap());
        }

        /// ---------------------------------------------------------------------------

        public virtual void Restore() { }

        public virtual void Preserve() { }

        public abstract void Flip(System.Windows.Media.Imaging.WriteableBitmapExtensions.FlipMode flipMode);

        public abstract void Rotate(int degrees);

        public static System.Windows.Media.Color Blend(BlendModes? mode, System.Windows.Media.Color a, System.Windows.Media.Color b)
        {
            double a1 = a.A.Divide(255.0), r1 = a.R.Divide(255.0), g1 = a.G.Divide(255.0), b1 = a.B.Divide(255.0),
                a2 = b.A.Divide(255.0), r2 = b.R.Divide(255.0), g2 = b.G.Divide(255.0), b2 = b.B.Divide(255.0);
            
            double a3 = 0, r3 = 0, g3 = 0, b3 = 0;

            Color<HSB> hsb1 = null, hsb2 = null;
            Color<RGB> rgb = null;

            switch (mode)
            {
                case BlendModes.Average:
                    a3 = (a1 + a2) / 2;
                    r3 = (r1 + r2) / 2;
                    g3 = (g1 + g2) / 2;
                    b3 = (b1 + b2) / 2;
                    break;
                case BlendModes.Color:
                    hsb1 = HSB.From(new Color<RGB>(r1, g1, b1));
                    hsb2 = HSB.From(new Color<RGB>(r2, g2, b2));
                    rgb = HSB.From(new Color<HSB>(hsb2[0], hsb2[1], hsb1[2]));

                    a3 = a1;
                    r3 = rgb[0];
                    g3 = rgb[1];
                    b3 = rgb[2];
                    break;
                case BlendModes.Darken:
                    a3 = a1 < a2 ? a1 : a2;
                    r3 = r1 < r2 ? r1 : r2;
                    g3 = g1 < g2 ? g1 : g2;
                    b3 = b1 < b2 ? b1 : b2;
                    break;
                case BlendModes.Difference:
                    a3 = (a1 - a2).Absolute();
                    r3 = (r1 - r2).Absolute();
                    g3 = (g1 - g2).Absolute();
                    b3 = (b1 - b2).Absolute();
                    break;
                case BlendModes.Reflect:
                    a3 = a1 / (a2 == 0 ? 0.01 : a2);
                    r3 = r1 / (r2 == 0 ? 0.01 : r2);
                    g3 = g1 / (g2 == 0 ? 0.01 : g2);
                    b3 = b1 / (b2 == 0 ? 0.01 : b2);
                    break;
                case BlendModes.Exclusion:
                    a3 = a1 + a2 - (2 * a1 * a2);
                    r3 = r1 + r2 - (2 * r1 * r2);
                    g3 = g1 + g2 - (2 * g1 * g2);
                    b3 = b1 + b2 - (2 * b1 * b2);
                    break;
                case BlendModes.HardLight:
                    a3 = a1 < 0.5 ? a1 * a2 : (a1 <= 1 || a2 <= 1 ? a1 + a2 - (a1 * a2) : Math.Max(a1, a2));
                    r3 = r1 < 0.5 ? r1 * r2 : (r1 <= 1 || r2 <= 1 ? r1 + r2 - (r1 * r2) : Math.Max(r1, r2));
                    g3 = g1 < 0.5 ? g1 * g2 : (g1 <= 1 || g2 <= 1 ? g1 + g2 - (g1 * g2) : Math.Max(g1, g2));
                    b3 = b1 < 0.5 ? b1 * b2 : (b1 <= 1 || b2 <= 1 ? b1 + b2 - (b1 * b2) : Math.Max(b1, b2));
                    break;
                case BlendModes.Hue:
                    hsb1 = HSB.From(new Color<RGB>(r1, g1, b1));
                    hsb2 = HSB.From(new Color<RGB>(r2, g2, b2));
                    rgb = HSB.From(new Color<HSB>(hsb2[0], hsb1[1], hsb1[2]));

                    a3 = a1;
                    r3 = rgb[0];
                    g3 = rgb[1];
                    b3 = rgb[2];
                    break;
                case BlendModes.Lighten:
                    a3 = a1 > a2 ? a1 : a2;
                    r3 = r1 > r2 ? r1 : r2;
                    g3 = g1 > g2 ? g1 : g2;
                    b3 = b1 > b2 ? b1 : b2;
                    break;
                case BlendModes.LinearDodge:
                    a3 = a1 + a2;
                    r3 = r1 + r2;
                    g3 = g1 + g2;
                    b3 = b1 + b2;
                    break;
                case BlendModes.Luminosity:
                    hsb1 = HSB.From(new Color<RGB>(r1, g1, b1));
                    hsb2 = HSB.From(new Color<RGB>(r2, g2, b2));
                    rgb = HSB.From(new Color<HSB>(hsb1[0], hsb1[1], hsb2[2]));

                    a3 = a1;
                    r3 = rgb[0];
                    g3 = rgb[1];
                    b3 = rgb[2];
                    break;
                case BlendModes.Multiply:
                    a3 = a1 * a2;
                    r3 = r1 * r2;
                    g3 = g1 * g2;
                    b3 = b1 * b2;
                    break;
                case BlendModes.Normal:
                    a3 = 1.0 - (1.0 - a2) * (1.0 - a1);
                    r3 = r2 * a2 / a3 + r1 * a1 * (1.0 - a2) / a3;
                    g3 = g2 * a2 / a3 + g1 * a1 * (1.0 - a2) / a3;
                    b3 = b2 * a2 / a3 + b1 * a1 * (1.0 - a2) / a3;
                    a3 = double.IsNaN(a3) ? 0 : a3;
                    r3 = double.IsNaN(r3) ? 0 : r3;
                    g3 = double.IsNaN(g3) ? 0 : g3;
                    b3 = double.IsNaN(b3) ? 0 : b3;
                    break;
                case BlendModes.Saturation:
                    hsb1 = HSB.From(new Color<RGB>(r1, g1, b1));
                    hsb2 = HSB.From(new Color<RGB>(r2, g2, b2));
                    rgb = HSB.From(new Color<HSB>(hsb1[0], hsb2[1], hsb1[2]));

                    a3 = a1;
                    r3 = rgb[0];
                    g3 = rgb[1];
                    b3 = rgb[2];
                    break;
                case BlendModes.Screen:
                    a3 = a1 <= 1 || a2 <= 1 ? a1 + a2 - (a1 * a2) : Math.Max(a1, a2);
                    r3 = r1 <= 1 || r2 <= 1 ? r1 + r2 - (r1 * r2) : Math.Max(r1, r2);
                    g3 = g1 <= 1 || g2 <= 1 ? g1 + g2 - (g1 * g2) : Math.Max(g1, g2);
                    b3 = b1 <= 1 || b2 <= 1 ? b1 + b2 - (b1 * b2) : Math.Max(b1, b2);
                    break;
                case BlendModes.Phoenix:
                    a3 = a1 - a2;
                    r3 = r1 - r2;
                    g3 = g1 - g2;
                    b3 = b1 - b2;
                    break;
            }
            return System.Windows.Media.Color.FromArgb(a3.Multiply(255), r3.Multiply(255), g3.Multiply(255), b3.Multiply(255));
        }
    }
}