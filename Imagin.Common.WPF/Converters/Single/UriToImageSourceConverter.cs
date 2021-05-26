using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(Uri), typeof(ImageSource))]
    public class UriToImageSourceConverter : Converter<Uri, ImageSource>
    {
        protected override Value<ImageSource> ConvertTo(ConverterData<Uri> input)
        {
            var i = new BitmapImage();

            i.BeginInit();
            i.UriSource = input.ActualValue;
            i.EndInit();

            return i as ImageSource;
        }

        protected override Value<Uri> ConvertBack(ConverterData<ImageSource> input) => Nothing.Do;
    }
}