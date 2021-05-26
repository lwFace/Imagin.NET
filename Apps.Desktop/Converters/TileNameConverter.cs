using System;
using System.Globalization;
using System.Windows.Data;

namespace Desktop.Converters
{
    [ValueConversion(typeof(byte), typeof(double))]
    public class TileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type)
            {
                if (value.Equals(typeof(Tiles.ClockTile)))
                    return "Clock";
                if (value.Equals(typeof(Tiles.CountDownTile)))
                    return "Count down";
                if (value.Equals(typeof(Tiles.FolderTile)))
                    return "Folder";
                if (value.Equals(typeof(Tiles.ImageTile)))
                    return "Image";
                if (value.Equals(typeof(Tiles.NoteTile)))
                    return "Note";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value;
    }
}