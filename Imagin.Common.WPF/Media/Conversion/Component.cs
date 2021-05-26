using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media.Conversion;
using System;

namespace Imagin.Common.Media.Models
{
    public abstract class Component : Base
    {
        public virtual DoubleConverter DisplayValueConverter { get; }

        public string DisplayValue
        {
            get
            {
                var result = value;
                result = DisplayValueConverter.ConvertTo(result);
                return result.ToString();
            }
            set
            {
                var result = value?.Double() ?? 0;
                result = DisplayValueConverter.ConvertFrom(result);
                Value = result;
            }
        }

        //...............................................................

        public Model Model { get; internal set; }

        //...............................................................

        public abstract string Label { get; }

        //...............................................................

        public double Maximum => Model.Maximum[Index()];

        public double Minimum => Model.Minimum[Index()];

        //...............................................................

        public virtual bool CanSelect => false;

        //...............................................................

        public virtual double Increment => 1.0;

        //...............................................................

        public virtual ComponentUnit Unit => ComponentUnit.None;

        //...............................................................

        public double NormalizedValue
        {
            get => Normalize(value);
            set => Value = Denormalize(value);
        }

        double value = 0;
        public double Value
        {
            get => value;
            set
            {
                this.Change(ref this.value, value);
                this.Changed(() => DisplayValue);
                this.Changed(() => NormalizedValue);
                Update();
            }
        }

        //...............................................................

        public override int GetHashCode() => GetType().ToString().GetHashCode();

        //...............................................................

        public Component() : base() { }

        internal void Update() => Model.Update();

        internal void Update(double input)
        {
            value = input;
            this.Changed(() => Value);
            this.Changed(() => DisplayValue);
            this.Changed(() => NormalizedValue);
        }

        //...............................................................

        public int Index()
        {
            if (this is IComponentA)
                return 0;

            if (this is IComponentB)
                return 1;

            if (this is IComponentC)
                return 2;

            if (this is IComponentD)
                return 3;

            throw new NotSupportedException();
        }

        //...............................................................

        public double Denormalize(One input) => (input * (Minimum.Absolute() + Maximum)) - Minimum.Absolute();

        public One Normalize(double input) => (input + Minimum.Absolute()) / (Minimum.Absolute() + Maximum);
    }
}