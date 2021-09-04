using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Paint
{
    public class HistogramPanel : Panel
    {
        bool update = false, updating = false;

        Document document = null;
        public Document Document
        {
            get => document;
            set => this.Change(ref document, value);
        }

        VisualLayer layer = null;
        public VisualLayer Layer
        {
            get => layer;
            set => this.Change(ref layer, value);
        }

        Histogram histogram = new Histogram();
        public Histogram Histogram
        {
            get => histogram;
            set => this.Change(ref histogram, value);
        }

        bool showBlue = true;
        public bool ShowBlue
        {
            get => showBlue;
            set => this.Change(ref showBlue, value);
        }

        bool showGreen = true;
        public bool ShowGreen
        {
            get => showGreen;
            set => this.Change(ref showGreen, value);
        }

        bool showRed = true;
        public bool ShowRed
        {
            get => showRed;
            set => this.Change(ref showRed, value);
        }

        bool showLuminance = true;
        public bool ShowLuminance
        {
            get => showLuminance;
            set => this.Change(ref showLuminance, value);
        }

        bool showSaturation = true;
        public bool ShowSaturation
        {
            get => showSaturation;
            set => this.Change(ref showSaturation, value);
        }

        public override string Title => "Histogram";

        public HistogramPanel() : base(Resources.Uri(nameof(Paint), "/Images/Histogram.png"))
        {
            Get.Current<MainViewModel>().ActiveDocumentChanged += OnActiveDocumentChanged;
            Get.Current<MainViewModel>().LayerSelected += OnLayerSelected;
        }

        async void Refresh()
        {
            if (updating)
            {
                update = true;
                return;
            }

            updating = true;

            if (layer != null)
            {
                var colors = App.Current.Dispatcher.Invoke(() => layer.Pixels.Colors());
                await Histogram.Read(colors);
            }

            updating = false;

            if (update)
            {
                update = false;
                Refresh();
            }
        }

        void OnActiveDocumentChanged(object sender, EventArgs<Document> e)
        {
            Document = (Document)e.Value;

            if (Document != null)
                Layer = Document.Layers.FirstOrDefault(i => i is VisualLayer && i.IsSelected) as VisualLayer;
        }

        void OnLayerSelected(object sender, EventArgs<Layer> e)
        {
            Layer = e.Value as VisualLayer;
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Layer):
                    Refresh();
                    break;
            }
        }

        ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                refreshCommand = refreshCommand ?? new RelayCommand(() => Refresh(), () => Layer != null);
                return refreshCommand;
            }
        }
    }
}