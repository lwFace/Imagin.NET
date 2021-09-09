using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Input;

namespace Paint
{
    [Serializable]
    public class RotateHandTool : HandTool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/RotateHand.png").OriginalString;

        Point origin => new Point(LayerView.ActualWidth / 2.0, LayerView.ActualHeight / 2.0);

        double snap = 30;
        public double Snap
        {
            get => snap;
            set => this.Change(ref snap, value);
        }

        void Do(Point target)
        {
            var vector2 = target - origin;
            var vector1 = new Point(0, 1);

            var angleInRadians = Math.Atan2(vector2.Y, vector2.X) - Math.Atan2(vector1.Y, vector1.X);
            var angleInDegrees = Imagin.Common.Math.Angle.GetDegree(angleInRadians);

            Document.Angle = ModifierKeys.Shift.Pressed() ? angleInDegrees.NearestFactor(snap) : angleInDegrees;
        }

        public override bool OnMouseDown(Point point)
        {
            base.OnMouseDown(point);
            ShowCompass = true;
            Do(MouseDownAbsolute ?? point);
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
                Do(MouseMoveAbsolute ?? point);
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            ShowCompass = false;
        }

        public override string ToString() => "Rotate hand";

        ICommand setAngleCommand;
        public ICommand SetAngleCommand
        {
            get
            {
                setAngleCommand = setAngleCommand ?? new RelayCommand<object>(i => Document.Angle = double.Parse(i.ToString()), i => Document != null && Document.Angle != double.Parse(i?.ToString() ?? "0"));
                return setAngleCommand;
            }
        }
    }
}