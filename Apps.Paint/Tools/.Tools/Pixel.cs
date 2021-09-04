using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Paint
{
    /// <summary>
    /// A tool used to manipulate the pixels of a <see cref="WriteableBitmap"/>.
    /// </summary>
    [Serializable]
    public abstract class PixelTool : Tool
    {
    }
}