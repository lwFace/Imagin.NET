using System;

namespace Paint
{
    [Serializable]
    public enum LayerType
    {
        Null,
        Adjustment,
        CustomShape,
        Group,
        Ellipse,
        Line,
        Path,
        Pixel,
        Polygon,
        Rectangle,
        RoundedRectangle,
        Text
    }
}