using System;

namespace Imagin.Common.Models
{
    public delegate void DocumentClosedEventHandler(object sender, DocumentClosedEventArgs e);

    public class DocumentClosedEventArgs : EventArgs
    {
        public readonly Document Document;

        public DocumentClosedEventArgs(Document document)
        {
            Document = document;
        }
    }
}