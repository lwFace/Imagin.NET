using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Paint
{
    public partial class LayerView : Viewer
    {
        readonly LayerArranger layerArranger;

        public static DependencyProperty CanvasHeightProperty = DependencyProperty.Register("CanvasHeight", typeof(double), typeof(LayerView), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double CanvasHeight
        {
            get => (double)GetValue(CanvasHeightProperty);
            set => SetValue(CanvasHeightProperty, value);
        }

        public static DependencyProperty CanvasWidthProperty = DependencyProperty.Register("CanvasWidth", typeof(double), typeof(LayerView), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double CanvasWidth
        {
            get => (double)GetValue(CanvasWidthProperty);
            set => SetValue(CanvasWidthProperty, value);
        }

        public static DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(Document), typeof(LayerView), new FrameworkPropertyMetadata(default(Document), FrameworkPropertyMetadataOptions.None, OnDocumentChanged));
        public Document Document
        {
            get => (Document)GetValue(DocumentProperty);
            set => SetValue(DocumentProperty, value);
        }
        protected static void OnDocumentChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as LayerView).OnDocumentChanged((Document)e.NewValue);

        void OnDocumentChanged(Document input) => layerArranger.Layers = input.Layers;

        public static DependencyProperty GridLinesVisibilityProperty = DependencyProperty.Register("GridLinesVisibility", typeof(Visibility), typeof(LayerView), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Visibility GridLinesVisibility
        {
            get => (Visibility)GetValue(GridLinesVisibilityProperty);
            set => SetValue(GridLinesVisibilityProperty, value);
        }

        public static DependencyProperty GridLinesVisibilityThresholdProperty = DependencyProperty.Register("GridLinesVisibilityThreshold", typeof(double), typeof(LayerView), new FrameworkPropertyMetadata(5.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double GridLinesVisibilityThreshold
        {
            get => (double)GetValue(GridLinesVisibilityThresholdProperty);
            set => SetValue(GridLinesVisibilityThresholdProperty, value);
        }

        public static DependencyProperty LayersProperty = DependencyProperty.Register("Layers", typeof(LayerCollection), typeof(LayerView), new FrameworkPropertyMetadata(default(LayerCollection), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public LayerCollection Layers
        {
            get => (LayerCollection)GetValue(LayersProperty);
            set => SetValue(LayersProperty, value);
        }

        public static DependencyProperty PreviewProperty = DependencyProperty.Register("Preview", typeof(WriteableBitmap), typeof(LayerView), new FrameworkPropertyMetadata(default(WriteableBitmap), FrameworkPropertyMetadataOptions.None));
        public WriteableBitmap Preview
        {
            get => (WriteableBitmap)GetValue(PreviewProperty);
            set => SetValue(PreviewProperty, value);
        }

        public static DependencyProperty SelectionsProperty = DependencyProperty.Register(nameof(Selections), typeof(object), typeof(LayerView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public object Selections
        {
            get => (object)GetValue(SelectionsProperty);
            set => SetValue(SelectionsProperty, value);
        }

        public static DependencyProperty ToolProperty = DependencyProperty.Register("Tool", typeof(Tool), typeof(LayerView), new FrameworkPropertyMetadata(default(Tool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Tool Tool
        {
            get => (Tool)GetValue(ToolProperty);
            set => SetValue(ToolProperty, value);
        }

        public LayerView()
        {
            layerArranger = new LayerArranger(this);
            InitializeComponent();
        }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Tool == null)
            {
                Dialog.Show(nameof(Tool), "No tool selected!", DialogImage.Error, DialogButtons.Ok);
                return;
            }

            var position = e.GetPosition(PART_TreeView);
            Tool.MouseDownAbsolute = e.GetPosition(this);
            Tool.LayerView = this;
            switch (e.ClickCount)
            {
                case 1:
                    Mouse.Capture((IInputElement)sender);
                    Tool?.OnMouseDown(position);
                    break;
                case 2:
                    Tool?.OnMouseDoubleClick(position);
                    break;
            }
            PART_ToolPreview.InvalidateVisual();
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Tool == null)
                return;

            Tool.MouseMoveAbsolute = e.GetPosition(this);
            Tool.LayerView = this;

            Tool?.OnMouseMove(e.GetPosition(PART_TreeView));
            PART_ToolPreview.InvalidateVisual();
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Tool == null)
                return;

            Tool.MouseUpAbsolute = e.GetPosition(this);
            Tool.LayerView = this;

            Tool?.OnMouseUp(e.GetPosition(PART_TreeView));
            ((IInputElement)sender).ReleaseMouseCapture();
            Tool.LayerView = null;
            PART_ToolPreview.InvalidateVisual();
        }
    }
}