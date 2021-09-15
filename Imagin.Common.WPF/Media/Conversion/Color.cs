using Imagin.Common.Media.Conversion;
using System;

namespace Imagin.Common.Media.Models
{
    public class IColor
    {
        public readonly Vector Value;

        public double this[int i] => Value[i];

        protected IColor(params double[] values) : base() => Value = new Vector(values);

        public static implicit operator Vector(IColor input) => input.Value;

        public static IColor New(Type type, params double[] values) => (IColor)Activator.CreateInstance(typeof(Color<>).MakeGenericType(type), values);

        public override string ToString() => Value.ToString();
    }

    public class Color<Model> : IColor where Model : IModel
    {
        public Color(params double[] values) : base(values) { }
    }
}