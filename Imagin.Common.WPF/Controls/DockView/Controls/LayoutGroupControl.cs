using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public sealed class LayoutGroupControl : Grid, ILayoutControl
    {
        public DockView DockView { get; private set; }

        public readonly Orientation Orientation;

        public LayoutRootControl Root { get; private set; }

        /// ......................................................................................................................

        public LayoutGroupControl(LayoutRootControl root, Orientation orientation) : base()
        {
            DockView = root.DockView;
            Root = root;
            Orientation = orientation;
        }
    }
}