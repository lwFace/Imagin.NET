using Imagin.Common;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Paint
{
    public class TransformTool : Tool
    {
        public override bool Hidden => true;

        public override string Icon => Resources.Uri(nameof(Paint), "Images/Transform.png").OriginalString;

        static Point ShearXY(Point source, double shearX, double shearY, int offsetX, int offsetY)
        {
            Point result = new Point();


            result.X = (int)(Math.Round(source.X + shearX * source.Y));
            result.X -= offsetX;


            result.Y = (int)(Math.Round(source.Y + shearY * source.X));
            result.Y -= offsetY;

            return result;
        }

        public static Bitmap Shear(Bitmap sourceBitmap, double shearX, double shearY)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            int xOffset = (int)Math.Round(sourceBitmap.Width * shearX / 2.0);
            int yOffset = (int)Math.Round(sourceBitmap.Height * shearY / 2.0);

            int sourceXY = 0;
            int resultXY = 0;

            Point sourcePoint = new Point();
            Point resultPoint = new Point();

            Rectangle imageBounds = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);
            for (int row = 0; row < sourceBitmap.Height; row++)
            {
                for (int col = 0; col < sourceBitmap.Width; col++)
                {
                    sourceXY = row * sourceData.Stride + col * 4;

                    sourcePoint.X = col;
                    sourcePoint.Y = row;

                    if (sourceXY >= 0 && sourceXY + 3 < pixelBuffer.Length)
                    {
                        resultPoint = ShearXY(sourcePoint, shearX, shearY, xOffset, yOffset);

                        resultXY = resultPoint.Y * sourceData.Stride + resultPoint.X * 4;
                        if (imageBounds.Contains(resultPoint) && resultXY >= 0)
                        {
                            if (resultXY + 6 <= resultBuffer.Length)
                            {
                                resultBuffer[resultXY + 4] =
                                    pixelBuffer[sourceXY];

                                resultBuffer[resultXY + 5] =
                                    pixelBuffer[sourceXY + 1];

                                resultBuffer[resultXY + 6] =
                                    pixelBuffer[sourceXY + 2];

                                resultBuffer[resultXY + 7] =
                                    pixelBuffer[sourceXY + 3];
                            }
                            if (resultXY - 3 >= 0)
                            {
                                resultBuffer[resultXY - 4] =
                                    pixelBuffer[sourceXY];

                                resultBuffer[resultXY - 3] =
                                    pixelBuffer[sourceXY + 1];

                                resultBuffer[resultXY - 2] =
                                    pixelBuffer[sourceXY + 2];

                                resultBuffer[resultXY - 1] =
                                    pixelBuffer[sourceXY + 3];
                            }
                            if (resultXY + 3 < resultBuffer.Length)
                            {
                                resultBuffer[resultXY] =
                                    pixelBuffer[sourceXY];

                                resultBuffer[resultXY + 1] =
                                    pixelBuffer[sourceXY + 1];

                                resultBuffer[resultXY + 2] =
                                    pixelBuffer[sourceXY + 2];

                                resultBuffer[resultXY + 3] =
                                    pixelBuffer[sourceXY + 3];
                            }
                        }
                    }
                }
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);

            resultBitmap.UnlockBits(resultData);
            return resultBitmap;
        }
    }
}