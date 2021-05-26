using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    public class DashStyleTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string actualValue = value as string;
            if (actualValue != null)
            {
                switch (actualValue)
                {
                    case nameof(DashStyles.Dash):
                        return DashStyles.Dash;
                    case nameof(DashStyles.DashDot):
                        return DashStyles.DashDot;
                    case nameof(DashStyles.DashDotDot):
                        return DashStyles.DashDotDot;
                    case nameof(DashStyles.Dot):
                        return DashStyles.Dot;
                    case nameof(DashStyles.Solid):
                        return DashStyles.Solid;
                }
                throw new NotSupportedException();
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