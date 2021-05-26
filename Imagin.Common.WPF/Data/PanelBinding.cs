using Imagin.Common.Models;
using System;
using System.Linq;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class PanelBinding : Binding
    {
        public PanelBinding(string path, Type type) : base(path)
        {
            Source = Get.Where<IDockViewModel>().Panels.FirstOrDefault(i => i.GetType().Equals(type));
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }
    }
}