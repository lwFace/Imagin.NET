using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Models;

namespace Paint
{
    public sealed class HistoryPanel : Panel
    {
        ActionCollection history;
        public ActionCollection History
        {
            get => history;
            set => this.Change(ref history, value);
        }

        public override string Title => "History";

        public HistoryPanel() : base(Resources.Uri(nameof(Paint), "/Images/Clock.png"))
        {
            IsVisible = false;

            Get.Current<MainViewModel>().ActiveContentChanged += OnActiveContentChanged;
            Get.Current<MainViewModel>().DocumentClosed += OnDocumentClosed;
        }

        void OnActiveContentChanged(object sender, EventArgs<Content> e)
        {
            if (e.Value is Document)
                History = (e.Value as Document).History;
        }

        void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (Get.Current<MainViewModel>().Documents.Count == 0)
                History = null;
        }
    }
}