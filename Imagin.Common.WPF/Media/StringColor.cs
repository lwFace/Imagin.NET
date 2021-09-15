using Imagin.Common.Linq;
using System;
using System.Windows.Media;

namespace Imagin.Common.Media
{
    [Serializable]
    public class StringColor : Base
    {
        string i = $"255,0,0,0";
        public SolidColorBrush Brush
        {
            get => new SolidColorBrush(Value);
            set => Value = value.Color;
        }

        public Color Value
        {
            get => Convert(this);
            set
            {
                this.Change(ref i, Convert(value));
                this.Changed(() => Brush);
            }
        }

        public byte A => Value.A;

        public byte R => Value.R;

        public byte G => Value.G;

        public byte B => Value.B;

        public Argb Argb
        {
            get
            {
                var result = Value;
                return new Argb(result.A, result.R, result.G, result.B);
            }
        }

        public static string Convert(Color color) => Convert(color.A, color.R, color.G, color.B);

        public static string Convert(byte a, byte r, byte g, byte b) => $"{a},{r},{g},{b}";

        public static Color Convert(string input)
        {
            var result = input.Split(',');
            return Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte());
        }

        public static Color Convert(StringColor input) => Convert(input.i);

        public StringColor(Color color) => i = Convert(color);

        public StringColor(byte a, byte r, byte g, byte b) : this(Color.FromArgb(a, r, g, b)) { }

        public static implicit operator Color(StringColor right) => Convert(right);

        public static implicit operator StringColor(Color right) => new StringColor(right);

        public override string ToString() => i;
    }
}