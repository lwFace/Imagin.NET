using Imagin.Common.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// A chip for displaying and selecting a <see cref="Brush"/>.
    /// </summary>
    public abstract class ChipBase : Control
    {
        MouseEvent _dialogEvent = MouseEvent.MouseDown;
        public MouseEvent DialogEvent
        {
            get => _dialogEvent;
            set => _dialogEvent = value;
        }

        public static DependencyProperty InnerBorderBrushProperty = DependencyProperty.Register(nameof(InnerBorderBrush), typeof(Brush), typeof(ChipBase), new PropertyMetadata(default(Brush)));
        public Brush InnerBorderBrush
        {
            get => (Brush)GetValue(InnerBorderBrushProperty);
            set => SetValue(InnerBorderBrushProperty, value);
        }

        public static DependencyProperty InnerBorderThicknessProperty = DependencyProperty.Register(nameof(InnerBorderThickness), typeof(Thickness), typeof(ChipBase), new PropertyMetadata(default(Thickness)));
        public Thickness InnerBorderThickness
        {
            get => (Thickness)GetValue(InnerBorderThicknessProperty);
            set => SetValue(InnerBorderThicknessProperty, value);
        }

        public static DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(ChipBase), new PropertyMetadata("Color"));
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public ChipBase() : base() => DefaultStyleKey = typeof(ChipBase);

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (DialogEvent == MouseEvent.MouseDoubleClick)
            {
                ShowDialog();
                e.Handled = true;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (DialogEvent == MouseEvent.MouseDown)
            {
                ShowDialog();
                e.Handled = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (DialogEvent == MouseEvent.MouseUp)
            {
                ShowDialog();
                e.Handled = true;
            }
        }

        public abstract bool? ShowDialog();
    }
}