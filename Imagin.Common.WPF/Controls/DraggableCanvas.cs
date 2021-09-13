using Imagin.Common.Linq;
using Imagin.Common.Math;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// A canvas that supports dragging. <see cref="Panel.Children"/> must implement <see cref="IPoint2D"/>, <see cref="ISize"/>, and <see cref="ISelectable"/> (and, optionally, <see cref="ILockable"/>).
    /// </summary>
    public class DraggableCanvas : Canvas
    {
        Point? origin;

        Point? start;

        FrameworkElement frameworkElement = null;

        ISelectable iSelectable = null;

        IPoint2D i2DPoint
            => iSelectable as IPoint2D;

        ISize iSize
            => iSelectable as ISize;

        ILockable iLockable 
            => iSelectable as ILockable;

        public static DependencyProperty SnapToGridProperty = DependencyProperty.Register(nameof(SnapToGrid), typeof(double), typeof(DraggableCanvas), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double SnapToGrid
        {
            get => (double)GetValue(SnapToGridProperty);
            set => SetValue(SnapToGridProperty, value);
        }

        public DraggableCanvas() : base()
        {
            SnapsToDevicePixels = true;
        }

        ISelectable GetSelectable(FrameworkElement frameworkElement)
        {
            DependencyObject parent = frameworkElement;
            while (!(parent is FrameworkElement) || !((parent as FrameworkElement).DataContext is ISelectable))
            {
                if (parent is System.Windows.Controls.TextBox)
                    return null;

                var nextParent = VisualTreeHelper.GetParent(parent);
                if (nextParent is Canvas)
                    break;

                parent = nextParent;
            }
            return (parent as FrameworkElement).DataContext as ISelectable;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            var startPoint = e.GetPosition(this);

            var target = VisualTreeHelper.HitTest(this, startPoint);
            if (target?.VisualHit is FrameworkElement)
            {
                //Get the element that was clicked
                frameworkElement = target.VisualHit as FrameworkElement;

                iSelectable = GetSelectable(frameworkElement);
                if (iSelectable == null)
                    return;

                foreach (FrameworkElement i in Children)
                {
                    if (i.DataContext is ISelectable)
                    {
                        var select = i.DataContext.Equals(iSelectable);
                        //Unselect everything except the current
                        i.DataContext.As<ISelectable>().IsSelected = select;
                        //Bring the current to the front
                        SetZIndex(i, select ? 1 : 0);
                    }
                }

                //Allow dragging elements only when left mouse button is pressed
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (iSelectable is IPoint2D && iLockable?.IsLocked != true)
                    {
                        origin = new Point(i2DPoint.Position.X, i2DPoint.Position.Y);
                        start = startPoint;
                        frameworkElement.CaptureMouse();
                    }
                }
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (start != null)
            {
                var current = e.GetPosition(this);
                var offset = new Point(frameworkElement.RenderTransform.Value.OffsetX, frameworkElement.RenderTransform.Value.OffsetY);

                var vector = current - start;
                if (vector != null)
                {
                    var result = vector.Value.BoundSize(origin, offset, new Size(ActualWidth, ActualHeight), new Size(iSize.Size.Width, iSize.Size.Height), SnapToGrid);
                    i2DPoint.Position = result;
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            start = null;

            if (frameworkElement != null)
            {
                frameworkElement.ReleaseMouseCapture();
                frameworkElement = null;
            }
        }
    }
}