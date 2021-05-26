using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class GraphicalUnitMultiConverter : MultiConverter<string>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length >= 2 && values[0]?.IsAny(typeof(double), typeof(double?), typeof(int), typeof(int?)) == true && values[1] is GraphicalUnit)
            {
                var value = double.Parse(values[0].ToString());

                var funit = parameter is GraphicalUnit ? (GraphicalUnit)parameter : GraphicalUnit.Pixel;
                var tunit = values[1].As<GraphicalUnit>();

                var resolution = values.Length > 2 ? (float)values[2] : 72f;
                var places = values.Length > 3 ? (int)values[3] : 3;

                return $"{value.Convert(funit, tunit, resolution).Round(places)} {tunit.GetAttribute<AbbreviationAttribute>().Abbreviation}";
            }
            return string.Empty;
        }
    }
}