using Imagin.Common.Linq;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace Imagin.Common.Converters
{
    public class GridLengthArrayTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string actualValue = value as string;
            if (actualValue != null)
            {
                var items = actualValue.Split(',');

                var result = new GridLength[items.Length];
                for (var i = 0; i < result.Length; i++)
                {
                    switch (items[i].ToLower())
                    {
                        case "auto":
                            result[i] = new GridLength(1, GridUnitType.Auto);
                            continue;

                        case "*":
                            result[i] = new GridLength(1, GridUnitType.Star);
                            continue;

                        default:

                            if (items[i].EndsWith("*"))
                            {
                                var number = items[i].Replace("*", string.Empty);
                                if (number.Numeric())
                                {
                                    var n = 0.0;
                                    double.TryParse(number, out n);
                                    result[i] = new GridLength(n, GridUnitType.Star);
                                }
                            }
                            else if (items[i].Numeric())
                            {
                                var n = 0.0;
                                double.TryParse(items[i], out n);
                                result[i] = new GridLength(n, GridUnitType.Pixel);
                            }

                            if (result[i] == null)
                                throw new NotSupportedException();

                            continue;
                    }
                }

                return result;
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