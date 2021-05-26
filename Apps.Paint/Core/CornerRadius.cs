using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    public class CornerRadius
    {
        public double BottomLeft;

        public double BottomRight;

        public double TopLeft;

        public double TopRight;

        public CornerRadius(double bottomLeft, double bottomRight, double topLeft, double topRight)
        {
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
            TopLeft = topLeft;
            TopRight = topRight;
        }
    }
}