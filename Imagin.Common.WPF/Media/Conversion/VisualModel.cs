using Imagin.Common.Math;
using Imagin.Common.Media.Conversion;
using System;

namespace Imagin.Common.Media.Models
{
    public abstract class VisualModel : Model
    {
        public abstract VisualModels Name { get; }

        public static explicit operator VisualModels(VisualModel input) => input.Name;

        //...............................................................

        VisualComponent selectedComponent;
        public VisualComponent SelectedComponent
        {
            get => selectedComponent;
            internal set => this.Change(ref selectedComponent, value);
        }

        //...............................................................

        public VisualModel(params Component[] components) : base(components)
        {
            (Components[0] as VisualComponent).IsSelected = true;
        }

        //...............................................................

        /// <summary>
        /// Prints all <see cref="RGB"/> values based on the specified <see cref="IModel"/> (for debugging purposes).
        /// </summary>
        public void Print()
        {
            double minA = 0, minB = 0, minC = 0, maxA = 0, maxB = 0, maxC = 0;
            for (var r = 0.0; r < 256; r++)
            {
                for (var g = 0.0; g < 256; g++)
                {
                    for (var b = 0.0; b < 256; b++)
                    {
                        var rgb = new Color<RGB>(new Vector(r, g, b) / 255.0);
                        var color = Convert(rgb);

                        if (color[0] < minA)
                            minA = color[0];
                        if (color[1] < minB)
                            minB = color[1];
                        if (color[2] < minC)
                            minC = color[2];

                        if (color[0] > maxA)
                            maxA = color[0];
                        if (color[1] > maxB)
                            maxB = color[1];
                        if (color[2] > maxC)
                            maxC = color[2];
                    }
                }
            }
            Console.WriteLine($"{GetType().Name}: minimum [{minA}, {minB}, {minC}], maximum [{maxA}, {maxB}, {maxC}]");
        }

        public void Select(Components component)
        {
            foreach (VisualComponent i in Components)
            {
                if ((Components)i == component)
                {
                    i.IsSelected = true;
                    break;
                }
            }
        }

        internal void Update(Vector2<One> ab, One c)
        {
            Vector3<One> result = null;
            if (SelectedComponent is IComponentA)
                result = new Vector3<One>(c, ab.X, ab.Y);

            if (SelectedComponent is IComponentB)
                result = new Vector3<One>(ab.X, c, ab.Y);

            if (SelectedComponent is IComponentC)
                result = new Vector3<One>(ab.X, ab.Y, c);

            var actualValue = Denormalize(result);
            Components[0].Update(actualValue.X);
            Components[1].Update(actualValue.Y);
            Components[2].Update(actualValue.Z);
            Update();
        }
    }
}