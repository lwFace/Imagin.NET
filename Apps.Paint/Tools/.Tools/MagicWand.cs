using Imagin.Common;
using System;
using System.Windows;

namespace Paint
{
    [Serializable]
    public class MagicWandTool : Tool
    {
        public override string Icon => Resources.Uri(nameof(Paint), "Images/MagicWand.png").OriginalString;

        double tolerance;
        public double Tolerance
        {
            get => tolerance;
            set => this.Change(ref tolerance, value);
        }

        public override bool OnMouseDown(Point point)
        {
            base.OnMouseDown(point);
            //The algorthim will basically just be a flash flood, but instead of setting pixels, we're getting pixels to construct a selection!
            //Tolerance would work fine for alpha, but what about opaque colors?
            return true;
        }

        public override string ToString() => "Magic wand";
    }
}