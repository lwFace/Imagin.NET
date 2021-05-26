using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(String), typeof(Boolean))]
    public class DirectoryExistsConverter : Converter<String, Boolean>
    {
        protected override Value<Boolean> ConvertTo(ConverterData<String> input) => input.ActualValue.Length > 0 ? System.IO.Directory.Exists(input.ActualValue) : default;

        protected override Value<String> ConvertBack(ConverterData<Boolean> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Enum), typeof(bool))]
    public class HasFlagConverter : Converter<Enum, bool>
    {
        protected override Value<bool> ConvertTo(ConverterData<Enum> input) => input.ActualValue.HasAll((Enum)input.Parameter);

        protected override Value<Enum> ConvertBack(ConverterData<Boolean> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(bool))]
    public class HiddenConverter : Converter<string, bool>
    {
        protected override Value<bool> ConvertTo(ConverterData<String> input) => Machine.Hidden(input.ActualValue);

        protected override Value<string> ConvertBack(ConverterData<Boolean> input) => Nothing.Do;
    }

    //.......................................................................

    [ValueConversion(typeof(Boolean), typeof(Boolean))]
    public class InverseBooleanConverter : Converter<Boolean, Boolean>
    {
        protected override Value<Boolean> ConvertTo(ConverterData<Boolean> input) => !input.ActualValue;

        protected override Value<Boolean> ConvertBack(ConverterData<Boolean> input) => !input.ActualValue;
    }

    //.......................................................................

    [ValueConversion(typeof(Int32), typeof(Boolean))]
    public class IntToBooleanConverter : Converter<Int32, Boolean>
    {
        protected override Value<Boolean> ConvertTo(ConverterData<Int32> input) => input.ActualValue > 0;

        protected override Value<Int32> ConvertBack(ConverterData<Boolean> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Object), typeof(Boolean))]
    public class ObjectToBooleanConverter : Converter<Object, Boolean>
    {
        protected override bool AllowNull => true;

        protected override Value<Boolean> ConvertTo(ConverterData<Object> input) => !(input.ActualValue == null);

        protected override Value<Object> ConvertBack(ConverterData<Boolean> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Object), typeof(Boolean))]
    public class ObjectIsConverter : Converter<Object, Boolean>
    {
        protected override Value<Boolean> ConvertTo(ConverterData<Object> input)
        {
            if (input.Parameter is Type i)
                return input.ActualValue.GetType().IsSubclassOf(i) || input.ActualValue.GetType().Equals(i);

            return false;
        }

        protected override Value<Object> ConvertBack(ConverterData<Boolean> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Orientation), typeof(Boolean))]
    public class OrientationToBooleanConverter : Converter<Orientation, Boolean>
    {
        protected override Value<Boolean> ConvertTo(ConverterData<Orientation> input) => input.ActualValue == (Orientation)Enum.Parse(typeof(Orientation), (string)input.Parameter);

        protected override Value<Orientation> ConvertBack(ConverterData<Boolean> input) => input.ActualValue ? (Orientation)Enum.Parse(typeof(Orientation), (string)input.Parameter) : default;
    }

    [ValueConversion(typeof(String), typeof(Boolean))]
    public class StringToBooleanConverter : Converter<String, Boolean>
    {
        protected override bool AllowNull => true;

        protected override Value<Boolean> ConvertTo(ConverterData<String> input) => input.ActualValue.NullOrEmpty() == false;

        protected override Value<String> ConvertBack(ConverterData<Boolean> input) => Nothing.Do;
    }
}