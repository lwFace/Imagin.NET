using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Models;
using Imagin.Common.Serialization;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Paint
{
    public class FiltersPanel : Panel
    {
        public event EventHandler<EventArgs> Applied;

        public event EventHandler<EventArgs<Filter>> FilterSelected;

        bool applying = false;

        Filter filter;
        public Filter Filter
        {
            get => filter;
            set
            {
                this.Change(ref filter, value);
                OnFilterChanged(value);
            }
        }

        PixelLayer selectedLayer;
        public PixelLayer SelectedLayer
        {
            get => selectedLayer;
            set => this.Change(ref selectedLayer, value);
        }

        public override string Title => "Filters";

        public FiltersPanel() : base(Resources.Uri(nameof(Paint), "/Images/Filters.png"))
        {
            Get.Current<Options>().Filters = new ObservableCollection<Filter>()
            {
                new BakedFilter(),
                new BlueFilter(),
                new CastawayFilter(),
                new CheshireFilter(),
                new ColdFilter(),
                new DarkSepiaFilter(),
                new FrostbiteFilter(),
                new GreenFilter(),
                new IndependenceFilter(),
                new JungleFilter(),
                new LightPurpleFilter(),
                new MardiGrasFilter(),
                new MariettaFilter(),
                new MetroFilter(),
                new PerpetualFilter(),
                new PurpleFilter(),
                new RedFilter(),
                new RustyFilter(),
                new SciFiFilter(),
                new SepiaFilter(),
                new SpaceCadetFilter(),
                new SubmarineFilter(),
                new SunnyFilter(),
                new VintageFilter(),
                new WarmFilter(),
                new WesternFilter(),
            };
            Get.Current<MainViewModel>().LayerSelected += OnSelectedLayerChanged;
        }

        void OnFilterChanged(Filter filter)
        {
            FilterSelected?.Invoke(this, new EventArgs<Filter>(filter));
            /*
            if (Loading)
            {
                applyFilter = true;
                return;
            }

            if (Preview != null)
            {
                if (filter == null)
                {
                    Preview.Clear(System.Windows.Media.Colors.Transparent);
                    return;
                }
                Loading = true;

                var oldColors = Source.Colors();
                var newColors = new Matrix<Color>(oldColors.Rows, oldColors.Columns);

                await Task.Run(() => filter.Apply(oldColors, newColors));
                Preview = Preview ?? PixelLayer.New(newColors.Rows.Int32(), newColors.Columns.Int32());
                Preview.ForEach((x, y, color) => newColors.GetValue(y.UInt32(), x.UInt32()));

                Loading = false;
            }
            */
        }

        void OnSelectedLayerChanged(object sender, EventArgs<Layer> e)
        {
            if (e.Value is PixelLayer)
                SelectedLayer = (PixelLayer)e.Value;
        }

        ICommand applyCommand;
        public ICommand ApplyCommand
        {
            get
            {
                applyCommand = applyCommand ?? new RelayCommand<Filter>(i =>
                {
                    selectedLayer.Disabled = true;
                    applying = true;

                    i.Apply(selectedLayer.Pixels);

                    applying = false;
                    selectedLayer.Disabled = false;

                    Applied?.Invoke(this, EventArgs.Empty);
                },
                i => !applying && i != null && selectedLayer != null);
                return applyCommand;
            }
        }

        ICommand cloneCommand;
        public ICommand CloneCommand
        {
            get
            {
                cloneCommand = cloneCommand ?? new RelayCommand<Filter>(i => Get.Current<Options>().Filters.Add(i.Clone()), i => !applying && i != null);
                return cloneCommand;
            }
        }

        ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                deleteCommand = deleteCommand ?? new RelayCommand<Filter>(i => Get.Current<Options>().Filters.Remove(i), i => !applying && i != null);
                return deleteCommand;
            }
        }

        ICommand exportCommand;
        public ICommand ExportCommand
        {
            get
            {
                exportCommand = exportCommand ?? new RelayCommand<Filter>(i =>
                {
                    var filePath = string.Empty;
                    if (ExplorerWindow.Show(out filePath, "Export filter", ExplorerWindow.Modes.SaveFile, Array<string>.New("filter")))
                        BinarySerializer.Serialize(filePath, i);
                },
                i => !applying && i != null);
                return exportCommand;
            }
        }
    }
}