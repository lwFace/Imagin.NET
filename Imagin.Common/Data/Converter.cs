using System;

namespace Imagin.Common.Data
{
    public class Converter<T>
    {
        public readonly Func<T, T> ConvertFrom;

        public readonly Func<T, T> ConvertTo;

        public Converter(Func<T, T> convertFrom, Func<T, T> convertTo)
        {
            ConvertFrom = convertFrom;
            ConvertTo = convertTo;
        }
    }

    public class DoubleConverter : Converter<double>
    {
        public DoubleConverter(Func<double, double> convertFrom, Func<double, double> convertTo) : base(convertFrom, convertTo) { }
    }
}