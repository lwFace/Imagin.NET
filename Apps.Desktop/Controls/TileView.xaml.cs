using Desktop.Tiles;
using Imagin.Common.Input;
using Imagin.Common.Storage;
using System.Windows;
using System.Windows.Controls;

namespace Desktop
{
    public partial class TileView : UserControl
    {
        public Tile Tile => DataContext as Tile;
        
        public static DependencyProperty HeaderAlignmentProperty = DependencyProperty.Register(nameof(HeaderAlignment), typeof(HeaderAlignments), typeof(TileView), new FrameworkPropertyMetadata(HeaderAlignments.Center, FrameworkPropertyMetadataOptions.None));
        public HeaderAlignments HeaderAlignment
        {
            get => (HeaderAlignments)GetValue(HeaderAlignmentProperty);
            set => SetValue(HeaderAlignmentProperty, value);
        }

        public static DependencyProperty HeaderPlacementProperty = DependencyProperty.Register(nameof(HeaderPlacement), typeof(HeaderPlacements), typeof(TileView), new FrameworkPropertyMetadata(HeaderPlacements.Top, FrameworkPropertyMetadataOptions.None));
        public HeaderPlacements HeaderPlacement
        {
            get => (HeaderPlacements)GetValue(HeaderPlacementProperty);
            set => SetValue(HeaderPlacementProperty, value);
        }

        public TileView() : base()
        {
            InitializeComponent();
        }

        void OnFolderOpened(object sender, EventArgs<string> e)
        {
            Machine.OpenInWindowsExplorer(e.Value);
        }
    }
}