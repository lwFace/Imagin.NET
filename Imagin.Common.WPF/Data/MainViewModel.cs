using Imagin.Common.Models;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class MainViewModel : Binding
    {
        public MainViewModel() : this(string.Empty) { }

        public MainViewModel(string path) : base(path)
        {
            Mode = BindingMode.OneWay;
            Source = Get.Where<IMainViewModel>();
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }
    }
}