using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(long), typeof(double))]
    public class BytesToMegaBytesConverter : Converter<long, double>
    {
        protected override bool AllowNull => true;

        protected override Value<Double> ConvertTo(ConverterData<Int64> input)
        {
            if (input.Value == null)
                return 0;

            double.TryParse(input.ActualValue.ToString(), out double result);
            return (result / 1024d / 1024d).Round(3);
        }

        protected override Value<Int64> ConvertBack(ConverterData<Double> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Double), typeof(Double))]
    public class PercentConverter : Converter<Double, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<Double> input) => input.ActualValue * 100.0;

        protected override Value<Double> ConvertBack(ConverterData<Double> input) => input.ActualValue / 100.0;
    }

    //.......................................................................

    [ValueConversion(typeof(Byte), typeof(Double))]
    public class ByteToDoubleConverter : Converter<Byte, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<Byte> input) => input.ActualValue.Double() / Byte.MaxValue.Double();

        protected override Value<Byte> ConvertBack(ConverterData<Double> input) => (input.ActualValue * Byte.MaxValue.Double()).Byte();
    }

    [ValueConversion(typeof(Decimal), typeof(Double))]
    public class DecimalToDoubleConverter : Converter<Decimal, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<Decimal> input) => input.ActualValue.Double();

        protected override Value<Decimal> ConvertBack(ConverterData<Double> input) => input.ActualValue.Decimal();
    }

    [ValueConversion(typeof(Int16), typeof(Double))]
    public class Int16ToDoubleConverter : Converter<Int16, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<Int16> input) => input.ActualValue.Double();

        protected override Value<Int16> ConvertBack(ConverterData<Double> input) => input.ActualValue.Int16();
    }

    [ValueConversion(typeof(Int32), typeof(Double))]
    public class Int32ToDoubleConverter : Converter<Int32, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<Int32> input) => input.ActualValue.Double();

        protected override Value<Int32> ConvertBack(ConverterData<Double> input) => input.ActualValue.Int32();
    }

    [ValueConversion(typeof(Int64), typeof(Double))]
    public class Int64ToDoubleConverter : Converter<Int64, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<Int64> input) => input.ActualValue.Double();

        protected override Value<Int64> ConvertBack(ConverterData<Double> input) => input.ActualValue.Int64();
    }

    [ValueConversion(typeof(Object), typeof(Double))]
    public class ObjectToDoubleConverter : Converter<Object, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<Object> input) => input.ActualValue.Double();

        protected override Value<Object> ConvertBack(ConverterData<Double> input) => input.ActualValue;
    }

    [ValueConversion(typeof(Single), typeof(Double))]
    public class SingleToDoubleConverter : Converter<Single, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<Single> input) => input.ActualValue.Double();

        protected override Value<Single> ConvertBack(ConverterData<Double> input) => input.ActualValue.Single();
    }

    [ValueConversion(typeof(UDouble), typeof(Double))]
    public class UDoubleToDoubleConverter : Converter<UDouble, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<UDouble> input) => (Double)input.ActualValue;

        protected override Value<UDouble> ConvertBack(ConverterData<Double> input) => (UDouble)input.ActualValue;
    }

    [ValueConversion(typeof(UInt16), typeof(Double))]
    public class UInt16ToDoubleConverter : Converter<UInt16, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<UInt16> input) => input.ActualValue.Double();

        protected override Value<UInt16> ConvertBack(ConverterData<Double> input) => input.ActualValue.UInt16();
    }

    [ValueConversion(typeof(UInt32), typeof(Double))]
    public class UInt32ToDoubleConverter : Converter<UInt32, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<UInt32> input) => input.ActualValue.Double();

        protected override Value<UInt32> ConvertBack(ConverterData<Double> input) => input.ActualValue.UInt32();
    }

    [ValueConversion(typeof(UInt64), typeof(Double))]
    public class UInt64ToDoubleConverter : Converter<UInt64, Double>
    {
        protected override Value<Double> ConvertTo(ConverterData<UInt64> input) => input.ActualValue.Double();

        protected override Value<UInt64> ConvertBack(ConverterData<Double> input) => input.ActualValue.UInt64();
    }
}