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

        //---------------------------------------------------------------------

        ExplorerDocument ActiveDocument;

        Item item = null;
        public Item Item
        {
            get => item;
            set => this.Change(ref item, value);
        }

        //---------------------------------------------------------------------

        public PropertiesPanel() : base(Resources.Uri(nameof(Explorer), "/Images/Properties.png"))
        {
            Get.Current<MainViewModel>().ActiveDocumentChanged += OnActiveDocumentChanged;
            Get.Current<MainViewModel>().DocumentClosed += OnDocumentClosed;
        }

        //---------------------------------------------------------------------

        void OnActiveDocumentChanged(ExplorerDocument sender)
        {
            Refresh(sender.Selection?.FirstOrDefault() ?? new Folder(sender.Path));
        }

        void OnActiveDocumentChanged(object sender, EventArgs<ExplorerDocument> e)
        {
            if (ActiveDocument != null)
                Unsubscribe(ActiveDocument);

            ActiveDocument = e.Value;
            if (ActiveDocument is ConsoleDocument a)
            {
                Refresh(new Folder(a.Path));
                return;
            }

            ActiveDocument.PathChanged += OnActiveDocumentChanged;
            ActiveDocument.SelectionChanged += OnActiveDocumentChanged;
            OnActiveDocumentChanged(ActiveDocument);
        }

        void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (Get.Current<MainViewModel>().Documents.Count == 0)
            {
                if (ActiveDocument != null)
                {
                    Unsubscribe(ActiveDocument);
                    ActiveDocument = null;
                }
                Refresh(null);
            }
        }

        //---------------------------------------------------------------------

        void Refresh(Item item)
        {
            _ = item?.RefreshAsync();
            Item = item;
        }

        //---------------------------------------------------------------------

        void Unsubscribe(ExplorerDocument document)
        {
            ActiveDocument.PathChanged -= OnActiveDocumentChanged;
            ActiveDocument.SelectionChanged -= OnActiveDocumentChanged;
        }
    }
}