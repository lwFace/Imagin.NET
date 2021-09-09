using Imagin.Common;
using Imagin.Common.Media;
using System.Windows;
using System.Windows.Input;

namespace Paint
{
    public partial class ColorsView : ResourceDictionary
    {
        public ColorsView() : base() { }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var stringColor = (StringColor)(sender as FrameworkElement).DataContext;

            if (e.LeftButton == MouseButtonState.Pressed)
                Get.Current<Options>().ForegroundColor = stringColor.Value;
        }
    }
}