using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections;
using System.Linq;

namespace Vault
{
    public class PropertiesPanel : Panel
    {
        object source;
        public object Source
        {
            get => source;
            set => this.Change(ref source, value);
        }

        public override string Title => "Properties";

        public PropertiesPanel() : base(Resources.Uri(nameof(Vault), "/Images/Properties.png"))
        {
            Get.Current<MainViewModel>().SelectedItemsChanged += OnSelectedItemsChanged;
        }

        void OnSelectedItemsChanged(object sender, EventArgs<IList> e)
        {
            var newSource = e.Value?.First();
            if (source.Equals(newSource) != true)
                Source = newSource;
        }
    }
}