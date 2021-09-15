using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Storage;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Object), typeof(String))]
    public class DisplayNameConverter : Converter<Object, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Object> input)
        {
            string result = null;
            if (input.ActualValue is Enum a)
            {
                result = a.GetAttribute<DisplayNameAttribute>()?.DisplayName ?? a.GetAttribute<System.ComponentModel.DisplayNameAttribute>()?.DisplayName;
                return result ?? a.ToString().SplitCamel();
            }

            if (input.ActualValue is Object b)
            {
                var type = b.GetType();
                result = type.GetAttribute<DisplayNameAttribute>()?.DisplayName ?? type.GetAttribute<System.ComponentModel.DisplayNameAttribute>()?.DisplayName;
                return result ?? type.Name;
            }

            return Nothing.Do;
        }

        protected override Value<Object> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    //.......................................................................

    [ValueConversion(typeof(String), typeof(String))]
    public class FileExtensionConverter : Converter<String, String>
    {
        protected override Value<String> ConvertTo(ConverterData<String> input)
        {
            string result = null;
            return !Try.Invoke(() => result = Storage.Path.GetExtension(input.ActualValue)) 
                ? (Value<String>)Nothing.Do 
                : input.ActualParameter == 0 ? result.Replace(".", string.Empty) : input.ActualParameter == 1 ? result : throw input.InvalidParameter;
        }

        protected override Value<String> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class FileNameConverter : Converter<String, String>
    {
        protected override Value<String> ConvertTo(ConverterData<String> input)
        {
            if (input.ActualValue == Folder.Long.Root)
                return Folder.Long.RootName;

            if (input.ActualValue == Folder.Long.RecycleBin)
                return Folder.Long.RecycleBinName;

            if (input.ActualValue.EndsWith(@":\"))
            {
                foreach (var i in Machine.Drives)
                {
                    if (input.ActualValue.Equals(i.Name))
                        return $"{i.VolumeLabel} ({i.Name.Replace(@"\", string.Empty)})";
                }
                return input.ActualValue;
            }

            return Folder.Long.Exists(input.ActualValue) || input.ActualParameter == 1
                ? Storage.Path.GetFileName(input.ActualValue)
                : input.ActualParameter == 0 
                    ? Storage.Path.GetFileNameWithoutExtension(input.ActualValue)
                    : throw input.InvalidParameter;
        }

        protected override Value<String> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Int64), typeof(String))]
    public class FileSizeConverter : Converter<Int64, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Int64> input)
        {
            var format = FileSizeFormat.BinaryUsingSI;
            Try.Invoke(() => format = (FileSizeFormat)Enum.Parse(typeof(FileSizeFormat), input.Parameter.ToString()));
            return input.ActualValue.FileSize(format);
        }

        protected override Value<Int64> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Double), typeof(String))]
    public class FileSpeedConverter : Converter<Double, String>
    {
        protected override bool Is(object input) => input is double || input is string;

        protected override Value<String> ConvertTo(ConverterData<Double> input)
        {
            long result = 0;

            if (input.Value is double a)
                result = a.Int64();

            if (input.Value is string b)
                result = b.Int64();

            return $"{result.FileSize(FileSizeFormat.BinaryUsingSI)}/s";
        }

        protected override Value<Double> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class FileTypeConverter : Converter<String, String>
    {
        protected override Value<String> ConvertTo(ConverterData<String> input) => Machine.Description(input.ActualValue);

        protected override Value<String> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }
    
    //.......................................................................

    [ValueConversion(typeof(Object), typeof(String))]
    public class PluralConverter : Converter<Object, String>
    {
        protected override bool Is(object input) => input is ushort || input is short || input is uint || input is int || input is long || input is long;

        protected override Value<String> ConvertTo(ConverterData<Object> input)
        {
            var result = input.ActualValue.Int32();
            return result == 1 ? string.Empty : input.ActualParameter == 0 ? "s" : "S";
        }

        protected override Value<Object> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    //.......................................................................

    [ValueConversion(typeof(Object), typeof(String))]
    public class RelativeTimeConverter : Converter<Object, String>
    {
        protected override bool Is(object input) => input is DateTime || input is DateTime?;

        protected override Value<String> ConvertTo(ConverterData<Object> input)
        {
            if (input.ActualValue is DateTime a)
                return a.Relative();

            return input.ActualValue.As<DateTime?>()?.Relative() ?? (Value<String>)Nothing.Do;
        }

        protected override Value<Object> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Object), typeof(String))]
    public class SecondsConverter : Converter<Object, String>
    {
        protected override bool Is(object input) => input is DateTime || input is DateTime? || input is int || input is string;

        protected override Value<String> ConvertTo(ConverterData<Object> input)
        {
            TimeSpan result = TimeSpan.Zero;
            if (input.ActualValue is int a)
                result = TimeSpan.FromSeconds(a);

            else if (input.ActualValue is string b)
                result = TimeSpan.FromSeconds(b.Int32());

            else
            {
                var now = DateTime.Now;
                if (input.ActualValue is DateTime c)
                    result = c > now ? c - now : now - c;

                if (input.ActualValue is DateTime?)
                {
                    var d = input.ActualValue as DateTime?;

                    if (d == null)
                        return string.Empty;

                    result = d.Value > now ? d.Value - now : now - d.Value;
                }
            }

            return result.ShortTime(input.ActualParameter == 1);
        }

        protected override Value<Object> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    //.......................................................................

    [ValueConversion(typeof(Object), typeof(String))]
    public class CamelCaseConverter : Converter<Object, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Object> input)
        {
            var result = input.ActualValue.ToString().SplitCamel() ?? string.Empty;
            return input.ActualParameter == 0 ? result : input.ActualParameter == 1 ? result.Capitalize() : input.ActualParameter == 2 ? result.ToLower() : throw new NotSupportedException();
        }

        protected override Value<Object> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class FirstLetterConverter : Converter<String, String>
    {
        protected override Value<String> ConvertTo(ConverterData<String> input)
        {
            if (!input.ActualValue.Empty())
                return input.ActualValue.Substring(0, 1);

            return default;
        }

        protected override Value<String> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Int32), typeof(String))]
    public class LeadingZeroConverter : Converter<Int32, String>
    {
        protected override Value<string> ConvertTo(ConverterData<Int32> input) => input.ActualValue.ToString("D2");

        protected override Value<int> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Object), typeof(String))]
    public class SubstringConverter : Converter<Object, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Object> input)
        {
            if (input.ActualValue is string || input.ActualValue is Enum)
            {
                var i = input.ActualValue.ToString();
                Try.Invoke(() => i = i.Substring(0, input.ActualParameter == 0 ? i.Length : input.ActualParameter));
                return i;
            }
            return default;
        }

        protected override Value<Object> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class ToLowerConverter : Converter<String, String>
    {
        protected override Value<String> ConvertTo(ConverterData<String> input) => input.ActualValue.ToLower();

        protected override Value<String> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(Object), typeof(String))]
    public class ToStringConverter : Converter<Object, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Object> input) => input.ActualValue.ToString();

        protected override Value<Object> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    [ValueConversion(typeof(String), typeof(String))]
    public class ToUpperConverter : Converter<String, String>
    {
        protected override Value<String> ConvertTo(ConverterData<String> input) => input.ActualValue.ToUpper();

        protected override Value<String> ConvertBack(ConverterData<String> input) => Nothing.Do;
    }

    //.......................................................................

    [ValueConversion(typeof(Object), typeof(String))]
    public abstract class ArrayToStringConverter : Converter<Object, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Object> input)
        {
            if (input.ActualValue.GetType().IsArray)
            {
                if (input.ActualValue is IList list)
                {
                    var result = string.Empty;

                    var j = 0;
                    var count = list.Count;

                    foreach (var i in list)
                    {
                        result += (j == count - 1 ? $"{i}" : $"{i}{input.Parameter?.ToString().Char()}");
                        j++;
                    }

                    return result;
                }
                return input.ActualValue.ToString();
            }
            return default;
        }
    }

    [ValueConversion(typeof(Object), typeof(String))]
    public class Int32ArrayToStringConverter : ArrayToStringConverter
    {
        protected override Value<Object> ConvertBack(ConverterData<String> input) => input.ActualValue.Int32Array(input.Parameter?.ToString().Char()).ToArray();
    }

    //.......................................................................

    [ValueConversion(typeof(Byte), typeof(String))]
    public class ByteToStringConverter : Converter<Byte, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Byte> input) => input.ActualValue.ToString();

        protected override Value<Byte> ConvertBack(ConverterData<String> input) => input.ActualValue.Byte();
    }

    [ValueConversion(typeof(Char), typeof(String))]
    public class CharacterToStringConverter : Converter<Char, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Char> input) => input.ActualValue.ToString();

        protected override Value<Char> ConvertBack(ConverterData<String> input) => input.ActualValue.Char();
    }

    [ValueConversion(typeof(Decimal), typeof(String))]
    public class DecimalToStringConverter : Converter<Decimal, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Decimal> input) => input.ActualValue.ToString();

        protected override Value<Decimal> ConvertBack(ConverterData<String> input) => input.ActualValue.Decimal();
    }

    [ValueConversion(typeof(Double), typeof(String))]
    public class DoubleToStringConverter : Converter<Double, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Double> input) => input.ActualValue.ToString();

        protected override Value<Double> ConvertBack(ConverterData<String> input) => input.ActualValue.Double();
    }

    [ValueConversion(typeof(Guid), typeof(String))]
    public class GuidToStringConverter : Converter<Guid, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Guid> input) => input.ActualValue.ToString();

        protected override Value<Guid> ConvertBack(ConverterData<String> input)
        {
            Guid.TryParse(input.ActualValue, out Guid result);
            return result;
        }
    }

    [ValueConversion(typeof(Hexadecimal), typeof(String))]
    public class HexadecimalToStringConverter : Converter<Hexadecimal, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Hexadecimal> input) => input.ActualValue.ToString(true);

        protected override Value<Hexadecimal> ConvertBack(ConverterData<String> input) => (Hexadecimal)input.ActualValue;
    }

    [ValueConversion(typeof(Int16), typeof(String))]
    public class Int16ToStringConverter : Converter<Int16, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Int16> input) => input.ActualValue.ToString();

        protected override Value<Int16> ConvertBack(ConverterData<String> input) => input.ActualValue.Int16();
    }

    [ValueConversion(typeof(Int32), typeof(String))]
    public class Int32ToStringConverter : Converter<Int32, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Int32> input) => input.ActualValue.ToString();

        protected override Value<Int32> ConvertBack(ConverterData<String> input) => input.ActualValue.Int32();
    }

    [ValueConversion(typeof(Int64), typeof(String))]
    public class Int64ToStringConverter : Converter<Int64, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Int64> input) => input.ActualValue.ToString();

        protected override Value<Int64> ConvertBack(ConverterData<String> input) => input.ActualValue.Int64();
    }

    [ValueConversion(typeof(ItemProperty), typeof(String))]
    public class ItemPropertyToStringConverter : Converter<ItemProperty, String>
    {
        protected override Value<String> ConvertTo(ConverterData<ItemProperty> input) => input.ActualValue.ToString();

        protected override Value<ItemProperty> ConvertBack(ConverterData<String> input) => throw new NotSupportedException();
    }

    [ValueConversion(typeof(Single), typeof(String))]
    public class SingleToStringConverter : Converter<Single, String>
    {
        protected override Value<String> ConvertTo(ConverterData<Single> input) => input.ActualValue.ToString();

        protected override Value<Single> ConvertBack(ConverterData<String> input) => input.ActualValue.Single();
    }

    [ValueConversion(typeof(SolidColorBrush), typeof(String))]
    public class SolidColorBrushToStringConverter : Converter<SolidColorBrush, String>
    {
        protected override Value<String> ConvertTo(ConverterData<SolidColorBrush> input) => input.ActualValue.Color.Hexadecimal().ToString(true);

        protected override Value<SolidColorBrush> ConvertBack(ConverterData<String> input) => new Hexadecimal(input.ActualValue).SolidColorBrush();
    }

    [ValueConversion(typeof(UDouble), typeof(String))]
    public class UDoubleToStringConverter : Converter<UDouble, String>
    {
        protected override Value<String> ConvertTo(ConverterData<UDouble> input) => input.ActualValue.ToString();

        protected override Value<UDouble> ConvertBack(ConverterData<String> input) => input.ActualValue.UDouble();
    }

    [ValueConversion(typeof(UInt16), typeof(String))]
    public class UInt16ToStringConverter : Converter<UInt16, String>
    {
        protected override Value<String> ConvertTo(ConverterData<UInt16> input) => input.ActualValue.ToString();

        protected override Value<UInt16> ConvertBack(ConverterData<String> input) => input.ActualValue.UInt16();
    }

    [ValueConversion(typeof(UInt32), typeof(String))]
    public class UInt32ToStringConverter : Converter<UInt32, String>
    {
        protected override Value<String> ConvertTo(ConverterData<UInt32> input) => input.ActualValue.ToString();

        protected override Value<UInt32> ConvertBack(ConverterData<String> input) => input.ActualValue.UInt32();
    }

    [ValueConversion(typeof(UInt64), typeof(String))]
    public class UInt64ToStringConverter : Converter<UInt64, String>
    {
        protected override Value<String> ConvertTo(ConverterData<UInt64> input) => input.ActualValue.ToString();

        protected override Value<UInt64> ConvertBack(ConverterData<String> input) => input.ActualValue.UInt64();
    }
}