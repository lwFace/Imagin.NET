using Imagin.Common;
using System.Runtime.CompilerServices;
using Imagin.Common.Data;

namespace Paint
{
    public abstract class HardBrush : BaseBrush
    {
        double hardness = 1;
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public virtual double Hardness
        {
            get => hardness;
            set => this.Change(ref hardness, value);
        }

        double noise = 1;
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public virtual double Noise
        {
            get => noise;
            set => this.Change(ref noise, value);
        }

        /// ---------------------------------------------------------------------------------------

        protected int GetRings(int columns, int rows)
        {
            int result = 0;
            while (columns > 1 && rows > 1)
            {
                result++;
                columns -= 1;
                rows -= 1;
            }
            return result;
        }

        /// ---------------------------------------------------------------------------------------

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Hardness):
                case nameof(Noise):
                    this.Changed(() => Preview);
                    break;
            }
        }
    }
}