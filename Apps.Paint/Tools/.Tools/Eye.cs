using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class EyeTool : Tool
    {
        public override Cursor Cursor => Cursors.None;

        public override string Icon => Resources.Uri(nameof(Paint), "Images/EyeDrop.png").OriginalString;

        [field: NonSerialized]
        Color color;
        public Color Color
        {
            get => color;
            set => this.Change(ref color, value);
        }

        public override bool OnMouseDown(Point point)
        {
            base.OnMouseDown(point);
            var color = Imagin.Common.Media.Display.Color();
            Color = Color.FromArgb(255, color.R, color.G, color.B);
            Get.Current<Options>().ForegroundColor = Color;
            return true;
        }

        public override void OnMouseMove(Point point)
        {
            base.OnMouseMove(point);
            if (MouseDown != null)
            {
                var color = Imagin.Common.Media.Display.Color();
                Color = Color.FromArgb(255, color.R, color.G, color.B);
                Get.Current<Options>().ForegroundColor = Color;
            }
        }

        public override void OnMouseUp(Point point)
        {
            base.OnMouseUp(point);
            Color = default(Color);
        }

        public override string ToString() => "Eye";
    }
}