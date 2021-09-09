using Imagin.Common;
using Imagin.Common.Configuration;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class Options : Data<MainViewModel>
    {
        enum Category
        {
            Browser,
            Colors,
            Histogram,
            Window
        }

        #region Browser

        string browserPath = string.Empty;
        [Category(Category.Browser)]
        [DisplayName("Path")]
        [StringFormat(StringFormat.FolderPath)]
        public string BrowserPath
        {
            get => browserPath;
            set => this.Change(ref browserPath, value);
        }

        #endregion

        #region Capture

        bool captureClipboard = true;
        [Hidden]
        public bool CaptureClipboard
        {
            get => captureClipboard;
            set => this.Change(ref captureClipboard, value);
        }

        bool captureFile = false;
        [Hidden]
        public bool CaptureFile
        {
            get => captureFile;
            set => this.Change(ref captureFile, value);
        }

        bool captureLayer = false;
        [Hidden]
        public bool CaptureLayer
        {
            get => captureLayer;
            set => this.Change(ref captureLayer, value);
        }

        #endregion

        #region Colors

        StringColor backgroundColor = System.Windows.Media.Colors.White;
        [Category(Category.Colors)]
        [DisplayName("Background")]
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => this.Change(ref backgroundColor, value);
        }

        StringColor foregroundColor = System.Windows.Media.Colors.Black;
        [Category(Category.Colors)]
        [DisplayName("Foreground")]
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => this.Change(ref foregroundColor, value);
        }

        #endregion

        #region Histogram

        StringColor histogramBackground = System.Windows.Media.Colors.Transparent;
        [Category(Category.Histogram)]
        [DisplayName("Background")]
        public Color HistogramBackground
        {
            get => histogramBackground;
            set => this.Change(ref histogramBackground, value);
        }

        double histogramOpacity = 0.3;
        [Category(Category.Histogram)]
        [DisplayName("Opacity")]
        [Range(0.1, 0.9, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double HistogramOpacity
        {
            get => histogramOpacity;
            set => this.Change(ref histogramOpacity, value);
        }

        StringColor histogramRed = System.Windows.Media.Colors.Red;
        [Category(Category.Histogram)]
        [DisplayName("Red")]
        public Color HistogramRed
        {
            get => histogramRed;
            set => this.Change(ref histogramRed, value);
        }

        StringColor histogramGreen = System.Windows.Media.Colors.Green;
        [Category(Category.Histogram)]
        [DisplayName("Green")]
        public Color HistogramGreen
        {
            get => histogramGreen;
            set => this.Change(ref histogramGreen, value);
        }

        StringColor histogramBlue = System.Windows.Media.Colors.Blue;
        [Category(Category.Histogram)]
        [DisplayName("Blue")]
        public Color HistogramBlue
        {
            get => histogramBlue;
            set => this.Change(ref histogramBlue, value);
        }

        StringColor histogramLuminance = System.Windows.Media.Colors.Black;
        [Category(Category.Histogram)]
        [DisplayName("Luminance")]
        public Color HistogramLuminance
        {
            get => histogramLuminance;
            set => this.Change(ref histogramLuminance, value);
        }

        StringColor histogramSaturation = System.Windows.Media.Colors.Magenta;
        [Category(Category.Histogram)]
        [DisplayName("Saturation")]
        public Color HistogramSaturation
        {
            get => histogramSaturation;
            set => this.Change(ref histogramSaturation, value);
        }

        #endregion

        #region Other

        ObservableCollection<StringColor> colors = new ObservableCollection<StringColor>();
        [Hidden]
        public ObservableCollection<StringColor> Colors
        {
            get => colors ?? (colors = new ObservableCollection<StringColor>());
            set => this.Change(ref colors, value);
        }

        ObservableCollection<Filter> filters;
        [Hidden]
        public ObservableCollection<Filter> Filters
        {
            get => filters;
            set => this.Change(ref filters, value);
        }

        int filterPreviewHeight = 72;
        [Category(nameof(Filter))]
        [DisplayName("Preview width")]
        [Range(32, 256, 1)]
        [RangeFormat(RangeFormat.Slider)]
        public int FilterPreviewHeight
        {
            get => filterPreviewHeight;
            set => this.Change(ref filterPreviewHeight, value);
        }

        string canvasBackground = $"255,{System.Windows.Media.Colors.DarkGray.R},{System.Windows.Media.Colors.DarkGray.G},{System.Windows.Media.Colors.DarkGray.B}";
        [Category(nameof(Canvas))]
        [DisplayName("Background")]
        public System.Windows.Media.Color CanvasBackground
        {
            get
            {
                var result = canvasBackground.Split(',');
                return System.Windows.Media.Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte());
            }
            set => this.Change(ref canvasBackground, $"{value.A},{value.R},{value.G},{value.B}");
        }

        string canvasGridLines = $"255,{System.Windows.Media.Colors.DarkGray.R},{System.Windows.Media.Colors.DarkGray.G},{System.Windows.Media.Colors.DarkGray.B}";
        [Category(nameof(Canvas))]
        [DisplayName("Grid lines")]
        public System.Windows.Media.Color CanvasGridLines
        {
            get
            {
                var result = canvasGridLines.Split(',');
                return System.Windows.Media.Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte());
            }
            set => this.Change(ref canvasGridLines, $"{value.A},{value.R},{value.G},{value.B}");
        }

        ObservableCollection<DocumentPreset> documentPresets = new ObservableCollection<DocumentPreset>();
        [Hidden]
        public ObservableCollection<DocumentPreset> DocumentPresets
        {
            get => documentPresets ?? (documentPresets = new ObservableCollection<DocumentPreset>());
            set => this.Change(ref documentPresets, value);
        }

        DocumentPreset documentPreset = null;
        [Hidden]
        public DocumentPreset DocumentPreset
        {
            get => documentPreset;
            set => this.Change(ref documentPreset, value);
        }

        ObservableCollection<LinearGradient> gradientsPreserved = new ObservableCollection<LinearGradient>();

        [field: NonSerialized]
        ObservableCollection<LinearGradientBrush> gradients = new ObservableCollection<LinearGradientBrush>();
        [Hidden]
        public ObservableCollection<LinearGradientBrush> Gradients
        {
            get => gradients;
            set => this.Change(ref gradients, value);
        }

        GraphicalUnit graphicalUnit = GraphicalUnit.Pixel;
        [Category("Graphics")]
        [DisplayName("Graphic unit")]
        public GraphicalUnit GraphicalUnit
        {
            get => graphicalUnit;
            set => this.Change(ref graphicalUnit, value);
        }

        ObservableCollection<string> recentFiles = new ObservableCollection<string>();
        [Hidden]
        public ObservableCollection<string> RecentFiles
        {
            get => recentFiles ?? (recentFiles = new ObservableCollection<string>());
            set => this.Change(ref recentFiles, value);
        }

        [field: NonSerialized]
        ToolCollection tools = new ToolCollection();
        [Hidden]
        public ToolCollection Tools
        {
            get => tools;
            set => this.Change(ref tools, value);
        }

        bool viewGridLines = true;
        [Hidden]
        public bool ViewGridLines
        {
            get => viewGridLines;
            set => this.Change(ref viewGridLines, value);
        }

        bool viewRulers = false;
        [Hidden]
        public bool ViewRulers
        {
            get => viewRulers;
            set => this.Change(ref viewRulers, value);
        }

        #endregion

        protected override void OnLoaded()
        {
            base.OnLoaded();
            gradients = new ObservableCollection<LinearGradientBrush>();
            gradientsPreserved.ForEach(i =>
            {
                var result = new LinearGradientBrush();
                i.Stops.ForEach(j => result.GradientStops.Add(new GradientStop(j.Color.Value, j.Offset)));
                gradients.Add(result);
            });
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            gradientsPreserved.Clear();
            gradients.ForEach(i => gradientsPreserved.Add(new LinearGradient(i)));
        }

        [field: NonSerialized]
        ICommand backgroundColorCommand;
        [Hidden]
        public ICommand BackgroundColorCommand
        {
            get
            {
                backgroundColorCommand = backgroundColorCommand ?? new RelayCommand<object>(i =>
                {
                    if (i is Color)
                    {
                        CanvasBackground = (Color)i;
                        return;
                    }

                    var window = new Imagin.Common.Controls.ColorWindow(CanvasBackground);
                    window.ShowDialog();
                    if (window.Result)
                        CanvasBackground = window.Color;
                });
                return backgroundColorCommand;
            }
        }
    }
}