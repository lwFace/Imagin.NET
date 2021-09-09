using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class CloneStampTool : EffectBrushTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/CloneStamp.png").OriginalString;

        System.Drawing.Point2D offset = new System.Drawing.Point2D(-10, 0);
        public System.Drawing.Point2D Offset
        {
            get => offset;
            set => this.Change(ref offset, value);
        }

        Stroke<int> offsetPreview = new Stroke<int>(0, 0, 0, 0);
        public Stroke<int> OffsetPreview
        {
            get => offsetPreview;
            set => this.Change(ref offsetPreview, value);
        }

        protected override Color Apply(int x, int y, Color color, double factor)
        {
            var x2 = x + Offset.X;
            var y2 = y + Offset.Y;

            if (x2 >= 0 && x2 < Bitmap.PixelWidth && y2 >= 0 && y2 < Bitmap.PixelHeight)
                return VisualLayer.Blend(Mode, color, Bitmap.GetPixel(x2, y2).A(factor.Multiply(255)));

            return color;
        }

        bool drawOffset = false;

        public override void OnPreviewRendered(DrawingContext input, double zoom)
        {
            base.OnPreviewRendered(input, zoom);
            if (drawOffset)
            {
                input.DrawLine(new Pen(System.Windows.Media.Brushes.Black, 1 / zoom), new Point(OffsetPreview.X1, OffsetPreview.Y1), new Point(OffsetPreview.X2, OffsetPreview.Y2));
                input.DrawLine(new Pen(System.Windows.Media.Brushes.White, 0.5 / zoom), new Point(OffsetPreview.X1, OffsetPreview.Y1), new Point(OffsetPreview.X2, OffsetPreview.Y2));
            }
        }

        public override bool OnMouseDown(Point point)
        {
            if (ModifierKeys.Alt.Pressed())
            {
                MouseDown = point;
                drawOffset = true;
                return true;
            }
            return base.OnMouseDown(point);
        }

        public override void OnMouseMove(Point point)
        {
            if (drawOffset)
            {
                MouseMove = point;

                OffsetPreview = new Stroke<int>(MouseDown.Value.X.Round().Int32(), MouseDown.Value.Y.Round().Int32(), MouseMove.Value.X.Round().Int32(), MouseMove.Value.Y.Round().Int32());
                Offset.X = (MouseDown.Value.X - MouseMove.Value.X).Round().Int32();
                Offset.Y = (MouseDown.Value.Y - MouseMove.Value.Y).Int32();
                return;
            }
            base.OnMouseMove(point);
        }

        public override void OnMouseUp(Point point)
        {
            if (drawOffset)
            {
                drawOffset = false;
                OffsetPreview = new Stroke<int>(0, 0, 0, 0);
            }

            base.OnMouseUp(point);
        }

        public override string ToString() => "Clone stamp";
    }
}