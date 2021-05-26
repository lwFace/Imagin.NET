using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Visibility), typeof(Visibility))]
    public class InverseVisibilityConverter : Converter<Visibility, Visibility>
    {
        protected override Value<Visibility> ConvertTo(ConverterData<Visibility> input) => input.ActualValue.Invert();

        protected override Value<Visibility> ConvertBack(ConverterData<Visibility> input) => input.ActualValue.Invert();
    }

    //.......................................................................

    [ValueConversion(typeof(Alpha), typeof(Visibility))]
    public class AlphaToVisibilityConverter : Converter<Alpha, Visibility>
    {
        Alpha Invert(Alpha input) => input == Alpha.Without ? Alpha.With : Alpha.Without;

        protected override Value<Visibility> ConvertTo(ConverterData<Alpha> input)
        {
            var result = input.ActualValue == Alpha.Without ? Visibility.Visible : Visibility.Collapsed;
            return input.ActualParameter == 0 ? result : input.ActualParameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override Value<Alpha> ConvertBack(ConverterData<Visibility> input)
        {
            var result = input.ActualValue == Visibility.Visible ? Alpha.Without : Alpha.With;
            return input.ActualParameter == 0 ? result : input.ActualParameter == 1 ? Invert(result) : throw input.InvalidParameter;
        }
    }

    [ValueConversion(typeof(Boolean), typeof(Visibility))]
    public class BooleanToVisibilityConverter : Converter<Object, Object>
    {
        protected override bool Is(object input) => input is bool || input is bool? || input is Handle || input is Visibility;

        protected override Value<Object> ConvertTo(ConverterData<Object> input)
        {
            if (input.Value is bool || input.Value is bool? || input.Value is Handle)
            {
                var i = input.Value is bool ? (bool)input.Value : input.Value is bool? ? ((bool?)input.Value).Value : input.Value is Handle ? (bool)(Handle)input.Value : throw new NotSupportedException();
                var result = i.Visibility(input.Parameter is Visibility ? (Visibility)input.Parameter : Visibility.Collapsed);

                return input.ActualParameter == 0
                    ? result
                    : input.ActualParameter == 1
                        ? result.Invert()
                        : throw input.InvalidParameter;
            }
            return ConvertBack(input);
        }

        protected override Value<Object> ConvertBack(ConverterData<Object> input)
        {
            if (input.Value is Visibility visibility)
            {
                var result = ((Visibility)input.Value).Boolean();
                return input.ActualParameter == 0
                    ? result
                    : input.ActualParameter == 1
                        ? !result
                        : throw input.InvalidParameter;
            }

            return ConvertTo(input);
        }
    }

    [ValueConversion(typeof(Double), typeof(Visibility))]
    public class DoubleToVisibilityConverter : Converter<Double, Visibility>
    {
        protected override Value<Visibility> ConvertTo(ConverterData<Double> input)
        {
            var result = (input.ActualValue > 0).Visibility();
            return input.ActualParameter == 0 ? result : input.ActualParameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override Value<Double> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Int32), typeof(Visibility))]
    public class Int32ToVisibilityConverter : Converter<Int32, Visibility>
    {
        protected override Value<Visibility> ConvertTo(ConverterData<Int32> input)
        {
            var result = (input.ActualValue > 0).Visibility();
            return input.ActualParameter == 0 ? result : input.ActualParameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override Value<Int32> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Int64), typeof(Visibility))]
    public class Int64ToVisibilityConverter : Converter<Int64, Visibility>
    {
        protected override Value<Visibility> ConvertTo(ConverterData<Int64> input)
        {
            var result = (input.ActualValue > 0).Visibility();
            return input.ActualParameter == 0 ? result : input.ActualParameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override Value<Int64> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Object), typeof(Visibility))]
    public class ObjectToVisibilityConverter : Converter<Object, Visibility>
    {
        protected override bool AllowNull => true;

        protected override Value<Visibility> ConvertTo(ConverterData<Object> input)
        {
            var result = (input.Value != null).Visibility();
            return input.ActualParameter == 0 ? result : input.ActualParameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override Value<Object> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Object), typeof(Visibility))]
    public class ObjectIsToVisibilityConverter : Converter<Object, Visibility>
    {
        protected override Value<Visibility> ConvertTo(ConverterData<Object> input)
        {
            if (input.Parameter is Type i)
                return input.ActualValue.GetType().IsSubclassOf(i).Visibility();

            return Nothing.Do;
        }

        protected override Value<Object> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Orientation), typeof(Visibility))]
    public class OrientationToVisibilityConverter : Converter<Orientation, Visibility>
    {
        Visibility Convert(Orientation input) => input == Orientation.Horizontal ? Visibility.Visible : Visibility.Collapsed;

        protected override Value<Visibility> ConvertTo(ConverterData<Orientation> input)
        {
            return input.ActualParameter == 0 ? Convert(input.ActualValue) : input.ActualParameter == 1 ? Convert(input.ActualValue).Invert() : throw input.InvalidParameter;
        }

        protected override Value<Orientation> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }

    [ValueConversion(typeof(String), typeof(Visibility))]
    public class StringToVisibilityConverter : Converter<String, Visibility>
    {
        protected override bool AllowNull => true;

        protected override Value<Visibility> ConvertTo(ConverterData<String> input)
        {
            var result = input.ActualValue.NullOrEmpty().Invert().Visibility();
            return input.ActualParameter == 0 ? result : input.ActualParameter == 1 ? result.Invert() : throw input.InvalidParameter;
        }

        protected override Value<String> ConvertBack(ConverterData<Visibility> input) => Nothing.Do;
    }
}