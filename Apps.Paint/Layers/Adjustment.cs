using Imagin.Common;
using Paint.Adjust;

namespace Paint
{
    /// <summary>
    /// This is a type of a layer that applies one or more adjustments to all layers beneath. One way this can be done is to have all
    /// <see cref="PixelLayer"/>s store an untouched map (we can call that the original) and a touched map (we can call this the preview).
    /// Whenever an adjustment layer would change, all <see cref="PixelLayer"/>s would have to render a preview with the changed
    /// adjustments.
    /// </summary>
    public class AdjustmentLayer : Layer
    {
        AdjustmentEffect effect;
        public AdjustmentEffect Effect
        {
            get => effect;
            set => this.Change(ref effect, value);
        }

        public AdjustmentLayer(AdjustmentEffect effect) : base(LayerType.Adjustment) => Effect = effect;

        public override Layer Clone() => new AdjustmentLayer(Effect.Copy());

        public override string ToString() => nameof(Effect);
    }
}