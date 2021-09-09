using Imagin.Common;
using Imagin.Common.Models;

namespace Paint
{
    public class PropertiesPanel : Panel
    {
        Document document = null;
        public Document Document
        {
            get => document;
            set => this.Change(ref document, value);
        }

        public override string Title => "Properties";

        public PropertiesPanel() : base(Resources.Uri(nameof(Paint), "/Images/Properties.png"))
        {
            Get.Current<MainViewModel>().ActiveDocumentChanged += OnActiveDocumentChanged;
            Get.Current<MainViewModel>().DocumentClosed += OnDocumentClosed; ;
        }

        void OnActiveDocumentChanged(object sender, Imagin.Common.Input.EventArgs<Document> e)
        {
            Document = e.Value;
        }

        void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (Get.Current<MainViewModel>().Documents.Count == 0)
                Document = null;
        }
    }
}