using System;
using System.Windows;

namespace Imagin.Common.Linq
{
    public static class TimeSpanExtensions
    {
        public static Duration Duration(this TimeSpan i) => new Duration(i);
    }
}