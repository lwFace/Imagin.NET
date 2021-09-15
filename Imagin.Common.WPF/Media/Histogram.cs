﻿using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Media
{
    public class Histogram : Base
    {
        /// <summary>
        /// RGB components.
        /// </summary>
        /// 
        /// <remarks><para>The class encapsulates <b>RGB</b> color components.</para>
        /// <para><note><see cref="System.Drawing.Imaging.PixelFormat">PixelFormat.Format24bppRgb</see>
        /// actually means BGR format.</note></para>
        /// </remarks>
        /// 
        public class RGB
        {
            /// <summary>
            /// Index of red component.
            /// </summary>
            public const short R = 2;

            /// <summary>
            /// Index of green component.
            /// </summary>
            public const short G = 1;

            /// <summary>
            /// Index of blue component.
            /// </summary>
            public const short B = 0;

            /// <summary>
            /// Index of alpha component for ARGB images.
            /// </summary>
            public const short A = 3;

            /// <summary>
            /// Red component.
            /// </summary>
            public byte Red;

            /// <summary>
            /// Green component.
            /// </summary>
            public byte Green;

            /// <summary>
            /// Blue component.
            /// </summary>
            public byte Blue;

            /// <summary>
            /// Alpha component.
            /// </summary>
            public byte Alpha;

            /// <summary>
            /// <see cref="System.Drawing.Color">Color</see> value of the class.
            /// </summary>
            public System.Drawing.Color Color
            {
                get { return System.Drawing.Color.FromArgb(Alpha, Red, Green, Blue); }
                set
                {
                    Red = value.R;
                    Green = value.G;
                    Blue = value.B;
                    Alpha = value.A;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RGB"/> class.
            /// </summary>
            public RGB()
            {
                Red = 0;
                Green = 0;
                Blue = 0;
                Alpha = 255;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RGB"/> class.
            /// </summary>
            /// 
            /// <param name="red">Red component.</param>
            /// <param name="green">Green component.</param>
            /// <param name="blue">Blue component.</param>
            /// 
            public RGB(byte red, byte green, byte blue)
            {
                this.Red = red;
                this.Green = green;
                this.Blue = blue;
                this.Alpha = 255;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RGB"/> class.
            /// </summary>
            /// 
            /// <param name="red">Red component.</param>
            /// <param name="green">Green component.</param>
            /// <param name="blue">Blue component.</param>
            /// <param name="alpha">Alpha component.</param>
            /// 
            public RGB(byte red, byte green, byte blue, byte alpha)
            {
                this.Red = red;
                this.Green = green;
                this.Blue = blue;
                this.Alpha = alpha;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RGB"/> class.
            /// </summary>
            /// 
            /// <param name="color">Initialize from specified <see cref="System.Drawing.Color">color.</see></param>
            /// 
            public RGB(System.Drawing.Color color)
            {
                this.Red = color.R;
                this.Green = color.G;
                this.Blue = color.B;
                this.Alpha = color.A;
            }
        }

        /// <summary>
        /// HSL components.
        /// </summary>
        /// 
        /// <remarks>The class encapsulates <b>HSL</b> color components.</remarks>
        /// 
        public class HSL
        {
            /// <summary>
            /// Hue component.
            /// </summary>
            /// 
            /// <remarks>Hue is measured in the range of [0, 359].</remarks>
            /// 
            public int Hue;

            /// <summary>
            /// Saturation component.
            /// </summary>
            /// 
            /// <remarks>Saturation is measured in the range of [0, 1].</remarks>
            /// 
            public float Saturation;

            /// <summary>
            /// Luminance value.
            /// </summary>
            /// 
            /// <remarks>Luminance is measured in the range of [0, 1].</remarks>
            /// 
            public float Luminance;

            /// <summary>
            /// Initializes a new instance of the <see cref="HSL"/> class.
            /// </summary>
            public HSL() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="HSL"/> class.
            /// </summary>
            /// 
            /// <param name="hue">Hue component.</param>
            /// <param name="saturation">Saturation component.</param>
            /// <param name="luminance">Luminance component.</param>
            /// 
            public HSL(int hue, float saturation, float luminance)
            {
                this.Hue = hue;
                this.Saturation = saturation;
                this.Luminance = luminance;
            }

            /// <summary>
            /// Convert from RGB to HSL color space.
            /// </summary>
            /// 
            /// <param name="rgb">Source color in <b>RGB</b> color space.</param>
            /// <param name="hsl">Destination color in <b>HSL</b> color space.</param>
            /// 
            /// <remarks><para>See <a href="http://en.wikipedia.org/wiki/HSI_color_space#Conversion_from_RGB_to_HSL_or_HSV">HSL and HSV Wiki</a>
            /// for information about the algorithm to convert from RGB to HSL.</para></remarks>
            /// 
            public static void FromRGB(RGB rgb, HSL hsl)
            {
                float r = (rgb.Red / 255.0f);
                float g = (rgb.Green / 255.0f);
                float b = (rgb.Blue / 255.0f);

                float min = System.Math.Min(System.Math.Min(r, g), b);
                float max = System.Math.Max(System.Math.Max(r, g), b);
                float delta = max - min;

                // get luminance value
                hsl.Luminance = (max + min) / 2;

                if (delta == 0)
                {
                    // gray color
                    hsl.Hue = 0;
                    hsl.Saturation = 0.0f;
                }
                else
                {
                    // get saturation value
                    hsl.Saturation = (hsl.Luminance <= 0.5) ? (delta / (max + min)) : (delta / (2 - max - min));

                    // get hue value
                    float hue;

                    if (r == max)
                    {
                        hue = ((g - b) / 6) / delta;
                    }
                    else if (g == max)
                    {
                        hue = (1.0f / 3) + ((b - r) / 6) / delta;
                    }
                    else
                    {
                        hue = (2.0f / 3) + ((r - g) / 6) / delta;
                    }

                    // correct hue if needed
                    if (hue < 0)
                        hue += 1;
                    if (hue > 1)
                        hue -= 1;

                    hsl.Hue = (int)(hue * 360);
                }
            }

            /// <summary>
            /// Convert from RGB to HSL color space.
            /// </summary>
            /// 
            /// <param name="rgb">Source color in <b>RGB</b> color space.</param>
            /// 
            /// <returns>Returns <see cref="HSL"/> instance, which represents converted color value.</returns>
            /// 
            public static HSL FromRGB(RGB rgb)
            {
                HSL hsl = new HSL();
                FromRGB(rgb, hsl);
                return hsl;
            }

            /// <summary>
            /// Convert from HSL to RGB color space.
            /// </summary>
            /// 
            /// <param name="hsl">Source color in <b>HSL</b> color space.</param>
            /// <param name="rgb">Destination color in <b>RGB</b> color space.</param>
            /// 
            public static void ToRGB(HSL hsl, RGB rgb)
            {
                if (hsl.Saturation == 0)
                {
                    // gray values
                    rgb.Red = rgb.Green = rgb.Blue = (byte)(hsl.Luminance * 255);
                }
                else
                {
                    float v1, v2;
                    float hue = (float)hsl.Hue / 360;

                    v2 = (hsl.Luminance < 0.5) ?
                        (hsl.Luminance * (1 + hsl.Saturation)) :
                        ((hsl.Luminance + hsl.Saturation) - (hsl.Luminance * hsl.Saturation));
                    v1 = 2 * hsl.Luminance - v2;

                    rgb.Red = (byte)(255 * Hue_2_RGB(v1, v2, hue + (1.0f / 3)));
                    rgb.Green = (byte)(255 * Hue_2_RGB(v1, v2, hue));
                    rgb.Blue = (byte)(255 * Hue_2_RGB(v1, v2, hue - (1.0f / 3)));
                }
                rgb.Alpha = 255;
            }

            /// <summary>
            /// Convert the color to <b>RGB</b> color space.
            /// </summary>
            /// 
            /// <returns>Returns <see cref="RGB"/> instance, which represents converted color value.</returns>
            /// 
            public RGB ToRGB()
            {
                RGB rgb = new RGB();
                ToRGB(this, rgb);
                return rgb;
            }

            #region Private members
            // HSL to RGB helper routine
            private static float Hue_2_RGB(float v1, float v2, float vH)
            {
                if (vH < 0)
                    vH += 1;
                if (vH > 1)
                    vH -= 1;
                if ((6 * vH) < 1)
                    return (v1 + (v2 - v1) * 6 * vH);
                if ((2 * vH) < 1)
                    return v2;
                if ((3 * vH) < 2)
                    return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);
                return v1;
            }
            #endregion
        }

        /// <summary>
        /// YCbCr components.
        /// </summary>
        /// 
        /// <remarks>The class encapsulates <b>YCbCr</b> color components.</remarks>
        /// 
        public class YCbCr
        {
            /// <summary>
            /// Index of <b>Y</b> component.
            /// </summary>
            public const short YIndex = 0;

            /// <summary>
            /// Index of <b>Cb</b> component.
            /// </summary>
            public const short CbIndex = 1;

            /// <summary>
            /// Index of <b>Cr</b> component.
            /// </summary>
            public const short CrIndex = 2;

            /// <summary>
            /// <b>Y</b> component.
            /// </summary>
            public float Y;

            /// <summary>
            /// <b>Cb</b> component.
            /// </summary>
            public float Cb;

            /// <summary>
            /// <b>Cr</b> component.
            /// </summary>
            public float Cr;

            /// <summary>
            /// Initializes a new instance of the <see cref="YCbCr"/> class.
            /// </summary>
            public YCbCr() { }

            /// <summary>
            /// Initializes a new instance of the <see cref="YCbCr"/> class.
            /// </summary>
            /// 
            /// <param name="y"><b>Y</b> component.</param>
            /// <param name="cb"><b>Cb</b> component.</param>
            /// <param name="cr"><b>Cr</b> component.</param>
            /// 
            public YCbCr(float y, float cb, float cr)
            {
                this.Y = System.Math.Max(0.0f, System.Math.Min(1.0f, y));
                this.Cb = System.Math.Max(-0.5f, System.Math.Min(0.5f, cb));
                this.Cr = System.Math.Max(-0.5f, System.Math.Min(0.5f, cr));
            }

            /// <summary>
            /// Convert from RGB to YCbCr color space (Rec 601-1 specification). 
            /// </summary>
            /// 
            /// <param name="rgb">Source color in <b>RGB</b> color space.</param>
            /// <param name="ycbcr">Destination color in <b>YCbCr</b> color space.</param>
            /// 
            public static void FromRGB(RGB rgb, YCbCr ycbcr)
            {
                float r = (float)rgb.Red / 255;
                float g = (float)rgb.Green / 255;
                float b = (float)rgb.Blue / 255;

                ycbcr.Y = (float)(0.2989 * r + 0.5866 * g + 0.1145 * b);
                ycbcr.Cb = (float)(-0.1687 * r - 0.3313 * g + 0.5000 * b);
                ycbcr.Cr = (float)(0.5000 * r - 0.4184 * g - 0.0816 * b);
            }

            /// <summary>
            /// Convert from RGB to YCbCr color space (Rec 601-1 specification).
            /// </summary>
            /// 
            /// <param name="rgb">Source color in <b>RGB</b> color space.</param>
            /// 
            /// <returns>Returns <see cref="YCbCr"/> instance, which represents converted color value.</returns>
            /// 
            public static YCbCr FromRGB(RGB rgb)
            {
                YCbCr ycbcr = new YCbCr();
                FromRGB(rgb, ycbcr);
                return ycbcr;
            }

            /// <summary>
            /// Convert from YCbCr to RGB color space.
            /// </summary>
            /// 
            /// <param name="ycbcr">Source color in <b>YCbCr</b> color space.</param>
            /// <param name="rgb">Destination color in <b>RGB</b> color spacs.</param>
            /// 
            public static void ToRGB(YCbCr ycbcr, RGB rgb)
            {
                // don't warry about zeros. compiler will remove them
                float r = System.Math.Max(0.0f, System.Math.Min(1.0f, (float)(ycbcr.Y + 0.0000 * ycbcr.Cb + 1.4022 * ycbcr.Cr)));
                float g = System.Math.Max(0.0f, System.Math.Min(1.0f, (float)(ycbcr.Y - 0.3456 * ycbcr.Cb - 0.7145 * ycbcr.Cr)));
                float b = System.Math.Max(0.0f, System.Math.Min(1.0f, (float)(ycbcr.Y + 1.7710 * ycbcr.Cb + 0.0000 * ycbcr.Cr)));

                rgb.Red = (byte)(r * 255);
                rgb.Green = (byte)(g * 255);
                rgb.Blue = (byte)(b * 255);
                rgb.Alpha = 255;
            }

            /// <summary>
            /// Convert the color to <b>RGB</b> color space.
            /// </summary>
            /// 
            /// <returns>Returns <see cref="RGB"/> instance, which represents converted color value.</returns>
            /// 
            public RGB ToRGB()
            {
                RGB rgb = new RGB();
                ToRGB(this, rgb);
                return rgb;
            }
        }

        bool smoothen = true;
        public bool Smoothen
        {
            get => smoothen;
            set => this.Change(ref smoothen, value);
        }

        PointCollection redPoints = null;
        public PointCollection RedPoints
        {
            get => redPoints;
            set => this.Change(ref redPoints, value, () => RedPoints);
        }

        PointCollection greenPoints = null;
        public PointCollection GreenPoints
        {
            get => greenPoints;
            set => this.Change(ref greenPoints, value, () => GreenPoints);
        }

        PointCollection bluePoints = null;
        public PointCollection BluePoints
        {
            get => bluePoints;
            set => this.Change(ref bluePoints, value, () => BluePoints);
        }

        PointCollection saturationPoints = null;
        public PointCollection SaturationPoints
        {
            get => saturationPoints;
            set => this.Change(ref saturationPoints, value, () => SaturationPoints);
        }

        PointCollection luminancePoints = null;
        public PointCollection LuminancePoints
        {
            get => luminancePoints;
            set => this.Change(ref luminancePoints, value, () => LuminancePoints);
        }

        public Histogram() { }

        static int[] s(int[] OriginalValues)
        {
            var result = new int[OriginalValues.Length];
            var mask = new double[]
            {
                0.25, 0.5, 0.25
            };

            for (var i = 1; i < OriginalValues.Length - 1; i++)
            {
                var value = 0.0;

                for (int j = 0; j < mask.Length; j++)
                    value += OriginalValues[i - 1 + j] * mask[j];

                result[i] = (int)value;
            }
            return result;
        }

        IEnumerable<Point> GetPoints(int[] values)
        {
            values = Smoothen ? s(values) : values;

            var max = values.Max();
            var result = new PointCollection();

            //First point (lower-left corner)
            yield return new Point(0, max);

            //Middle points
            for (int i = 0; i < values.Length; i++)
                yield return new Point(i, max - values[i]);

            //Last point (lower-right corner)
            yield return new System.Windows.Point(values.Length - 1, max);
        }

        public async Task Read(ColorMatrix colors)
        {
            PointCollection rr = new PointCollection(), rg = new PointCollection(), rb = new PointCollection(), rs = new PointCollection(), rl = new PointCollection();
            IEnumerable<Point> pr = null, pg = null, pb = null, ps = null, pl = null;

            await Task.Run(() =>
            {
                int[] r = new int[256];
                int[] g = new int[256];
                int[] b = new int[256];
                int[] s = new int[256];
                int[] l = new int[256];

                RGB rgb = new RGB();
                HSL hsl = new HSL();

                colors.Each(color =>
                {
                    r[color.R]++;
                    g[color.G]++;
                    b[color.B]++;

                    rgb.Red = color.R;
                    rgb.Green = color.G;
                    rgb.Blue = color.B;

                    // convert to HSL color space
                    HSL.FromRGB(rgb, hsl);

                    s[(int)(hsl.Saturation * 255)]++;
                    l[(int)(hsl.Luminance * 255)]++;
                    return color;
                });

                pr = GetPoints(r);
                pg = GetPoints(g);
                pb = GetPoints(b);
                ps = GetPoints(s);
                pl = GetPoints(l);
            });

            pr.ForEach(i => rr.Add(i));
            pg.ForEach(i => rg.Add(i));
            pb.ForEach(i => rb.Add(i));
            ps.ForEach(i => rs.Add(i));
            pl.ForEach(i => rl.Add(i));

            RedPoints
                = rr;
            GreenPoints 
                = rg;
            BluePoints 
                = rb;
            SaturationPoints 
                = rs;
            LuminancePoints 
                = rl;
        }
    }
}