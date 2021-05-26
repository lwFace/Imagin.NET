using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Paint
{
    [ValueConversion(typeof(Layer), typeof(Thickness))]
    public class LayerDepthThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                Console.WriteLine($"{nameof(LayerDepthThicknessConverter)}: value == null");
            }

            if (value is Layer)
            {
                Console.WriteLine($"{nameof(LayerDepthThicknessConverter)}: value is Layer");
                Console.WriteLine($"{nameof(LayerDepthThicknessConverter)}: value.Name = {((Layer)value).Name}");

                var layer = (Layer)value;
                layer.PropertyChanged -= OnLayerPropertyChanged;
                layer.PropertyChanged += OnLayerPropertyChanged;

                var offset = 5;
                int.TryParse(parameter?.ToString(), out offset);

                var result = 0;
                while (layer.Parent != null)
                {
                    result += offset;
                    layer = layer.Parent;
                }

                Console.WriteLine($"{nameof(LayerDepthThicknessConverter)}: result = {result}");
                return new Thickness(result, 0, 0, 0);
            }
            return default(Thickness);
        }

        void OnLayerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Layer.Parent):
                    Convert((Layer)sender, null, null, null);
                    break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}