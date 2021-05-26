using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Desktop.Controls
{
    public partial class HeaderView : UserControl
    {
        public event EventHandler<EventArgs> HeaderMouseDoubleClick;

        public static DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object), typeof(HeaderView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static DependencyProperty HeaderAlignmentProperty = DependencyProperty.Register(nameof(HeaderAlignment), typeof(HeaderAlignments), typeof(HeaderView), new FrameworkPropertyMetadata(HeaderAlignments.Center, FrameworkPropertyMetadataOptions.None));
        public HeaderAlignments HeaderAlignment
        {
            get => (HeaderAlignments)GetValue(HeaderAlignmentProperty);
            set => SetValue(HeaderAlignmentProperty, value);
        }

        public static DependencyProperty HeaderPlacementProperty = DependencyProperty.Register(nameof(HeaderPlacement), typeof(HeaderPlacements), typeof(HeaderView), new FrameworkPropertyMetadata(HeaderPlacements.Top, FrameworkPropertyMetadataOptions.None));
        public HeaderPlacements HeaderPlacement
        {
            get => (HeaderPlacements)GetValue(HeaderPlacementProperty);
            set => SetValue(HeaderPlacementProperty, value);
        }

        public static DependencyProperty BodyProperty = DependencyProperty.Register(nameof(Body), typeof(object), typeof(HeaderView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public object Body
        {
            get => GetValue(BodyProperty);
            set => SetValue(BodyProperty, value);
        }

        public HeaderView()
        {
            InitializeComponent();
        }

        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                HeaderMouseDoubleClick?.Invoke(this, new EventArgs());
        }
    }
}