using System.IO;
using System.Reflection;

namespace System.Windows.Media.Imaging
{
    /// <summary>
    /// Cross-platform factory for WriteableBitmaps
    /// </summary>
    public static class BitmapFactory
    {
        public static WriteableBitmap WriteableBitmap(System.Drawing.Size size) => WriteableBitmap(size.Height, size.Width);

        public static WriteableBitmap WriteableBitmap(int height, int width) => new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

        public static System.Drawing.Bitmap Bitmap(System.Drawing.Size size) => Bitmap(size.Height, size.Width);

        public static System.Drawing.Bitmap Bitmap(int height, int width) => new System.Drawing.Bitmap(width, height);

        /// <summary>
        /// Creates a new WriteableBitmap of the specified width and height
        /// </summary>
        /// <remarks>For WPF the default DPI is 96x96 and PixelFormat is Pbgra32</remarks>
        /// <param name="pixelWidth"></param>
        /// <param name="pixelHeight"></param>
        /// <returns></returns>
        public static WriteableBitmap New(int pixelWidth, int pixelHeight)
        {
            if (pixelHeight < 1) pixelHeight = 1;
            if (pixelWidth < 1) pixelWidth = 1;
            return new WriteableBitmap(pixelWidth, pixelHeight, 96.0, 96.0, PixelFormats.Pbgra32, null);
        }

        /// <summary>
        /// Converts the input BitmapSource to the Pbgra32 format WriteableBitmap which is internally used by the WriteableBitmapEx.
        /// </summary>
        /// <param name="source">The source bitmap.</param>
        /// <returns></returns>
        public static WriteableBitmap ConvertToPbgra32Format(BitmapSource source)
        {
            // Convert to Pbgra32 if it's a different format
            if (source.Format == PixelFormats.Pbgra32)
            {
                return new WriteableBitmap(source);
            }

            var formatedBitmapSource = new FormatConvertedBitmap();
            formatedBitmapSource.BeginInit();
            formatedBitmapSource.Source = source;
            formatedBitmapSource.DestinationFormat = PixelFormats.Pbgra32;
            formatedBitmapSource.EndInit();
            return new WriteableBitmap(formatedBitmapSource);
        }
   
        /// <summary>
        /// Loads an image from the applications resource file and returns a new WriteableBitmap.
        /// </summary>
        /// <param name="relativePath">Only the relative path to the resource file. The assembly name is retrieved automatically.</param>
        /// <returns>A new WriteableBitmap containing the pixel data.</returns>
        public static WriteableBitmap FromResource(string relativePath)
        {
            var fullName = Assembly.GetCallingAssembly().FullName;
            var asmName = new AssemblyName(fullName).Name;
            return FromContent(asmName + ";component/" + relativePath);
        }

        /// <summary>
        /// Loads an image from the applications content and returns a new WriteableBitmap.
        /// </summary>
        /// <param name="relativePath">Only the relative path to the content file.</param>
        /// <returns>A new WriteableBitmap containing the pixel data.</returns>
        public static WriteableBitmap FromContent(string relativePath)
        {
            using (var bmpStream = Application.GetResourceStream(new Uri(relativePath, UriKind.Relative)).Stream)
            {
                return FromStream(bmpStream);
            }
        }

        /// <summary>
        /// Loads the data from an image stream and returns a new WriteableBitmap.
        /// </summary>
        /// <param name="stream">The stream with the image data.</param>
        /// <returns>A new WriteableBitmap containing the pixel data.</returns>
        public static WriteableBitmap FromStream(Stream stream)
        {
            var bmpi = new BitmapImage();
            bmpi.BeginInit();
            bmpi.CreateOptions = BitmapCreateOptions.None;
            bmpi.StreamSource = stream;
            bmpi.EndInit();
            var bmp = new WriteableBitmap(bmpi);
            bmpi.UriSource = null;
            return bmp;
        }
    }
}