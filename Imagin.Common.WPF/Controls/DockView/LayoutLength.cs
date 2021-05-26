using System;
using System.Windows;
using System.Xml.Serialization;
using Imagin.Common.Linq;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class LayoutLength
    {
        public static LayoutLength Auto = new LayoutLength(1, LayoutUnitType.Auto);

        public static LayoutLength Default = Star;

        public static LayoutLength Star = new LayoutLength(1, LayoutUnitType.Star);

        public static LayoutLength Zero = new LayoutLength(0, LayoutUnitType.Pixel);

        [XmlAttribute]
        public LayoutUnitType Unit { get; set; } = LayoutUnitType.Star;

        [XmlAttribute]
        public double Value { get; set; } = 1;

        public LayoutLength() { }

        public LayoutLength(double value, LayoutUnitType unit)
        {
            Value = value;
            Unit = unit;
        }

        public static implicit operator GridLength(LayoutLength input) => new GridLength(input.Value, (GridUnitType)Enum.Parse(typeof(GridUnitType), input.Unit.ToString()));

        public static implicit operator LayoutLength(GridLength input) => new LayoutLength(input.Value, (LayoutUnitType)Enum.Parse(typeof(LayoutUnitType), input.GridUnitType.ToString()));

        public static implicit operator LayoutLength(string input)
        {
            if (input.ToLower() == "auto")
                return Auto;

            if (input.Numeric())
            {
                double.TryParse(input, out double result);
                return new LayoutLength(result, LayoutUnitType.Pixel);
            }

            if (input == "*")
                return Star;

            if (input.EndsWith("*"))
            {
                var number = new char[input.Length - 1];
                for (var i = 0; i < input.Length - 1; i++)
                    number[i] = input[i];

                double.TryParse(new string(number), out double result);
                return new LayoutLength(result, LayoutUnitType.Star);
            }

            return Default;
        }

        public static implicit operator string(LayoutLength input)
        {
            switch (input.Unit)
            {
                case LayoutUnitType.Auto:
                    return "Auto";
                case LayoutUnitType.Pixel:
                    return $"{input.Value}";
                case LayoutUnitType.Star:
                    return $"{input.Value}*";
            }
            throw new InvalidOperationException();
        }
    }
}