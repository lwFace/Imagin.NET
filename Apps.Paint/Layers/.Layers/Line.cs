using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class LineLayer : ShapeLayer
    {
        [DisplayName("X1")]
        [Range(-5000, 5000, 1)]
        public override int X
        {
            get => base.X;
            set => base.X = value;
        }

        [DisplayName("Y1")]
        [Range(-5000, 5000, 1)]
        public override int Y
        {
            get => base.Y;
            set => base.Y = value;
        }

        int x2 = 0;
        [Range(-5000, 5000, 1)]
        public int X2
        {
            get => x2;
            set => this.Change(ref x2, value);
        }

        int y2 = 0;
        [Range(-5000, 5000, 1)]
        public int Y2
        {
            get => y2;
            set => this.Change(ref y2, value);
        }

        [Hidden]
        public Stroke<int> Point => new Stroke<int>(x, y, x2, y2);

        [Hidden]
        public override PointCollection Bounds => new PointCollection();

        [Hidden]
        public override Int32Region Region => new Int32Region(0, 0, 0, 0);

        public LineLayer(string name, int height, int width, SolidColorBrush stroke, double strokeThickness) : base(LayerType.Line, name, height, width, stroke, strokeThickness)
        {
        }

        public sealed override Layer Clone()
        {
            return new LineLayer(Name, Preview.PixelHeight, Preview.PixelWidth, Stroke, StrokeThickness)
            {
                IsLocked = IsLocked,
                IsVisible = IsVisible,
                Opacity = Opacity,
                Style = Style.Clone(),
                X = X,
                Y = Y,
                X2 = X2,
                Y2 = Y2
            };
        }

        public override void Crop(int x, int y, int height, int width)
        {
            var dx = x - X;
            var dy = y - Y;
            X -= x;
            Y -= y;
            X2 -= x;
            Y2 -= y;
        }

        public override void Crop(Int32Size oldSize, Int32Size newSize, CardinalDirection direction)
        {
            /*
            if (height < Height)
            {
                //We don't have to worry about resizing layers vertically
            }
            else if (height > Height)
            {
                //Resize each layer vertically...
                foreach (var i in Layers)
                {
                    if (i is VisualLayer)
                    {
                        //...if the height of the layer is less than the new canvas height.
                        if ((i as VisualLayer).Height < height)
                        {
                            //Set i.Height to height
                        }
                    }
                }
            }

            if (width < Width)
            {
                //We don't have to worry about resizing layers horizontally
            }
            else if (width > Width)
            {
                //Resize all layers horizontally...
                foreach (var i in Layers)
                {
                    if (i is VisualLayer)
                    {
                        //...if the width of the layer is less than the new canvas width.
                        if ((i as VisualLayer).Width < width)
                        {
                            //Set i.Width to width
                        }
                    }
                }
            }

            //Now we have to apply the anchor
            foreach (var i in Layers)
            {
                switch (direction)
                {
                    case CardinalDirection.E:
                        break;
                    case CardinalDirection.N:
                        break;
                    case CardinalDirection.NE:
                        break;
                    case CardinalDirection.NW:
                        break;
                    case CardinalDirection.Origin:
                        break;
                    case CardinalDirection.S:
                        break;
                    case CardinalDirection.SE:
                        break;
                    case CardinalDirection.SW:
                        break;
                    case CardinalDirection.W:
                        break;
                }
            }
            */
        }

        public override IEnumerable<System.Windows.Point> GetPoints()
        {
            yield return new System.Windows.Point(x, y);
            yield return new System.Windows.Point(x2, y2);
        }

        public override void Flip(System.Windows.Media.Imaging.WriteableBitmapExtensions.FlipMode flipMode)
        {
            throw new NotImplementedException();
        }

        public override void Rotate(int degrees)
        {
            throw new NotImplementedException();
        }

        public override void Render(System.Drawing.Graphics g)
        {
            g.DrawLine(new System.Drawing.Pen(WithOpacity(Stroke.Color), StrokeThickness.Single()), x.Single(), y.Single(), x2.Single(), y2.Single());
        }

        public override void Render(WriteableBitmap input)
        {
            input.DrawLineAa(x, y, x2, y2, Stroke.Color, StrokeThickness.Int32());
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(X2):
                case nameof(Y2):
                    Render();
                    break;
            }
        }

        public override string ToString() => "Line";
    }
}