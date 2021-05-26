using Imagin.Common;
using Imagin.Common.Models;
using System.Collections.ObjectModel;

namespace Paint
{
    public class ChannelsPanel : Panel
    {
        ObservableCollection<Channel> channels = new ObservableCollection<Channel>();
        public ObservableCollection<Channel> Channels
        {
            get => channels;
            set => this.Change(ref channels, value);
        }

        public override string Title => "Channels";

        public ChannelsPanel() : base(Resources.Uri(nameof(Paint), "/Images/Channels.png"))
        {
            Get.Current<MainViewModel>().ActiveDocumentChanged += OnDocumentChanged;
            Get.Current<MainViewModel>().DocumentClosed += OnDocumentClosed;
        }

        void OnDocumentChanged(object sender, Imagin.Common.Input.EventArgs<Document> e)
        {
            Channels = (e.Value as Document).Channels;
        }

        void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            if (Get.Current<MainViewModel>().Documents.Count == 0)
            {
                Channels = null;
            }
        }
    }
}