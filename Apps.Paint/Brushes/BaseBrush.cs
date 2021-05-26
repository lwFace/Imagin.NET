using Imagin.Common;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public abstract class BaseBrush : NamedObject //, IEquatable<BaseBrush>
    {
        [Hidden]
        protected Document Document => Get.Current<MainViewModel>().ActiveDocument;

        [Hidden]
        public virtual WriteableBitmap Preview => default(WriteableBitmap);

        int size = 25;
        [Range(1, 3000, 1)]
        [RangeFormat(Imagin.Common.Data.RangeFormat.Slider)]
        public virtual int Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        /// ---------------------------------------------------------------------------------------

        protected BaseBrush(string name) : base(name)
        {
            this.Changed(() => Preview);
        }

        public BaseBrush() : this(string.Empty) { }

        /// ---------------------------------------------------------------------------------------

        /*
        public static bool operator ==(BaseBrush left, BaseBrush right) => left.EqualsOverload(right);

        public static bool operator !=(BaseBrush left, BaseBrush right) => !(left == right);

        public override bool Equals(object i) => Equals(i as BaseBrush);

        public virtual bool Equals(BaseBrush right) => this.Equals<BaseBrush>(right) && size == right.size;

        public override int GetHashCode() => Array<double>.New(Size).GetHashCode();
        */

        /// ---------------------------------------------------------------------------------------

        protected void CoerceSelection(ref double x1, ref double y1, ref double x2, ref double y2)
        {
            //x1 = x1.Coerce(Document.Selections.Points[2].X, Document.Selections.Points[0].X);
            //y1 = y1.Coerce(Document.Selections.Points[2].Y, Document.Selections.Points[0].Y);
            //x2 = x2.Coerce(Document.Selections.Points[2].X, Document.Selections.Points[0].X);
            //y2 = y2.Coerce(Document.Selections.Points[2].Y, Document.Selections.Points[0].Y);
        }

        /// ---------------------------------------------------------------------------------------

        public abstract Matrix<Byte> GetBytes(int size);

        /// ---------------------------------------------------------------------------------------

        public abstract void Draw(WriteableBitmap bitmap, Vector2<int> point, Color color, int size, BlendModes? mode);

        /// ---------------------------------------------------------------------------------------

        public abstract WriteableBitmap Render(Color color, int size);

        public WriteableBitmap RenderLong(Color color, Size<int> size, int radius, Ease easing = Ease.EaseInOutSine)
        {
            var result = BitmapFactory.WriteableBitmap(size.Height, size.Width);

            var y1 = radius.Double() / 2.0;
            var y2 = size.Height - 1 - (radius.Double() / 2.0);

            var ease = Easing.Get(Ease.EaseInOutSine);

            bool up = true;

            var j = 0.0;
            for (var x = radius; x < size.Width - radius; x++, j++)
            {
                var y01 = up ? y2 : y1;
                var y02 = up ? y1 : y2;
                var y = ease(y01, y02, j / size.Height.Double());
                Draw(result, new Vector2<int>(x, y.Int32()), color, radius, BlendModes.Normal);

                if (y >= y2)
                {
                    j = 0;
                    up = true;
                }
                else if (y <= y1)
                {
                    j = 0;
                    up = false;
                }
            }

            return result;
        }
    }
}