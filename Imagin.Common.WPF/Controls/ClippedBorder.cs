using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class ClippedBorder : Border
    {
        RectangleGeometry clip = new RectangleGeometry();

        object oldClip = null;

        public override UIElement Child
        {
            get => base.Child;
            set
            {
                if (Child != value)
                {
                    if (Child != null)
                        Child.SetValue(ClipProperty, oldClip);

                    oldClip = value != null ? value.ReadLocalValue(ClipProperty) : null;
                    base.Child = value;
                }
            }
        }

        public ClippedBorder() : base() { }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            OnApplyClip();
            base.OnRender(dc);
        }

        protected virtual void OnApplyClip()
        {
            if (Child != null)
            {
                clip.RadiusX = clip.RadiusY = System.Math.Max(0.0, CornerRadius.TopLeft - (BorderThickness.Left * 0.5));
                clip.Rect = new Rect(Child.RenderSize);
                Child.Clip = clip;
            }
        }
    }
}