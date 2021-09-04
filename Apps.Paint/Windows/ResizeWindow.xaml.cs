using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media;
using System.Windows.Input;

namespace Paint
{
    public partial class ResizeWindow : BaseWindow
    {
        Document Document;

        CardinalDirection anchor = CardinalDirection.Origin;
        public CardinalDirection Anchor
        {
            get => anchor;
            set => this.Change(ref anchor, value);
        }

        int bitsPerPixel = 32;
        public int BitsPerPixel
        {
            get => bitsPerPixel;
            set => this.Change(ref bitsPerPixel, value);
        }

        double currentHeight = 0;
        public double CurrentHeight
        {
            get => currentHeight;
            set
            {
                this.Change(ref currentHeight, value);
                this.Changed(() => CurrentSize);
            }
        }

        public long CurrentSize
        {
            get
            {
                var result = currentHeight * currentWidth;
                result *= bitsPerPixel / 8.0;
                return result.Int64();
            }
        }

        double currentWidth = 0;
        public double CurrentWidth
        {
            get => currentWidth;
            set
            {
                this.Change(ref currentWidth, value);
                this.Changed(() => CurrentSize);
            }
        }

        bool link = false;
        public bool Link
        {
            get => link;
            set => this.Change(ref link, value);
        }

        double newHeight = 0;
        public double NewHeight
        {
            get => newHeight;
            set
            {
                if (link)
                {
                    var newSize = new DoubleSize(newHeight, newWidth).Resize(SizeField.Height, value);
                    this.Change(ref newWidth, newSize.Width, () => NewWidth);
                }

                this.Change(ref newHeight, value);
                this.Changed(() => NewSize);
            }
        }

        public long NewSize
        {
            get
            {
                var result = newHeight * newWidth;
                result *= bitsPerPixel / 8.0;
                return result.Int64();
            }
        }

        double newWidth = 0;
        public double NewWidth
        {
            get => newWidth;
            set
            {
                if (link)
                {
                    var newSize = new DoubleSize(newHeight, newWidth).Resize(SizeField.Width, value);
                    this.Change(ref newHeight, newSize.Height, () => NewHeight);
                }

                this.Change(ref newWidth, value);
                this.Changed(() => NewSize);
            }
        }

        float resolution = 72f;
        public float Resolution
        {
            get => resolution;
            set => this.Change(ref resolution, value);
        }

        bool stretch = false;
        public bool Stretch
        {
            get => stretch;
            set => this.Change(ref stretch, value);
        }

        Interpolations interpolation = Interpolations.Bilinear;
        public Interpolations Interpolation
        {
            get => interpolation;
            set => this.Change(ref interpolation, value);
        }

        GraphicalUnit unit;
        public GraphicalUnit Unit
        {
            get => unit;
            set => this.Change(ref unit, value);
        }

        public ResizeWindow(Document document) : base()
        {
            InitializeComponent();

            Document = document;

            CurrentWidth = document.Width;
            CurrentHeight = document.Height;

            NewWidth = document.Width;
            NewHeight = document.Height;

            Resolution = document.Resolution;
            Unit = GraphicalUnit.Pixel;
        }

        public GraphicalUnit GetUnitType(int NumericalType)
        {
            switch (NumericalType)
            {
                case 1:
                    return GraphicalUnit.Inch;
                case 2:
                    return GraphicalUnit.Centimeter;
                case 3:
                    return GraphicalUnit.Millimeter;
                case 4:
                    return GraphicalUnit.Point;
                case 5:
                    return GraphicalUnit.Pica;
                case 6:
                    return GraphicalUnit.Twip;
                case 7:
                    return GraphicalUnit.Character;
                case 8:
                    return GraphicalUnit.En;
            }
            return GraphicalUnit.Pixel;
        }
        
        ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                cancelCommand = cancelCommand ?? new RelayCommand(() => Close(), () => true);
                return cancelCommand;
            }
        }
        ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                saveCommand = saveCommand ?? new RelayCommand(() =>
                {
                    var nh = NewHeight.Round().Int32();
                    var nw = NewWidth.Round().Int32();
                    Document.Resize(nh, nw, anchor, stretch, interpolation);
                    Close();
                },
                () => true);
                return saveCommand;
            }
        }
    }
}
