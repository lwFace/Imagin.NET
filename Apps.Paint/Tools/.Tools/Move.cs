using Imagin.Common;
using Imagin.Common.Input;
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
    public class MoveTool : Tool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Arrow.png").OriginalString;

        [field: NonSerialized]
        Vector2<int> point1;

        [field: NonSerialized]
        Vector2<int> point2;

        int? x = null;
        public int? X
        {
            get => x;
            set => this.Change(ref x, value);
        }

        int? y = null;
        public int? Y
        {
            get => y;
            set => this.Change(ref y, value);
        }

        void MoveLine(Point point)
        {
            var x1 = MouseDown.Value.X - point.X;
            x1 = x1 < 0 ? point1.X + x1.Absolute() : point1.X - x1.Absolute();

            var y1 = MouseDown.Value.Y - point.Y;
            y1 = y1 < 0 ? point1.Y + y1.Absolute() : point1.Y - y1.Absolute();

            var x2 = MouseDown.Value.X - point.X;
            x2 = x2 < 0 ? point2.X + x2.Absolute() : point2.X - x2.Absolute();

            var y2 = MouseDown.Value.Y - point.Y;
            y2 = y2 < 0 ? point2.Y + y2.Absolute() : point2.Y - y2.Absolute();

            var layer = (LineLayer)Layer;
            layer.X = x1.Int32();
            layer.Y = y1.Int32();
            layer.X2 = x2.Int32();
            layer.Y2 = y2.Int32();
        }

        void MovePixels(Point point)
        {
            var x = MouseDown.Value.X - point.X;
            var y = MouseDown.Value.Y - point.Y;

            x = x < 0 ? point1.X + x.Absolute() : point1.X - x.Absolute();
            y = y < 0 ? point1.Y + y.Absolute() : point1.Y - y.Absolute();

            Layer.X = x.Round().Int32();
            Layer.Y = y.Round().Int32();
        }

        void MoveShape(Point point)
        {
            var x = MouseDown.Value.X - point.X;
            var y = MouseDown.Value.Y - point.Y;

            x = x < 0 ? point1.X + x.Absolute() : point1.X - x.Absolute();
            y = y < 0 ? point1.Y + y.Absolute() : point1.Y - y.Absolute();

            var layer = (RegionShapeLayer)Layer;

            var region = layer.Region;
            region.X = x.Int32();
            region.Y = y.Int32();

            layer.Render(region);
        }

        void Resize(PixelLayer layer)
        {
            //1.Check if layer position and size covers entire document
            if (layer.X > 0 || layer.Y > 0 || layer.X + layer.Pixels.PixelWidth < Document.Width || layer.Y + layer.Pixels.PixelHeight < Document.Height)
            {
                WriteableBitmap oldPixels = layer.Pixels, newPixels = null;

                //2. Get a height and width that allows the layer to cover the entire document based on it's current position
                var newHeight
                    = layer.Y > 0
                    ? layer.Y + layer.Pixels.PixelHeight
                    : layer.Pixels.PixelHeight + (Document.Height - (layer.Y + layer.Pixels.PixelHeight)).Minimum(0);
                var newWidth
                    = layer.X > 0
                    ? layer.X + layer.Pixels.PixelWidth
                    : layer.Pixels.PixelWidth + (Document.Width - (layer.X + layer.Pixels.PixelWidth)).Minimum(0);

                //3. Create a new image
                newPixels = BitmapFactory.WriteableBitmap(newHeight, newWidth);

                //4. Write old pixels onto new pixels based on the layer's position relative to the document!
                newPixels.ForEach((x, y, color) =>
                {
                    if (layer.X > 0 && layer.Y > 0)
                    {
                        if (x >= layer.X && y >= layer.Y)
                            return oldPixels.GetPixel(x - layer.X, y - layer.Y);
                    }
                    else if (layer.X > 0 && layer.Y < 0)
                    {
                        if (x >= layer.X && y <= layer.Pixels.PixelHeight)
                            return oldPixels.GetPixel(x - layer.X, y);
                    }
                    else if (layer.X < 0 && layer.Y > 0)
                    {
                        if (x <= layer.Pixels.PixelWidth && y >= layer.Y)
                            return oldPixels.GetPixel(x, y - layer.Y);
                    }
                    else if (layer.X < 0 && layer.Y < 0)
                    {
                        if (x <= layer.Pixels.PixelWidth && y <= layer.Pixels.PixelHeight)
                            return oldPixels.GetPixel(x, y);
                    }
                    return Colors.Transparent;
                });

                layer.Pixels = newPixels;
                if (layer.X > 0)
                    layer.X = 0;

                if (layer.Y > 0)
                    layer.Y = 0;
            }
        }

        public override bool OnMouseDown(Point point)
        {
            if (Layer != null)
            {
                base.OnMouseDown(point);

                int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                switch (Layer.Type)
                {
                    case LayerType.Pixel:

                        x1 = Layer.Position.X.Int32();
                        y1 = Layer.Position.Y.Int32();
                        break;

                    case LayerType.CustomShape:
                    case LayerType.Ellipse:
                    case LayerType.Polygon:
                    case LayerType.Rectangle:
                    case LayerType.RoundedRectangle:
                    case LayerType.Text:

                        var topLeft = ((RegionShapeLayer)Layer).Region.TopLeft;
                        x1 = topLeft.X;
                        y1 = topLeft.Y;
                        break;

                    case LayerType.Line:

                        var linearPoint = ((LineLayer)Layer).Point;
                        x1 = linearPoint.X1;
                        y1 = linearPoint.Y1;
                        x2 = linearPoint.X2;
                        y2 = linearPoint.Y2;
                        break;
                }

                point1 = new Vector2<int>(x1, y1);
                point2 = new Vector2<int>(x2, y2);

                X = 0;
                Y = 0;
            }
            return false;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                switch (Layer.Type)
                {
                    case LayerType.Pixel:
                        MovePixels(point);
                        break;

                    case LayerType.CustomShape:
                    case LayerType.Ellipse:
                    case LayerType.Polygon:
                    case LayerType.Rectangle:
                    case LayerType.RoundedRectangle:
                    case LayerType.Text:
                        MoveShape(point);
                        break;

                    case LayerType.Line:
                        MoveLine(point);
                        break;
                }

                X = (MouseMove.Value.X - MouseDown.Value.X).Round().Int32();
                Y = (MouseMove.Value.Y - MouseDown.Value.Y).Round().Int32();
            }
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            point1 = null;

            if (Layer is PixelLayer)
                Resize((PixelLayer)Layer);

            X = null;
            Y = null;
        }

        public override string ToString() => "Move";

        ICommand alignTopEdgesCommand;
        public ICommand AlignTopEdgesCommand
        {
            get
            {
                alignTopEdgesCommand = alignTopEdgesCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return alignTopEdgesCommand;
            }
        }

        ICommand alignVerticalCentersCommand;
        public ICommand AlignVerticalCentersCommand
        {
            get
            {
                alignVerticalCentersCommand = alignVerticalCentersCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return alignVerticalCentersCommand;
            }
        }

        ICommand alignBottomEdgesCommand;
        public ICommand AlignBottomEdgesCommand
        {
            get
            {
                alignBottomEdgesCommand = alignBottomEdgesCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return alignBottomEdgesCommand;
            }
        }

        ICommand alignLeftEdgesCommand;
        public ICommand AlignLeftEdgesCommand
        {
            get
            {
                alignLeftEdgesCommand = alignLeftEdgesCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return alignLeftEdgesCommand;
            }
        }

        ICommand alignHorizontalCentersCommand;
        public ICommand AlignHorizontalCentersCommand
        {
            get
            {
                alignHorizontalCentersCommand = alignHorizontalCentersCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return alignHorizontalCentersCommand;
            }
        }

        ICommand alignRightEdgesCommand;
        public ICommand AlignRightEdgesCommand
        {
            get
            {
                alignRightEdgesCommand = alignRightEdgesCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return alignRightEdgesCommand;
            }
        }

        ICommand distributeTopEdgesCommand;
        public ICommand DistributeTopEdgesCommand
        {
            get
            {
                distributeTopEdgesCommand = distributeTopEdgesCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return distributeTopEdgesCommand;
            }
        }

        ICommand distributeVerticalCentersCommand;
        public ICommand DistributeVerticalCentersCommand
        {
            get
            {
                distributeVerticalCentersCommand = distributeVerticalCentersCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return distributeVerticalCentersCommand;
            }
        }

        ICommand distributeBottomEdgesCommand;
        public ICommand DistributeBottomEdgesCommand
        {
            get
            {
                distributeBottomEdgesCommand = distributeBottomEdgesCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return distributeBottomEdgesCommand;
            }
        }

        ICommand distributeLeftEdgesCommand;
        public ICommand DistributeLeftEdgesCommand
        {
            get
            {
                distributeLeftEdgesCommand = distributeLeftEdgesCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return distributeLeftEdgesCommand;
            }
        }

        ICommand distributeHorizontalCentersCommand;
        public ICommand DistributeHorizontalCentersCommand
        {
            get
            {
                distributeHorizontalCentersCommand = distributeHorizontalCentersCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return distributeHorizontalCentersCommand;
            }
        }

        ICommand distributeRightEdgesCommand;
        public ICommand DistributeRightEdgesCommand
        {
            get
            {
                distributeRightEdgesCommand = distributeRightEdgesCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return distributeRightEdgesCommand;
            }
        }

        ICommand autoAlignLayersCommand;
        public ICommand AutoAlignLayersCommand
        {
            get
            {
                autoAlignLayersCommand = autoAlignLayersCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return autoAlignLayersCommand;
            }
        }
    }
}