using Imagin.Common.Media;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public static class WriteableBitmapExtensions
    {
        public static Bitmap Bitmap(this WriteableBitmap bitmap)
        {
            var result = default(Bitmap);
            using (var memoryStream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(memoryStream);
                result = new Bitmap(memoryStream);
            }
            return result;
        }

        public static WriteableBitmap Clone(this WriteableBitmap bitmap)
        {
            var result = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight, bitmap.DpiX, bitmap.DpiY, bitmap.Format, null);
            var colors = bitmap.Colors();
            result.ForEach((x, y, color) => colors.GetValue(y.UInt32(), x.UInt32()));
            return result;
        }

        public static ColorMatrix Colors(this WriteableBitmap bitmap)
        {
            var colors = new ColorMatrix(bitmap.PixelHeight.UInt32(), bitmap.PixelWidth.UInt32());
            bitmap.ForEach((x, y, color) =>
            {
                colors.SetValue(y.UInt32(), x.UInt32(), color);
                return color;
            });
            return colors;
        }

        public static WriteableBitmap Gradient(this WriteableBitmap bitmap, LinearGradientMode mode, Color color1, Color color2)
        {
            return bitmap.Gradient(mode, color1, color2, new Rectangle(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
        }

        public static WriteableBitmap Gradient(this WriteableBitmap bitmap, LinearGradientMode mode, Color color1, Color color2, System.Drawing.Rectangle region)
        {
            var result = bitmap.Bitmap();

            using (var g = System.Drawing.Graphics.FromImage(result))
                g.FillRectangle(new LinearGradientBrush(region, color1, color2, mode), region);

            return result.WriteableBitmap();
        }

        public static WriteableBitmap Gradient(this WriteableBitmap bitmap, System.Windows.Media.LinearGradientBrush brush)
        {
            return default(WriteableBitmap);
        }

        public static WriteableBitmap Gradient(this WriteableBitmap bitmap, System.Windows.Media.RadialGradientBrush gradient)
        {
            return default(WriteableBitmap);
        }

        public static void ForEach(this WriteableBitmap bitmap, Func<Pixel, Color> action)
        {
            bitmap.ForEach(-1, 0, bitmap.PixelWidth, 0, bitmap.PixelHeight, i => i + 1, i => i, action);
        }

        public static void ForEach
        #region <params>
        (
            this WriteableBitmap bitmap, 
            int start, 
            int xstart,
            int xend, 
            int ystart, 
            int yend, 
            Func<int, int> xincrement,
            Func<int, int> yincrement,
            Func<Pixel, Color> action, 
            Action<int> xpre = default,
            Action<int> xpost = default, 
            Action<int> ypre = default, 
            Action<int> ypost = default
        )
        #endregion
        #region <body>
        {
            unsafe
            {
                bitmap.Lock();
                var CurrentPixel = start;
                byte* Start = (byte*)(void*)bitmap.BackBuffer;

                for (var row = ystart; row < yend; row++)
                {
                    ypre?.Invoke(row);
                    for (var column = xstart; column < xend; column++)
                    {
                        xpre?.Invoke(column);
                        CurrentPixel = xincrement(CurrentPixel);
                        var color = default(Color);

                        if (bitmap.Format == System.Windows.Media.PixelFormats.Bgr24)
                        {
                            color = Color.FromArgb
                            (
                                255,
                                *(Start + CurrentPixel * 3 + 2),
                                *(Start + CurrentPixel * 3 + 1),
                                *(Start + CurrentPixel * 3 + 0)
                            );
                        }
                        else if (bitmap.Format == System.Windows.Media.PixelFormats.Bgra32)
                        {
                            color = Color.FromArgb
                            (
                                *(Start + CurrentPixel * 4 + 3),
                                *(Start + CurrentPixel * 4 + 2),
                                *(Start + CurrentPixel * 4 + 1),
                                *(Start + CurrentPixel * 4 + 0)
                            );
                        }

                        if (action != null)
                        {
                            var ncolor = action(new Pixel(color, column, row));

                            if (ncolor != color)
                            {
                                if (bitmap.Format == System.Windows.Media.PixelFormats.Bgr24)
                                {
                                    *(Start + CurrentPixel * 3 + 0) = ncolor.B;
                                    *(Start + CurrentPixel * 3 + 1) = ncolor.G;
                                    *(Start + CurrentPixel * 3 + 2) = ncolor.R;
                                }
                                else if (bitmap.Format == System.Windows.Media.PixelFormats.Bgra32)
                                {
                                    *(Start + CurrentPixel * 4 + 0) = ncolor.B;
                                    *(Start + CurrentPixel * 4 + 1) = ncolor.G;
                                    *(Start + CurrentPixel * 4 + 2) = ncolor.R;
                                    *(Start + CurrentPixel * 4 + 3) = ncolor.A;
                                }
                            }
                        }
                        xpost?.Invoke(column);
                    }
                    CurrentPixel = yincrement(CurrentPixel);
                    ypost?.Invoke(row);
                }

                bitmap.AddDirtyRect(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                bitmap.Unlock();
            }
        }
        #endregion

        public static WriteableBitmap PixelFormat(this WriteableBitmap bitmap, PixelFormat format)
        {
            var source = bitmap.Bitmap();
            var result = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            using (var g = Graphics.FromImage(result))
                g.DrawImage(source, new Rectangle(0, 0, result.Width, result.Height));

            return result.WriteableBitmap();
        }

        public static void ReplaceColors(this WriteableBitmap bitmap, Color color1, Color color2)
        {
            bitmap.ForEach(pixel => pixel.Color == color1 ? color2 : pixel.Color);
        }

        public static WriteableBitmap Resize(this WriteableBitmap bitmap, double scale)
        {
            var s = new System.Windows.Media.ScaleTransform(scale, scale);

            var result = new TransformedBitmap(bitmap, s);

            WriteableBitmap a(BitmapSource b)
            {
                // Calculate stride of source
                int stride = b.PixelWidth * (b.Format.BitsPerPixel / 8);

                // Create data array to hold source pixel data
                byte[] data = new byte[stride * b.PixelHeight];

                // Copy source image pixels to the data array
                b.CopyPixels(data, stride, 0);

                // Create WriteableBitmap to copy the pixel data to.      
                WriteableBitmap target = new WriteableBitmap(b.PixelWidth, b.PixelHeight, b.DpiX, b.DpiY, b.Format, null);

                // Write the pixel data to the WriteableBitmap.
                target.WritePixels(new Int32Rect(0, 0, b.PixelWidth, b.PixelHeight), data, stride, 0);

                return target;
            }

            return a(result);
        }

        public static void SwapColors(this WriteableBitmap bitmap, ComponentSwap type)
        {
            bitmap.ForEach(pixel => pixel.Color.Double().Swap(type).Int32());
        }
    }
}