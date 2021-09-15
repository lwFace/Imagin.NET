using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System.Linq;

namespace Vault
{
    public class QueuePanel : Panel
    {
        Queue queue = null;
        public Queue Queue
        {
            get => queue;
            set
            {
                if (queue != null)
                    queue.Changed -= OnQueueChanged;

                if (value != null)
                    value.Changed += OnQueueChanged;

                this.Change(ref queue, value);
                OnQueueChanged();
            }
        }

        public override string Title
        {
            get
            {
                if (queue == null)
                    return "Queue";

                return $"Queue ({queue.Count})";
            }
        }

        public QueuePanel() : base(Resources.Uri(nameof(Vault), "/Images/Graph.png"))
        {
            Get.Current<MainViewModel>().SelectedItemsChanged += OnSelectedItemsChanged;
        }

        void OnQueueChanged() => this.Changed(() => Title);

        void OnQueueChanged(object sender) => OnQueueChanged();

        void OnSelectedItemsChanged(object sender, Imagin.Common.Input.EventArgs<System.Collections.IList> e)
        {
            Queue = e.Value?.First<CopyTask>()?.Queue;
        }
    }
}