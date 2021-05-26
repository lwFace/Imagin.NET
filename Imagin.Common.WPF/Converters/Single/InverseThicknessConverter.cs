using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Thickness), typeof(Thickness))]
    public class InverseThicknessConverter : Converter<Thickness, Thickness>
    {
        protected override Value<Thickness> ConvertTo(ConverterData<Thickness> input) => input.ActualValue.Invert();

        protected override Value<Thickness> ConvertBack(ConverterData<Thickness> input) => input.ActualValue.Invert();
    }
}