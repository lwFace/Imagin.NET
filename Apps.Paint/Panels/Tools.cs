using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Models;
using System;

namespace Paint
{
    public class ToolsPanel : Panel
    {
        public event EventHandler<EventArgs<Tool>> ToolSelected;

        Tool tool;
        public Tool Tool
        {
            get => tool;
            set
            {
                this.Change(ref tool, value);
                OnToolSelected(value);
            }
        }

        public override string Title => "Tools";

        public ToolsPanel() : base(Resources.Uri(nameof(Paint), "/Images/Wrenches.png"))
        {
            TitleVisibility = false;

            if (Get.Current<Options>().Tools == null)
                Get.Current<Options>().Tools = new ToolCollection();

            var tools = Get.Current<Options>().Tools;
            if (!Get.Current<Options>().Tools.Any)
            {
                tools.Add(new ToolGroup(new SelectionTool(), new EllipseSelectionTool(), new ColumnSelectionTool(), new RowSelectionTool()));
                tools.Add(new ToolGroup(new MoveTool()));
                tools.Add(new ToolGroup(new EyeTool()));
                tools.Add(new ToolGroup(new RulerTool(), new NoteTool(), new CountTool()));
                tools.Add(new ToolGroup(new LassoTool(), new PolygonalLassoTool()));
                tools.Add(new ToolGroup(new MagicWandTool()));
                tools.Add(new ToolGroup(new CropTool()));
                tools.Add(new ToolGroup(new CloneStampTool()));
                tools.Add(new ToolGroup(new PencilTool(), new BrushTool(), new ColorReplacementTool(), new ColorSwapTool()));
                tools.Add(new ToolGroup(new BlurTool(), new SharpenTool(), new SmudgeTool()));
                tools.Add(new ToolGroup(new DodgeTool(), new BurnTool(), new SpongeTool()));
                tools.Add(new ToolGroup(new BucketTool()));
                tools.Add(new ToolGroup(new GradientTool()));
                tools.Add(new ToolGroup(new EraserTool()));
                tools.Add(new ToolGroup(new LineTool(), new EllipseTool(), new RectangleTool(), new RoundedRectangleTool(), new PolygonTool(), new CustomShapeTool()));
                tools.Add(new ToolGroup(new PathTool(), new FreePathTool()));
                tools.Add(new ToolGroup(new TextTool()));
                tools.Add(new ToolGroup(new HandTool(), new RotateHandTool()));
                tools.Add(new ToolGroup(new ZoomTool()));
            }
            tools.Each(i => i.Selected += OnToolSelected);
            Get.Current<MainViewModel>().ActiveDocumentChanged += OnDocumentChanged;
        }

        void OnDocumentChanged(object sender, EventArgs<Document> e)
        {
            Get.Current<Options>().Tools.Each(i => i.Document = (Document)e.Value);
        }

        void OnToolSelected(object sender, SelectedEventArgs e)
        {
            Tool = sender as Tool;
            Get.Current<Options>().Tools.Each(i =>
            {
                if (i != Tool)
                    i.IsSelected = false;
            });
        }

        void OnToolSelected(Tool value)
        {
            ToolSelected?.Invoke(this, new EventArgs<Tool>(value));
        }
    }
}