using System;

namespace Imagin.Common.Linq
{
    public static class SpecialFolderExtensions
    {
        public static string Path(this Environment.SpecialFolder Value) => Environment.GetFolderPath(Value);
    }
}