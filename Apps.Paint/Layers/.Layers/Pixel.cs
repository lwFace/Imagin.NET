using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Math;
using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class PixelLayer : VisualLayer
    {
        Matrix<Argb> preservedPixels;

        [Hidden]
        public override PointCollection Bounds
        {
            get
            {
                var result = new PointCollection();
                result.Add(new System.Windows.Point(X, Y));
                result.Add(new System.Windows.Point(X + Pixels.PixelWidth, Y));
                result.Add(new System.Windows.Point(X + Pixels.PixelWidth, Y + Pixels.PixelHeight));
                result.Add(new System.Windows.Point(X, Y + Pixels.PixelHeight));
                return result;
            }
        }

        [Hidden]
        public override Int32Region Region => new Int32Region(X, Y, pixels.PixelWidth, pixels.PixelHeight);

        [field: NonSerialized]
        WriteableBitmap pixels;
        [Hidden]
        public override WriteableBitmap Pixels
        {
            get => pixels;
            set => this.Change(ref pixels, value);
        }

        public PixelLayer() : this(string.Empty, null) { }

        public PixelLayer(string name, WriteableBitmap pixels) : base(LayerType.Pixel, name)
        {
            Pixels = pixels;
        }

        public PixelLayer(string name, Size size) : this(name, default(WriteableBitmap))
        {
            Pixels = BitmapFactory.WriteableBitmap(size);
        }

        public override Layer Clone()
        {
            return new PixelLayer()
            {
                IsLocked = IsLocked,
                IsVisible = IsVisible,
                Opacity = Opacity,
                Pixels = Imagin.Common.Linq.WriteableBitmapExtensions.Clone(Pixels),
                Style = Style.Clone(),
                X = X,
                Y = Y,
            };
        }

        public void Combine(WriteableBitmap input)
        {
            for (var x = 0; x < Pixels.PixelWidth; x++)
            {
                for (var y = 0; y < Pixels.PixelHeight; y++)
                {
                    var color = input.GetPixel(x, y);
                    if (color.A > 0)
                        Pixels.SetPixel(x, y, color);
                }
            }
        }

        public override void Crop(int x, int y, int height, int width)
        {
            var dx = x - X;
            var dy = y - Y;
            X -= x;
            Y -= y;
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

        public override void Flip(System.Windows.Media.Imaging.WriteableBitmapExtensions.FlipMode flipMode)
        {
            Pixels = Pixels.Flip(flipMode);
        }

        public override void Rotate(int degrees)
        {
            Pixels = Pixels.RotateFree(degrees);
        }

        public override void Render(Graphics g)
        {
            var result = WithOpacity(pixels).Bitmap();
            g.DrawImage(result, 0, 0);
        }

        public override void Resize(Int32Size oldSize, Int32Size newSize, Interpolations interpolation)
        {
            Pixels = Pixels.Resize((Pixels.PixelWidth.Double() / (oldSize.Width.Double() / newSize.Width.Double())).Int32(), (Pixels.PixelHeight.Double() / (oldSize.Height.Double() / oldSize.Width.Double())).Int32(), interpolation);
        }

        public override string ToString() => "Pixels";

        public override void Preserve()
        {
            base.Preserve();
            preservedPixels = From(pixels);
        }

        public override void Restore()
        {
            base.Restore();
            Pixels = From(preservedPixels);
        }
    }
}