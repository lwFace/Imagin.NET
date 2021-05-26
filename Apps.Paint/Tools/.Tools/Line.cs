using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class LineTool : ShapeTool<LineLayer>
    {
        PixelLayer pixelLayer;

        LineLayer lineLayer;

        public override string Icon => Resources.Uri(nameof(Paint), "Images/Line.png").OriginalString;

        string stroke = $"255,0,0,0";
        public SolidColorBrush Stroke
        {
            get
            {
                var result = stroke.Split(',');
                return new SolidColorBrush(Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte()));
            }
            set => this.Change(ref stroke, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        double strokeThickness = 1.0;
        public double StrokeThickness
        {
            get => strokeThickness;
            set => this.Change(ref strokeThickness, value);
        }

        protected override LineLayer NewLayer() 
            => new LineLayer("Untitled", Document.Height, Document.Width, Stroke, StrokeThickness);

        public override bool OnMouseDown(Point point)
        {
            if (!base.OnMouseDown(point))
                return false;

            var x = point.X.Int32();
            var y = point.Y.Int32();

            if (Mode == ShapeToolModes.Pixels)
            {
                lineLayer = NewLayer();
                lineLayer.X = x;
                lineLayer.Y = y;
                lineLayer.X2 = x;
                lineLayer.Y2 = y;

                pixelLayer = (PixelLayer)this.Layer;
                pixelLayer.Pixels = lineLayer.Preview;
            }

            if (Mode == ShapeToolModes.Shape)
            {
                Document.Layers.ForEach(i => i.IsSelected = false);

                CurrentLayer = NewLayer();
                CurrentLayer.IsSelected = true;

                CurrentLayer.X = x;
                CurrentLayer.Y = y;
                CurrentLayer.X2 = x;
                CurrentLayer.Y2 = y;
                Layers.Insert(Layer != null ? Layers.IndexOf(Layer) : 0, CurrentLayer);
            }
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                var p = new Stroke<int>(point.X.Int32(), point.Y.Int32(), MouseDown.Value.X.Int32(), MouseDown.Value.Y.Int32());

                if (Mode == ShapeToolModes.Pixels)
                {
                    lineLayer.X = p.X1;
                    lineLayer.Y = p.Y1;
                    lineLayer.X2 = p.X2;
                    lineLayer.Y2 = p.Y2;
                    pixelLayer.Pixels = lineLayer.Preview;
                }

                if (Mode == ShapeToolModes.Shape)
                {
                    CurrentLayer.X = p.X1;
                    CurrentLayer.Y = p.Y1;
                    CurrentLayer.X2 = p.X2;
                    CurrentLayer.Y2 = p.Y2;
                }
            }
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            if (Mode == ShapeToolModes.Pixels)
            {
                lineLayer.Render(pixelLayer.Pixels);
                lineLayer = null;
                pixelLayer = null;
            }

            if (Mode == ShapeToolModes.Shape)
                CurrentLayer = null;
        }

        public override string ToString() => "Line";
    }
}