using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(DataGridLength), typeof(GridLength))]
    public class DataGridLengthConverter : Converter<DataGridLength, GridLength>
    {
        protected override Value<GridLength> ConvertTo(ConverterData<DataGridLength> input)
        {
            return new GridLength();
        }

        protected override Value<DataGridLength> ConvertBack(ConverterData<GridLength> input)
        {
            return new DataGridLength();
        }
    }
}
