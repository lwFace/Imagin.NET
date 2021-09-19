using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Models;
using Imagin.Common.Storage;
using System.Linq;

namespace Explorer
{
    public class PropertiesPanel : Panel
    {
        public override string Title => "Properties";

        //..............................................................

        ExplorerDocument ActiveDocument;

        Item item = null;
        public Item Item
        {
            get => item;
            set => this.Change(ref item, value);
        }

        //..............................................................

        public PropertiesPanel() : base(Resources.Uri(nameof(Explorer), "/Images/Properties.png"))
        {
            Get.Current<MainViewModel>().ActiveDocumentChanged += OnActiveDocumentChanged;
        }

        //..............................................................

        void Refresh(ExplorerDocument document)
        {
            Refresh(document.Selection?.FirstOrDefault() ?? new Folder(document.Path));
        }

        void OnActiveDocumentChanged(object sender, EventArgs<ExplorerDocument> e)
        {
            if (ActiveDocument != null)
            {
                ActiveDocument.PathChanged -= Refresh;
                ActiveDocument.SelectionChanged -= Refresh;
            }

            ActiveDocument = e.Value;

            if (ActiveDocument == null)
            {
                Item i = null;
                Refresh(i);
                return;
            }

            if (ActiveDocument is ConsoleDocument a)
            {
                Refresh(new Folder(a.Path));
            }
            else if (ActiveDocument is ExplorerDocument b)
            {
                Refresh(b);
            }

            ActiveDocument.PathChanged += Refresh;
            ActiveDocument.SelectionChanged += Refresh;
        }

        //..............................................................

        void Refresh(Item item)
        {
            _ = item?.RefreshAsync();
            Item = item;
        }
    }
}