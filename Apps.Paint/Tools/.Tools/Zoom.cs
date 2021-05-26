using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paint
{
    [Serializable]
    public class ZoomTool : Tool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/Zoom.png").OriginalString;

        double initialValue = 0.0;

        double increment = 0.1;
        public double Increment
        {
            get => increment;
            set => this.Change(ref increment, value);
        }

        bool scrubby = true;
        public bool Scrubby
        {
            get => scrubby;
            set => this.Change(ref scrubby, value);
        }

        bool zoomIn = true;
        public bool ZoomIn
        {
            get => zoomIn;
            set
            {
                this.Change(ref zoomIn, value);

                zoomOut = !value;
                this.Changed(() => ZoomOut);
            }
        }

        bool zoomOut = false;
        public bool ZoomOut
        {
            get => zoomOut;
            set
            {
                this.Change(ref zoomOut, value);

                zoomIn = !value;
                this.Changed(() => ZoomIn);
            }
        }

        void Do()
        {
            if (ZoomIn)
            {
                Document.ZoomValue += Increment;
            }
            else if (ZoomOut)
                Document.ZoomValue -= Increment;
        }

        public override void OnMouseDoubleClick(Point point)
        {
            base.OnMouseDoubleClick(point);
            if (!Scrubby)
                Do();
        }

        public override bool OnMouseDown(Point point)
        {
            base.OnMouseDown(point);
            if (!Scrubby)
            {
                Do();
            }
            else initialValue = Document.ZoomValue;
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (Scrubby)
            {
                if (MouseDown != null)
                {
                    var m = 0.01;
                    var d = MouseDownAbsolute.Value.X.NearestFactor(1) - MouseMoveAbsolute.Value.X.NearestFactor(1);
                    var i = d.Absolute() * m;

                    var result = initialValue;
                    if (d < 0)
                    {
                        result = initialValue + i;
                    }
                    else if (d > 0)
                        result = initialValue - i;

                    Document.ZoomValue = result.Coerce(50, 0.05);
                }
            }
        }

        public override string ToString() => "Zoom";

        [field: NonSerialized]
        ICommand oneHundredPercentCommand;
        public ICommand OneHundredPercentCommand
        {
            get
            {
                oneHundredPercentCommand = oneHundredPercentCommand ?? new RelayCommand(() => Document.ZoomValue = 1, () => Document != null && Document.ZoomValue != 1);
                return oneHundredPercentCommand;
            }
        }

        [field: NonSerialized]
        ICommand fitScreenCommand;
        public ICommand FitScreenCommand
        {
            get
            {
                fitScreenCommand = fitScreenCommand ?? new RelayCommand(() => Document.FitScreen(), () => Document != null);
                return fitScreenCommand;
            }
        }

        [field: NonSerialized]
        ICommand fillScreenCommand;
        public ICommand FillScreenCommand
        {
            get
            {
                fillScreenCommand = fillScreenCommand ?? new RelayCommand(() => Document.FillScreen(), () => Document != null);
                return fillScreenCommand;
            }
        }
    }
}