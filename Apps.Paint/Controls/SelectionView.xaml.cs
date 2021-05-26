using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint
{
    public partial class SelectionView : UserControl
    {
        public static DependencyProperty SelectionsProperty = DependencyProperty.Register(nameof(Selections), typeof(object), typeof(SelectionView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public object Selections
        {
            get => (object)GetValue(SelectionsProperty);
            set => SetValue(SelectionsProperty, value);
        }

        public static DependencyProperty ShapeContextMenuProperty = DependencyProperty.Register(nameof(ShapeContextMenu), typeof(ContextMenu), typeof(SelectionView), new FrameworkPropertyMetadata(default(ContextMenu), FrameworkPropertyMetadataOptions.None));
        public ContextMenu ShapeContextMenu
        {
            get => (ContextMenu)GetValue(ShapeContextMenuProperty);
            set => SetValue(ShapeContextMenuProperty, value);
        }

        public static DependencyProperty ShapeCursorProperty = DependencyProperty.Register(nameof(ShapeCursor), typeof(Cursor), typeof(SelectionView), new FrameworkPropertyMetadata(default(Cursor), FrameworkPropertyMetadataOptions.None));
        public Cursor ShapeCursor
        {
            get => (Cursor)GetValue(ShapeCursorProperty);
            set => SetValue(ShapeCursorProperty, value);
        }

        public static DependencyProperty ShapeFillProperty = DependencyProperty.Register(nameof(ShapeFill), typeof(Brush), typeof(SelectionView), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.None));
        public Brush ShapeFill
        {
            get => (Brush)GetValue(ShapeFillProperty);
            set => SetValue(ShapeFillProperty, value);
        }

        public static DependencyProperty ShapeStrokePrimaryProperty = DependencyProperty.Register(nameof(ShapeStrokePrimary), typeof(Brush), typeof(SelectionView), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));
        public Brush ShapeStrokePrimary
        {
            get => (Brush)GetValue(ShapeStrokePrimaryProperty);
            set => SetValue(ShapeStrokePrimaryProperty, value);
        }

        public static DependencyProperty ShapeStrokeSecondaryProperty = DependencyProperty.Register(nameof(ShapeStrokeSecondary), typeof(Brush), typeof(SelectionView), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.None));
        public Brush ShapeStrokeSecondary
        {
            get => (Brush)GetValue(ShapeStrokeSecondaryProperty);
            set => SetValue(ShapeStrokeSecondaryProperty, value);
        }

        public static DependencyProperty ShapeStrokeDashArrayProperty = DependencyProperty.Register(nameof(ShapeStrokeDashArray), typeof(double), typeof(SelectionView), new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.None));
        public double ShapeStrokeDashArray
        {
            get => (double)GetValue(ShapeStrokeDashArrayProperty);
            set => SetValue(ShapeStrokeDashArrayProperty, value);
        }

        public static DependencyProperty ShapeStrokeThicknessProperty = DependencyProperty.Register(nameof(ShapeStrokeThickness), typeof(double), typeof(SelectionView), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None));
        public double ShapeStrokeThickness
        {
            get => (double)GetValue(ShapeStrokeThicknessProperty);
            set => SetValue(ShapeStrokeThicknessProperty, value);
        }

        public SelectionView() : base()
        {
            InitializeComponent();
        }
    }
}