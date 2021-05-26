using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(FrameworkElement), typeof(Int32))]
    public class IndexConverter : Converter<FrameworkElement, Int32>
    {
        protected override Value<Int32> ConvertTo(ConverterData<FrameworkElement> input)
        {
            var item = input.ActualValue;
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(item);

            var index = itemsControl?.ItemContainerGenerator.IndexFromContainer(item) ?? 0;
            return input.ActualParameter + index;
        }

        protected override Value<FrameworkElement> ConvertBack(ConverterData<Int32> input) => Nothing.Do;
    }
}