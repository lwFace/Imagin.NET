using System;
using System.Collections.ObjectModel;

namespace Paint
{
    [Serializable]
    public class ToolCollection : ObservableCollection<ToolGroup>
    {
        public bool Any => Count > 0;

        public void Each(Action<Tool> action)
        {
            foreach (var i in this)
            {
                foreach (var j in i.Tools)
                    action(j);
            }
        }

        public T Get<T>() where T: Tool
        {
            foreach (var i in this)
            {
                foreach (var j in i.Tools)
                {
                    if (j is T)
                        return (T)j;
                }
            }
            return default(T);
        }
    }
}