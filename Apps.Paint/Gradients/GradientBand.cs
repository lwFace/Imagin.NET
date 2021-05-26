using Imagin.Common.Media;
using System;
using System.Windows.Media;

namespace Paint
{
    /// <summary>
    /// Analogous to <see cref="GradientStop"/>, but is immutable and supports serialization!
    /// </summary>
    [Serializable]
    public class GradientBand
    {
        public readonly StringColor Color;

        public readonly double Offset;

        public GradientBand(GradientStop gradientStop) : this(new StringColor(gradientStop.Color), gradientStop.Offset) { }

        public GradientBand(StringColor color, double offset)
        {
            Color = color;
            Offset = offset;
        }
    }
}