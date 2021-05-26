using System;

namespace Imagin.Common.Controls
{
    [Flags]
    [Serializable]
    public enum InheritedPropertyVisibility
    {
        [Hidden]
        None = 0,
        Base = 1,
        [Namespace(nameof(System), nameof(System.Windows), nameof(System.Windows.Controls), nameof(System.Windows.Controls.ContentControl))]
        ContentControl = 2,
        [Namespace(nameof(System), nameof(System.Windows), nameof(System.Windows.Controls), nameof(System.Windows.Controls.Control))]
        Control = 4,
        [Namespace(nameof(System), nameof(System.Windows), nameof(System.Windows.DependencyObject))]
        DependencyObject = 8,
        [Namespace(nameof(System), nameof(System.Windows), nameof(System.Windows.Threading), nameof(System.Windows.Threading.DispatcherObject))]
        DispatcherObject = 16,
        [Namespace(nameof(System), nameof(System.Windows), nameof(System.Windows.FrameworkElement))]
        FrameworkElement = 32,
        [Namespace(nameof(System), nameof(System.Windows), nameof(System.Windows.Controls), nameof(System.Windows.Controls.ItemsControl))]
        ItemsControl = 64,
        [Namespace(nameof(System), nameof(System.Windows), nameof(System.Windows.Controls), nameof(System.Windows.Controls.Panel))]
        Panel = 128,
        [Namespace(nameof(System), nameof(System.Windows), nameof(System.Windows.UIElement))]
        UIElement = 256,
        [Namespace(nameof(System), nameof(System.Windows), nameof(System.Windows.Controls), nameof(System.Windows.Controls.UserControl))]
        UserControl = 512,
        [Namespace(nameof(System), nameof(System.Windows), nameof(System.Windows.Media), nameof(System.Windows.Media.Visual))]
        Visual = 1024,
        [Hidden]
        All = Base | ContentControl | Control | DependencyObject | DispatcherObject | FrameworkElement | ItemsControl | Panel | UIElement | UserControl | Visual
    }
}