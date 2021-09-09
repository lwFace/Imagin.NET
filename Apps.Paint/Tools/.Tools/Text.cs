using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class TextTool : RegionShapeTool<TextLayer>
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Rename.png").OriginalString;

        SolidColorBrush fontColor = Brushes.Black;
        public SolidColorBrush FontColor
        {
            get => fontColor;
            set => this.Change(ref fontColor, value);
        }

        FontFamily fontFamily = new FontFamily("Arial");
        public FontFamily FontFamily
        {
            get => fontFamily;
            set => this.Change(ref fontFamily, value);
        }

        double fontSize = 12.0;
        public double FontSize
        {
            get => fontSize;
            set => this.Change(ref fontSize, value);
        }

        System.Drawing.FontStyle fontStyle = System.Drawing.FontStyle.Regular;
        public System.Drawing.FontStyle FontStyle
        {
            get => fontStyle;
            set => this.Change(ref fontStyle, value);
        }

        public ObservableCollection<System.Drawing.FontStyle> FontStyles => new ObservableCollection<System.Drawing.FontStyle>()
        {
            System.Drawing.FontStyle.Bold,
            System.Drawing.FontStyle.Italic,
            System.Drawing.FontStyle.Regular,
            System.Drawing.FontStyle.Strikeout,
            System.Drawing.FontStyle.Underline,
        };

        TextAlignment horizontalTextAlignment = TextAlignment.Left;
        public TextAlignment HorizontalTextAlignment
        {
            get => horizontalTextAlignment;
            set => this.Change(ref horizontalTextAlignment, value);
        }

        VerticalAlignment verticalTextAlignment = VerticalAlignment.Top;
        public VerticalAlignment VerticalTextAlignment
        {
            get => verticalTextAlignment;
            set => this.Change(ref verticalTextAlignment, value);
        }

        protected override TextLayer NewLayer()
            => new TextLayer("Untitled", Document.Height, Document.Width, fontColor, fontFamily, fontSize, FontStyle, "Some text", horizontalTextAlignment, verticalTextAlignment);

        Int32Region region;

        CustomPath selection;

        public override bool OnMouseDown(Point point)
        {
            MouseDown = point;

            selection = new CustomPath();
            selection.Points = new PointCollection()
            {
                new Point(point.X, point.Y),
                new Point(point.X + 1, point.Y),
                new Point(point.X + 1, point.Y + 1),
                new Point(point.X, point.Y + 1),
            };
            Document.Selections.Add(selection);
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            MouseMove = point;
            if (MouseDown != null)
            {
                region = CalculateRegion(MouseDown.Value, point);
                selection.Points = new PointCollection()
                {
                    new Point(region.X, region.Y),
                    new Point(region.X + region.Width, region.Y),
                    new Point(region.X + region.Width, region.Y + region.Height),
                    new Point(region.X, region.Y + region.Height),
                };
            }
        }

        public override void OnMouseUp(Point point)
        {
            if (region == null)
                return;

            Document.Layers.ForEach(i => i.IsSelected = false);

            var newLayer = NewLayer();
            newLayer.IsSelected = true;

            newLayer.Render(region);

            LayerCollection layers = Layer == null || Layer.Parent == null ? Document.Layers : Layer.Parent.Layers;
            newLayer.Parent = Layer?.Parent; 

            var index = Layer == null || Layer.Parent == null ? 0 : layers.IndexOf(Layer);
            layers.Insert(index, newLayer);

            Document.Selections.Remove(selection);
            selection = null;

            region = null;
            MouseDown = null;
            MouseMove = null;
        }

        public override string ToString() => "Ellipse";
    }
}