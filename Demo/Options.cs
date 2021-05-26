using Imagin.Common;
using Imagin.Common.Configuration;
using System;

namespace Demo
{
    [Serializable]
    public class Options : Data<MainViewModel>
    {
        [field: NonSerialized]
        Control control = null;
        [Hidden]
        public Control Control
        {
            get => control;
            set => this.Change(ref control, value);
        }
    }
}