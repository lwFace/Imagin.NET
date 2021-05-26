﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    public static class IconExtensions
    {
        public static ImageSource ImageSource(this Icon Icon)
        {
            Bitmap Bitmap = Icon.ToBitmap();
            IntPtr HBitmap = Bitmap.GetHbitmap();
            ImageSource ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(HBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            if (!HBitmap.Dispose()) throw new Win32Exception();
            return ImageSource;
        }
    }
}