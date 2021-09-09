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
    public abstract class RegionShapeTool<Layer> : ShapeTool<Layer> where Layer : RegionShapeLayer
    {
        PixelLayer pixelLayer;

        RegionShapeLayer shapeLayer;

        string fill = $"255,0,0,0";
        public SolidColorBrush Fill
        {
            get
            {
                var result = fill.Split(',');
                return new SolidColorBrush(Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte()));
            }
            set => this.Change(ref fill, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

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

        public override bool OnMouseDown(Point point)
        {
            if (!base.OnMouseDown(point))
                return false;

            if (Mode == ShapeToolModes.Pixels)
            {
                shapeLayer = NewLayer();
                shapeLayer.Render(new Int32Region(point.X.Int32(), point.Y.Int32(), 0, 0));

                pixelLayer = (PixelLayer)this.Layer;
                pixelLayer.Pixels = shapeLayer.Preview;
            }

            if (Mode == ShapeToolModes.Shape)
            {
                Document.Layers.ForEach(i => i.IsSelected = false);

                CurrentLayer = NewLayer();
                CurrentLayer.IsSelected = true;

                CurrentLayer.Render(new Int32Region(point.X.Int32(), point.Y.Int32(), 0, 0));
                Layers.Insert(this.Layer != null ? Layers.IndexOf(this.Layer) : 0, CurrentLayer);
            }

            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                if (Mode == ShapeToolModes.Pixels)
                {
                    shapeLayer.Render(CalculateRegion(MouseDown.Value, point));
                    pixelLayer.Pixels = shapeLayer.Preview;
                }

                if (Mode == ShapeToolModes.Shape)
                    CurrentLayer.Render(CalculateRegion(MouseDown.Value, point));
            }
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            if (Mode == ShapeToolModes.Pixels)
            {
                shapeLayer.Render(pixelLayer.Pixels);
                shapeLayer = null;
                pixelLayer = null;
            }

            if (Mode == ShapeToolModes.Shape)
                CurrentLayer = null;
        }
    }
}