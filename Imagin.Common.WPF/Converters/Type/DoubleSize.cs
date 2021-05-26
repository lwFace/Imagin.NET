using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Imagin.Common.Converters
{
    public class DoubleSizeTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string actualValue = value as string;
            if (actualValue != null)
            {
                var items = actualValue.Split(',');
                var result = new double[items.Length];

                if (result.Length > 2)
                    throw new ArgumentOutOfRangeException(nameof(value));

                for (var i = 0; i < result.Length; i++)
                {
                    if (!items[i].Numeric())
                        throw new ArgumentException();

                    result[i] = items[i].Double();
                }

                return new DoubleSize(result[0], result[1]);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }
    }
}