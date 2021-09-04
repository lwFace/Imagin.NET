using System;
using System.Collections.ObjectModel;

namespace Vault
{
    [Serializable]
    public class TaskCollection : ObservableCollection<CopyTask>
    {
        public TaskCollection() : base() { }
    }
}