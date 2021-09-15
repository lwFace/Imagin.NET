using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public static class ImageSourceExtensions
    {
        public static Bitmap Bitmap(this ImageSource input) => input.As<BitmapSource>().Bitmap();
    }
}