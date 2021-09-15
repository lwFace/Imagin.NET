using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media.Conversion;
using System;
using System.Windows.Media;

namespace Imagin.Common.Media.Models
{
    public abstract class VisualComponent : Component
    {
        public sealed override bool CanSelect => true;

        new public VisualModel Model
        {
            get => base.Model as VisualModel;
            internal set => base.Model = value;
        }

        public virtual ComponentType Type => ComponentType.Dynamic;

        //...............................................................

        bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                this.Change(ref isSelected, value);
                value.If(true, () => Model.SelectedComponent = this);
            }
        }

        //...............................................................

        public static explicit operator Components(VisualComponent input)
        {
            if (input is IComponentA)
                return Components.A;

            if (input is IComponentB)
                return Components.B;

            if (input is IComponentC)
                return Components.C;

            throw new NotSupportedException();
        }

        //...............................................................

        /// <returns>Two values in the range of [0, 1].</returns>
        public Vector2<One> AB(Color color)
        {
            //We need to convert the current color to a color in the current color model
            var result = Model.Normalize(Model.Convert(color.Convert()));

            //We then need to grab two of the three values based on the current component
            switch ((Components)this)
            {
                case Components.A:
                    return new Vector2<One>(result.Y, result.Z);
                case Components.B:
                    return new Vector2<One>(result.X, result.Z);
                case Components.C:
                    return new Vector2<One>(result.X, result.Y);
            }
            throw new InvalidOperationException();
        }

        //...............................................................

        /// Each value is in the range of [0, 1].
        public Color ColorFrom(One a, One b, One value)
        {
            Vector3<One> result = null;
            if (this is IComponentA)
                result = new Vector3<One>(value, a, b);

            if (this is IComponentB)
                result = new Vector3<One>(a, value, b);

            if (this is IComponentC)
                result = new Vector3<One>(a, b, value);

            var actualValue = Model.Denormalize(result);
            return Model.Convert(new Vector(actualValue.X, actualValue.Y, actualValue.Z)).Convert();
        }

        //...............................................................

        public Vector2<One> PointFrom(Color color)
        {
            var result = Model.Normalize(Model.Convert(color.Convert()));
            if (this is IComponentA)
                return new Vector2<One>(result.Y, result.Z);

            if (this is IComponentB)
                return new Vector2<One>(result.X, result.Z);

            if (this is IComponentC)
                return new Vector2<One>(result.X, result.Y);

            throw new NotSupportedException();
        }
    }

    public abstract class VisualComponent<Model> : VisualComponent where Model : IModel { }
}