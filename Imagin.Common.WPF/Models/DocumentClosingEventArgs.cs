using System.ComponentModel;

namespace Imagin.Common.Models
{
    public delegate void DocumentClosingEventHandler(object sender, DocumentClosingEventArgs e);

    public class DocumentClosingEventArgs : CancelEventArgs
    {
        public readonly Document Document;

        public DocumentClosingEventArgs(Document document)
        {
            Document = document;
        }
    }
}