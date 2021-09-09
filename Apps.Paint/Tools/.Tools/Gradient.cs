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
    public class GradientTool : RulerTool
    {   
        #region Properties

        public override string Icon => Resources.Uri(nameof(Paint), "Images/Gradient.png").OriginalString;

        /// --------------------------------------------------------------------------

        BlendModes blend = BlendModes.Normal;
        public BlendModes Blend
        {
            get => blend;
            set => this.Change(ref blend, value);
        }

        [field: NonSerialized]
        LinearGradientBrush gradient = default(LinearGradientBrush);
        public LinearGradientBrush Gradient
        {
            get => gradient;
            set => this.Change(ref gradient, value);
        }

        GradientType gradientType = GradientType.Linear;
        public GradientType GradientType
        {
            get => gradientType;
            set => this.Change(ref gradientType, value);
        }

        int bands = 0;
        public int Bands
        {
            get => bands;
            set => this.Change(ref bands, value);
        }

        int coverage = 0;
        public int Coverage
        {
            get => coverage;
            set => this.Change(ref coverage, value);
        }

        int steps = 0;
        public int Steps
        {
            get => steps;
            set => this.Change(ref steps, value);
        }

        int originX = 0;
        public int OriginX
        {
            get => originX;
            set => this.Change(ref originX, value);
        }

        int originY = 0;
        public int OriginY
        {
            get => originY;
            set => this.Change(ref originY, value);
        }

        #endregion

        #region Methods

        CardinalDirection GetDirection()
        {
            if (DifferenceX > DifferenceY)
            {
                //Return E or W
                if (MouseMove.Value.X > MouseDown.Value.X)
                    return CardinalDirection.E;

                return CardinalDirection.W;
            }
            if (DifferenceY > DifferenceX)
            {
                //Return E or W
                if (MouseMove.Value.Y > MouseDown.Value.Y)
                    return CardinalDirection.S;

                return CardinalDirection.N;
            }
            return CardinalDirection.S;
        }

        /// --------------------------------------------------------------------------

        void Apply()
        {
            Gradient gradient = Paint.Gradient.New(GradientType, new LinearGradient(Gradient));
            gradient.Angle = Angle.Value;
            gradient.Direction = GetDirection();
            gradient.Bands = Bands;
            gradient.Coverage = Coverage;
            gradient.Steps = Steps;
            gradient.Center = new Vector2<int>(OriginX, OriginY);

            Matrix<Color> i = gradient.Render(Document.Size);
            for (int y = 0; y < Document.Height; y++)
            {
                for (int x = 0; x < Document.Width; x++)
                {
                    var oldColor = Bitmap.GetPixel(x, y);
                    var newColor = VisualLayer.Blend(Blend, oldColor, i.GetValue(y.UInt32(), x.UInt32()));
                    Bitmap.SetPixel(x, y, newColor);
                }
            }
        }

        /// --------------------------------------------------------------------------

        protected override bool AssertLayer() => AssertPixelLayer();

        public override void OnMouseUp(Point point)
        {
            Apply();
            X = Y = X2 = Y2 = 0;
            base.OnMouseUp(point);
        }

        /// --------------------------------------------------------------------------

        ICommand addGradientCommand;
        public ICommand AddGradientCommand
        {
            get
            {
                addGradientCommand = addGradientCommand ?? new RelayCommand(() =>
                {
                    var result = new LinearGradientBrush();
                    result.GradientStops.Add(new GradientStop(Colors.Black, 0));
                    result.GradientStops.Add(new GradientStop(Colors.White, 1));
                    Get.Current<Options>().Gradients.Add(result);
                }, 
                () => true);
                return addGradientCommand;
            }
        }

        public override string ToString() => "Gradient";
        
        #endregion
    }
}