using Imagin.Common;
using Imagin.Common.Data;

namespace Paint
{
    public class FixedSquareBrush : SquareBrush
    {
        [Hidden]
        public override double Hardness => 1;

        [Hidden]
        public override double Noise => 0;

        [Range(1, 3000, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public override int Size
        {
            get => base.Size;
            set => base.Size = value;
        }
    }
}