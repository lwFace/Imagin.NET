using System.Windows;

namespace Imagin.Common.Linq
{
    public static class ThicknessExtensions
    {
        public static Thickness Invert(this Thickness input) => new Thickness(-input.Left, -input.Top, -input.Right, -input.Bottom);
    }
}