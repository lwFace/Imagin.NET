using System;

namespace Imagin.Common.Linq
{
    public static class BooleanExtensions
    {
        public static void If(this bool a, bool b, Action action)
        {
            if (a == b)
                action();
        }

        public static bool Invert(this bool input) => !input;
    }
}