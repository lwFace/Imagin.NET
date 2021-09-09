namespace Paint
{ 
    /*
    using Imagin.Common;
    using Imagin.Common.Input;
    using Imagin.Common.Linq;
    using Imagin.Common.Media;
    using Imagin.Common.Numerics;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Class to implement the vignette effect. 
    /// </summary>
    public class Effect
    {    

        List<byte> pixRedOrig;       // List of red pixels in original image.
        List<byte> pixGreenOrig;     // List of green pixels in original image.
        List<byte> pixBlueOrig;      // List of blue pixels in original image.

        /// --------------------------------------------------------------------------

        List<byte> pixRedModified;   // List of red pixels in modified image.
        List<byte> pixGreenModified; // List of green pixels in modified image.
        List<byte> pixBlueModified;  // List of blue pixels in modified image.

        /// --------------------------------------------------------------------------

        List<double> aVals;          // Major axis value of the vignette shape.
        List<double> bVals;          // Minor axis value of the vignette shape.

        /// --------------------------------------------------------------------------

        List<double> aValsMidPoints; // Major axis of mid-figures of the vignette shape.
        List<double> bValsMidPoints; // Minor axis of mid-figures of the vignette shape.

        /// --------------------------------------------------------------------------

        List<double> weight1;        // Weights for the original image.
        List<double> weight2;        // Weights for the border colour.

        /// --------------------------------------------------------------------------

        int dpi;                     // DPI for the image for saving.

        /// --------------------------------------------------------------------------

        int width;                   // Width of image.
        int height;                  // Height of image.

        /// --------------------------------------------------------------------------

        double geometryFactor;       // See note below, in the constructor.

        /// --------------------------------------------------------------------------

        /// <summary>
        /// Orientation of the Ellipse, Diamond, Square or Rectangle in degrees. 
        /// This parameter is not of relevance for the Circle vignette.
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Coverage of the vignette in percentage of the image dimension (width or height).
        /// </summary>
        public double Coverage { get; set; }

        /// <summary>
        /// Width of the "band" between the inner "original image" region and the outer
        /// "border" region. This width is measured in pixels.
        /// </summary>
        public int Bands { get; set; }

        /// <summary>
        /// Number of steps of "gradation" to be accommodated within the above parameter BandPixels.
        /// This is just a number, and has no units.
        /// </summary>
        public int NumberSteps { get; set; }

        /// <summary>
        /// X Offset of the centre of rotation in terms of percentage with respect to half the 
        /// width of the image.
        /// </summary>
        public int CenterX { get; set; }

        /// <summary>
        /// Y Offset of the centre of rotation in terms of percentage with respect to half the 
        /// height of the image.
        /// </summary>
        public int CenterY { get; set; }

        /// <summary>
        /// Border Colour of the vignette. We consider only R, G, B values here. Alpha value is ignored.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Shape of the Vignette - one of Circle, Ellipse, Diamond, Rectangle or Square.
        /// </summary>
        public Shapes Shape { get; set; }

        /// --------------------------------------------------------------------------

        Effect()
        {
            pixRedOrig = new List<byte>();
            pixGreenOrig = new List<byte>();
            pixBlueOrig = new List<byte>();
            pixRedModified = new List<byte>();
            pixGreenModified = new List<byte>();
            pixBlueModified = new List<byte>();

            aVals = new List<double>();
            bVals = new List<double>();
            aValsMidPoints = new List<double>();
            bValsMidPoints = new List<double>();
            weight1 = new List<double>();
            weight2 = new List<double>();

            // Regarding the variable "geometryFactor": 
            // - This is not a magic number. This is the factor by which the x-Centre and 
            //   y-Centre slider values are to be multiplied so as to cause the following:
            //   When the x-Centre slider is at its midpoint, the vignette should be centred 
            //   at the midpoint of the image. When the x-Centre slider is at its extreme right
            //   point, the vignette centre should be along the right edge of the image. When the
            //   x-Centre slider is at its extreme left point, the vignette centre should be 
            //   along the left edge of the image. 
            //   Similarly with the y-Centre slider.
            //   So, the factor of multiplication is (Half-of the width)/(Maximum value of slider)
            //    = 0.5 / 100.0
            geometryFactor = 0.5 / 100.0;

            dpi = 72; // I guess this suffices
        }

        /// --------------------------------------------------------------------------

        public static Shapes From(GradientBrush gradientBrush)
        {
            if (gradientBrush is RadialGradientBrush)
                return Shapes.Ellipse;

            if (gradientBrush is AngleGradientBrush)
            {
            }

            if (gradientBrush is ReflectedGradientBrush)
            {
            }

            if (gradientBrush is DiamondGradientBrush)
                return Shapes.Diamond;

            throw new NotSupportedException();
        }

        /// --------------------------------------------------------------------------

        public static Matrix<Color> Render(WriteableBitmap oldBitmap, Shapes shape, Int32Size size, int angle, int coverage, int bands, int steps, Vector2<int> origin, Color borderColor)
        {
            var effect = new Effect();
            effect.Angle = 0; //angle;
            effect.Coverage = coverage;
            effect.Bands = bands;
            effect.NumberSteps = steps;
            effect.CenterX = origin.X;
            effect.CenterY = origin.Y;
            effect.BorderColor = borderColor;
            effect.Shape = shape;

            var rOriginal = new List<byte>();
            var gOriginal = new List<byte>();
            var bOriginal = new List<byte>();

            var rModified = new List<byte>();
            var gModified = new List<byte>();
            var bModified = new List<byte>();

            int stride = (size.Width * 32 + 7) / 8;
            var pixels = new byte[stride * size.Height];
            oldBitmap.CopyPixels(Int32Rect.Empty, pixels, stride, 0);

            Console.WriteLine($"The size is height = {size.Height}, width = {size.Width}");
            Console.WriteLine($"Colors: ");
            for (int i = 0; i < pixels.Count(); i += 4)
            {
                // In a 32-bit per pixel image, the bytes are stored in the order of BGRA
                var b 
                    = pixels[i];
                var g 
                    = pixels[i + 1];
                var r
                    = pixels[i + 2];
                var a
                    = pixels[i + 3];

                rOriginal.Add(r);
                gOriginal.Add(g);
                bOriginal.Add(b);

                rModified.Add(r);
                gModified.Add(g);
                bModified.Add(b);
                Console.Write($"({r}, {g}, {b}), ");
            }

            return effect.Render(size, ref rOriginal, ref gOriginal, ref bOriginal, ref rModified, ref gModified, ref bModified);
        }

        /// --------------------------------------------------------------------------

        public Matrix<Color> Render(ref List<byte> pixels8RedScaledModified, ref List<byte> pixels8GreenScaledModified, ref List<byte> pixels8BlueScaledModified)
        {
            Console.WriteLine("Render(ref List<byte> pixels8RedScaledModified, ref List<byte> pixels8GreenScaledModified, ref List<byte> pixels8BlueScaledModified)");
            var result = new Matrix<Color>(height.UInt32(), width.UInt32());

            int bitsPerPixel = 24;
            int stride = (width * bitsPerPixel + 7) / 8;
            byte[] pixelsToWrite = new byte[stride * height];
            int i1;

            uint x = 0, y = 0;
            for (int i = 0; i < pixelsToWrite.Count(); i += 3)
            {
                i1 = i / 3;

                var color = Color.FromArgb(255, pixels8RedScaledModified[i1], pixels8GreenScaledModified[i1], pixels8BlueScaledModified[i1]);
                pixelsToWrite[i] = color.R;
                pixelsToWrite[i + 1] = color.G;
                pixelsToWrite[i + 2] = color.B;
                result.SetValue(y, x, color);

                if (x == width - 1)
                {
                    x = 0;
                    y++;
                }
            }

            return result;
        }

        /// <summary>
        /// Method to apply the vignette.
        /// </summary>
        public Matrix<Color> Render
        (
            Int32Size size, 
            ref List<byte> redOrig, 
            ref List<byte> greenOrig, 
            ref List<byte> blueOrig,
            ref List<byte> redModified, 
            ref List<byte> greenModified, 
            ref List<byte> blueModified
        )
        {
            Console.WriteLine("Render(Int32Size size, ref List<byte> redOrig, ref List<byte> greenOrig, ref List<byte> blueOrig, ref List<byte> redModified, ref List<byte> greenModified, ref List<byte> blueModified)");
            pixRedOrig = redOrig;
            pixGreenOrig = greenOrig;
            pixBlueOrig = blueOrig;

            pixRedModified = redModified;
            pixGreenModified = greenModified;
            pixBlueModified = blueModified;

            height = size.Height;
            width = size.Width;

            SetupParameters();
            if (Shape == Shapes.Circle || Shape == Shapes.Ellipse || Shape == Shapes.Diamond)
            {
                ApplyEffectCircleEllipseDiamond();
            }
            //if (Shape == VignetteShape.Rectangle || Shape == VignetteShape.Square)
            else ApplyEffectRectangleSquare();
            return Render(ref pixRedModified, ref pixGreenModified, ref pixBlueModified);
        }

        /// --------------------------------------------------------------------------

        /// <summary>
        /// Set up the different parameters.
        /// </summary>
        private void SetupParameters()
        {
            Console.WriteLine("SetupParameters");

            aVals.Clear();
            bVals.Clear();

            aValsMidPoints.Clear();
            bValsMidPoints.Clear();

            weight1.Clear();
            weight2.Clear();

            double a0, b0, aLast, bLast, aEll, bEll;
            double stepSize = Bands * 1.0 / NumberSteps;
            double bandPixelsBy2 = 0.5 * Bands;
            double arguFactor = Math.PI / Bands;

            double vignetteWidth = width * Coverage / 100.0;
            double vignetteHeight = height * Coverage / 100.0;

            double vwb2 = vignetteWidth * 0.5;
            double vhb2 = vignetteHeight * 0.5;

            a0 = vwb2 - bandPixelsBy2;
            b0 = vhb2 - bandPixelsBy2;

            // For a circle or square, both 'major' and 'minor' axes are identical
            if (Shape == Shapes.Circle || Shape == Shapes.Square)
            {
                a0 = Math.Min(a0, b0);
                b0 = a0;
            }

            if (Shape == Shapes.Circle || Shape == Shapes.Ellipse ||
                Shape == Shapes.Rectangle || Shape == Shapes.Square)
            {
                for (int i = 0; i <= NumberSteps; ++i)
                {
                    aEll = a0 + stepSize * i;
                    bEll = b0 + stepSize * i;
                    aVals.Add(aEll);
                    bVals.Add(bEll);
                }
                for (int i = 0; i < NumberSteps; ++i)
                {
                    aEll = a0 + stepSize * (i + 0.5);
                    bEll = b0 + stepSize * (i + 0.5);
                    aValsMidPoints.Add(aEll);
                    bValsMidPoints.Add(bEll);
                }
            }
            else// if (Shape == VignetteShape.Diamond)
            {
                aLast = vwb2 + bandPixelsBy2;
                bLast = b0 * aLast / a0;
                double stepXdiamond = (aLast - a0) / NumberSteps;
                double stepYdiamond = (bLast - b0) / NumberSteps;

                for (int i = 0; i <= NumberSteps; ++i)
                {
                    aEll = a0 + stepXdiamond * i;
                    bEll = b0 + stepYdiamond * i;
                    aVals.Add(aEll);
                    bVals.Add(bEll);
                }
                for (int i = 0; i <= NumberSteps; ++i)
                {
                    aEll = a0 + stepXdiamond * (i + 0.5);
                    bEll = b0 + stepYdiamond * (i + 0.5);
                    aValsMidPoints.Add(aEll);
                    bValsMidPoints.Add(bEll);
                }
            }

            // The weight functions given below form the crux of the code. It was a struggle after which 
            // I got these weighting functions. 
            // Initially, I tried linear interpolation, and the effect was not so pleasing. The 
            // linear interpolation function is C0-continuous at the boundary, and therefore shows 
            // a distinct border.
            // Later, upon searching, I found a paper by Burt and Adelson on Mosaics. Though I did 
            // not use the formulas given there, one of the initial figures in that paper set me thinking
            // on using the cosine function. This function is C1-continuous at the boundary, and therefore
            // the effect is pleasing on the eye. Yields quite a nice blending effect. The cosine 
            // functions are incorporated into the wei1 and wei2 definitions below.
            //
            // Reference: Burt and Adelson [Peter J Burt and Edward H Adelson, A Multiresolution Spline
            //  With Application to Image Mosaics, ACM Transactions on Graphics, Vol 2. No. 4,
            //  October 1983, Pages 217-236].
            double wei1, wei2, arg, argCosVal;
            for (int i = 0; i < NumberSteps; ++i)
            {
                arg = arguFactor * (aValsMidPoints[i] - a0);
                argCosVal = Math.Cos(arg);
                wei1 = 0.5 * (1.0 + argCosVal);
                wei2 = 0.5 * (1.0 - argCosVal);
                weight1.Add(wei1);
                weight2.Add(wei2);
            }
        }

        /// <summary>
        /// Method to apply the Circular, Elliptical or Diamond-shaped vignette on an image.
        /// </summary>
        private void ApplyEffectCircleEllipseDiamond()
        {
            Console.WriteLine("ApplyEffectCircleEllipseDiamond...");

            int k, el, w1, w2;
            byte r, g, b;
            double wb2 = width * 0.5 + CenterX * width * geometryFactor;
            double hb2 = height * 0.5 + CenterY * height * geometryFactor;
            double thetaRadians = Angle * Math.PI / 180.0;
            double cos = Math.Cos(thetaRadians);
            double sin = Math.Sin(thetaRadians);
            double xprime, yprime, potential1, potential2, potential;
            double factor1, factor2, factor3, factor4;
            byte redBorder = BorderColor.R;
            byte greenBorder = BorderColor.G;
            byte blueBorder = BorderColor.B;

            // Loop over the number of pixels
            for (el = 0; el < height; ++el)
            {
                w2 = width * el;
                for (k = 0; k < width; ++k)
                {
                    // This is the usual rotation formula, along with translation.
                    // I could have perhaps used the Transform feature of WPF.
                    xprime = (k - wb2) * cos + (el - hb2) * sin;
                    yprime = -(k - wb2) * sin + (el - hb2) * cos;

                    factor1 = 1.0 * Math.Abs(xprime) / aVals[0];
                    factor2 = 1.0 * Math.Abs(yprime) / bVals[0];
                    factor3 = 1.0 * Math.Abs(xprime) / aVals[NumberSteps];
                    factor4 = 1.0 * Math.Abs(yprime) / bVals[NumberSteps];

                    if (Shape == Shapes.Circle || Shape == Shapes.Ellipse)
                    {
                        // Equations for the circle / ellipse. 
                        // "Potentials" are analogous to distances from the inner and outer boundaries
                        // of the two ellipses.
                        potential1 = factor1 * factor1 + factor2 * factor2 - 1.0;
                        potential2 = factor3 * factor3 + factor4 * factor4 - 1.0;
                    }
                    else //if (Shape == VignetteShape.Diamond)
                    {
                        // Equations for the diamond. 
                        potential1 = factor1 + factor2 - 1.0;
                        potential2 = factor3 + factor4 - 1.0;
                    }
                    w1 = w2 + k;

                    if (potential1 <= 0.0)
                    {
                        // Point is within the inner circle / ellipse / diamond
                        r = pixRedOrig[w1];
                        g = pixGreenOrig[w1];
                        b = pixBlueOrig[w1];
                    }
                    else if (potential2 >= 0.0)
                    {
                        // Point is outside the outer circle / ellipse / diamond
                        r = redBorder;
                        g = greenBorder;
                        b = blueBorder;
                    }
                    else
                    {
                        // Point is in between the outermost and innermost circles / ellipses / diamonds
                        int j, j1;

                        for (j = 1; j < NumberSteps; ++j)
                        {
                            factor1 = Math.Abs(xprime) / aVals[j];
                            factor2 = Math.Abs(yprime) / bVals[j];

                            if (Shape == Shapes.Circle ||
                                Shape == Shapes.Ellipse)
                            {
                                potential = factor1 * factor1 + factor2 * factor2 - 1.0;
                            }
                            else // if (Shape == VignetteShape.Diamond)
                            {
                                potential = factor1 + factor2 - 1.0;
                            }
                            if (potential < 0.0) break;
                        }
                        j1 = j - 1;
                        // The formulas where the weights are applied to the image, and border.
                        r = (byte)(pixRedOrig[w1] * weight1[j1] + redBorder * weight2[j1]);
                        g = (byte)(pixGreenOrig[w1] * weight1[j1] + greenBorder * weight2[j1]);
                        b = (byte)(pixBlueOrig[w1] * weight1[j1] + blueBorder * weight2[j1]);
                    }
                    pixRedModified[w1] = r;
                    pixGreenModified[w1] = g;
                    pixBlueModified[w1] = b;
                }
            }
        }

        /// <summary>
        /// Method to apply the Rectangular or Square-shaped vignette on an image.
        /// </summary>
        private void ApplyEffectRectangleSquare()
        {
            Console.WriteLine("ApplyEffectRectangleSquare...");

            Rect rect1 = new Rect(), rect2 = new Rect(), rect3 = new Rect();
            Point point = new Point();
            int k, el, w1, w2;
            byte r, g, b;

            double wb2 = width * 0.5 + CenterX * width * geometryFactor;
            double hb2 = height * 0.5 + CenterY * height * geometryFactor;
            double thetaRadians = Angle * Math.PI / 180.0;
            double cos = Math.Cos(thetaRadians);
            double sin = Math.Sin(thetaRadians);
            double xprime, yprime, potential;
            byte redBorder = BorderColor.R;
            byte greenBorder = BorderColor.G;
            byte blueBorder = BorderColor.B;

            rect1.X = 0.0;
            rect1.Y = 0.0;
            rect1.Width = aVals[0];
            rect1.Height = bVals[0];
            rect2.X = 0.0;
            rect2.Y = 0.0;
            rect2.Width = aVals[NumberSteps];
            rect2.Height = bVals[NumberSteps];

            for (el = 0; el < height; ++el)
            {
                w2 = width * el;
                for (k = 0; k < width; ++k)
                {
                    // The usual rotation-translation formula
                    xprime = (k - wb2) * cos + (el - hb2) * sin;
                    yprime = -(k - wb2) * sin + (el - hb2) * cos;

                    potential = 0.0;
                    point.X = Math.Abs(xprime);
                    point.Y = Math.Abs(yprime);

                    // For a rectangle, we can use the Rect.Contains(Point) method to determine
                    //  whether the point is in the rectangle or not
                    if (rect1.Contains(point))
                        potential = -2.0; // Arbitrary negative number N1

                    if (!rect2.Contains(point))
                        potential = 2.0; // Arbitrary positive number = - N1

                    w1 = w2 + k;

                    if (potential < -1.0) // Arbitrary negative number, greater than N1
                    {
                        // Point is within the inner square / rectangle,
                        r = pixRedOrig[w1];
                        g = pixGreenOrig[w1];
                        b = pixBlueOrig[w1];
                    }
                    else if (potential > 1.0) // Arbitrary positive number lesser than - N1
                    {
                        // Point is outside the outer square / rectangle
                        r = redBorder;
                        g = greenBorder;
                        b = blueBorder;
                    }
                    else
                    {
                        // Point is in between outermost and innermost squares / rectangles
                        int j, j1;

                        for (j = 1; j < NumberSteps; ++j)
                        {
                            rect3.X = 0.0;
                            rect3.Y = 0.0;
                            rect3.Width = aVals[j];
                            rect3.Height = bVals[j];

                            if (rect3.Contains(point))
                                break;
                        }
                        j1 = j - 1;
                        r = (byte)(pixRedOrig[w1] * weight1[j1] + redBorder * weight2[j1]);
                        g = (byte)(pixGreenOrig[w1] * weight1[j1] + greenBorder * weight2[j1]);
                        b = (byte)(pixBlueOrig[w1] * weight1[j1] + blueBorder * weight2[j1]);
                    }
                    pixRedModified[w1] = r;
                    pixGreenModified[w1] = g;
                    pixBlueModified[w1] = b;
                }
            }
        }
    }
    */

    /// --------------------------------------------------------------------------

    /*
    //50 to 120
    var coverage = 80;
    //3 to 200
    var bands = 40;
    //4 to 100
    var steps = 40;
    //-100 to 100
    var originX = 0;
    //-100 to 100
    var originY = 0;

    var shape = Effect.From(gradientBrush);
    Console.WriteLine($"Creating special gradient: {shape}");

    i = Effect.Render(Bitmap, shape, new Int32Size(Document.Height, Document.Width), Angle.Value.Int32(), coverage, bands, steps, new Vector2<int>(originX, originY), Colors.Black);
    */
}