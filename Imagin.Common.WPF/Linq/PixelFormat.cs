namespace Imagin.Common.Linq
{
    public static partial class PixelFormatExtensions
    {
        #region Imagin.Common.Media

        public static System.Drawing.Imaging.PixelFormat Imaging(this Media.PixelFormat format)
        {
            switch (format)
            {
                /*
                case Drawing.PixelFormat.Bgr101010:
                    return System.Drawing.Imaging.PixelFormat.;
                case Drawing.PixelFormat.BlackWhite:
                    return System.Drawing.Imaging.PixelFormat.;
                case Drawing.PixelFormat.Cmyk32:
                    return System.Drawing.Imaging.PixelFormat.;
                case Drawing.PixelFormat.Gray2:
                    return System.Drawing.Imaging.PixelFormat.;
                case Drawing.PixelFormat.Gray32Float:
                    return System.Drawing.Imaging.PixelFormat.;
                case Drawing.PixelFormat.Gray4:
                    return System.Drawing.Imaging.PixelFormat.;
                case Drawing.PixelFormat.Gray8:
                    return System.Drawing.Imaging.PixelFormat.;
                case Drawing.PixelFormat.Indexed2:
                    return System.Drawing.Imaging.PixelFormat.;
                case Drawing.PixelFormat.Prgba128Float:
                    return System.Drawing.Imaging.PixelFormat.;
                case Drawing.PixelFormat.Rgb128Float:
                    return System.Drawing.Imaging.PixelFormat.;
                case Drawing.PixelFormat.Rgba128Float:
                    return System.Drawing.Imaging.PixelFormat.;
                */
                case Media.PixelFormat.Bgr24:
                    return System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                case Media.PixelFormat.Bgr32:
                    return System.Drawing.Imaging.PixelFormat.Format32bppRgb;
                case Media.PixelFormat.Bgr555:
                    return System.Drawing.Imaging.PixelFormat.Format16bppRgb555;
                case Media.PixelFormat.Bgr565:
                    return System.Drawing.Imaging.PixelFormat.Format16bppRgb565;
                case Media.PixelFormat.Bgra32:
                    return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                case Media.PixelFormat.Gray16:
                    return System.Drawing.Imaging.PixelFormat.Format16bppGrayScale;
                case Media.PixelFormat.Indexed1:
                    return System.Drawing.Imaging.PixelFormat.Format1bppIndexed;
                case Media.PixelFormat.Indexed4:
                    return System.Drawing.Imaging.PixelFormat.Format4bppIndexed;
                case Media.PixelFormat.Indexed8:
                    return System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                case Media.PixelFormat.Pbgra32:
                    return System.Drawing.Imaging.PixelFormat.Format32bppPArgb;
                case Media.PixelFormat.Prgba64:
                    return System.Drawing.Imaging.PixelFormat.Format64bppPArgb;
                case Media.PixelFormat.Rgb24:
                    return System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                case Media.PixelFormat.Rgb48:
                    return System.Drawing.Imaging.PixelFormat.Format48bppRgb;
                case Media.PixelFormat.Rgba64:
                    return System.Drawing.Imaging.PixelFormat.Format64bppArgb;
            }
            return default;
        }

        public static System.Windows.Media.PixelFormat Convert(this Media.PixelFormat format)
        {
            switch (format)
            {
                case Media.PixelFormat.Bgr101010:
                    return System.Windows.Media.PixelFormats.Bgr101010;
                case Media.PixelFormat.Bgr24:
                    return System.Windows.Media.PixelFormats.Bgr24;
                case Media.PixelFormat.Bgr32:
                    return System.Windows.Media.PixelFormats.Bgr32;
                case Media.PixelFormat.Bgr555:
                    return System.Windows.Media.PixelFormats.Bgr555;
                case Media.PixelFormat.Bgr565:
                    return System.Windows.Media.PixelFormats.Bgr565;
                case Media.PixelFormat.Bgra32:
                    return System.Windows.Media.PixelFormats.Bgra32;
                case Media.PixelFormat.BlackWhite:
                    return System.Windows.Media.PixelFormats.BlackWhite;
                case Media.PixelFormat.Cmyk32:
                    return System.Windows.Media.PixelFormats.Cmyk32;
                case Media.PixelFormat.Gray16:
                    return System.Windows.Media.PixelFormats.Gray16;
                case Media.PixelFormat.Gray2:
                    return System.Windows.Media.PixelFormats.Gray2;
                case Media.PixelFormat.Gray32Float:
                    return System.Windows.Media.PixelFormats.Gray32Float;
                case Media.PixelFormat.Gray4:
                    return System.Windows.Media.PixelFormats.Gray4;
                case Media.PixelFormat.Gray8:
                    return System.Windows.Media.PixelFormats.Gray8;
                case Media.PixelFormat.Indexed1:
                    return System.Windows.Media.PixelFormats.Indexed1;
                case Media.PixelFormat.Indexed2:
                    return System.Windows.Media.PixelFormats.Indexed2;
                case Media.PixelFormat.Indexed4:
                    return System.Windows.Media.PixelFormats.Indexed4;
                case Media.PixelFormat.Indexed8:
                    return System.Windows.Media.PixelFormats.Indexed8;
                case Media.PixelFormat.Pbgra32:
                    return System.Windows.Media.PixelFormats.Pbgra32;
                case Media.PixelFormat.Prgba128Float:
                    return System.Windows.Media.PixelFormats.Prgba128Float;
                case Media.PixelFormat.Prgba64:
                    return System.Windows.Media.PixelFormats.Prgba64;
                case Media.PixelFormat.Rgb24:
                    return System.Windows.Media.PixelFormats.Rgb24;
                case Media.PixelFormat.Rgb128Float:
                    return System.Windows.Media.PixelFormats.Rgb128Float;
                case Media.PixelFormat.Rgb48:
                    return System.Windows.Media.PixelFormats.Rgb48;
                case Media.PixelFormat.Rgba128Float:
                    return System.Windows.Media.PixelFormats.Rgba128Float;
                case Media.PixelFormat.Rgba64:
                    return System.Windows.Media.PixelFormats.Rgba64;
            }
            return default;
        }

        #endregion

        #region System.Windows.Media

        public static Media.PixelFormat Convert(this System.Windows.Media.PixelFormat format)
        {
            if (format == System.Windows.Media.PixelFormats.Bgr101010)
                return Media.PixelFormat.Bgr101010;
            if (format == System.Windows.Media.PixelFormats.Bgr24)
                return Media.PixelFormat.Bgr24;
            if (format == System.Windows.Media.PixelFormats.Bgr32)
                return Media.PixelFormat.Bgr32;
            if (format == System.Windows.Media.PixelFormats.Bgr555)
                return Media.PixelFormat.Bgr555;
            if (format == System.Windows.Media.PixelFormats.Bgr565)
                return Media.PixelFormat.Bgr565;
            if (format == System.Windows.Media.PixelFormats.Bgra32)
                return Media.PixelFormat.Bgra32;
            if (format == System.Windows.Media.PixelFormats.BlackWhite)
                return Media.PixelFormat.BlackWhite;
            if (format == System.Windows.Media.PixelFormats.Cmyk32)
                return Media.PixelFormat.Cmyk32;
            if (format == System.Windows.Media.PixelFormats.Gray16)
                return Media.PixelFormat.Gray16;
            if (format == System.Windows.Media.PixelFormats.Gray2)
                return Media.PixelFormat.Gray2;
            if (format == System.Windows.Media.PixelFormats.Gray32Float)
                return Media.PixelFormat.Gray32Float;
            if (format == System.Windows.Media.PixelFormats.Gray4)
                return Media.PixelFormat.Gray4;
            if (format == System.Windows.Media.PixelFormats.Gray8)
                return Media.PixelFormat.Gray8;
            if (format == System.Windows.Media.PixelFormats.Indexed1)
                return Media.PixelFormat.Indexed1;
            if (format == System.Windows.Media.PixelFormats.Indexed2)
                return Media.PixelFormat.Indexed2;
            if (format == System.Windows.Media.PixelFormats.Indexed4)
                return Media.PixelFormat.Indexed4;
            if (format == System.Windows.Media.PixelFormats.Indexed8)
                return Media.PixelFormat.Indexed8;
            if (format == System.Windows.Media.PixelFormats.Pbgra32)
                return Media.PixelFormat.Pbgra32;
            if (format == System.Windows.Media.PixelFormats.Prgba128Float)
                return Media.PixelFormat.Prgba128Float;
            if (format == System.Windows.Media.PixelFormats.Prgba64)
                return Media.PixelFormat.Prgba64;
            if (format == System.Windows.Media.PixelFormats.Rgb24)
                return Media.PixelFormat.Rgb24;
            if (format == System.Windows.Media.PixelFormats.Rgb128Float)
                return Media.PixelFormat.Rgb128Float;
            if (format == System.Windows.Media.PixelFormats.Rgb48)
                return Media.PixelFormat.Rgb48;
            if (format == System.Windows.Media.PixelFormats.Rgba128Float)
                return Media.PixelFormat.Rgba128Float;
            if (format == System.Windows.Media.PixelFormats.Rgba64)
                return Media.PixelFormat.Rgba64;
            return default;
        }

        #endregion
    }
}