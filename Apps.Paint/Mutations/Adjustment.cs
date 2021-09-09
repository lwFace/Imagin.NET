using Imagin.Common;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Media.Models;
using System;
using System.Windows.Media;

namespace Paint
{
    #region Base

    [Serializable]
    public abstract class Adjustment : Mutation, IChange
    {
        [field: NonSerialized]
        public event ChangedEventHandler Changed;

        public virtual void OnChanged()
        {
            Changed?.Invoke(this);
        }

        public Adjustment() : base() { }

        public Adjustment(string name) : base(name) { }

        public abstract Adjustment Clone();

        /*public static Bitmap ColorDepth(this Bitmap input, int depth)
        {
            try
            {
                if (input != null)
                {
                    AForge.Imaging.ColorReduction.ColorImageQuantizer Quantizer = new AForge.Imaging.ColorReduction.ColorImageQuantizer(new AForge.Imaging.ColorReduction.MedianCutQuantizer());
                    input = Quantizer.ReduceColors((Bitmap)input.Clone(), depth);
                    return input;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        */

        /*public static Bitmap Memify(this Bitmap input, Meme type, string[] text, Font font = null)
        {
            try
            {
                float FontSize;

                Graphics Graphics;

                System.Drawing.Drawing2D.GraphicsPath GraphicsPath = new System.Drawing.Drawing2D.GraphicsPath();

                StringFormat StringFormat = new StringFormat();

                //Used for outline
                System.Drawing.Pen Pen;

                //Used to draw and auto-wrap the text
                Rectangle Rectangle = new Rectangle(0, 0, input.Width, input.Height);

                switch (type)
                {
                    case Meme.Impact:
                        if (text.Length != 2) return input;

                        //These affect lines such as those in paths 
                        Graphics = Graphics.FromImage(input);
                        Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                        font = new Font("Impact", 60, System.Drawing.FontStyle.Bold, GraphicsUnit.Pixel);
                        FontSize = font.Size;

                        StringFormat.Alignment = StringAlignment.Center;
                        StringFormat.LineAlignment = StringAlignment.Near;

                        Pen = new System.Drawing.Pen(ColorTranslator.FromHtml("#000000"), 8); //Pen for outline
                        Pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round; //Prevent "spikes" at the path

                        GraphicsPath.AddString(text[0], font.FontFamily, (int)font.Style, FontSize, Rectangle, StringFormat);
                        StringFormat.LineAlignment = StringAlignment.Far;
                        GraphicsPath.AddString(text[1], font.FontFamily, (int)font.Style, FontSize, Rectangle, StringFormat);

                        Graphics.DrawPath(Pen, GraphicsPath);
                        Graphics.FillPath(System.Drawing.Brushes.White, GraphicsPath);
                        Graphics.Dispose();
                        break;
                    case Meme.Demotivational:
                        Bitmap OriginalBitmap = (Bitmap)input.Clone();
                        Bitmap ResizedBitmap = Imagin.Imaging.Transform.Resize(input, 70, input.HorizontalResolution, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic);

                        int x = (input.Width / 2) - (ResizedBitmap.Width / 2);
                        int y = (input.Height / 2) - (ResizedBitmap.Height / 2);
                        int Add = y;

                        Graphics = Graphics.FromImage(input);
                        Graphics.FillRectangle(System.Drawing.Brushes.Black, Rectangle);
                        Bitmap MasterBitmap = new Bitmap(OriginalBitmap.Width, OriginalBitmap.Height, Graphics);
                        Graphics.Dispose();

                        //MasterBitmap = Image.Resize(MasterBitmap, System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic, ResizeMode.Fixed, MasterBitmap.HorizontalResolution, 0, new System.Drawing.Size(OriginalBitmap.Width, OriginalBitmap.Height + Add), new Imagin.Imaging.Color.DynamicColor(0, 0, 0));

                        Graphics = Graphics.FromImage(MasterBitmap);
                        //These affect lines such as those in paths 
                        Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                        font = new Font("Arial", 60, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);

                        Graphics.FillRectangle(System.Drawing.Brushes.Black, Rectangle);

                        Graphics.DrawImage(ResizedBitmap, new System.Drawing.Point(x, y));
                        Graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Brushes.White, 1), new Rectangle(x - 2, y - 2, ResizedBitmap.Width + 4, ResizedBitmap.Height + 4));
                        StringFormat.LineAlignment = StringAlignment.Center;
                        StringFormat.Alignment = StringAlignment.Center;

                        Rectangle TempRect = new Rectangle() { X = 0, Y = input.Height - Add, Width = input.Width, Height = Add };

                        Font NewFont = Imagin.Imaging.Drawing.GetAdjustedFont(Graphics, text[0], font, TempRect.Width, Rectangle.Height, Rectangle.Height / 10, true);
                        NewFont = new Font(font.FontFamily, 50 * NewFont.Size / 100, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);
                        Graphics.DrawString(text[0], NewFont, System.Drawing.Brushes.White, TempRect, StringFormat);
                        Graphics.Dispose();

                        return MasterBitmap;
                }

                GraphicsPath.Dispose();
                font.Dispose();
                StringFormat.Dispose();

                return input;
            }
            catch
            {
                return null;
            }
        }
        */

        /*public static Bitmap Memify(string heading, List<Bitmap> input, List<string> labels)
        {
            return null;
        }
        */
    }

    #endregion

    #region Color

    [Serializable]
    public abstract class ColorAdjustment : Adjustment
    {
        int red;
        [Range(-100, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public virtual int Red
        {
            get => red;
            set
            {
                this.Change(ref red, value);
                OnChanged();
            }
        }

        int green;
        [Range(-100, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public virtual int Green
        {
            get => green;
            set
            {
                this.Change(ref green, value);
                OnChanged();
            }
        }

        int blue;
        [Range(-100, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public virtual int Blue
        {
            get => blue;
            set
            {
                this.Change(ref blue, value);
                OnChanged();
            }
        }

        public ColorAdjustment() : this(0, 0, 0) { }

        public ColorAdjustment(int red, int green, int blue) : base()
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }

    #endregion

    #region Color model

    [Serializable]
    public abstract class ColorModelAdjustment : Adjustment
    {
        public ColorModelAdjustment(string name) : base(name) { }
    }

    #endregion

    //---------------------------------------------------------------------

    #region AlphaReplace

    [Serializable]
    public class AlphaReplaceAdjustment : Adjustment
    {
        public AlphaReplaceAdjustment() : base("Alpha replace") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new AlphaReplaceAdjustment();
        }
    }

    #endregion

    #region DirectionalBlur

    [Serializable]
    public class DirectionalBlurAdjustment : Adjustment
    {
        public DirectionalBlurAdjustment() : base("Directional blur") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new DirectionalBlurAdjustment();
        }
    }

    #endregion

    #region Emboss

    [Serializable]
    public class EmbossAdjustment : Adjustment
    {
        public EmbossAdjustment() : base("Emboss") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new EmbossAdjustment();
        }
    }

    #endregion

    #region GlassTiles

    [Serializable]
    public class GlassTilesAdjustment : Adjustment
    {
        public GlassTilesAdjustment() : base("Glass tiles") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new GlassTilesAdjustment();
        }
    }

    #endregion

    #region Gloom

    [Serializable]
    public class GloomAdjustment : Adjustment
    {
        public GloomAdjustment() : base("Gloom") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new GloomAdjustment();
        }
    }

    #endregion

    #region ParametricEdgeDetection

    [Serializable]
    public class ParametricEdgeDetectionAdjustment : Adjustment
    {
        public ParametricEdgeDetectionAdjustment() : base("Parametric edge detection") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new ParametricEdgeDetectionAdjustment();
        }
    }

    #endregion

    #region Pinch

    [Serializable]
    public class PinchAdjustment : Adjustment
    {
        public PinchAdjustment() : base("Pinch") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new PinchAdjustment();
        }
    }

    #endregion

    #region Pixelate

    [Serializable]
    public class PixelateAdjustment : Adjustment
    {
        public PixelateAdjustment() : base("Pixelate") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new PixelateAdjustment();
        }
    }

    #endregion

    #region Ripple

    [Serializable]
    public class RippleAdjustment : Adjustment
    {
        public RippleAdjustment() : base("Adjustment") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new RippleAdjustment();
        }
    }

    #endregion

    #region Sepia

    [Serializable]
    public class SepiaAdjustment : Adjustment
    {
        public SepiaAdjustment() : base("Sepia") { }

        public override Color Apply(Color color)
        {
            double r = color.R, g = color.G, b = color.B;
            var nr = ((r * 0.393) + (g * 0.769) + (b * 0.189)).Coerce(255).Byte();
            var ng = ((r * 0.349) + (g * 0.686) + (b * 0.168)).Coerce(255).Byte();
            var nb = ((r * 0.272) + (g * 0.534) + (b * 0.131)).Coerce(255).Byte();
            return Color.FromArgb(color.A, nr, ng, nb);
        }

        public override Adjustment Clone()
        {
            return new SepiaAdjustment();
        }
    }

    #endregion

    #region Sharpen

    [Serializable]
    public class SharpenAdjustment : Adjustment
    {
        public SharpenAdjustment() : base("Sharpen") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new SharpenAdjustment();
        }
    }

    #endregion

    #region SketchGranite

    [Serializable]
    public class SketchGraniteAdjustment : Adjustment
    {
        public SketchGraniteAdjustment() : base("Sketch (granite)") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new SketchGraniteAdjustment();
        }
    }

    #endregion

    #region SketchPencil

    [Serializable]
    public class SketchPencilAdjustment : Adjustment
    {
        public SketchPencilAdjustment() : base("Sketch (pencil)") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new SketchPencilAdjustment();
        }
    }

    #endregion

    #region SmoothMagnify

    [Serializable]
    public class SmoothMagnifyAdjustment : Adjustment
    {
        public SmoothMagnifyAdjustment() : base("Smooth magnify") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new SmoothMagnifyAdjustment();
        }
    }

    #endregion

    #region Swirl

    [Serializable]
    public class SwirlAdjustment : Adjustment
    {
        public SwirlAdjustment() : base("Swirl") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new SwirlAdjustment();
        }
    }

    #endregion

    #region Tone

    [Serializable]
    public class ToneAdjustment : Adjustment
    {
        public ToneAdjustment() : base("Tone") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new ToneAdjustment();
        }
    }

    #endregion

    #region WaveWarper

    [Serializable]
    public class WaveWarperAdjustment : Adjustment
    {
        public WaveWarperAdjustment() : base("Wave warper") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new WaveWarperAdjustment();
        }
    }

    #endregion

    #region ZoomBlur

    [Serializable]
    public class ZoomBlurAdjustment : Adjustment
    {
        public ZoomBlurAdjustment() : base("Zoom blur") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new ZoomBlurAdjustment();
        }
    }

    #endregion

    //---------------------------------------------------------------------

    #region Balance (needs attention!)

    [Serializable]
    public class BalanceAdjustment : ColorAdjustment
    {
        ColorRanges range = ColorRanges.Midtones;
        [Featured]
        public ColorRanges Range
        {
            get => range;
            set
            {
                this.Change(ref range, value);
                OnChanged();
            }
        }
        
        public BalanceAdjustment() : this(0, 0, 0) { }

        public BalanceAdjustment(int red, int green, int blue) : base(red, green, blue) => Name = "Balance";

        public override Color Apply(Color color)
        {
            int b = color.B, g = color.G, r = color.R;

            var d = color.Int32();
            var l = d.GetBrightness();

            Color result(int r0, int g0, int b0) => Color.FromArgb(color.A, (r + r0).Coerce(255).Byte(), (g + g0).Coerce(255).Byte(), (b + b0).Coerce(255).Byte());

            //Highlight
            if (range == ColorRanges.Highlights && l > 0.66)
                return result(Red, Green, Blue);

            //Midtone
            else if (range == ColorRanges.Midtones && l > 0.33)
                return result(Red, Green, Blue);

            //Shadow
            else if (range == ColorRanges.Shadows && l <= 0.33)
                return result(Red, Green, Blue);

            return result((Red.Double() * l).Round().Int32(), (Green.Double() * l).Round().Int32(), (Blue.Double() * l).Round().Int32());
        }

        public override Adjustment Clone()
        {
            return new BalanceAdjustment()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
    }

    #endregion

    #region Black/white (requires smoothing; needs additional attention!)

    [Serializable]
    public class BlackWhiteAdjustment : Adjustment
    {
        int reds = 0;
        [Range(-200, 300, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Reds
        {
            get => reds;
            set
            {
                this.Change(ref reds, value);
                OnChanged();
            }
        }

        int greens = 0;
        [Range(-200, 300, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Greens
        {
            get => greens;
            set
            {
                this.Change(ref greens, value);
                OnChanged();
            }
        }

        int blues = 0;
        [Range(-200, 300, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Blues
        {
            get => blues;
            set
            {
                this.Change(ref blues, value);
                OnChanged();
            }
        }

        int yellows = 0;
        [Range(-200, 300, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Yellows
        {
            get => yellows;
            set
            {
                this.Change(ref yellows, value);
                OnChanged();
            }
        }

        int cyans = 0;
        [Range(-200, 300, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Cyans
        {
            get => cyans;
            set
            {
                this.Change(ref cyans, value);
                OnChanged();
            }
        }

        int magentas = 0;
        [Range(-200, 300, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Magentas
        {
            get => magentas;
            set
            {
                this.Change(ref magentas, value);
                OnChanged();
            }
        }

        public BlackWhiteAdjustment() : base() => Name = "Black/white";

        public override Color Apply(Color color)
        {
            var c = color.Int32();
            var hue
                = c.GetHue().Double();
            var saturation
                = c.GetSaturation().Double();
            var brightness
                = c.GetBrightness().Double();

            var r = c.R;
            var g = c.G;
            var b = c.B;

            //Figure out which component is most dominant and increase or decrease appropriately
            var oldBrightness = brightness;

            //Yellow
            if (r > b && g > b)
            {
                brightness += (oldBrightness * yellows.Double().Shift(-2));
            }
            //Magenta
            else if (r > g && b > g)
            {
                brightness += (oldBrightness * magentas.Double().Shift(-2));
            }
            //Cyan
            else if (g > r && b > r)
            {
                brightness += (oldBrightness * cyans.Double().Shift(-2));
            }

            //Red
            if (c.R > c.G && c.R > c.B)
            {
                brightness += (oldBrightness * reds.Double().Shift(-2));
            }
            //Green
            else if (c.G > c.R && c.G > c.B)
            {
                brightness += (oldBrightness * greens.Double().Shift(-2));
            }
            //Blue
            else if (c.B > c.R && c.B > c.G)
            {
                brightness += (oldBrightness * blues.Double().Shift(-2));
            }
            return color;
            /*
            var hsb = new Color<HSB>(converter.Profile, hue, saturation, brightness.Coerce(1));
            var rgb = RGB.Convert(hsb);
            return Color.FromArgb(color.A, rgb[0].Shift(2).Multiply(255), rgb[1].Shift(2).Multiply(255), rgb[2].Shift(2).Multiply(255));
            */
        }

        public override Adjustment Clone()
        {
            return new BlackWhiteAdjustment();
        }
    }

    #endregion

    #region Blend

    [Serializable]
    public class BlendAdjustment : Adjustment
    {
        BlendModes blend = BlendModes.Normal;
        public BlendModes Blend
        {
            get => blend;
            set
            {
                this.Change(ref blend, value);
                OnChanged();
            }
        }

        StringColor color = new StringColor(Colors.White);
        public Color Color
        {
            get => color;
            set
            {
                this.Change(ref color, value);
                OnChanged();
            }
        }

        public BlendAdjustment() : base("Blend") { }

        public override Color Apply(Color color) => VisualLayer.Blend(blend, color, this.color);

        public override Adjustment Clone()
        {
            return new BlendAdjustment()
            {
                Blend = blend,
                Color = color
            };
        }
    }

    #endregion

    #region Blur

    /*
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Drawing.Drawing2D;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.IO;
    using System.Drawing.Imaging;

    namespace ImageBlurFilter
    {
        public static class ExtBitmap
        {
            public static Bitmap CopyToSquareCanvas(this Bitmap sourceBitmap, int canvasWidthLenght)
            {
                float ratio = 1.0f;
                int maxSide = sourceBitmap.Width > sourceBitmap.Height ?
                              sourceBitmap.Width : sourceBitmap.Height;

                ratio = (float)maxSide / (float)canvasWidthLenght;

                Bitmap bitmapResult = (sourceBitmap.Width > sourceBitmap.Height ?
                                        new Bitmap(canvasWidthLenght, (int)(sourceBitmap.Height / ratio))
                                        : new Bitmap((int)(sourceBitmap.Width / ratio), canvasWidthLenght));

                using (Graphics graphicsResult = Graphics.FromImage(bitmapResult))
                {
                    graphicsResult.CompositingQuality = CompositingQuality.HighQuality;
                    graphicsResult.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphicsResult.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    graphicsResult.DrawImage(sourceBitmap,
                                            new Rectangle(0, 0,
                                                bitmapResult.Width, bitmapResult.Height),
                                            new Rectangle(0, 0,
                                                sourceBitmap.Width, sourceBitmap.Height),
                                                GraphicsUnit.Pixel);
                    graphicsResult.Flush();
                }

                return bitmapResult;
            }

            public static Bitmap ImageBlurFilter(this Bitmap sourceBitmap, 
                                                        BlurType blurType)
            {
                Bitmap resultBitmap = null;
    
                switch (blurType)
                {
                    case BlurType.Mean3x3:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                                           Matrix.Mean3x3, 1.0 / 9.0, 0);
                        }break;
                    case BlurType.Mean5x5:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                                           Matrix.Mean5x5, 1.0 / 25.0, 0);
                        }break;
                    case BlurType.Mean7x7:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                                           Matrix.Mean7x7, 1.0 / 49.0, 0);
                        }break;
                    case BlurType.Mean9x9:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                                           Matrix.Mean9x9, 1.0 / 81.0, 0);
                        }break;
                    case BlurType.GaussianBlur3x3:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                                    Matrix.GaussianBlur3x3, 1.0 / 16.0, 0);
                        }break;
                    case BlurType.GaussianBlur5x5:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                                   Matrix.GaussianBlur5x5, 1.0 / 159.0, 0);
                        }break;
                    case BlurType.MotionBlur5x5:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                                       Matrix.MotionBlur5x5, 1.0 / 10.0, 0);
                        }break;
                    case BlurType.MotionBlur5x5At45Degrees:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.MotionBlur5x5At45Degrees, 1.0 / 5.0, 0);
                        }break;
                    case BlurType.MotionBlur5x5At135Degrees:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.MotionBlur5x5At135Degrees, 1.0 / 5.0, 0);
                        }break;
                    case BlurType.MotionBlur7x7:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.MotionBlur7x7, 1.0 / 14.0, 0);
                        }break;
                    case BlurType.MotionBlur7x7At45Degrees:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.MotionBlur7x7At45Degrees, 1.0 / 7.0, 0);
                        }break;
                    case BlurType.MotionBlur7x7At135Degrees:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.MotionBlur7x7At135Degrees, 1.0 / 7.0, 0);
                        }break;
                    case BlurType.MotionBlur9x9:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.MotionBlur9x9, 1.0 / 18.0, 0);
                        }break;
                    case BlurType.MotionBlur9x9At45Degrees:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.MotionBlur9x9At45Degrees, 1.0 / 9.0, 0);
                        }break;
                    case BlurType.MotionBlur9x9At135Degrees:
                        {
                            resultBitmap = sourceBitmap.ConvolutionFilter(
                            Matrix.MotionBlur9x9At135Degrees, 1.0 / 9.0, 0);
                        }break;
                    case BlurType.Median3x3:
                        {
                            resultBitmap = sourceBitmap.MedianFilter(3);
                        }break;
                    case BlurType.Median5x5:
                        {
                            resultBitmap = sourceBitmap.MedianFilter(5);
                        }break;
                    case BlurType.Median7x7:
                        {
                            resultBitmap = sourceBitmap.MedianFilter(7);
                        }break;
                    case BlurType.Median9x9:
                        {
                            resultBitmap = sourceBitmap.MedianFilter(9);
                        }break;
                    case BlurType.Median11x11:
                        {
                            resultBitmap = sourceBitmap.MedianFilter(11);
                        }break;
                }

                return resultBitmap;
            }

            private static Bitmap ConvolutionFilter(this Bitmap sourceBitmap,
                                                      double[,] filterMatrix,
                                                           double factor = 1,
                                                                int bias = 0)
            {
                BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                         sourceBitmap.Width, sourceBitmap.Height),
                                                           ImageLockMode.ReadOnly,
                                                     PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
                byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
                sourceBitmap.UnlockBits(sourceData);

                double blue = 0.0;
                double green = 0.0;
                double red = 0.0;

                int filterWidth = filterMatrix.GetLength(1);
                int filterHeight = filterMatrix.GetLength(0);

                int filterOffset = (filterWidth - 1) / 2;
                int calcOffset = 0;

                int byteOffset = 0;

                for (int offsetY = filterOffset; offsetY <
                    sourceBitmap.Height - filterOffset; offsetY++)
                {
                    for (int offsetX = filterOffset; offsetX <
                        sourceBitmap.Width - filterOffset; offsetX++)
                    {
                        blue = 0;
                        green = 0;
                        red = 0;

                        byteOffset = offsetY *
                                     sourceData.Stride +
                                     offsetX * 4;

                        for (int filterY = -filterOffset;
                            filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset;
                                filterX <= filterOffset; filterX++)
                            {
                                calcOffset = byteOffset +
                                             (filterX * 4) +
                                             (filterY * sourceData.Stride);

                                blue += (double)(pixelBuffer[calcOffset]) *
                                        filterMatrix[filterY + filterOffset,
                                                            filterX + filterOffset];

                                green += (double)(pixelBuffer[calcOffset + 1]) *
                                         filterMatrix[filterY + filterOffset,
                                                            filterX + filterOffset];

                                red += (double)(pixelBuffer[calcOffset + 2]) *
                                       filterMatrix[filterY + filterOffset,
                                                          filterX + filterOffset];
                            }
                        }

                        blue = factor * blue + bias;
                        green = factor * green + bias;
                        red = factor * red + bias;

                        blue = (blue > 255 ? 255 :
                               (blue < 0 ? 0 :
                                blue));

                        green = (green > 255 ? 255 :
                                (green < 0 ? 0 :
                                 green));

                        red = (red > 255 ? 255 :
                              (red < 0 ? 0 :
                               red));

                        resultBuffer[byteOffset] = (byte)(blue);
                        resultBuffer[byteOffset + 1] = (byte)(green);
                        resultBuffer[byteOffset + 2] = (byte)(red);
                        resultBuffer[byteOffset + 3] = 255;
                    }
                }

                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

                BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                         resultBitmap.Width, resultBitmap.Height),
                                                          ImageLockMode.WriteOnly,
                                                     PixelFormat.Format32bppArgb);

                Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
                resultBitmap.UnlockBits(resultData);

                return resultBitmap;
            }

            public enum BlurType
            {
                Mean3x3,
                Mean5x5,
                Mean7x7,
                Mean9x9,
                GaussianBlur3x3,
                GaussianBlur5x5,
                MotionBlur5x5,
                MotionBlur5x5At45Degrees,
                MotionBlur5x5At135Degrees,
                MotionBlur7x7,
                MotionBlur7x7At45Degrees,
                MotionBlur7x7At135Degrees,
                MotionBlur9x9,
                MotionBlur9x9At45Degrees,
                MotionBlur9x9At135Degrees,
                Median3x3,
                Median5x5,
                Median7x7,
                Median9x9,
                Median11x11
            }

            public static Bitmap MedianFilter(this Bitmap sourceBitmap,
                                                        int matrixSize)
            {
                BitmapData sourceData =
                           sourceBitmap.LockBits(new Rectangle(0, 0,
                           sourceBitmap.Width, sourceBitmap.Height),
                           ImageLockMode.ReadOnly,
                           PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[sourceData.Stride *
                                              sourceData.Height];

                byte[] resultBuffer = new byte[sourceData.Stride *
                                               sourceData.Height];

                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0,
                                           pixelBuffer.Length);

                sourceBitmap.UnlockBits(sourceData);

                int filterOffset = (matrixSize - 1) / 2;
                int calcOffset = 0;

                int byteOffset = 0;

                List<int> neighbourPixels = new List<int>();
                byte[] middlePixel;

                for (int offsetY = filterOffset; offsetY <
                    sourceBitmap.Height - filterOffset; offsetY++)
                {
                    for (int offsetX = filterOffset; offsetX <
                        sourceBitmap.Width - filterOffset; offsetX++)
                    {
                        byteOffset = offsetY *
                                     sourceData.Stride +
                                     offsetX * 4;

                        neighbourPixels.Clear();

                        for (int filterY = -filterOffset;
                            filterY <= filterOffset; filterY++)
                        {
                            for (int filterX = -filterOffset;
                                filterX <= filterOffset; filterX++)
                            {

                                calcOffset = byteOffset +
                                             (filterX * 4) +
                                             (filterY * sourceData.Stride);

                                neighbourPixels.Add(BitConverter.ToInt32(
                                                 pixelBuffer, calcOffset));
                            }
                        }

                        neighbourPixels.Sort();

                        middlePixel = BitConverter.GetBytes(
                                           neighbourPixels[filterOffset]);

                        resultBuffer[byteOffset] = middlePixel[0];
                        resultBuffer[byteOffset + 1] = middlePixel[1];
                        resultBuffer[byteOffset + 2] = middlePixel[2];
                        resultBuffer[byteOffset + 3] = middlePixel[3];
                    }
                }

                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width,
                                                 sourceBitmap.Height);

                BitmapData resultData =
                           resultBitmap.LockBits(new Rectangle(0, 0,
                           resultBitmap.Width, resultBitmap.Height),
                           ImageLockMode.WriteOnly,
                           PixelFormat.Format32bppArgb);

                Marshal.Copy(resultBuffer, 0, resultData.Scan0,
                                           resultBuffer.Length);

                resultBitmap.UnlockBits(resultData);

                return resultBitmap;
            }
        }  
    }

    namespace ImageBlurFilter
    {
        public static class Matrix
        {
            public static double[,] Mean3x3
            {
                get
                {
                    return new double[,]  
                    { { 1, 1, 1, }, 
                      { 1, 1, 1, }, 
                      { 1, 1, 1, }, };
                }
            }

            public static double[,] Mean5x5
            {
                get
                {
                    return new double[,]  
                    { { 1, 1, 1, 1, 1}, 
                      { 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1}, };
                }
            }

            public static double[,] Mean7x7
            {
                get
                {
                    return new double[,]  
                    { { 1, 1, 1, 1, 1, 1, 1}, 
                      { 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1}, };
                }
            }

            public static double[,] Mean9x9
            {
                get
                {
                    return new double[,]  
                    { { 1, 1, 1, 1, 1, 1, 1, 1, 1}, 
                      { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                      { 1, 1, 1, 1, 1, 1, 1, 1, 1}, };
                }
            }

            public static double[,] GaussianBlur3x3
            {
                get
                {
                    return new double[,]  
                    { { 1, 2, 1, }, 
                      { 2, 4, 2, }, 
                      { 1, 2, 1, }, };
                }
            }

            public static double[,] GaussianBlur5x5
            {
                get
                {
                    return new double[,]  
                    { { 2, 04, 05, 04, 2 }, 
                      { 4, 09, 12, 09, 4 }, 
                      { 5, 12, 15, 12, 5 },
                      { 4, 09, 12, 09, 4 },
                      { 2, 04, 05, 04, 2 }, };
                }
            }

            public static double[,] MotionBlur5x5
            {
                get
                {
                    return new double[,]  
                    { { 1, 0, 0, 0, 1}, 
                      { 0, 1, 0, 1, 0}, 
                      { 0, 0, 1, 0, 0},
                      { 0, 1, 0, 1, 0},
                      { 1, 0, 0, 0, 1}, };
                }
            }

            public static double[,] MotionBlur5x5At45Degrees
            {
                get
                {
                    return new double[,]  
                    { { 0, 0, 0, 0, 1}, 
                      { 0, 0, 0, 1, 0}, 
                      { 0, 0, 1, 0, 0},
                      { 0, 1, 0, 0, 0},
                      { 1, 0, 0, 0, 0}, };
                }
            }

            public static double[,] MotionBlur5x5At135Degrees
            {
                get
                {
                    return new double[,]  
                    { { 1, 0, 0, 0, 0}, 
                      { 0, 1, 0, 0, 0}, 
                      { 0, 0, 1, 0, 0},
                      { 0, 0, 0, 1, 0},
                      { 0, 0, 0, 0, 1}, };
                }
            }

            public static double[,] MotionBlur7x7
            {
                get
                {
                    return new double[,]  
                    { { 1, 0, 0, 0, 0, 0, 1}, 
                      { 0, 1, 0, 0, 0, 1, 0}, 
                      { 0, 0, 1, 0, 1, 0, 0},
                      { 0, 0, 0, 1, 0, 0, 0},
                      { 0, 0, 1, 0, 1, 0, 0},
                      { 0, 1, 0, 0, 0, 1, 0},
                      { 1, 0, 0, 0, 0, 0, 1}, };
                }
            }

            public static double[,] MotionBlur7x7At45Degrees
            {
                get
                {
                    return new double[,]  
                    { { 0, 0, 0, 0, 0, 0, 1}, 
                      { 0, 0, 0, 0, 0, 1, 0}, 
                      { 0, 0, 0, 0, 1, 0, 0},
                      { 0, 0, 0, 1, 0, 0, 0},
                      { 0, 0, 1, 0, 0, 0, 0},
                      { 0, 1, 0, 0, 0, 0, 0},
                      { 1, 0, 0, 0, 0, 0, 0}, };
                }
            }

            public static double[,] MotionBlur7x7At135Degrees
            {
                get
                {
                    return new double[,]  
                    { { 1, 0, 0, 0, 0, 0, 0}, 
                      { 0, 1, 0, 0, 0, 0, 0}, 
                      { 0, 0, 1, 0, 0, 0, 0},
                      { 0, 0, 0, 1, 0, 0, 0},
                      { 0, 0, 0, 0, 1, 0, 0},
                      { 0, 0, 0, 0, 0, 1, 0},
                      { 0, 0, 0, 0, 0, 0, 1}, };
                }
            }

            public static double[,] MotionBlur9x9
            {
                get
                {
                    return new double[,]  
                    { {1, 0, 0, 0, 0, 0, 0, 0, 1,},
                      {0, 1, 0, 0, 0, 0, 0, 1, 0,},
                      {0, 0, 1, 0, 0, 0, 1, 0, 0,},
                      {0, 0, 0, 1, 0, 1, 0, 0, 0,},
                      {0, 0, 0, 0, 1, 0, 0, 0, 0,},
                      {0, 0, 0, 1, 0, 1, 0, 0, 0,},
                      {0, 0, 1, 0, 0, 0, 1, 0, 0,},
                      {0, 1, 0, 0, 0, 0, 0, 1, 0,},
                      {1, 0, 0, 0, 0, 0, 0, 0, 1,}, };
                }
            }

            public static double[,] MotionBlur9x9At45Degrees
            {
                get
                {
                    return new double[,]  
                    { {0, 0, 0, 0, 0, 0, 0, 0, 1,},
                      {0, 0, 0, 0, 0, 0, 0, 1, 0,},
                      {0, 0, 0, 0, 0, 0, 1, 0, 0,},
                      {0, 0, 0, 0, 0, 1, 0, 0, 0,},
                      {0, 0, 0, 0, 1, 0, 0, 0, 0,},
                      {0, 0, 0, 1, 0, 0, 0, 0, 0,},
                      {0, 0, 1, 0, 0, 0, 0, 0, 0,},
                      {0, 1, 0, 0, 0, 0, 0, 0, 0,},
                      {1, 0, 0, 0, 0, 0, 0, 0, 0,}, };
                }
            }

            public static double[,] MotionBlur9x9At135Degrees
            {
                get
                {
                    return new double[,]  
                    { {1, 0, 0, 0, 0, 0, 0, 0, 0,},
                      {0, 1, 0, 0, 0, 0, 0, 0, 0,},
                      {0, 0, 1, 0, 0, 0, 0, 0, 0,},
                      {0, 0, 0, 1, 0, 0, 0, 0, 0,},
                      {0, 0, 0, 0, 1, 0, 0, 0, 0,},
                      {0, 0, 0, 0, 0, 1, 0, 0, 0,},
                      {0, 0, 0, 0, 0, 0, 1, 0, 0,},
                      {0, 0, 0, 0, 0, 0, 0, 1, 0,},
                      {0, 0, 0, 0, 0, 0, 0, 0, 1,}, };
                }
            }
        }
    }

    namespace ImageBlurFilter
    {
        public partial class MainForm : Form
        {
            private Bitmap originalBitmap = null;
            private Bitmap previewBitmap = null;
            private Bitmap resultBitmap = null;
        
            public MainForm()
            {
                InitializeComponent();

                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.GaussianBlur3x3);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.GaussianBlur5x5);

                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.Mean3x3);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.Mean5x5);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.Mean7x7);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.Mean9x9);
            
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.Median3x3);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.Median5x5);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.Median7x7);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.Median9x9);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.Median11x11);

                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.MotionBlur5x5);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.MotionBlur5x5At135Degrees);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.MotionBlur5x5At45Degrees);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.MotionBlur7x7);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.MotionBlur7x7At135Degrees);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.MotionBlur7x7At45Degrees);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.MotionBlur9x9);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.MotionBlur9x9At135Degrees);
                cmbBlurFilter.Items.Add(ExtBitmap.BlurType.MotionBlur9x9At45Degrees);

                cmbBlurFilter.SelectedIndex = 0;
            }

            private void btnOpenOriginal_Click(object sender, EventArgs e)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Select an image file.";
                ofd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
                ofd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StreamReader streamReader = new StreamReader(ofd.FileName);
                    originalBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                    streamReader.Close();

                    previewBitmap = originalBitmap.CopyToSquareCanvas(picPreview.Width);
                    picPreview.Image = previewBitmap;

                    ApplyFilter(true);
                }
            }

            private void btnSaveNewImage_Click(object sender, EventArgs e)
            {
                ApplyFilter(false);

                if (resultBitmap != null)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Specify a file name and file path";
                    sfd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
                    sfd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string fileExtension = Path.GetExtension(sfd.FileName).ToUpper();
                        ImageFormat imgFormat = ImageFormat.Png;

                        if (fileExtension == "BMP")
                        {
                            imgFormat = ImageFormat.Bmp;
                        }
                        else if (fileExtension == "JPG")
                        {
                            imgFormat = ImageFormat.Jpeg;
                        }

                        StreamWriter streamWriter = new StreamWriter(sfd.FileName, false);
                        resultBitmap.Save(streamWriter.BaseStream, imgFormat);
                        streamWriter.Flush();
                        streamWriter.Close();

                        resultBitmap = null;
                    }
                }
            }

            private void ApplyFilter(bool preview)
            {
                if (previewBitmap == null || cmbBlurFilter.SelectedIndex == -1)
                {
                    return;
                }

                Bitmap selectedSource = null;
                Bitmap bitmapResult = null;

                if (preview == true)
                {
                    selectedSource = previewBitmap;
                }
                else
                {
                    selectedSource = originalBitmap;
                }

                if (selectedSource != null)
                {
                    ExtBitmap.BlurType blurType =
                        ((ExtBitmap.BlurType)cmbBlurFilter.SelectedItem);

                    bitmapResult = selectedSource.ImageBlurFilter(blurType);
                }

                if (bitmapResult != null)
                {
                    if (preview == true)
                    {
                        picPreview.Image = bitmapResult;
                    }
                    else
                    {
                        resultBitmap = bitmapResult;
                    }
                }
            }

            private void FilterValueChangedEventHandler(object sender, EventArgs e)
            {
                ApplyFilter(true);
            }
        }
    }
    */

    #endregion

    #region Blur (needs work!)

    [Serializable]
    public class BlurAdjustment : Adjustment
    {
        public BlurAdjustment() : base("Blur") { }

        public override Color Apply(Color color) => color;

        public override Adjustment Clone()
        {
            return new BlurAdjustment();
        }
    }

    #endregion

    #region Brightness/Contrast

    [Serializable]
    public class BrightnessContrastAdjustment : Adjustment
    {
        double contrastFactor;

        int brightness;
        [Range(-255, 255, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Brightness
        {
            get => brightness;
            set
            {
                this.Change(ref brightness, value);
                OnChanged();
            }
        }

        int contrast;
        [Range(-128, 128, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Contrast
        {
            get => contrast;
            set
            {
                contrastFactor = (259.0 * (value + 255.0)) / (255.0 * (259.0 - value));
                this.Change(ref contrast, value);
                OnChanged();
            }
        }

        public BrightnessContrastAdjustment() : this(0, 0) { }

        public BrightnessContrastAdjustment(int brightness, int contrast) : base("Brightness/contrast")
        {
            Brightness = brightness;
            Contrast = contrast;
        }

        public override Color Apply(Color color)
        {
            int nr = color.R, ng = color.G, nb = color.B;
            nr = ((contrastFactor * (nr - 128)) + 128).Coerce(255).Round().Int32();
            ng = ((contrastFactor * (ng - 128)) + 128).Coerce(255).Round().Int32();
            nb = ((contrastFactor * (nb - 128)) + 128).Coerce(255).Round().Int32();

            nr += brightness;
            ng += brightness;
            nb += brightness;
            return Color.FromArgb(color.A, nr.Coerce(255).Byte(), ng.Coerce(255).Byte(), nb.Coerce(255).Byte());
        }

        public override Adjustment Clone()
        {
            return new BrightnessContrastAdjustment()
            {
                Brightness = brightness,
                Contrast = contrast
            };
        }
    }

    #endregion

    #region Cartoon (needs work!)

    [Serializable]
    public class CartoonAdjustment : Adjustment
    {
        /*public static Bitmap CartoonEffect(this Bitmap input, int levels, int filterSize, byte threshold)
        {
            unsafe
            {
                Bitmap paintFilterImage = Adjustment.OilPainting(input, levels, filterSize);
                Bitmap edgeDetectImage = Adjustment.GradientBasedEdgeDetection(input, threshold);

                BitmapData paintData = paintFilterImage.LockBits(new Rectangle(0, 0, paintFilterImage.Width, paintFilterImage.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                byte[] paintPixelBuffer = new byte[paintData.Stride * paintData.Height];
                Marshal.Copy(paintData.Scan0, paintPixelBuffer, 0, paintPixelBuffer.Length);
                paintFilterImage.UnlockBits(paintData);

                BitmapData edgeData = edgeDetectImage.LockBits(new Rectangle(0, 0, edgeDetectImage.Width, edgeDetectImage.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                byte[] edgePixelBuffer = new byte[edgeData.Stride * edgeData.Height];
                Marshal.Copy(edgeData.Scan0, edgePixelBuffer, 0, edgePixelBuffer.Length);
                edgeDetectImage.UnlockBits(edgeData);

                BitmapData bitmapData = input.LockBits(new System.Drawing.Rectangle(0, 0, input.Width, input.Height), ImageLockMode.ReadWrite, input.PixelFormat);
                int BytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(input.PixelFormat) / 8;
                int HeightInPixels = bitmapData.Height;
                int WidthInBytes = bitmapData.Width * BytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;
                Parallel.For(0, HeightInPixels, y =>
                {
                    byte* CurrentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < WidthInBytes; x = x + BytesPerPixel)
                    {
                        int OldBlue = CurrentLine[x];
                        int OldGreen = CurrentLine[x + 1];
                        int OldRed = CurrentLine[x + 2];

                        if (edgePixelBuffer[x] == 255 || edgePixelBuffer[x + 1] == 255 || edgePixelBuffer[x + 2] == 255)
                        {
                            CurrentLine[x] = 0;
                            CurrentLine[x + 1] = 0;
                            CurrentLine[x + 2] = 0;
                        }
                        else
                        {
                            CurrentLine[x] = paintPixelBuffer[x];
                            CurrentLine[x + 1] = paintPixelBuffer[x + 1];
                            CurrentLine[x + 2] = paintPixelBuffer[x + 2];
                        }
                    }
                });
                input.UnlockBits(bitmapData);
            }
            return input;
        }
        */

        /*public static Bitmap CartoonEffect(this Bitmap input, byte threshold = 0, SmoothingFilterType smoothFilter = SmoothingFilterType.None)
        {
            input = input.SmoothingFilter(smoothFilter);

            BitmapData sourceData = input.LockBits(new Rectangle(0, 0, input.Width, input.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            input.UnlockBits(sourceData);

            int byteOffset = 0;
            int blueGradient, greenGradient, redGradient = 0;
            double blue = 0, green = 0, red = 0;
            bool exceedsThreshold = false;

            for (int offsetY = 1; offsetY < input.Height - 1; offsetY++)
            {
                for (int offsetX = 1; offsetX < input.Width - 1; offsetX++)
                {
                    byteOffset = offsetY * sourceData.Stride + offsetX * 4;

                    blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                    blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);

                    byteOffset++;
                    greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                    greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);

                    byteOffset++;
                    redGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);
                    redGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);

                    if (blueGradient + greenGradient + redGradient > threshold)
                    {
                        exceedsThreshold = true;
                    }
                    else
                    {
                        byteOffset -= 2;

                        blueGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);

                        byteOffset++;
                        greenGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);

                        byteOffset++;
                        redGradient = Math.Abs(pixelBuffer[byteOffset - 4] - pixelBuffer[byteOffset + 4]);

                        if (blueGradient + greenGradient + redGradient > threshold)
                        {
                            exceedsThreshold = true;
                        }
                        else
                        {
                            byteOffset -= 2;

                            blueGradient = Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);

                            byteOffset++;
                            greenGradient = Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);

                            byteOffset++;
                            redGradient = Math.Abs(pixelBuffer[byteOffset - sourceData.Stride] - pixelBuffer[byteOffset + sourceData.Stride]);

                            if (blueGradient + greenGradient + redGradient > threshold)
                            {
                                exceedsThreshold = true;
                            }
                            else
                            {
                                byteOffset -= 2;

                                blueGradient =
                                Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - pixelBuffer[byteOffset + 4 + sourceData.Stride]);
                                blueGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - pixelBuffer[byteOffset + sourceData.Stride - 4]);

                                byteOffset++;
                                greenGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - pixelBuffer[byteOffset + 4 + sourceData.Stride]);
                                greenGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - pixelBuffer[byteOffset + sourceData.Stride - 4]);

                                byteOffset++;
                                redGradient = Math.Abs(pixelBuffer[byteOffset - 4 - sourceData.Stride] - pixelBuffer[byteOffset + 4 + sourceData.Stride]);
                                redGradient += Math.Abs(pixelBuffer[byteOffset - sourceData.Stride + 4] - pixelBuffer[byteOffset + sourceData.Stride - 4]);

                                exceedsThreshold = blueGradient + greenGradient + redGradient > threshold ? true : false;
                            }
                        }
                    }

                    byteOffset -= 2;
                    if (exceedsThreshold)
                    {
                        blue = 0;
                        green = 0;
                        red = 0;
                    }
                    else
                    {
                        blue = pixelBuffer[byteOffset];
                        green = pixelBuffer[byteOffset + 1];
                        red = pixelBuffer[byteOffset + 2];
                    }

                    blue = (blue > 255 ? 255 : (blue < 0 ? 0 : blue));
                    green = (green > 255 ? 255 : (green < 0 ? 0 : green));
                    red = (red > 255 ? 255 : (red < 0 ? 0 : red));

                    resultBuffer[byteOffset] = (byte)blue;
                    resultBuffer[byteOffset + 1] = (byte)green;
                    resultBuffer[byteOffset + 2] = (byte)red;
                    resultBuffer[byteOffset + 3] = 255;
                }
            }
            Bitmap resultBitmap = new Bitmap(input.Width, input.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
        */

        public CartoonAdjustment() : base("Cartoon") { }

        public override Color Apply(Color color)
        {
            return color;
        }

        public override Adjustment Clone()
        {
            return new CartoonAdjustment();
        }
    }

    #endregion

    #region Equalize (consider adding an amount property of some type...)

    [Serializable]
    public class EqualizeAdjustment : Adjustment
    {
        public EqualizeAdjustment() : base("Equalize") { }

        public override Color Apply(Color color) => color;

        public override void Apply(ColorMatrix oldColors, ColorMatrix newColors)
        {
            int height = oldColors.Rows.Int32(), width = oldColors.Columns.Int32();

            var rHistogram = oldColors.Histogram(RedGreenBlue.Red);
            var gHistogram = oldColors.Histogram(RedGreenBlue.Green);
            var bHistogram = oldColors.Histogram(RedGreenBlue.Blue);

            var histR = new float[256];
            var histG = new float[256];
            var histB = new float[256];

            histR[0] = (rHistogram[0] * rHistogram.Length) / (width * height).Single();
            histG[0] = (gHistogram[0] * gHistogram.Length) / (width * height).Single();
            histB[0] = (bHistogram[0] * bHistogram.Length) / (width * height).Single();

            long cumulativeR = rHistogram[0];
            long cumulativeG = gHistogram[0];
            long cumulativeB = bHistogram[0];

            for (var i = 1; i < histR.Length; i++)
            {
                cumulativeR += rHistogram[i];
                histR[i] = (cumulativeR * rHistogram.Length).Single() / (width * height).Single();

                cumulativeG += gHistogram[i];
                histG[i] = (cumulativeG * gHistogram.Length).Single() / (width * height).Single();

                cumulativeB += bHistogram[i];
                histB[i] = (cumulativeB * bHistogram.Length).Single() / (width * height).Single();
            }

            oldColors.Each((y, x, oldColor) =>
            {
                var intensityR = oldColor.R;
                var intensityG = oldColor.G;
                var intensityB = oldColor.B;

                var nValueR = (byte)histR[intensityR].Coerce(255);
                var nValueG = (byte)histG[intensityG].Coerce(255);
                var nValueB = (byte)histB[intensityB].Coerce(255);

                newColors.SetValue(y.UInt32(), x.UInt32(), Color.FromArgb(oldColor.A, nValueR, nValueG, nValueB));
                return oldColor;
            });
        }

        public override Adjustment Clone()
        {
            return new EqualizeAdjustment();
        }
    }

    #endregion

    #region Erosion/dilation

    /*
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Drawing.Drawing2D;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.IO;
    using System.Drawing.Imaging;

    namespace ImageErosionDilation
    {
        public static class ExtBitmap
        {
            public static Bitmap CopyToSquareCanvas(this Bitmap sourceBitmap, int canvasWidthLenght)
            {
                float ratio = 1.0f;
                int maxSide = sourceBitmap.Width > sourceBitmap.Height ?
                              sourceBitmap.Width : sourceBitmap.Height;

                ratio = (float)maxSide / (float)canvasWidthLenght;

                Bitmap bitmapResult = (sourceBitmap.Width > sourceBitmap.Height ?
                                        new Bitmap(canvasWidthLenght, (int)(sourceBitmap.Height / ratio))
                                        : new Bitmap((int)(sourceBitmap.Width / ratio), canvasWidthLenght));

                using (Graphics graphicsResult = Graphics.FromImage(bitmapResult))
                {
                    graphicsResult.CompositingQuality = CompositingQuality.HighQuality;
                    graphicsResult.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphicsResult.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    graphicsResult.DrawImage(sourceBitmap,
                                            new Rectangle(0, 0,
                                                bitmapResult.Width, bitmapResult.Height),
                                            new Rectangle(0, 0,
                                                sourceBitmap.Width, sourceBitmap.Height),
                                                GraphicsUnit.Pixel);
                    graphicsResult.Flush();
                }

                return bitmapResult;
            }

            public static Bitmap OpenMorphologyFilter(this Bitmap sourceBitmap,
                                                      int matrixSize,
                                                      bool applyBlue = true,
                                                      bool applyGreen = true,
                                                      bool applyRed = true)
            {
                Bitmap resultBitmap = sourceBitmap.DilateAndErodeFilter(matrixSize, 
                                                            MorphologyType.Erosion,
                                                   applyBlue, applyGreen, applyRed);

                resultBitmap = resultBitmap.DilateAndErodeFilter(matrixSize, 
                                                    MorphologyType.Dilation, 
                                                   applyBlue, applyGreen, applyRed);

                return resultBitmap;
            }

            public static Bitmap CloseMorphologyFilter(this Bitmap sourceBitmap,
                                                       int matrixSize,
                                                       bool applyBlue = true,
                                                       bool applyGreen = true,
                                                       bool applyRed = true)
            {
                Bitmap resultBitmap = sourceBitmap.DilateAndErodeFilter(matrixSize,
                                                            MorphologyType.Dilation,
                                                    applyBlue, applyGreen, applyRed);

                resultBitmap = resultBitmap.DilateAndErodeFilter(matrixSize,
                                                     MorphologyType.Erosion, 
                                                    applyBlue, applyGreen, applyRed);

                return resultBitmap;
            }

            public static Bitmap DilateAndErodeFilter(this Bitmap sourceBitmap, 
                                                    int matrixSize,
                                                    MorphologyType morphType,
                                                    bool applyBlue = true,
                                                    bool applyGreen = true,
                                                    bool applyRed = true ) 
            {
                BitmapData sourceData = 
                           sourceBitmap.LockBits(new Rectangle(0, 0,
                           sourceBitmap.Width, sourceBitmap.Height),
                           ImageLockMode.ReadOnly, 
                           PixelFormat.Format32bppArgb);

                byte[] pixelBuffer = new byte[sourceData.Stride * 
                                              sourceData.Height];

                byte[] resultBuffer = new byte[sourceData.Stride * 
                                               sourceData.Height];

                Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, 
                                           pixelBuffer.Length);

                sourceBitmap.UnlockBits(sourceData);

                int filterOffset = (matrixSize - 1) / 2;
                int calcOffset = 0;

                int byteOffset = 0;

                byte blue = 0;
                byte green = 0;
                byte red = 0;

                byte morphResetValue = 0;

                if (morphType == MorphologyType.Erosion)
                {
                    morphResetValue = 255;
                }

                for (int offsetY = filterOffset; offsetY < 
                    sourceBitmap.Height - filterOffset; offsetY++)
                {
                    for (int offsetX = filterOffset; offsetX < 
                        sourceBitmap.Width - filterOffset; offsetX++)
                    {
                        byteOffset = offsetY * 
                                     sourceData.Stride + 
                                     offsetX * 4;

                        blue = morphResetValue;
                        green = morphResetValue;
                        red = morphResetValue;

                        if (morphType == MorphologyType.Dilation)
                        {
                            for (int filterY = -filterOffset;
                                filterY <= filterOffset; filterY++)
                            {
                                for (int filterX = -filterOffset;
                                    filterX <= filterOffset; filterX++)
                                {
                                    calcOffset = byteOffset +
                                                 (filterX * 4) +
                                    (filterY * sourceData.Stride);

                                    if (pixelBuffer[calcOffset] > blue)
                                    {
                                        blue = pixelBuffer[calcOffset];
                                    }

                                    if (pixelBuffer[calcOffset + 1] > green)
                                    {
                                        green = pixelBuffer[calcOffset + 1];
                                    }

                                    if (pixelBuffer[calcOffset + 2] > red)
                                    {
                                        red = pixelBuffer[calcOffset + 2];
                                    }
                                }
                            }
                        }
                        else if (morphType == MorphologyType.Erosion)
                        {
                            for (int filterY = -filterOffset;
                                filterY <= filterOffset; filterY++)
                            {
                                for (int filterX = -filterOffset;
                                    filterX <= filterOffset; filterX++)
                                {

                                    calcOffset = byteOffset +
                                                 (filterX * 4) +
                                    (filterY * sourceData.Stride);

                                    if (pixelBuffer[calcOffset] < blue)
                                    {
                                        blue = pixelBuffer[calcOffset];
                                    }

                                    if (pixelBuffer[calcOffset + 1] < green)
                                    {
                                        green = pixelBuffer[calcOffset + 1];
                                    }

                                    if (pixelBuffer[calcOffset + 2] < red)
                                    {
                                        red = pixelBuffer[calcOffset + 2];
                                    }
                                }
                            }
                        }

                        if (applyBlue == false)
                        {
                            blue = pixelBuffer[byteOffset];
                        }

                        if (applyGreen == false)
                        {
                            green = pixelBuffer[byteOffset + 1];
                        }

                        if (applyRed == false)
                        {
                            red = pixelBuffer[byteOffset + 2];
                        }

                        resultBuffer[byteOffset] = blue;
                        resultBuffer[byteOffset + 1] = green;
                        resultBuffer[byteOffset + 2] = red;
                        resultBuffer[byteOffset + 3] = 255;
                    }
                }

                Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, 
                                                 sourceBitmap.Height);

                BitmapData resultData = 
                           resultBitmap.LockBits(new Rectangle(0, 0,
                           resultBitmap.Width, resultBitmap.Height),
                           ImageLockMode.WriteOnly,
                           PixelFormat.Format32bppArgb);

                Marshal.Copy(resultBuffer, 0, resultData.Scan0, 
                                           resultBuffer.Length);

                resultBitmap.UnlockBits(resultData);

                return resultBitmap;
            }

            public enum MorphologyType
            {
                Dilation,
                Erosion
            }
        }  
    }

    namespace ImageErosionDilation
    {
        public partial class MainForm : Form
        {
            private Bitmap originalBitmap = null;
            private Bitmap previewBitmap = null;
            private Bitmap resultBitmap = null;
        
            public MainForm()
            {
                InitializeComponent();

                cmbFilterSize.SelectedIndex = 0;
            }

            private void btnOpenOriginal_Click(object sender, EventArgs e)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Select an image file.";
                ofd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
                ofd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    StreamReader streamReader = new StreamReader(ofd.FileName);
                    originalBitmap = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
                    streamReader.Close();

                    previewBitmap = originalBitmap.CopyToSquareCanvas(picPreview.Width);
                    picPreview.Image = previewBitmap;

                    ApplyFilter(true);
                }
            }

            private void btnSaveNewImage_Click(object sender, EventArgs e)
            {
                ApplyFilter(false);

                if (resultBitmap != null)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Specify a file name and file path";
                    sfd.Filter = "Png Images(*.png)|*.png|Jpeg Images(*.jpg)|*.jpg";
                    sfd.Filter += "|Bitmap Images(*.bmp)|*.bmp";

                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string fileExtension = Path.GetExtension(sfd.FileName).ToUpper();
                        ImageFormat imgFormat = ImageFormat.Png;

                        if (fileExtension == "BMP")
                        {
                            imgFormat = ImageFormat.Bmp;
                        }
                        else if (fileExtension == "JPG")
                        {
                            imgFormat = ImageFormat.Jpeg;
                        }

                        StreamWriter streamWriter = new StreamWriter(sfd.FileName, false);
                        resultBitmap.Save(streamWriter.BaseStream, imgFormat);
                        streamWriter.Flush();
                        streamWriter.Close();

                        resultBitmap = null;
                    }
                }
            }

            private void ApplyFilter(bool preview)
            {
                if (previewBitmap == null || cmbFilterSize.SelectedIndex == -1)
                {
                    return;
                }

                Bitmap selectedSource = null;
                Bitmap bitmapResult = null;

                if (preview == true)
                {
                    selectedSource = previewBitmap;
                }
                else
                {
                    selectedSource = originalBitmap;
                }

                if (selectedSource != null)
                {
                    if (cmbFilterSize.SelectedItem.ToString() == "None")
                    {
                        bitmapResult = selectedSource;
                    }
                    else
                    {
                        int filterSize = 0;

                        if(Int32.TryParse(cmbFilterSize.SelectedItem.ToString(), out filterSize))
                        {
                            if (rdDilate.Checked == true)
                            {
                                bitmapResult = selectedSource.DilateAndErodeFilter(filterSize, ExtBitmap.MorphologyType.Dilation, chkB.Checked, chkG.Checked, chkR.Checked);
                            }
                            else if (rdErode.Checked == true)
                            {
                                bitmapResult = selectedSource.DilateAndErodeFilter(filterSize, ExtBitmap.MorphologyType.Erosion, chkB.Checked, chkG.Checked, chkR.Checked);
                            }
                            else if (rdOpen.Checked == true)
                            {
                                bitmapResult = selectedSource.OpenMorphologyFilter(filterSize, chkB.Checked, chkG.Checked, chkR.Checked);
                            }
                            else if (rdClosed.Checked == true)
                            {
                                bitmapResult = selectedSource.CloseMorphologyFilter(filterSize, chkB.Checked, chkG.Checked, chkR.Checked);
                            }
                        }
                    }
                }

                if (bitmapResult != null)
                {
                    if (preview == true)
                    {
                        picPreview.Image = bitmapResult;
                    }
                    else
                    {
                        resultBitmap = bitmapResult;
                    }
                }
            }

            private void FilterValueChangedEventHandler(object sender, EventArgs e)
            {
                ApplyFilter(true);
            }
        }
    }
    */

    #endregion

    #region Difference

    [Serializable]
    public class DifferenceAdjustment : ColorAdjustment
    {
        [Range(0, 255, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public override int Red
        {
            get => base.Red;
            set => base.Red = value;
        }

        [Range(0, 255, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public override int Green
        {
            get => base.Green;
            set => base.Green = value;
        }

        [Range(0, 255, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public override int Blue
        {
            get => base.Blue;
            set => base.Blue = value;
        }

        public DifferenceAdjustment() : base() => Name = "Difference";

        public override Color Apply(Color color)
        {
            int ob = color.B, og = color.G, or = color.R;
            int nr = or, ng = og, nb = ob;

            nr = Red > or
                ? Red - or
                : or - Red;
            ng = Green > og
                ? Green - og
                : og - Green;
            nb = Blue > ob
                ? Blue - ob
                : ob - Blue;

            return Color.FromArgb(color.A, nr.Coerce(255).Byte(), ng.Coerce(255).Byte(), nb.Coerce(255).Byte());
        }

        public override Adjustment Clone()
        {
            return new DifferenceAdjustment()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
    }

    #endregion

    #region Exposure

    [Serializable]
    public class ExposureAdjustment : ColorAdjustment
    {
        public ExposureAdjustment() : base() => Name = "Exposure";

        public override Color Apply(Color color)
        {
            return color;
        }

        public override Adjustment Clone()
        {
            return new DifferenceAdjustment()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
    }

    #endregion

    #region Gamma

    [Serializable]
    public class GammaAdjustment : Adjustment
    {
        int[] r, g, b;

        float value;
        [Range(0.2f, 5f, 0.01f)]
        [RangeFormat(RangeFormat.Slider)]
        public float Value
        {
            get => value;
            set
            {
                Ramps(value, out r, out g, out b);
                this.Change(ref this.value, value);
                OnChanged();
            }
        }

        public GammaAdjustment() : this(0f) { }

        public GammaAdjustment(float value) : base("Gamma") => Value = value;

        static void Ramps(float input, out int[] r, out int[] g, out int[] b)
        {
            r = new int[256];
            g = new int[256];
            b = new int[256];
            for (int x = 0; x < 256; ++x)
            {
                r[x] = ((255.0 * Math.Pow(x / 255.0, 1.0 / input)) + 0.5).Round().Int32().Coerce(255);
                g[x] = ((255.0 * Math.Pow(x / 255.0, 1.0 / input)) + 0.5).Round().Int32().Coerce(255);
                b[x] = ((255.0 * Math.Pow(x / 255.0, 1.0 / input)) + 0.5).Round().Int32().Coerce(255);
            }
        }

        public override Color Apply(Color color)
            => Color.FromArgb(color.A, r[color.R].Byte(), g[color.G].Byte(), b[color.B].Byte());

        public override Adjustment Clone()
        {
            return new GammaAdjustment()
            {
                Value = value
            };
        }
    }

    #endregion

    #region Highlights/shadows (requires smoothing; needs additional attention!)

    [Serializable]
    public class HighlightShadowAdjustment : Adjustment
    {
        int highlightAmount = 0;
        [Range(0, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int HighlightAmount
        {
            get => highlightAmount;
            set
            {
                this.Change(ref highlightAmount, value);
                OnChanged();
            }
        }

        int highlightRange = 50;
        [Range(0, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int HighlightRange
        {
            get => highlightRange;
            set
            {
                this.Change(ref highlightRange, value);
                OnChanged();
            }
        }

        int highlightRadius = 30;
        [Range(0, 2500, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int HighlightRadius
        {
            get => highlightRadius;
            set
            {
                this.Change(ref highlightRadius, value);
                OnChanged();
            }
        }

        int shadowAmount = 35;
        [Range(0, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int ShadowAmount
        {
            get => shadowAmount;
            set
            {
                this.Change(ref shadowAmount, value);
                OnChanged();
            }
        }

        int shadowRange = 50;
        [Range(0, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int ShadowRange
        {
            get => shadowRange;
            set
            {
                this.Change(ref shadowRange, value);
                OnChanged();
            }
        }

        int shadowRadius = 30;
        [Range(0, 2500, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int ShadowRadius
        {
            get => shadowRadius;
            set
            {
                this.Change(ref shadowRadius, value);
                OnChanged();
            }
        }

        public HighlightShadowAdjustment() : base() => Name = "Highlights/shadows";

        public override Color Apply(Color color)
        {
            var c = color.Int32();

            //The following two components are used later for calculating the new color
            var hue 
                = c.GetHue().Double();
            var saturation 
                = c.GetSaturation().Double();

            //This determines the tonal range
            var brightness 
                = c.GetBrightness().Double();

            //It's a shadow!
            if (brightness <= shadowRange.Double().Shift(-2))
            {
                //If it is a shadow, increase the brightness of the color by a percentage
                brightness += brightness * shadowAmount.Double().Shift(-2);
            }
            //It's a highlight!
            else if (brightness >= 1 - highlightRange.Double().Shift(-2))
            {
                //If it is a highlight, decrease the brightness of the color by a percentage
                brightness -= brightness * highlightAmount.Double().Shift(-2);
            }
            //It's an unaffected midtone!
            else return color;

            return color;
            /*
            var hsb = new Color<HSB>(converter.Profile, hue, saturation, brightness);
            var rgb = RGB.Convert(hsb);
            return Color.FromArgb(color.A, rgb[0].Shift(2).Multiply(255), rgb[1].Shift(2).Multiply(255), rgb[2].Shift(2).Multiply(255));
            */
        }

        public override Adjustment Clone()
        {
            return new HighlightShadowAdjustment()
            {
                HighlightAmount = HighlightAmount,
                HighlightRadius = HighlightRadius,
                HighlightRange = HighlightRange,
                ShadowAmount = ShadowAmount,
                ShadowRadius = ShadowRadius,
                ShadowRange = ShadowRange,
            };
        }
    }

    #endregion

    #region HSL

    [Serializable]
    public class HSLAdjustment : ColorModelAdjustment
    {
        int hue;
        [Range(-180, 180, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Hue
        {
            get => hue;
            set
            {
                this.Change(ref hue, value);
                OnChanged();
            }
        }

        int saturation;
        [Range(-50, 50, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Saturation
        {
            get => saturation;
            set
            {
                this.Change(ref saturation, value);
                OnChanged();
            }
        }

        int lightness;
        [Range(-127, 128, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Lightness
        {
            get => lightness;
            set
            {
                this.Change(ref lightness, value);
                OnChanged();
            }
        }

        public HSLAdjustment() : this(0, 0, 0) { }

        public HSLAdjustment(int hue, int saturation, int lightness) : base("HSL")
        {
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }

        public override Color Apply(Color color)
        {
            return color;
            /*
            var rgb1 = (Color<RGB>)color.Convert<RGB>(converter);
            var hsp1 = HSP.Convert(rgb1);
            var hsp2 = new Color<HSP>(converter.Profile, (hsp1[0] + hue) % 359, (hsp1[1] + saturation.Double()).Coerce(100), (hsp1[2] + lightness.Double()).Coerce(255));
            var rgb2 = RGB.Convert(hsp2);
            return rgb2.Convert(converter, color.A);
            */
        }

        public override Adjustment Clone()
        {
            return new HSLAdjustment();
        }
    }

    #endregion

    #region Lab

    [Serializable]
    public class LabAdjustment : ColorModelAdjustment
    {
        int lightness;
        [Range(-50, 50, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Lightness
        {
            get => lightness;
            set
            {
                this.Change(ref lightness, value);
                OnChanged();
            }
        }

        int a;
        [Range(-50, 50, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int A
        {
            get => a;
            set
            {
                this.Change(ref a, value);
                OnChanged();
            }
        }

        int b;
        [Range(-50, 50, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int B
        {
            get => b;
            set
            {
                this.Change(ref b, value);
                OnChanged();
            }
        }

        public LabAdjustment() : this(0, 0, 0) { }

        public LabAdjustment(int lightness, int a, int b) : base("Lab")
        {
            Lightness = lightness;
            A = a;
            B = b;
        }

        public override Color Apply(Color color)
        {
            return color;
            /*
            var rgb1 = new Color<RGB>(converter.Profile, color.R, color.G, color.B);
            var lab1 = (Color<Lab>)rgb1.Convert<Lab>(converter);
            var lab2 = new Color<Lab>(converter.Profile, lab1[0] + lightness, lab1[1] + a, lab1[2] + b);
            var rgb2 = lab2.Convert<RGB>(converter);
            return rgb2.Convert(converter, color.A);
            */
        }

        public override Adjustment Clone()
        {
            return new LabAdjustment();
        }
    }

    #endregion

    #region LChab

    [Serializable]
    public class LChabAdjustment : ColorModelAdjustment
    {
        int lightness;
        [Range(-50, 50, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Lightness
        {
            get => lightness;
            set
            {
                this.Change(ref lightness, value);
                OnChanged();
            }
        }

        int chroma;
        [Range(-50, 50, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Chroma
        {
            get => chroma;
            set
            {
                this.Change(ref chroma, value);
                OnChanged();
            }
        }

        int hue;
        [Range(-50, 50, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Hue
        {
            get => hue;
            set
            {
                this.Change(ref hue, value);
                OnChanged();
            }
        }

        public LChabAdjustment() : base("LChab") { }

        public override Color Apply(Color color)
        {
            return color;
            /*
            var rgb1 = new Color<RGB>(converter.Profile, color.R, color.G, color.B);
            var lChab1 = (Color<LChab>)rgb1.Convert<LChab>(converter);
            var lChab2 = new Color<LChab>(converter.Profile, lChab1[0] + lightness, lChab1[1] + chroma, lChab1[2] + hue);
            var rgb2 = lChab2.Convert<RGB>(converter);
            return rgb2.Convert(converter, color.A);
            */
        }

        public override Adjustment Clone()
        {
            return new LChabAdjustment();
        }
    }

    #endregion

    #region LChuv

    [Serializable]
    public class LChuvAdjustment : ColorModelAdjustment
    {
        int lightness;
        [Range(-50, 50, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Lightness
        {
            get => lightness;
            set
            {
                this.Change(ref lightness, value);
                OnChanged();
            }
        }

        int chroma;
        [Range(-50, 50, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Chroma
        {
            get => chroma;
            set
            {
                this.Change(ref chroma, value);
                OnChanged();
            }
        }

        int hue;
        [Range(-50, 50, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Hue
        {
            get => hue;
            set
            {
                this.Change(ref hue, value);
                OnChanged();
            }
        }

        public LChuvAdjustment() : base("LChuv") { }

        public override Color Apply(Color color)
        {
            return color;
            /*
            var rgb1 = new Color<RGB>(converter.Profile, color.R, color.G, color.B);
            var lChuv1 = (Color<LChuv>)rgb1.Convert<LChuv>(converter);
            var lChuv2 = new Color<LChuv>(converter.Profile, lChuv1[0] + lightness, lChuv1[1] + chroma, lChuv1[2] + hue);
            var rgb2 = lChuv2.Convert<RGB>(converter);
            return rgb2.Convert(converter, color.A);
            */
        }

        public override Adjustment Clone()
        {
            return new LChuvAdjustment();
        }
    }

    #endregion

    #region Noise

    [Serializable]
    public class NoiseAdjustment : Adjustment
    {
        int amount = 0;
        [Range(0, 128, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Amount
        {
            get => amount;
            set
            {
                this.Change(ref amount, value);
                OnChanged();
            }
        }

        StringColor color = Colors.Black;
        public Color Color
        {
            get => color;
            set
            {
                this.Change(ref color, value);
                OnChanged();
            }
        }

        bool monochromatic = false;
        public bool Monochromatic
        {
            get => monochromatic;
            set
            {
                this.Change(ref monochromatic, value);
                OnChanged();
            }
        }

        public NoiseAdjustment() : base("Noise") { }

        public override Color Apply(Color color)
        {
            int or = color.R, og = color.G, ob = color.B;
            int ir = Imagin.Common.Random.Current.Next(-Amount, Amount + 1), ig = 0, ib = 0;

            if (monochromatic)
            {
                ig = ir;
                ib = ir;
            }
            else
            {
                ig = Imagin.Common.Random.Current.Next(-Amount, Amount + 1);
                ib = Imagin.Common.Random.Current.Next(-Amount, Amount + 1);
            }

            byte
                nr = (or + ir).Coerce(255).Byte(),
                ng = (og + ig).Coerce(255).Byte(),
                nb = (ob + ib).Coerce(255).Byte();

            return Color.FromArgb(color.A, nr, ng, nb);
        }

        public override Adjustment Clone()
        {
            return new NoiseAdjustment()
            {
                Amount = amount,
                Monochromatic = monochromatic
            };
        }
    }

    #endregion

    #region Oil painting

    [Serializable]
    public class OilPaintingAdjustment : Adjustment
    {
        int levels = 1;
        [Range(1, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Levels
        {
            get => levels;
            set
            {
                this.Change(ref levels, value);
                OnChanged();
            }
        }

        int size = 3;
        [Range(3, 27, 2)]
        [RangeFormat(RangeFormat.Slider)]
        public int Size
        {
            get => size;
            set
            {
                this.Change(ref size, value);
                OnChanged();
            }
        }

        public OilPaintingAdjustment() : base("Oil painting") { }

        public Color Apply(ColorMatrix colors, int x, int y, Color color)
        {
            int filterOffset = (size - 1) / 2;

            int currentIntensity = 0;
            int maxIndex = 0, maxIntensity = 0;

            double red = 0, green = 0, blue = 0;

            int[] intensityBin
                = new int[levels];
            int[] blueBin
                = new int[levels];
            int[] greenBin
                = new int[levels];
            int[] redBin
                = new int[levels];

            for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
            {
                for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
                {
                    var x2 = x + filterX;
                    var y2 = y + filterY;

                    if (x2 >= 0 && x2 < colors.Columns && y2 >= 0 && y2 < colors.Rows)
                    {
                        var c = colors.GetValue(y2.UInt32(), x2.UInt32());
                        currentIntensity = (int)((((c.R.Double() + c.G.Double() + c.B.Double()) / 3.0) * levels) / 255.0);
                        intensityBin[currentIntensity]++;

                        redBin[currentIntensity] += c.R;
                        greenBin[currentIntensity] += c.G;
                        blueBin[currentIntensity] += c.B;

                        if (intensityBin[currentIntensity] > maxIntensity)
                        {
                            maxIntensity = intensityBin[currentIntensity];
                            maxIndex = currentIntensity;
                        }
                    }
                }
            }

            var mid = maxIntensity.Double();
            red = redBin[maxIndex].Double() / mid;
            green = greenBin[maxIndex].Double() / mid;
            blue = blueBin[maxIndex].Double() / mid;

            return Color.FromArgb(color.A, red.Coerce(255).Byte(), green.Coerce(255).Byte(), blue.Coerce(255).Byte());
        }

        public override Color Apply(Color color) => color;

        public override void Apply(ColorMatrix oldMatrix, ColorMatrix newMatrix)
        {
            oldMatrix.Each((y, x, oldColor) =>
            {
                var newColor = Apply(oldMatrix, x, y, oldColor);
                newMatrix.SetValue(y.UInt32(), x.UInt32(), newColor);
                return oldColor;
            });
        }

        public override Adjustment Clone()
        {
            return new OilPaintingAdjustment()
            {
                Levels = levels,
                Size = size
            };
        }
    }

    #endregion

    #region Posterize

    [Serializable]
    public class PosterizeAdjustment : Adjustment
    {
        int defaultThreshold = 64;

        float NumAreas;

        float NumValues;

        int threshold;
        [Range(3, 64, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Threshold
        {
            get => threshold;
            set
            {
                Update();
                this.Change(ref threshold, value);
                OnChanged();
            }
        }

        public PosterizeAdjustment() : base("Posterize")
        {
            threshold = defaultThreshold;
            Update();
        }

        void Update()
        {
            NumAreas = 256f / threshold.Single();
            NumValues = 255f / (threshold.Single() - 1f);
        }

        public override Color Apply(Color color)
        {
            int currentRed = color.R, currentGreen = color.G, currentBlue = color.B;

            float redAreaFloat = currentRed.Single() / NumAreas;
            int redArea = redAreaFloat.Int32();
            if (redArea > redAreaFloat) redArea = redArea - 1;
            float newRedFloat = NumValues * redArea;
            int newRed = newRedFloat.Int32();
            if (newRed > newRedFloat) newRed = newRed - 1;

            float greenAreaFloat = currentGreen.Single() / NumAreas;
            int greenArea = greenAreaFloat.Int32();
            if (greenArea > greenAreaFloat) greenArea = greenArea - 1;
            float newGreenFloat = NumValues * greenArea;
            int newGreen = newGreenFloat.Int32();
            if (newGreen > newGreenFloat) newGreen = newGreen - 1;

            float blueAreaFloat = currentBlue.Single() / NumAreas;
            int blueArea = blueAreaFloat.Int32();
            if (blueArea > blueAreaFloat) blueArea = blueArea - 1;
            float newBlueFloat = NumValues * blueArea;
            int newBlue = newBlueFloat.Int32();
            if (newBlue > newBlueFloat) newBlue = newBlue - 1;

            return Color.FromArgb(color.A, newRed.Coerce(255).Byte(), newGreen.Coerce(255).Byte(), newBlue.Coerce(255).Byte());
        }

        public override Adjustment Clone()
        {
            return new PosterizeAdjustment()
            {
                Threshold = threshold
            };
        }
    }

    #endregion

    #region Replace

    [Serializable]
    public class ReplaceAdjustment : Adjustment
    {
        StringColor color1 = Colors.White;
        [DisplayName("Color 1")]
        public Color Color1
        {
            get => color1;
            set
            {
                this.Change(ref color1, value);
                OnChanged();
            }
        }

        StringColor color2 = Colors.Black;
        [DisplayName("Color 2")]
        public Color Color2
        {
            get => color2;
            set
            {
                this.Change(ref color2, value);
                OnChanged();
            }
        }

        public ReplaceAdjustment() : base("Replace") { }

        public override Color Apply(Color color) => color == color1.Value ? color2.Value : color;

        public override Adjustment Clone()
        {
            return new ReplaceAdjustment()
            {
                Color1 = color1,
                Color2 = color2
            };
        }
    }

    #endregion

    #region Shading

    [Serializable]
    public class ShadingAdjustment : ColorAdjustment
    {
        [Range(0, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public override int Red
        {
            get => base.Red;
            set => base.Red = value;
        }

        [Range(0, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public override int Green
        {
            get => base.Green;
            set => base.Green = value;
        }

        [Range(0, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public override int Blue
        {
            get => base.Blue;
            set => base.Blue = value;
        }

        public ShadingAdjustment() : this(100, 100, 100) { }

        public ShadingAdjustment(int red, int green, int blue) : base(red, green, blue) => Name = "Shading";

        public override Color Apply(Color color)
        {
            var r = (color.R.Double() * (Red.Double() / 100.0)).Round().Int32().Coerce(255);
            var g = (color.G.Double() * (Green.Double() / 100.0)).Round().Int32().Coerce(255);
            var b = (color.B.Double() * (Blue.Double() / 100.0)).Round().Int32().Coerce(255);
            return Color.FromArgb(color.A, r.Byte(), g.Byte(), b.Byte());
        }

        public override Adjustment Clone()
        {
            return new ShadingAdjustment()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
    }

    #endregion

    #region Stained glass (needs work!)

    /// <summary>
    /// Requires enumerating entire image several times (presumably) to detect edges among other things...
    /// </summary>
    [Serializable]
    public class StainedGlassAdjustment : Adjustment
    {
        /* public static Bitmap StainedGlass(this Bitmap input, int blockSize, double blockFactor, DistanceFormulaType distanceType, bool highlightEdges, byte edgeThreshold, System.Drawing.Color edgeColor)
        {
            BitmapData sourceData = input.LockBits(new Rectangle(0, 0, input.Width, input.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            input.UnlockBits(sourceData);

            int neighbourHoodTotal = 0;
            int sourceOffset = 0;
            int resultOffset = 0;
            int currentPixelDistance = 0;
            int nearestPixelDistance = 0;
            int nearesttPointIndex = 0;
            int Width = input.Width, Height = input.Height;
            Random randomizer = new Random();

            List<VoronoiPoint> randomPointList = new List<VoronoiPoint>();

            Parallel.For(0, Height - blockSize, row =>
            {
                for (int col = 0; col < Width - blockSize; col += blockSize)
                {
                    sourceOffset = row * sourceData.Stride + col * 4;

                    neighbourHoodTotal = 0;

                    for (int y = 0; y < blockSize; y++)
                    {
                        for (int x = 0; x < blockSize; x++)
                        {
                            resultOffset = sourceOffset + y * sourceData.Stride + x * 4;
                            neighbourHoodTotal += pixelBuffer[resultOffset];
                            neighbourHoodTotal += pixelBuffer[resultOffset + 1];
                            neighbourHoodTotal += pixelBuffer[resultOffset + 2];
                        }
                    }

                    randomizer = new Random(neighbourHoodTotal);

                    VoronoiPoint randomPoint = new VoronoiPoint();
                    randomPoint.XOffset = randomizer.Next(0, blockSize) + col;
                    randomPoint.YOffset = randomizer.Next(0, blockSize) + row;

                    randomPointList.Add(randomPoint);

                    row += blockSize - 1;
                }
            });
            int rowOffset = 0;
            int colOffset = 0;
            for (int bufferOffset = 0; bufferOffset < pixelBuffer.Length - 4; bufferOffset += 4)
            {
                rowOffset = bufferOffset / sourceData.Stride;
                colOffset = (bufferOffset % sourceData.Stride) / 4;

                currentPixelDistance = 0;
                nearestPixelDistance = blockSize * 4;
                nearesttPointIndex = 0;

                List<VoronoiPoint> pointSubset = new List<VoronoiPoint>();

                pointSubset.AddRange(from t in randomPointList where rowOffset >= t.YOffset - blockSize * 2 && rowOffset <= t.YOffset + blockSize * 2 select t);
                for (int k = 0; k < pointSubset.Count; k++)
                {
                    if (distanceType == DistanceFormulaType.Euclidean)
                    {
                        currentPixelDistance = Imagin.Imaging.Distance.Calculate(DistanceFormulaType.Euclidean, pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);
                    }
                    else if (distanceType == DistanceFormulaType.Manhattan)
                    {
                        currentPixelDistance = Imagin.Imaging.Distance.Calculate(DistanceFormulaType.Manhattan, pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);
                    }
                    else if (distanceType == DistanceFormulaType.Chebyshev)
                    {
                        currentPixelDistance = Imagin.Imaging.Distance.Calculate(DistanceFormulaType.Chebyshev, pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);
                    }

                    if (currentPixelDistance <= nearestPixelDistance)
                    {
                        nearestPixelDistance = currentPixelDistance;
                        nearesttPointIndex = k;

                        if (nearestPixelDistance <= blockSize / blockFactor)
                        {
                            break;
                        }
                    }
                }
                Pixel tmpPixel = new Pixel();
                tmpPixel.XOffset = colOffset;
                tmpPixel.YOffset = rowOffset;
                tmpPixel.Blue = pixelBuffer[bufferOffset];
                tmpPixel.Green = pixelBuffer[bufferOffset + 1];
                tmpPixel.Red = pixelBuffer[bufferOffset + 2];
                pointSubset[nearesttPointIndex].AddPixel(tmpPixel);
            }
            Parallel.For(0, randomPointList.Count, k =>
            {
                randomPointList[k].CalculateAverages();
                for (int i = 0; i < randomPointList[k].PixelCollection.Count; i++)
                {
                    resultOffset = randomPointList[k].PixelCollection[i].YOffset * sourceData.Stride + randomPointList[k].PixelCollection[i].XOffset * 4;
                    resultBuffer[resultOffset] = (byte)randomPointList[k].BlueAverage;
                    resultBuffer[resultOffset + 1] = (byte)randomPointList[k].GreenAverage;
                    resultBuffer[resultOffset + 2] = (byte)randomPointList[k].RedAverage;
                    resultBuffer[resultOffset + 3] = 255;
                }
            });
            Bitmap resultBitmap = new Bitmap(input.Width, input.Height);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            if (highlightEdges == true)
            {
                resultBitmap = Adjustment.GradientBasedEdgeDetection(resultBitmap, edgeThreshold, edgeColor);
            }
            return resultBitmap;
        }
        */

        public StainedGlassAdjustment() : base("Stained glass") { }

        public Color Apply(ColorMatrix colors, int x, int y, Color color)
        {

            return color;
        }

        public override Color Apply(Color color) => color;

        public override void Apply(ColorMatrix oldMatrix, ColorMatrix newMatrix)
        {
            oldMatrix.Each((y, x, oldColor) =>
            {
                var newColor = Apply(oldMatrix, x, y, oldColor);
                newMatrix.SetValue(y.UInt32(), x.UInt32(), newColor);
                return oldColor;
            });
        }

        public override Adjustment Clone()
        {
            return new StainedGlassAdjustment();
        }
    }

    #endregion

    #region Swap

    [Serializable]
    public class SwapAdjustment : Adjustment
    {
        ComponentSwap swapType;
        public ComponentSwap SwapType
        {
            get => swapType;
            set
            {
                this.Change(ref swapType, value);
                (this as IPropertyChanged).Changed(() => Channels);
                OnChanged();
            }
        }

        public double Channels => (int)swapType;

        public SwapAdjustment() : base("Swap") { }

        public override Color Apply(Color color) => color.Swap(swapType);

        public override Adjustment Clone()
        {
            return new SwapAdjustment()
            {
                SwapType = swapType
            };
        }
    }

    #endregion

    #region Threshold

    [Serializable]
    public class ThresholdAdjustment : Adjustment
    {
        StringColor color1 = Colors.White;
        [DisplayName("Color 1")]
        public Color Color1
        {
            get => color1;
            set
            {
                this.Change(ref color1, value);
                OnChanged();
            }
        }

        StringColor color2 = Colors.Black;
        [DisplayName("Color 2")]
        public Color Color2
        {
            get => color2;
            set
            {
                this.Change(ref color2, value);
                OnChanged();
            }
        }

        int level = 128;
        [Range(1, 255, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Level
        {
            get => level;
            set
            {
                this.Change(ref level, value);
                OnChanged();
            }
        }

        public ThresholdAdjustment() : base("Threshold") { }

        public sealed override Color Apply(Color color)
        {
            var brightness = color.Int32().GetBrightness();
            return brightness > level.Double() / 255.0 ? color1 : color2;
        }

        public override Adjustment Clone()
        {
            return new ThresholdAdjustment()
            {
                Color1 = color1,
                Color2 = color2,
                Level = level
            };
        }
    }

    #endregion

    #region Tint

    [Serializable]
    public class TintAdjustment : ColorAdjustment
    {
        [Range(0, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public override int Red
        {
            get => base.Red;
            set => base.Red = value;
        }

        [Range(0, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public override int Green
        {
            get => base.Green;
            set => base.Green = value;
        }

        [Range(0, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public override int Blue
        {
            get => base.Blue;
            set => base.Blue = value;
        }

        public TintAdjustment() : this(0, 0, 0)
        {
        }

        public TintAdjustment(int red, int green, int blue) : base(red, green, blue) => Name = "Tint";

        public override Color Apply(Color color)
        {
            int b = color.B, g = color.G, r = color.R, a = color.A;
            return Color.FromArgb(a.Byte(), (r + (255 - r) * (Red / 100.0)).Round().Int32().Coerce(255).Byte(), (g + (255 - g) * (Green / 100.0)).Round().Int32().Coerce(255).Byte(), (b + (255 - b) * (Blue / 100.0)).Round().Int32().Coerce(255).Byte());
        }

        public override Adjustment Clone()
        {
            return new TintAdjustment()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            };
        }
    }

    #endregion

    #region Vibrance

    [Serializable]
    public class VibranceAdjustment : Adjustment
    {
        int vibrance = 0;
        [Range(-100, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Vibrance
        {
            get => vibrance;
            set
            {
                this.Change(ref vibrance, value);
                OnChanged();
            }
        }

        int threshold = 1;
        [Range(-100, 100, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int Threshold
        {
            get => threshold;
            set
            {
                this.Change(ref threshold, value);
                OnChanged();
            }
        }

        public VibranceAdjustment() : base("Vibrance") { }

        public override Color Apply(Color color)
        {
            return color;
            /*
            var rgb = (Color<RGB>)color.Convert<RGB>(converter);

            var hsl = HSL.Convert(rgb);
            var s = hsl[1];

            var v = vibrance.Double().Shift(-2);

            var t = threshold.Double().Shift(-2);
            var ta = t.Absolute();

            var i = s * v;
            if ((t > 0 && s > ta) || (t < 0 && s < ta))
            {
                hsl = new Color<HSL>(converter.Profile, hsl[0], (s + i).Coerce(1), hsl[2]);
                return RGB.Convert(hsl).Convert(converter);
            }

            s += (s / (1 - ta)) * v;
            hsl = new Color<HSL>(converter.Profile, hsl[0], s.Coerce(1), hsl[2]);
            return RGB.Convert(hsl).Convert(converter);
            */
        }

        public override Adjustment Clone()
        {
            return new VibranceAdjustment();
        }
    }

    #endregion

    #region Vignette (needs work!)

    [Serializable]
    public class VignetteAdjustment : Adjustment
    {
        /* public static Bitmap Vignette(this Bitmap input, System.Windows.Media.Color Color)
        {
            using (Graphics g = Graphics.FromImage(input))
            {
                Rectangle bounds = new Rectangle(0, 0, input.Width, input.Height);
                Rectangle ellipsebounds = bounds;
                ellipsebounds.Offset(-ellipsebounds.X, -ellipsebounds.Y);
                int x = ellipsebounds.Width - (int)Math.Round(.70712 * ellipsebounds.Width);
                int y = ellipsebounds.Height - (int)Math.Round(.70712 * ellipsebounds.Height);
                ellipsebounds.Inflate(x, y);
                using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddEllipse(ellipsebounds);
                    using (System.Drawing.Drawing2D.PathGradientBrush brush = new System.Drawing.Drawing2D.PathGradientBrush(path))
                    {
                        brush.WrapMode = System.Drawing.Drawing2D.WrapMode.Tile;
                        brush.CenterColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
                        brush.SurroundColors = new System.Drawing.Color[] { System.Drawing.Color.FromArgb(Color.A, Color.R, Color.G, Color.B) };
                        System.Drawing.Drawing2D.Blend blend = new System.Drawing.Drawing2D.Blend();
                        blend.Positions = new float[] { 0.0f, 0.2f, 0.4f, 0.6f, 0.8f, 1.0F };
                        blend.Factors = new float[] { 0.0f, 0.5f, 1f, 1f, 1.0f, 1.0f };
                        brush.Blend = blend;
                        Region oldClip = g.Clip;
                        g.Clip = new Region(bounds);
                        g.FillRectangle(brush, ellipsebounds);
                        g.Clip = oldClip;
                    }
                }
            }
            return input;
        }
        */

        public VignetteAdjustment() : base("Vignette") { }

        public override Color Apply(Color color)
        {
            return color;
        }

        public override Adjustment Clone()
        {
            return new VignetteAdjustment();
        }
    }

    #endregion

    #region XYZ

    [Serializable]
    public class XYZAdjustment : ColorModelAdjustment
    {
        double x;
        [Range(-50.0, 50.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double X
        {
            get => x;
            set
            {
                this.Change(ref x, value);
                OnChanged();
            }
        }

        double y;
        [Range(-50.0, 50.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Y
        {
            get => y;
            set
            {
                this.Change(ref y, value);
                OnChanged();
            }
        }

        double z;
        [Range(-50.0, 50.0, 1.0)]
        [RangeFormat(RangeFormat.Slider)]
        public double Z
        {
            get => z;
            set
            {
                this.Change(ref z, value);
                OnChanged();
            }
        }

        public XYZAdjustment() : this(0, 0, 0) { }

        public XYZAdjustment(double x, double y, double z) : base("XYZ")
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override Color Apply(Color color)
        {
            return color;
            /*
            var rgb1 = new Color<RGB>(converter.Profile, color.R, color.G, color.B);
            var xyz1 = (Color<XYZ>)rgb1.Convert<XYZ>(converter);
            var xyz2 = new Color<XYZ>(converter.Profile, xyz1[0] + x, xyz1[1] + y, xyz1[2] + z);
            var rgb2 = xyz2.Convert<RGB>(converter);
            return rgb2.Convert(converter, color.A);
            */
        }

        public override Adjustment Clone()
        {
            return new XYZAdjustment();
        }
    }

    #endregion
}