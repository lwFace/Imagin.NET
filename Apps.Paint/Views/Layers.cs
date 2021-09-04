using Imagin.Common;
using System.Windows;

namespace Paint
{
    public partial class LayersView : ResourceDictionary
    {
        public LayersView() : base() { }

        void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Get.Current<MainViewModel>().Panel<LayersPanel>().SelectedLayer = (Layer)e.NewValue;
        }
    }
}