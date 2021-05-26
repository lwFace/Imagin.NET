using Imagin.Common;
using Imagin.Common.Media;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Paint
{
    public enum Adaptation
    {
        None,
        Grow,
        Shrink
    }

    public class ImageViewModel : BaseObject
    {
        double height;
        public double Height
        {
            get => height;
            set => this.Change(ref height, value);
        }

        bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => this.Change(ref isSelected, value);
        }

        WriteableBitmap source;
        public WriteableBitmap Source
        {
            get => source;
            set => this.Change(ref source, value);
        }

        double width;
        public double Width
        {
            get => width;
            set => this.Change(ref width, value);
        }

        public ImageViewModel() : base() { }

        public ImageViewModel(WriteableBitmap source) : base()
        {
            Source = source;

            Height = source.PixelHeight;
            Width = source.PixelWidth;
        }
    }

    [Serializable]
    public class CollectionDocument : Imagin.Common.AvalonDock.Document
    {
        Adaptation adaptation;
        public Adaptation Adaptation
        {
            get => adaptation;
            set => this.Change(ref adaptation, value);
        }

        StringColor background = new StringColor(System.Windows.Media.Colors.White);
        public StringColor Background
        {
            get => background;
            set => this.Change(ref background, value);
        }

        LayerCollection layers;
        [Hidden]
        public LayerCollection Layers
        {
            get => layers;
            set => this.Change(ref layers, value);
        }

        Orientation orientation;
        public Orientation Orientation
        {
            get => orientation;
            set => this.Change(ref orientation, value);
        }

        double zoom;
        public double Zoom
        {
            get => zoom;
            set => this.Change(ref zoom, value);
        }
        
        public CollectionDocument() : base()
        {
            //Layers = new LayerCollection(this);
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Adaptation):
                case nameof(Orientation):
                    Adapt(Adaptation);
                    break;
            }
        }

        void Adapt(Adaptation type)
        {
            int minimum = 0, maximum = 0;
            switch (type)
            {
                case Adaptation.Grow:
                case Adaptation.Shrink:
                    Limit(out minimum, out maximum);
                    break;
            }

            Layers.Each<PixelLayer>(i =>
            {
                var length = default(int);
                switch (type)
                {
                    case Adaptation.Grow:
                        length = maximum;
                        break;
                    case Adaptation.Shrink:
                        length = minimum;
                        break;
                    case Adaptation.None:
                        i.Height = double.NaN;
                        i.Width = double.NaN;
                        return;
                }

                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        i.Height = length;
                        i.Width = double.NaN;
                        break;
                    case Orientation.Vertical:
                        i.Height = double.NaN;
                        i.Width = length;
                        break;
                }
            });
        }

        void Limit(out int minimum, out int maximum)
        {
            var mi = int.MaxValue;
            var ma = int.MinValue;
            if (Adaptation == Adaptation.Grow || Adaptation == Adaptation.Shrink)
            {
                Layers.Each<PixelLayer>(i =>
                {
                    switch (Orientation)
                    {
                        case Orientation.Horizontal:
                            switch (Adaptation)
                            {
                                case Adaptation.Grow:
                                    ma = i.Height > ma ? (int)i.Height : ma;
                                    break;
                                case Adaptation.Shrink:
                                    mi = i.Height < mi ? (int)i.Height : mi;
                                    break;
                            }
                            break;
                        case Orientation.Vertical:
                            switch (Adaptation)
                            {
                                case Adaptation.Grow:
                                    ma = (int)i.Width > ma ? (int)i.Width : ma;
                                    break;
                                case Adaptation.Shrink:
                                    mi = (int)i.Width < mi ? (int)i.Width : mi;
                                    break;
                            }
                            break;
                    }
                });
            }
            minimum = mi;
            maximum = ma;
        }
        
        /*
        Bitmap Draw(System.Drawing.Size size)
        {
            var result = new Bitmap(size.Width, size.Height);

            using (var g = Graphics.FromImage(result))
            {
                g.FillRectangle(new SolidBrush(Background.Value.Drawing()), new Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(result.Width, result.Height)));

                int x = 0, y = 0;
                for (int i = 0, Count = Files.Count; i < Count; i++)
                {
                    var file = Files[i];
                    var image = file.Source;
                    var bitmap = image.Bitmap();

                    switch (Orientation)
                    {
                        case Orientation.Horizontal:
                            g.DrawImage(bitmap, x, (size.Height / 2) - (bitmap.Height / 2));
                            x += bitmap.Width;
                            break;
                        case Orientation.Vertical:
                            g.DrawImage(bitmap, (size.Width / 2) - (bitmap.Width / 2), y);
                            y += bitmap.Height;
                            break;
                    }
                    //SetCurrentValue(ProgressProperty, (i * 100) / Count);
                }

                //SetCurrentValue(ProgressProperty, 0);
            }
            return result;
        }

        System.Drawing.Size GetSize(Orientation orientation, Adaptation adaptation)
        {
            var height = 0;
            var width = 0;

            foreach (var i in Files)
            {
                switch (orientation)
                {
                    case Orientation.Horizontal:
                        width += i.Width.Int32();
                        switch (adaptation)
                        {
                            case Adaptation.Grow:
                            case Adaptation.Shrink:
                                height = i.Height.Int32();
                                continue;
                            case Adaptation.None:
                                height = i.Height.Int32() > height ? i.Height.Int32() : height;
                                break;
                        }
                        break;
                    case Orientation.Vertical:
                        height += i.Height.Int32();
                        switch (adaptation)
                        {
                            case Adaptation.Grow:
                            case Adaptation.Shrink:
                                width = i.Width.Int32();
                                continue;
                            case Adaptation.None:
                                width = i.Width.Int32() > width ? i.Width.Int32() : width;
                                break;
                        }
                        break;
                }

            }
            return new System.Drawing.Size(width, height);
        }
        */

        public override void Save()
        {
            /*
            var error = new Action<string>(message => MessageBox.Show(message, "Save", MessageBoxButton.OK, MessageBoxImage.Error));

            if
            (
                Files.Count == 0
                ||
                !System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(Destination))
            )
            {
                error(string.Empty);
                return;
            }

            var size = GetSize(Orientation, Adaptation);

            if (size.Height == 0 || size.Width == 0)
            {
                error(string.Empty);
                return;
            }

            try
            {
                var result = Draw(size);

                if (result == null)
                    throw new NullReferenceException();

                new MagickImage(result).Write(Destination);
            }
            catch (Exception e)
            {
                error(e.Message);
            }
            */
        }
    }
}