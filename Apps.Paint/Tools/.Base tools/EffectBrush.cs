using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Media.Models;
using Imagin.Common.Math;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public abstract class EffectBrushTool : BrushTool
    {
        BaseBrush cachedBrush = null;

        WriteableBitmap cachedImage = null;

        Matrix<byte> cachedMatrix = null;

        protected override void Draw(Vector2<int> point, Color color)
        {
            if (cachedImage != null)
            {
                Brush = Brush ?? new CircleBrush();

                if ((cachedBrush == Brush && cachedMatrix == null) || cachedBrush != Brush)
                    cachedMatrix = Brush.GetBytes(Brush.Size);

                cachedBrush = Brush;

                uint x2 = 0;
                for (var x = point.X; x < point.X + cachedMatrix.Columns; x++, x2++)
                {
                    uint y2 = 0;
                    for (var y = point.Y; y < point.Y + cachedMatrix.Rows; y++, y2++)
                    {
                        if (x >= 0 && x < Bitmap.PixelWidth && y >= 0 && y < Bitmap.PixelHeight)
                        {
                            if (x2 < cachedMatrix.Columns && y2 < cachedMatrix.Rows)
                            {
                                var factor = cachedMatrix.GetValue(y2, x2).Divide(255.0);
                                if (factor > 0)
                                {
                                    var oldColor = cachedImage.GetPixel(x, y);
                                    var newColor = Apply(x, y, oldColor, factor);
                                    Bitmap.SetPixel(x, y, newColor);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected abstract Color Apply(int x, int y, Color color, double factor);

        public override bool OnMouseDown(Point point)
        {
            if (base.OnMouseDown(point))
            {
                cachedImage = Bitmap.Clone();
                Draw(new Vector2<int>(point.X.Int32(), point.Y.Int32()), default(Color));
                return true;
            }
            return false;
        }
    }

    /// ----------------------------------------------------------------------------------------

    [Serializable]
    public abstract class ExposureBrushTool : EffectBrushTool
    {
        double exposure = 0.5;
        public double Exposure
        {
            get => exposure;
            set => this.Change(ref exposure, value);
        }

        ColorRanges range = ColorRanges.Midtones;
        public ColorRanges Range
        {
            get => range;
            set => this.Change(ref range, value);
        }
    }

    /// ----------------------------------------------------------------------------------------

    [Serializable]
    public class BurnTool : ExposureBrushTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Burn.png").OriginalString;

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            var newExposure = Exposure * factor;
            double r = color.R.Divide(255.0), g = color.G.Divide(255.0), b = color.B.Divide(255.0);
            switch (Range)
            {
                case ColorRanges.Highlights:
                    r *= 1 + (-newExposure / 3);
                    g *= 1 + (-newExposure / 3);
                    b *= 1 + (-newExposure / 3);
                    break;
                case ColorRanges.Midtones:
                    r = Math.Pow(r, 1 + (newExposure / 3));
                    g = Math.Pow(g, 1 + (newExposure / 3));
                    b = Math.Pow(b, 1 + (newExposure / 3));
                    break;
                case ColorRanges.Shadows:
                    r = (r - (newExposure / 3)) / (1 - (newExposure / 3));
                    g = (g - (newExposure / 3)) / (1 - (newExposure / 3));
                    b = (b - (newExposure / 3)) / (1 - (newExposure / 3));
                    break;
            }
            return Color.FromArgb(color.A, r.Multiply(255), g.Multiply(255), b.Multiply(255));
        }

        public override string ToString() => "Burn";
    }

    [Serializable]
    public class DodgeTool : ExposureBrushTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Dodge.png").OriginalString;

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            var newExposure = Exposure * factor;
            double r = color.R.Divide(255.0), g = color.G.Divide(255.0), b = color.B.Divide(255.0);
            switch (Range)
            {
                case ColorRanges.Highlights:
                    r *= 1 + (newExposure / 3);
                    g *= 1 + (newExposure / 3);
                    b *= 1 + (newExposure / 3);
                    break;
                case ColorRanges.Midtones:
                    r = Math.Pow(r, 1 / (1 + newExposure));
                    g = Math.Pow(g, 1 / (1 + newExposure));
                    b = Math.Pow(b, 1 / (1 + newExposure));
                    break;
                case ColorRanges.Shadows:
                    r = (newExposure / 3) + r - (newExposure / 3) * r;
                    g = (newExposure / 3) + g - (newExposure / 3) * g;
                    b = (newExposure / 3) + b - (newExposure / 3) * b;
                    break;
            }
            return Color.FromArgb(color.A, r.Multiply(255), g.Multiply(255), b.Multiply(255));
        }

        public override string ToString() => "Dodge";
    }

    /// ----------------------------------------------------------------------------------------

    [Serializable]
    public class SpongeTool : EffectBrushTool
    {
        double flow = 1;
        public double Flow
        {
            get => flow;
            set => this.Change(ref flow, value);
        }

        double intensity = 0.1;
        public double Intensity
        {
            get => intensity;
            set => this.Change(ref intensity, value);
        }

        SpongeModes spongeMode = SpongeModes.Desaturate;
        public SpongeModes SpongeMode
        {
            get => spongeMode;
            set => this.Change(ref spongeMode, value);
        }

        public override string Icon => Resources.Uri(nameof(Paint), "Images/Sponge.png").OriginalString;

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            var hsl = HSL.From(new Color<RGB>(color.R.Divide(255.0), color.G.Divide(255.0), color.B.Divide(255.0)));

            var increment = Intensity * Flow * factor;
            increment *= SpongeMode == SpongeModes.Desaturate ? -1 : 1;

            var rgb = HSL.From(new Color<HSL>(hsl[0], (hsl[1] + increment).Coerce(1), hsl[2]));
            return Color.FromArgb(color.A, rgb[0].Multiply(255), rgb[1].Multiply(255), rgb[2].Multiply(255));
        }

        public override string ToString() => "Sponge";
    }

    /// ----------------------------------------------------------------------------------------

    [Serializable]
    public class ColorReplacementTool : EffectBrushTool
    {
        Color color1;
        public Color Color1
        {
            get => color1;
            set => this.Change(ref color1, value);
        }

        Color color2;
        public Color Color2
        {
            get => color2;
            set => this.Change(ref color2, value);
        }

        public override string Icon => Resources.Uri(nameof(Paint), "Images/ColorReplacement.png").OriginalString;

        public ColorReplacementTool() : base() { }

        protected override Color Apply(int x, int y, Color color, double factor) => color == Color1 ? Color2.A(factor.Multiply(255)) : color;

        public override string ToString() => "Color replacement";
    }

    [Serializable]
    public class ColorSwapTool : EffectBrushTool
    {
        ComponentSwap type;
        public ComponentSwap Type
        {
            get => type;
            set => this.Change(ref type, value);
        }

        public override string Icon => Resources.Uri(nameof(Paint), "Images/ColorSwap.png").OriginalString;

        public ColorSwapTool() : base() { }

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            switch (Type)
            {
                case ComponentSwap.BGR:
                    return Color.FromArgb(color.A, color.B, color.G, color.R);
                case ComponentSwap.BRG:
                    return Color.FromArgb(color.A, color.B, color.R, color.G);
                case ComponentSwap.GBR:
                    return Color.FromArgb(color.A, color.G, color.B, color.R);
                case ComponentSwap.GRB:
                    return Color.FromArgb(color.A, color.G, color.R, color.B);
                case ComponentSwap.RBG:
                    return Color.FromArgb(color.A, color.R, color.B, color.G);
            }
            return color;
        }

        public override string ToString() => "Color swap";
    }

    /// ----------------------------------------------------------------------------------------

    [Serializable]
    public abstract class SurroundingBrushTool : EffectBrushTool
    {
        double strength = 0.5;
        public double Strength
        {
            get => strength;
            set => this.Change(ref strength, value);
        }
    }

    /// ----------------------------------------------------------------------------------------

    [Serializable]
    public class BlurTool : SurroundingBrushTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Blur.png").OriginalString;

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            return color;
        }

        public override string ToString() => "Blur";
    }

    [Serializable]
    public class SharpenTool : SurroundingBrushTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Sharpen.png").OriginalString;

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            //Do algorithm work
            double M = 0, N = 0;

            var m0 = color - Bitmap.GetPixel(x - 1, y);
            var m1 = color - Bitmap.GetPixel(x + 1, y);
            var m2 = color - Bitmap.GetPixel(x, y - 1);
            var m3 = color - Bitmap.GetPixel(x, y + 1);

            //M = Math.Max(m0.Absolute(), m1.Absolute(), m2.Absolute(), m3.Absolute());
            N = M * Strength;

            var r = (color.R + N).Byte();
            var g = (color.R + N).Byte();
            var b = (color.R + N).Byte();

            return Color.FromArgb(color.A, r, g, b);
        }

        public override string ToString() => "Sharpen";
    }

    [Serializable]
    public class SmudgeTool : SurroundingBrushTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Smudge.png").OriginalString;

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            return color;
        }

        public override string ToString() => "Smudge";
    }
}