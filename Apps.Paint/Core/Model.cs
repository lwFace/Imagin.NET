using Imagin.Common;
using System;
using System.Windows.Data;

namespace Paint
{
    public class Model : Binding
    {
        public Model(string path, Type type) : base(path)
        {
            Mode = BindingMode.OneWay;
            Source = type == typeof(MainViewModel) ? Get.Current<MainViewModel>() : Get.Current<MainViewModel>().Panel(type);
        }
    }
}