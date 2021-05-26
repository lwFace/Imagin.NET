using Imagin.Common.Math;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class DragSelection : UserControl
    {
        public static DependencyProperty SelectionProperty = DependencyProperty.Register(nameof(Selection), typeof(DoubleRegion), typeof(DragSelection), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DoubleRegion Selection
        {
            get => (DoubleRegion)GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }

        public DragSelection() : base()=> DefaultStyleKey = typeof(DragSelection);
    }
}