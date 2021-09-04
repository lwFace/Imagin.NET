using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public abstract class Mutation : NamedObject
    {
        [Hidden]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        public abstract Color Apply(Color color);

        public virtual void Apply(WriteableBitmap OldBitmap, WriteableBitmap NewBitmap)
        {
            OldBitmap.ForEach((x, y, oldColor) =>
            {
                var newColor = Apply(oldColor);

                if (OldBitmap != NewBitmap)
                    NewBitmap.SetPixel(x, y, newColor);

                return OldBitmap != NewBitmap ? oldColor : newColor;
            });
        }

        public virtual void Apply(ColorMatrix oldMatrix, ColorMatrix newMatrix)
        {
            oldMatrix.Each((y, x, oldColor) =>
            {
                var newColor = Apply(oldColor);
                newMatrix.SetValue(y.UInt32(), x.UInt32(), newColor);
                return oldColor;
            });
        }

        public Mutation() : base() { }

        public Mutation(string name) : base(name) { }
    }
}