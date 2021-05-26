using Imagin.Common;
using System;
using System.Collections.ObjectModel;

namespace Paint
{
    [Serializable]
    public class ToolGroup : Base
    {
        Tool selectedTool = null;
        public Tool SelectedTool
        {
            get => selectedTool;
            set => this.Change(ref selectedTool, value);
        }

        ObservableCollection<Tool> tools = new ObservableCollection<Tool>();
        public ObservableCollection<Tool> Tools
        {
            get => tools;
            set => this.Change(ref tools, value);
        }

        public ToolGroup(params Tool[] tools) : base()
        {
            if (tools?.Length > 0)
            {
                SelectedTool = tools[0];
                foreach (var i in tools)
                    Tools.Add(i);
            }
        }
    }
}