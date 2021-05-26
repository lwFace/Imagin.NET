using System;
using System.Globalization;
using System.Windows.Data;

namespace Paint
{
    [ValueConversion(typeof(Tool), typeof(SelectionShape))]
    public class ToolToShapeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tool = (Tool)value;

            if (tool is EllipseSelectionTool)
                return SelectionShape.Ellipse;

            return SelectionShape.Rectangle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}