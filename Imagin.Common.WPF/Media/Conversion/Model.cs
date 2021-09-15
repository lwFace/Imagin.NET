using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    public abstract class Model : Base, IModel
    {
        public ColorConverter Converter;

        //...............................................................

        public Collection<Component> Components { get; private set; } = new Collection<Component>();

        //...............................................................

        public abstract Vector Maximum { get; }

        public abstract Vector Minimum { get; }

        //...............................................................

        Handle handle = false;

        //...............................................................

        public Vector Value
        {
            get
            {
                var result = new double[Components.Count];

                for (int i = 0, count = Components.Count; i < count; i++)
                    result[i] = Components[i].Value;

                return result;
            }
            private set
            {
                for (var i = 0; i < Components.Count; i++)
                    Components[i].Update(value[i]);
            }
        }

        //...............................................................

        public Model(params Component[] components) : base()
        {
            components.ForEach(i =>
            {
                i.Model = this;
                Components.Add(i);
            });
        }

        internal void Update() => Converter.Update(this, Convert(Value).Convert());

        internal void Update(System.Windows.Media.Color input)
        {
            Value = Convert(input.Convert());
        }

        //...............................................................

        public Vector3<double> Denormalize(Vector3<One> input)
        {
            var i = new Vector(input.X, input.Y, input.Z);
            var result = (i * (Minimum.Absolute() + Maximum)) - Minimum.Absolute();
            return new Vector3<double>(result[0], result[1], result[2]);
        }

        public Vector3<One> Normalize(IColor input)
        {
            var result = (input + Minimum.Absolute()) / (Minimum.Absolute() + Maximum);
            return new Vector3<One>(result[0], result[1], result[2]);
        }

        ///--------------------------------------------------------------------------------------------------------

        public abstract IColor Convert(Color<RGB> input);

        public abstract Color<RGB> Convert(Vector input);

        ///--------------------------------------------------------------------------------------------------------

        public override string ToString() => GetType().Name;
    }
}