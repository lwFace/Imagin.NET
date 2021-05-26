using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Paint.Adjust;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class Filter : Mutation
    {
        [Featured]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        AdjustmentEffectCollection adjustments = new AdjustmentEffectCollection();
        public AdjustmentEffectCollection Adjustments
        {
            get => adjustments;
            set => this.Change(ref adjustments, value);
        }

        protected Filter(IEnumerable<AdjustmentEffect> result) : base()
        {
            foreach (var i in result)
                adjustments.Add(i);
        }

        public Filter() : base()
        {
            var result = GetAdjustments();
            foreach (var i in result)
                adjustments.Add(i);
        }

        public Filter(string name) : base(name) { }

        protected virtual IEnumerable<AdjustmentEffect> GetAdjustments() => Enumerable.Empty<AdjustmentEffect>();

        public override Color Apply(Color color)
        {
            var result = color;
            //adjustments.ForEach(i => result = i.Apply(result));
            return result;
        }

        public void Apply(WriteableBitmap input)
        {
            input.ForEach((x, y, oldColor) => Apply(oldColor));
        }

        public Filter Clone()
        {
            var result = new Filter(Name);
            foreach (var i in adjustments)
                result.Adjustments.Add(i.Copy());

            return result;
        }
    }

    [Serializable]
    public class BakedFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new ShadingEffect(100, 100, 60);
            yield return new TintEffect(20, 30, 60);
            yield return new GammaEffect(0.7f);
        }

        public BakedFilter() : base() => Name = "Baked";
    }

    [Serializable]
    public class BlueFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new BalanceEffect(0, 0, 30);
            yield return new ShadingEffect(0, 70, 80);
            yield return new TintEffect(0, 0, 40);
        }

        public BlueFilter() : base() => Name = "Blue";
    }

    [Serializable]
    public class CastawayFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new LabEffect(0, 35, -100);
            yield return new XYZEffect(0, 0, -20);
            yield return new TintEffect(30, 0, 0);
            yield return new GammaEffect(0.7f);
            yield return new HSLEffect(0, -30, 0);
        }

        public CastawayFilter() : base() => Name = "Castaway";
    }

    [Serializable]
    public class CheshireFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new LabEffect(0, 25, -50);
            yield return new GammaEffect(0.6f);
            yield return new TintEffect(0, 0, 40);
        }

        public CheshireFilter() : base() => Name = "Cheshire";
    }

    [Serializable]
    public class ColdFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new BalanceEffect(10, 0, 20);
            yield return new ShadingEffect(0, 10, 20);
            yield return new HSLEffect(0, -10, 0);
        }

        public ColdFilter() : base() => Name = "Cold";
    }

    [Serializable]
    public class DarkSepiaFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new SepiaEffect();
            yield return new BrightnessContrastEffect(-40, 25);
            yield return new TintEffect(45, 25, 40);
        }

        public DarkSepiaFilter() : base() => Name = "Dark Sepia";
    }

    [Serializable]
    public class FrostbiteFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new HSLEffect(0, -30, 0);
            yield return new TintEffect(0, 0, 20);
            yield return new GammaEffect(0.8f);
        }

        public FrostbiteFilter() : base() => Name = "Frostbite";
    }

    [Serializable]
    public class GreenFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new BalanceEffect(0, 30, 0);
            yield return new ShadingEffect(0, 80, 70);
            yield return new TintEffect(0, 40, 0);
        }

        public GreenFilter() : base() => Name = "Green";
    }

    [Serializable]
    public class IndependenceFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new LabEffect(0, -10, -40);
            yield return new GammaEffect(0.7f);
            yield return new TintEffect(0, 0, 30);
        }

        public IndependenceFilter() : base() => Name = "Independence";
    }

    [Serializable]
    public class JungleFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new XYZEffect(0d, 10d, 10d);
            yield return new GammaEffect(0.7f);
        }

        public JungleFilter() : base() => Name = "Jungle";
    }

    [Serializable]
    public class LightPurpleFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new TintEffect(65, 0, 70);
        }

        public LightPurpleFilter() : base() => Name = "Light Purple";
    }

    [Serializable]
    public class MardiGrasFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new XYZEffect(10d, 0d, 30d);
            yield return new GammaEffect(0.6f);
            yield return new LabEffect(0, -10, -10);
        }

        public MardiGrasFilter() : base() => Name = "Mardi Gras";
    }

    [Serializable]
    public class MariettaFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new LabEffect(0, -10, -40);
            yield return new GammaEffect(0.7f);
            yield return new TintEffect(0, 0, 30);
            yield return new BalanceEffect(32, 32, 32);
            yield return new ShadingEffect(100, 100, 50);
        }

        public MariettaFilter() : base() => Name = "Marietta";
    }

    [Serializable]
    public class MetroFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new TintEffect(0, 0, 60);
            yield return new BrightnessContrastEffect(-20, 25);
            yield return new BalanceEffect(-25, -35, 50);
            yield return new TintEffect(35, 0, 70);
            yield return new BrightnessContrastEffect(-20, 15);
        }

        public MetroFilter() : base() => Name = "Metro";
    }

    [Serializable]
    public class PerpetualFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new ShadingEffect(100, 100, 60);
            yield return new GammaEffect(0.7f);
            yield return new BalanceEffect(-60, 20, 100);
        }

        public PerpetualFilter() : base()
        {
            Name = "Perpetual";
        }
    }

    [Serializable]
    public class PurpleFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new TintEffect(65, 0, 70);
            yield return new BalanceEffect(-5, -50, 100);
            yield return new BrightnessContrastEffect(-20, 25);
        }

        public PurpleFilter() : base()
        {
            Name = "Purple";
        }
    }

    [Serializable]
    public class RedFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new BalanceEffect(30, 0, 0);
            yield return new ShadingEffect(80, 70, 0);
            yield return new TintEffect(40, 0, 0);
        }

        public RedFilter() : base()
        {
            Name = "Red";
        }
    }

    [Serializable]
    public class RustyFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new XYZEffect(0, -2, -15);
            yield return new LabEffect(0, -20, 30);
            yield return new TintEffect(25, 15, 5);
            yield return new XYZEffect(0, -2, -5);
        }

        public RustyFilter() : base()
        {
            Name = "Rusty";
        }
    }

    [Serializable]
    public class SciFiFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new BrightnessContrastEffect(-40, 25);
            yield return new TintEffect(45, 25, 40);
        }

        public SciFiFilter() : base()
        {
            Name = "Sci fi";
        }
    }

    [Serializable]
    public class SepiaFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new SepiaEffect();
        }

        public SepiaFilter() : base() => Name = "Sepia";
    }

    [Serializable]
    public class SpaceCadetFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new XYZEffect(-10d, -8d, 10d);
        }

        public SpaceCadetFilter() : base()
        {
            Name = "Space cadet";
        }
    }

    [Serializable]
    public class SubmarineFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new LabEffect(0, -45, -30);
            yield return new GammaEffect(0.8f);
            yield return new TintEffect(0, 0, 15);
            yield return new XYZEffect(0, -2, -5);
        }

        public SubmarineFilter() : base()
        {
            Name = "Submarine";
        }
    }

    [Serializable]
    public class SunnyFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new XYZEffect(0d, 0d, -30d);
        }

        public SunnyFilter() : base()
        {
            Name = "Sunny";
        }
    }

    [Serializable]
    public class VintageFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new TintEffect(10, 30, 60);
            yield return new GammaEffect(0.5f);
        }

        public VintageFilter() : base()
        {
            Name = "Vintage";
        }
    }

    [Serializable]
    public class WarmFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new BalanceEffect(20, 0, 10);
            yield return new ShadingEffect(0, 20, 10);
            yield return new HSLEffect(0, 10, 0);
        }

        public WarmFilter() : base()
        {
            Name = "Warm";
        }
    }

    [Serializable]
    public class WesternFilter : Filter
    {
        protected override IEnumerable<AdjustmentEffect> GetAdjustments()
        {
            yield return new BalanceEffect(50, 0, -50);
        }

        public WesternFilter() : base()
        {
            Name = "Western";
        }
    }
}