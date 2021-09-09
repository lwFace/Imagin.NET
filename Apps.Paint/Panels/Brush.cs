using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    public class BrushPanel : Panel
    {
        BaseBrush update = null;

        bool updating = false;

        BaseBrush brush = null;
        public BaseBrush Brush
        {
            get => brush;
            set => OnBrushChanged(brush, value);
        }

        WriteableBitmap preview = null;
        public WriteableBitmap Preview
        {
            get => preview;
            set => this.Change(ref preview, value);
        }

        WriteableBitmap longPreview = null;
        public WriteableBitmap LongPreview
        {
            get => longPreview;
            set => this.Change(ref longPreview, value);
        }

        public override string Title => "Brush";

        public BrushPanel() : base(Resources.Uri(nameof(Paint), "/Images/Brush.png"))
        {
            IsVisible = false;
        }

        async void OnBrushChanged(BaseBrush oldBrush, BaseBrush newBrush)
        {
            if (updating)
            {
                update = newBrush;
                return;
            }

            updating = true;

            if (oldBrush != null)
                oldBrush.PropertyChanged -= OnBrushChanged;

            if (newBrush != null)
            {
                newBrush.PropertyChanged += OnBrushChanged;
                this.Change(ref brush, newBrush, () => Brush);
                await App.Current.Dispatcher.BeginInvoke(() =>
                {
                    Preview = newBrush.Render(Colors.Black, 64);
                    LongPreview = newBrush.RenderLong(Colors.Black, new Size<int>(100, 512), 32, Ease.EaseInOutSine);
                });

                if (update != null)
                {
                    oldBrush = newBrush;
                    newBrush = update;

                    update = null;
                    updating = false;

                    OnBrushChanged(oldBrush, newBrush);
                    return;
                }
            }
            updating = false;
        }

        void OnBrushChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(HardBrush.Hardness):
                case nameof(HardBrush.Noise):
                    OnBrushChanged(brush, brush);
                    break;
            }
        }
    }
}