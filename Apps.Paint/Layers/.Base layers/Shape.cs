using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public abstract class ShapeLayer : RasterizableLayer
    {
        Matrix<Argb> preservedPreview;

        protected bool ignoreRender = false;

        PointCollection points = new PointCollection();
        [Hidden]
        public PointCollection Points
        {
            get => points;
            set => this.Change(ref points, value);
        }

        [field: NonSerialized]
        WriteableBitmap preview;
        [Hidden]
        public WriteableBitmap Preview
        {
            get => preview;
            set => this.Change(ref preview, value);
        }

        public override WriteableBitmap Pixels
        {
            get => Preview;
            set => Preview = value;
        }

        string stroke = $"255,0,0,0";
        public virtual SolidColorBrush Stroke
        {
            get
            {
                var result = stroke.Split(',');
                var actualResult = default(SolidColorBrush);
                App.Current.Dispatcher.Invoke(() => actualResult = new SolidColorBrush(System.Windows.Media.Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte())));
                return actualResult;
            }
            set => this.Change(ref stroke, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        double strokeThickness = 1.0;
        [DisplayName("Stroke thickness")]
        [Range(0.0, 1000.0, 1.0)]
        [RangeFormat(Imagin.Common.Data.RangeFormat.UpDown)]
        public virtual double StrokeThickness
        {
            get => strokeThickness;
            set => this.Change(ref strokeThickness, value);
        }

        [Hidden]
        public System.Drawing.Pen StrokePen => new System.Drawing.Pen(WithOpacity(Stroke.Color), StrokeThickness.Single());

        protected ShapeLayer(LayerType type, string name, int height, int width, SolidColorBrush stroke, double strokeThickness) : base(type, name)
        {
            this.stroke = $"{stroke.Color.A},{stroke.Color.R},{stroke.Color.G},{stroke.Color.B}";
            this.strokeThickness = strokeThickness;
            Preview = BitmapFactory.WriteableBitmap(height, width);
        }

        protected virtual void OnRendered()
        {
            Points = new PointCollection(GetPoints());
        }

        protected void Render()
        {
            if (!ignoreRender)
            {
                if (Preview != null)
                {
                    Preview.Clear(Colors.Transparent);

                    OnRendered();
                    Render(Preview);

                    /*
                    if (Transform != null && Transform.Rotation != 0)
                        Preview = Preview.RotateFree(Transform.Rotation);
                    */
                }
            }
        }

        public abstract void Render(WriteableBitmap input);

        public abstract IEnumerable<Point> GetPoints();

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Stroke):
                case nameof(StrokeThickness):
                case nameof(Style):
                case nameof(Transform):
                case nameof(X):
                case nameof(Y):
                    Render();
                    break;
            }
        }

        public override void Restore() => Preview = From(preservedPreview);

        public override void Preserve() => preservedPreview = From(preview);

        public virtual ShapeLayer Merge(ShapeLayer layer) => default(ShapeLayer);
    }
}