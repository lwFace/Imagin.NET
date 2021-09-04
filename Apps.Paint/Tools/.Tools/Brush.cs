using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class BrushTool : Tool
    {
        public override Cursor Cursor => Cursors.None;

        public override string Icon => Resources.Uri(nameof(Paint), "Images/PaintBrush.png").OriginalString;

        BaseBrush brush = default(BaseBrush);
        public BaseBrush Brush
        {
            get => brush;
            set => this.Change(ref brush, value);
        }

        ObservableCollection<BaseBrush> brushes = new ObservableCollection<BaseBrush>();
        public ObservableCollection<BaseBrush> Brushes
        {
            get => brushes;
            set => this.Change(ref brushes, value);
        }

        BlendModes? mode = BlendModes.Normal;
        public BlendModes? Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
        }

        public BrushTool() : base()
        {
            brushes.Add(new CircleBrush());
            brushes.Add(new SquareBrush());
            Brush = Brushes[0];
        }

        protected override bool AssertLayer() => AssertPixelLayer();

        public override bool OnMouseDown(Point point)
        {
            if (base.OnMouseDown(point))
            {
                Draw(new Vector2<int>((point.X + Layer.X.Absolute()).Int32(), (point.Y + Layer.Y.Absolute()).Int32()), Get.Current<Options>().ForegroundColor);
                return true;
            }
            return false;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
                Draw(new Vector2<int>((point.X + Layer.X.Absolute()).Int32(), (point.Y + Layer.Y.Absolute()).Int32()), Get.Current<Options>().ForegroundColor);
        }

        public override string ToString() => "Brush";

        protected virtual void Draw(Vector2<int> point, Color color)
        {
            Brush = Brush ?? new SquareBrush();
            Brush.Draw(Bitmap, point, color, Brush.Size, Mode);
        }

        ICommand addBrushCommand;
        public ICommand AddBrushCommand
        {
            get
            {
                addBrushCommand = addBrushCommand ?? new RelayCommand(async () =>
                {
                    string path;
                    if (ExplorerWindow.Show(out path, "Open...", ExplorerWindow.Modes.OpenFile, null, null))
                    {
                        var result = await CustomBrush.New(path);
                        Brushes.Add(result);
                        Brush = result;
                    }
                }, () => true);
                return addBrushCommand;
            }
        }
    }
}