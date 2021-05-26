using Imagin.Common.Collections.Generic;

namespace Imagin.Common.Models
{
    public sealed class DocumentCollection : Collection<Document>
    {
        public event DocumentClosingEventHandler Closing;

        public bool IsModified
        {
            get
            {
                foreach (var i in this)
                {
                    if (i.IsModified)
                        return true;
                }
                return false;
            }
        }

        public DocumentCollection() : base() { }

        void OnClosing(DocumentClosingEventArgs e) => Closing?.Invoke(this, e);

        protected override void OnAdded(Document item)
        {
            base.OnAdded(item);
            item.OnAdded();
        }

        protected override void OnRemoved(Document item)
        {
            base.OnRemoved(item);
            item.OnRemoved();
        }

        /// <summary>
        /// Check to see if anything is currently subscribing to the "Cleared" and "PreviewCleared" events. Figure out way to call them
        /// here!
        /// </summary>
        public override void Clear()
        {
            for (var i = Count - 1; i >= 0; i--)
                Remove(this[i]);
        }

        public override bool Remove(Document i)
        {
            if (i.CanClose)
            {
                var result = new DocumentClosingEventArgs(i);
                OnClosing(result);
                return !result.Cancel ? base.Remove(i) : false;
            }
            return false;
        }

        public override void RemoveAt(int i)
        {
            if (this[i].CanClose)
            {
                var result = new DocumentClosingEventArgs(this[i]);
                OnClosing(result);

                if (!result.Cancel)
                    base.RemoveAt(i);
            }
        }

        public void Save()
        {
            foreach (var i in this)
                i.Save();
        }
    }
}