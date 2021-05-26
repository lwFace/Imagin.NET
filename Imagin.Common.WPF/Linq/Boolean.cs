﻿using System.Windows;

namespace Imagin.Common.Linq
{
    public static class BooleanExtensions
    {
        public static Visibility Visibility(this bool input, Visibility falseVisibility = System.Windows.Visibility.Collapsed) => input ? System.Windows.Visibility.Visible : falseVisibility;
    }
}