using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public abstract class StringFormatMultiConverter<T> : MultiConverter<string>
    {
        protected abstract string Convert(T a, string b);

        public sealed override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2)
            {
                if (values[0] is T a)
                {
                    if (values[1] is string b)
                    {
                        var i = b.Between("{", "}");
                        if (!i.NullOrEmpty())
                            return b.ReplaceBetween('{', '}', "0").F(Convert(a, i));

                        return Convert(a, b);
                    }
                }
            }
            return Binding.DoNothing;
        }
    }

    public class ByteStringFormatMultiConverter : StringFormatMultiConverter<Byte>
    {
        protected override string Convert(Byte a, string b) => a.ToString(b);
    }

    public class DecimalStringFormatMultiConverter : StringFormatMultiConverter<Decimal>
    {
        protected override string Convert(Decimal a, string b) => a.ToString(b);
    }

    public class DoubleStringFormatMultiConverter : StringFormatMultiConverter<Double>
    {
        protected override string Convert(Double a, string b) => a.ToString(b);
    }

    public class Int16StringFormatMultiConverter : StringFormatMultiConverter<Int16>
    {
        protected override string Convert(Int16 a, string b) => a.ToString(b);
    }

    public class Int32StringFormatMultiConverter : StringFormatMultiConverter<Int32>
    {
        protected override string Convert(Int32 a, string b) => a.ToString(b);
    }

    public class Int64StringFormatMultiConverter : StringFormatMultiConverter<Int64>
    {
        protected override string Convert(Int64 a, string b) => a.ToString(b);
    }

    public class SingleStringFormatMultiConverter : StringFormatMultiConverter<Single>
    {
        protected override string Convert(Single a, string b) => a.ToString(b);
    }

    public class UDoubleStringFormatMultiConverter : StringFormatMultiConverter<UDouble>
    {
        protected override string Convert(UDouble a, string b) => a.ToString(b);
    }

    public class UInt16StringFormatMultiConverter : StringFormatMultiConverter<UInt16>
    {
        protected override string Convert(UInt16 a, string b) => a.ToString(b);
    }

    public class UInt32StringFormatMultiConverter : StringFormatMultiConverter<UInt32>
    {
        protected override string Convert(UInt32 a, string b) => a.ToString(b);
    }

    public class UInt64StringFormatMultiConverter : StringFormatMultiConverter<UInt64>
    {
        protected override string Convert(UInt64 a, string b) => a.ToString(b);
    }
}