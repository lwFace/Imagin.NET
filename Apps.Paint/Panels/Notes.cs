using Imagin.Common;
using Imagin.Common.Models;

namespace Paint
{
    public class NotesPanel : Panel
    {
        Note note = default(Note);
        public Note Note
        {
            get => note;
            set => this.Change(ref note, value);
        }

        public override string Title => "Notes";

        public NotesPanel() : base(Resources.Uri(nameof(Paint), "/Images/Note.png"))
        {
            IsVisible = false;

            Get.Current<MainViewModel>().DocumentClosed += OnDocumentClosed;
        }

        void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            Note = null;
        }
    }
}