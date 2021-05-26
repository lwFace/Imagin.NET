using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Storage;
using System.Windows;

namespace Paint
{
    public partial class BrowserView : ResourceDictionary
    {
        public BrowserView() : base() { }

        void OnDoubleClick(object sender, EventArgs<File> e)
        {
            _ = Get.Current<MainViewModel>().OpenFile(e.Value.Path);
        }
    }
}