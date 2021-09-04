using Imagin.Common;
using Imagin.Common.AvalonDock;

namespace Paint
{
    public class StyleViewModel : Pane
    {
        Layer layer;
        public Layer Layer
        {
            get => layer;
            set => this.Change(ref layer, value);
        }

        public StyleViewModel() : base("StylePane", "Style", Resources.Uri(nameof(Paint), "/Images/Fx.png"))
        {
        }
    }
}