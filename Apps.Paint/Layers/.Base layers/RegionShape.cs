using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public abstract class RegionShapeLayer : ShapeLayer
    {
        string fill = $"255,0,0,0";
        public virtual SolidColorBrush Fill
        {
            get
            {
                var result = fill.Split(',');
                var actualResult = default(SolidColorBrush);
                App.Current.Dispatcher.Invoke(() => actualResult = new SolidColorBrush(System.Windows.Media.Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte())));
                return actualResult;
            }
            set => this.Change(ref fill, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        [Hidden]
        public System.Drawing.SolidBrush FillBrush => new System.Drawing.SolidBrush(WithOpacity(Fill.Color));

        protected int height = 0;
        [Range(1, 5000, 1)]
        [RangeFormat(Imagin.Common.Data.RangeFormat.UpDown)]
        public int Height
        {
            get => height;
            set => this.Change(ref height, value);
        }

        protected int width = 0;
        [Range(1, 5000, 1)]
        [RangeFormat(Imagin.Common.Data.RangeFormat.UpDown)]
        public int Width
        {
            get => width;
            set => this.Change(ref width, value);
        }

        [Hidden]
        public override PointCollection Bounds
        {
            get
            {
                var result = new PointCollection();
                result.Add(new System.Windows.Point(X, Y));
                result.Add(new System.Windows.Point(X + width, Y));
                result.Add(new System.Windows.Point(X + width, Y + height));
                result.Add(new System.Windows.Point(X, Y + height));
                return result;
            }
        }

        [Hidden]
        public PointCollection ShapeBounds
        {
            get
            {
                var result = new PointCollection();
                result.Add(new System.Windows.Point());
                result.Add(new System.Windows.Point());
                result.Add(new System.Windows.Point());
                result.Add(new System.Windows.Point());
                return result;
            }
        }
        
        [Hidden]
        public override Int32Region Region => new Int32Region(X, Y, width, height);

        protected RegionShapeLayer(LayerType type, string name, int height, int width, SolidColorBrush fill, SolidColorBrush stroke, double strokeThickness) : base(type, name, height, width, stroke, strokeThickness)
        {
            this.fill = $"{fill.Color.A},{fill.Color.R},{fill.Color.G},{fill.Color.B}";
            this.height = height;
            this.width = width;
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
            //Flip all the points within the bounds of the shape!
        }

        public override void Rotate(int degrees)
        {
            //Rotate all the points within the bounds of the shape!
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Fill):
                case nameof(Height):
                case nameof(Width):
                    Render();
                    break;
            }
        }

        public virtual void Render(Int32Region region)
        {
            ignoreRender = true;
            Height = region.Height;
            Width = region.Width;
            X = region.X;
            Y = region.Y;
            ignoreRender = false;

            Render();
        }
    }
}