using Imagin.Common.Data;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(SortDirection), typeof(ListSortDirection))]
    public class SortDirectionConverter : Converter<SortDirection, ListSortDirection>
    {
        protected override Value<ListSortDirection> ConvertTo(ConverterData<SortDirection> input)
        {
            Enum.TryParse($"{input.ActualValue}", out ListSortDirection result);
            return result;
        }

        protected override Value<SortDirection> ConvertBack(ConverterData<ListSortDirection> input)
        {
            Enum.TryParse($"{input.ActualValue}", out SortDirection result);
            return result;
        }
    }
}