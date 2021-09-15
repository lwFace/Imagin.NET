using Imagin.Common;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using Imagin.Common.Serialization;
using Imagin.Common.Storage;
using Paint.Adjust;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    public class MainViewModel : MainViewModel<MainWindow, Document>
    {
        #region Properties

        public event EventHandler<EventArgs<Layer>> LayerSelected;

        /// ........................................................................

        public LayersPanel LayersViewModel => Panel<LayersPanel>();

        /// ........................................................................

        new public Document ActiveDocument => (Document)base.ActiveDocument;

        /// ........................................................................

        ObservableCollection<AdjustmentEffect> adjustments = new ObservableCollection<AdjustmentEffect>();
        public ObservableCollection<AdjustmentEffect> Adjustments
        {
            get => adjustments;
            set => this.Change(ref adjustments, value);
        }

        Layer selectedLayer;
        public Layer SelectedLayer
        {
            get => selectedLayer;
            set => this.Change(ref selectedLayer, value);
        }

        Tool tool;
        public Tool Tool
        {
            get => tool;
            set => this.Change(ref tool, value);
        }

        #endregion

        #region MainViewModel

        public MainViewModel() : base() { }

        #endregion

        #region Methods

        public override IEnumerable<Panel> Load()
        {
            #region Document presets

            //Load default document presets if none
            if (Get.Current<Options>().DocumentPresets.Count == 0)
            {
                Get.Current<Options>().DocumentPresets.Add(new DocumentPreset("3X small")
                { Background = Colors.White, Height = 32, Width = 32 });
                Get.Current<Options>().DocumentPresets.Add(new DocumentPreset("2X small")
                { Background = Colors.White, Height = 64, Width = 64 });
                Get.Current<Options>().DocumentPresets.Add(new DocumentPreset("1X small")
                { Background = Colors.White, Height = 128, Width = 128 });
                Get.Current<Options>().DocumentPresets.Add(new DocumentPreset("Small")
                { Background = Colors.White, Height = 256, Width = 256 });
                Get.Current<Options>().DocumentPresets.Add(new DocumentPreset("Medium")
                { Background = Colors.White, Height = 500, Width = 500 });
                Get.Current<Options>().DocumentPresets.Add(new DocumentPreset("Large")
                { Background = Colors.White, Height = 750, Width = 750 });
                Get.Current<Options>().DocumentPresets.Add(new DocumentPreset("1X large")
                { Background = Colors.White, Height = 1000, Width = 1000 });
                Get.Current<Options>().DocumentPresets.Add(new DocumentPreset("2X large")
                { Background = Colors.White, Height = 1200, Width = 1200 });
                Get.Current<Options>().DocumentPresets.Add(new DocumentPreset("3X large")
                { Background = Colors.White, Height = 1500, Width = 1500 });
            }

            #endregion

            #region Adjustments

            var adjustments = Assembly.GetCallingAssembly().GetTypes().Select(i => i);
            adjustments = adjustments.Where(i => i.Namespace == $"{nameof(Paint)}.{nameof(Adjust)}" && typeof(AdjustmentEffect).IsAssignableFrom(i));
            adjustments = adjustments.Where(i => !i.Equals(typeof(AdjustmentEffect)));

            foreach (var i in adjustments)
            {
                var a = (AdjustmentEffect)Activator.CreateInstance(i);
                Adjustments.Add(a);
            }

            #endregion

            yield return new BrowserPanel();
            yield return new BrushPanel();
            yield return new ChannelsPanel();
            yield return new CharacterPanel();
            yield return new ColorPanel();
            yield return new ColorsPanel();
            yield return new FilterPanel();

            var filters = new FiltersPanel();
            filters.FilterSelected += (s, e) => Panel<FilterPanel>().SelectedFilter = e.Value;
            yield return filters;

            yield return new HistogramPanel();
            yield return new HistoryPanel();
            yield return new LayerPanel();

            var layers = new LayersPanel();
            layers.LayerSelected += (s, e) => Panel<LayerPanel>().Layers = Array<Layer>.New(e.Value);
            layers.LayerSelected += (s, e) => SelectedLayer = e.Value;
            layers.LayerSelected += (s, e) => LayerSelected?.Invoke(this, e);
            yield return layers;

            yield return new NotesPanel();
            yield return new OptionsPanel();
            yield return new ParagraphPanel();
            yield return new PropertiesPanel();
            yield return new ToolPanel();

            var tools = new ToolsPanel();
            tools.ToolSelected += (s, e) => Tool = e.Value;
            tools.ToolSelected += (s, e) =>
            {
                if (e.Value is BrushTool)
                    Panel<BrushPanel>().Brush = (e.Value as BrushTool).Brush;
            };
            tools.ToolSelected += (s, e) => Panel<ToolPanel>().Tool = e.Value;
            yield return tools;
        }

        public async Task OpenFile(string filePath)
        {
            var fileExtension = System.IO.Path.GetExtension(filePath).Replace(".", string.Empty).ToString();

            //If the file is already open, just activate it
            if (ActivateDocument(filePath))
                return;

            if (ImageFormats.IsReadable(fileExtension))
            {
                Document document = null;
                void error() => Dialog.Show("Open", $"The file '{filePath}' is invalid or corrupt.", DialogImage.Error, DialogButtons.Ok);

                switch (fileExtension)
                {
                    case Document.DefaultExtension:
                        BinarySerializer.Deserialize(filePath, out document);
                        if (document == null)
                        {
                            error();
                            return;
                        }

                        document.Restore();
                        break;

                    default:
                        var result = await Document.Open(filePath);
                        if (result == null)
                        {
                            error();
                            return;
                        }

                        document = new Document(result.PixelHeight, result.PixelWidth)
                        {
                            FilePath = filePath,
                            PixelFormat = result.Format.Convert()
                        };

                        var layer = new PixelLayer("Layer 0", result) { IsSelected = true };
                        document.Layers.Add(layer);
                        break;
                }
                OnOpened(document);
            }
            else Dialog.Show("Open", "The file extension '{0}' is not supported.".F(fileExtension), DialogImage.Error, DialogButtons.Ok);
        }

        //---------------------------------------------------------------------

        public void SaveDefault(string filePath)
        {
        }

        //---------------------------------------------------------------------

        void OnOpened(Document document)
        {
            Documents.Add(document);
            Get.Current<Options>().RecentFiles.Add(document.FilePath);
        }

        //---------------------------------------------------------------------

        bool ActivateDocument(string path)
        {
            var document = Documents.FirstOrDefault(i => i.As<Document>().FilePath == path);

            if (document != null)
            {
                ActiveContent = document;
                return true;
            }

            return false;
        }

        //---------------------------------------------------------------------

        bool? Save(System.Drawing.Bitmap bitmap)
        {
            var path = string.Empty;
            if (ExplorerWindow.Show(out path, "Save...", ExplorerWindow.Modes.SaveFile, ImageFormats.Writable.Select(i => i.Extension)))
            {
                if (path?.Length > 0)
                {
                    try
                    {
                        using (var image = new ImageMagick.MagickImage(bitmap))
                            image.Write(path);

                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return null;
        }

        void Capture(System.Drawing.Bitmap bitmap)
        {
            if (Get.Current<Options>().CaptureClipboard)
            {
                Clipboard.SetImage(bitmap.BitmapSource());
            }
            else if (Get.Current<Options>().CaptureFile)
            {
                var result = Save(bitmap);
                if (result == false)
                {
                    Dialog.Show(nameof(Capture), "Couldn't capture file.", DialogImage.Error, DialogButtons.Ok);
                    return;
                }
            }
            else if (Get.Current<Options>().CaptureLayer)
            {
                //Double check there is a collection of layers to access
                if (Panel<LayersPanel>().Layers != null)
                {
                    var newLayer = new PixelLayer("Capture", bitmap.WriteableBitmap());
                    Panel<LayersPanel>().Add(newLayer, true);
                }
            }
        }

        #region <File>

        ICommand newCommand;
        public ICommand NewCommand
        {
            get
            {
                newCommand = newCommand ?? new RelayCommand(() =>
                {
                    var window = new NewWindow();
                    window.ShowDialog();

                    switch (window.Result)
                    {
                        case 1:
                            var document = new Document(window.DocumentPreset) { IsModified = true };
                            Documents.Add(document);
                            break;
                    }

                }, () => true);
                return newCommand;
            }
        }

        ICommand newCollectionCommand;
        public ICommand NewCollectionCommand
        {
            get
            {
                newCollectionCommand = newCollectionCommand ?? new RelayCommand(() =>
                {
                }, () => true);
                return newCollectionCommand;
            }
        }

        ICommand openCommand;
        public ICommand OpenCommand
        {
            get
            {
                openCommand = openCommand ?? new RelayCommand(async () =>
                {
                    if (ExplorerWindow.Show(out string[] paths, "Open...", ExplorerWindow.Modes.OpenFile, ImageFormats.Readable.Select(i => i.Extension), ActiveDocument?.FilePath))
                    {
                        foreach (var i in paths)
                            await OpenFile(i);
                    }
                }, 
                () => true);
                return openCommand;
            }
        }

        ICommand openRecentFileCommand;
        public ICommand OpenRecentFileCommand
        {
            get
            {
                openRecentFileCommand = openRecentFileCommand ?? new RelayCommand<object>(i => _ = OpenFile(i.ToString()), i => i is string && System.IO.File.Exists(i.ToString()));
                return openRecentFileCommand;
            }
        }

        ICommand clearRecentFilesCommand;
        public ICommand ClearRecentFilesCommand
        {
            get
            {
                clearRecentFilesCommand = clearRecentFilesCommand ?? new RelayCommand(() => Get.Current<Options>().RecentFiles.Clear(), () => Get.Current<Options>()?.RecentFiles.Count > 0);
                return clearRecentFilesCommand;
            }
        }

        ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                saveCommand = saveCommand ?? new RelayCommand(() =>
                {
                    //If the document was modified...
                    if (ActiveDocument.IsModified)
                    {
                        //...and a file path is defined...
                        if (!ActiveDocument.FilePath.NullOrEmpty())
                        {
                            //...and the file extension is writable...
                            if (ImageFormats.IsWritable(ActiveDocument.FileExtension))
                            {
                                //Serialize it!
                                BinarySerializer.Serialize(ActiveDocument.FilePath, ActiveDocument);
                            }
                        }
                    }

                }, () => ActiveDocument?.IsModified == true);
                return saveCommand;
            }
        }

        ICommand saveAsCommand;
        public ICommand SaveAsCommand
        {
            get
            {
                saveAsCommand = saveAsCommand ?? new RelayCommand(() => _ = ActiveDocument.SaveAs(), () => ActiveDocument != null);
                return saveAsCommand;
            }
        }

        ICommand propertiesCommand;
        public ICommand PropertiesCommand
        {
            get
            {
                propertiesCommand = propertiesCommand ?? new RelayCommand(() => Machine.Properties.Show(ActiveDocument.FilePath), () => ActiveDocument != null && System.IO.File.Exists(ActiveDocument.FilePath));
                return propertiesCommand;
            }
        }

        ICommand showInWindowsExplorerCommand;
        public ICommand ShowInWindowsExplorerCommand
        {
            get
            {
                showInWindowsExplorerCommand = showInWindowsExplorerCommand ?? new RelayCommand(() => Machine.ShowInWindowsExplorer(ActiveDocument.FilePath), () => ActiveDocument != null && System.IO.File.Exists(ActiveDocument.FilePath));
                return showInWindowsExplorerCommand;
            }
        }

        #endregion

        #region <Edit>

        ICommand undoCommand;
        public ICommand UndoCommand
        {
            get
            {
                undoCommand = undoCommand ?? new RelayCommand(() => ActiveDocument.History.Undo(), () => ActiveDocument != null && ActiveDocument.History.U.Any());
                return undoCommand;
            }
        }

        ICommand redoCommand;
        public ICommand RedoCommand
        {
            get
            {
                redoCommand = redoCommand ?? new RelayCommand(() => ActiveDocument.History.Redo(), () => ActiveDocument != null && ActiveDocument.History.R.Any());
                return redoCommand;
            }
        }

        ICommand repeatCommand;
        public ICommand RepeatCommand
        {
            get
            {
                repeatCommand = repeatCommand ?? new RelayCommand(() => ActiveDocument.History.Repeat(), () => ActiveDocument != null && ActiveDocument.History.Any());
                return repeatCommand;
            }
        }

        ICommand copyCommand;
        public ICommand CopyCommand
        {
            get
            {
                copyCommand = copyCommand ?? new RelayCommand(() =>
                {
                    var visualLayer = SelectedLayer as VisualLayer;
                    Clipboard.SetImage(visualLayer.Render(new System.Drawing.Size(ActiveDocument.Size.Width, ActiveDocument.Size.Height)).BitmapSource());
                }, () => SelectedLayer is VisualLayer);
                return copyCommand;
            }
        }

        ICommand copyMergedCommand;
        public ICommand CopyMergedCommand
        {
            get
            {
                copyMergedCommand = copyMergedCommand ?? new RelayCommand(async () =>
                {
                    var bitmap = await ActiveDocument.Render();
                    Clipboard.SetImage(bitmap.BitmapSource());
                }, () => ActiveDocument != null);
                return copyMergedCommand;
            }
        }

        ICommand pasteCommand;
        public ICommand PasteCommand
        {
            get
            {
                pasteCommand = pasteCommand ?? new RelayCommand(() =>
                {
                    if (SelectedLayer is PixelLayer)
                    {
                        var pixelLayer = SelectedLayer as PixelLayer;

                        var oldBitmap
                            = pixelLayer.Pixels;
                        var newBitmap
                            = Clipboard.GetImage().Bitmap().WriteableBitmap();

                        int
                            sx = (oldBitmap.PixelWidth / 2.0).Int32()
                                - (newBitmap.PixelWidth / 2.0).Int32(),
                            sy = (oldBitmap.PixelHeight / 2.0).Int32()
                                - (newBitmap.PixelHeight / 2.0).Int32();

                        newBitmap.ForEach((x1, y1, color) =>
                        {
                            var x2 = x1 + sx;
                            var y2 = y1 + sy;

                            if (x2 < 0 || y2 < 0 || x2 > oldBitmap.PixelWidth || y2 > oldBitmap.PixelHeight)
                                return color;

                            oldBitmap.SetPixel(x2, y2, color);
                            return color;
                        });
                    }
                    else PasteNewLayerCommand.Execute(null);
                },
                () => ActiveDocument != null && Clipboard.ContainsImage());
                return pasteCommand;
            }
        }

        ICommand pasteNewLayerCommand;
        public ICommand PasteNewLayerCommand
        {
            get
            {
                pasteNewLayerCommand = pasteNewLayerCommand ?? new RelayCommand(() =>
                {
                    var newLayer = new PixelLayer("Untitled", System.Windows.Clipboard.GetImage().Bitmap().WriteableBitmap());
                    LayersViewModel.InsertAboveCommand.Execute(newLayer);
                }, () => ActiveDocument != null && System.Windows.Clipboard.ContainsImage());
                return pasteNewLayerCommand;
            }
        }

        ICommand clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                clearCommand = clearCommand ?? new RelayCommand(() =>
                {
                    if (SelectedLayer is PixelLayer)
                    {
                        var pixelLayer = SelectedLayer as PixelLayer;
                        foreach (var i in ActiveDocument.Selections)
                            pixelLayer.Pixels.FillPolygon(CustomShape.From(i.Points), Colors.Transparent, null);
                    }
                    else
                    {
                        if (Dialog.Show("Are you sure you want to delete this?", "Clear", DialogImage.Warning, DialogButtons.YesNo) == 0)
                        {
                            var layers
                                = SelectedLayer.Parent != null
                                ? SelectedLayer.Parent.Layers
                                : LayersViewModel.Layers;

                            layers.Remove(SelectedLayer);
                        }
                    }
                }, () => SelectedLayer is VisualLayer);
                return clearCommand;
            }
        }

        #endregion

        #region <Capture>

        ICommand captureWindowCommand;
        public ICommand CaptureWindowCommand
        {
            get
            {
                captureWindowCommand = captureWindowCommand ?? new RelayCommand(() => Capture(Computer.Screen.CaptureForegroundWindow()), () => true);
                return captureWindowCommand;
            }
        }

        ICommand captureForegroundWindowCommand;
        public ICommand CaptureForegroundWindowCommand
        {
            get
            {
                captureForegroundWindowCommand = captureForegroundWindowCommand ?? new RelayCommand(() =>
                {
                    var bitmap = default(System.Drawing.Bitmap);

                    var methods = new Action[3];
                    methods[0] = new Action(() => View.Hide());
                    methods[1] = new Action(() => bitmap = Computer.Screen.CaptureForegroundWindow());
                    methods[2] = new Action(() => View.Show());

                    for (int i = 0; i < methods.Count(); i++)
                        methods[i]();

                    Capture(bitmap);

                }, () => true);
                return captureForegroundWindowCommand;
            }
        }

        ICommand captureScreenCommand;
        public ICommand CaptureScreenCommand
        {
            get
            {
                captureScreenCommand = captureScreenCommand ?? new RelayCommand(() =>
                {
                    var bitmap = default(System.Drawing.Bitmap);

                    var methods = new Action[3];
                    methods[0] = new Action(() => View.Hide());
                    methods[1] = new Action(() => bitmap = Computer.Screen.CaptureDesktop());
                    methods[2] = new Action(() => View.Show());

                    for (int i = 0; i < methods.Count(); i++)
                        methods[i]();

                    Capture(bitmap);

                }, () => true);
                return captureScreenCommand;
            }
        }

        #endregion

        #region <Image>

        ICommand imageResizeCommand;
        public ICommand ImageResizeCommand
        {
            get
            {
                imageResizeCommand = imageResizeCommand ?? new RelayCommand(() => new ResizeWindow(ActiveDocument).ShowDialog(), () => ActiveDocument != null);
                return imageResizeCommand;
            }
        }

        ICommand imageRotateCommand;
        public ICommand ImageRotateCommand
        {
            get
            {
                imageRotateCommand = imageRotateCommand ?? new RelayCommand(() => { }, () => ActiveDocument != null);
                return imageRotateCommand;
            }
        }

        #endregion

        #region <Filter>

        bool filtering = false;

        ICommand newFilterCommand;
        public ICommand NewFilterCommand
        {
            get
            {
                newFilterCommand = newFilterCommand ?? new RelayCommand(() => Get.Current<Options>().Filters.Add(new Filter("Untitled")), () => true);
                return newFilterCommand;
            }
        }

        ICommand importFiltersCommand;
        public ICommand ImportFiltersCommand
        {
            get
            {
                importFiltersCommand = importFiltersCommand ?? new RelayCommand(() =>
                {
                    if (ExplorerWindow.Show(out string[] filePaths, "Import filter(s)", ExplorerWindow.Modes.OpenFile, Array<string>.New("filter")))
                    {
                        foreach (var i in filePaths)
                        {
                            BinarySerializer.Deserialize(i, out Filter filter);
                            if (filter != null)
                                Get.Current<Options>().Filters.Add(filter);
                        }
                    }
                },
                () => true);
                return importFiltersCommand;
            }
        }

        ICommand applyFilterCommand;
        public ICommand ApplyFilterCommand
        {
            get
            {
                applyFilterCommand = applyFilterCommand ?? new RelayCommand<Filter>(i =>
                {
                    if (!filtering)
                    {
                        filtering = true;
                        i.Apply((SelectedLayer as PixelLayer).Pixels);
                        filtering = false;
                    }
                },
                i => !filtering && i is Filter && SelectedLayer is PixelLayer);
                return applyFilterCommand;
            }
        }

        #endregion

        #region <Transform>

        ICommand flipHorizontalCommand;
        public ICommand FlipHorizontalCommand
        {
            get
            {
                flipHorizontalCommand = flipHorizontalCommand ?? new RelayCommand(() => (SelectedLayer as VisualLayer).Flip(System.Windows.Media.Imaging.WriteableBitmapExtensions.FlipMode.Horizontal), () => SelectedLayer is VisualLayer);
                return flipHorizontalCommand;
            }
        }

        ICommand flipVerticalCommand;
        public ICommand FlipVerticalCommand
        {
            get
            {
                flipVerticalCommand = flipVerticalCommand ?? new RelayCommand(() => (SelectedLayer as VisualLayer).Flip(System.Windows.Media.Imaging.WriteableBitmapExtensions.FlipMode.Vertical), () => SelectedLayer is VisualLayer);
                return flipVerticalCommand;
            }
        }

        ICommand rotateCommand;
        public ICommand RotateCommand
        {
            get
            {
                rotateCommand = rotateCommand ?? new RelayCommand<object>(i => (SelectedLayer as VisualLayer).Rotate(i.ToString().Int32()), i => i != null && SelectedLayer is VisualLayer);
                return rotateCommand;
            }
        }

        ICommand rotateTransformCommand;
        public ICommand RotateTransformCommand
        {
            get
            {
                rotateTransformCommand = rotateTransformCommand ?? new RelayCommand(() => (SelectedLayer as VisualLayer).Transform = new RotateTransform(SelectedLayer as VisualLayer), () => SelectedLayer is VisualLayer);
                return rotateTransformCommand;
            }
        }

        ICommand scaleTransformCommand;
        public ICommand ScaleTransformCommand
        {
            get
            {
                scaleTransformCommand = scaleTransformCommand ?? new RelayCommand(() => (SelectedLayer as VisualLayer).Transform = new ScaleTransform(SelectedLayer as VisualLayer), () => SelectedLayer is VisualLayer);
                return scaleTransformCommand;
            }
        }

        ICommand skewTransformCommand;
        public ICommand SkewTransformCommand
        {
            get
            {
                skewTransformCommand = skewTransformCommand ?? new RelayCommand(() => (SelectedLayer as VisualLayer).Transform = new SkewTransform(SelectedLayer as VisualLayer), () => SelectedLayer is VisualLayer);
                return skewTransformCommand;
            }
        }

        ICommand distortTransformCommand;
        public ICommand DistortTransformCommand
        {
            get
            {
                distortTransformCommand = distortTransformCommand ?? new RelayCommand(() => (SelectedLayer as VisualLayer).Transform = new DistortTransform(SelectedLayer as VisualLayer), () => SelectedLayer is VisualLayer);
                return distortTransformCommand;
            }
        }

        ICommand perspectiveTransformCommand;
        public ICommand PerspectiveTransformCommand
        {
            get
            {
                perspectiveTransformCommand = perspectiveTransformCommand ?? new RelayCommand(() => (SelectedLayer as VisualLayer).Transform = new PerspectiveTransform(SelectedLayer as VisualLayer), () => SelectedLayer is VisualLayer);
                return perspectiveTransformCommand;
            }
        }

        ICommand warpTransformCommand;
        public ICommand WarpTransformCommand
        {
            get
            {
                warpTransformCommand = warpTransformCommand ?? new RelayCommand(() => (SelectedLayer as VisualLayer).Transform = new WarpTransform(SelectedLayer as VisualLayer), () => SelectedLayer is VisualLayer);
                return warpTransformCommand;
            }
        }

        #endregion

        #region <Select>

        ICommand selectAllCommand;
        public ICommand SelectAllCommand
        {
            get
            {
                selectAllCommand = selectAllCommand ?? new RelayCommand(() =>
                {
                    ActiveDocument.Selections.Clear();
                    ActiveDocument.Selections.Add(new CustomPath(new Point(0, 0), new Point(ActiveDocument.Width, 0), new Point(ActiveDocument.Width, ActiveDocument.Height), new Point(0, ActiveDocument.Height)));
                }, () => ActiveDocument != null);
                return selectAllCommand;
            }
        }

        ICommand invertSelectionCommand;
        public ICommand InvertSelectionCommand
        {
            get
            {
                invertSelectionCommand = invertSelectionCommand ?? new RelayCommand(() =>
                {
                }, () => ActiveDocument?.Selections.Count > 0);
                return invertSelectionCommand;
            }
        }

        ICommand loadSelectionCommand;
        public ICommand LoadSelectionCommand
        {
            get
            {
                loadSelectionCommand = loadSelectionCommand ?? new RelayCommand(() =>
                {
                    var filePath = string.Empty;
                    if (ExplorerWindow.Show(out filePath, "Load selection...", ExplorerWindow.Modes.OpenFile, Array<string>.New("selection")))
                    {
                        BinarySerializer.Deserialize(filePath, out List<List<Point>> selections);
                        selections?.ForEach(i => ActiveDocument.Selections.Add(new CustomPath(i)));
                    }
                }, () => ActiveDocument != null);
                return loadSelectionCommand;
            }
        }

        ICommand saveSelectionCommand;
        public ICommand SaveSelectionCommand
        {
            get
            {
                saveSelectionCommand = saveSelectionCommand ?? new RelayCommand(() =>
                {
                    var filePath = string.Empty;
                    if (ExplorerWindow.Show(out filePath, "Save selection", ExplorerWindow.Modes.SaveFile, Array<string>.New("selection")))
                    {
                        List<List<Point>> result = new List<List<Point>>();
                        ActiveDocument.Selections.ForEach(i => result.Add(new List<Point>(i.Points)));

                        BinarySerializer.Serialize(filePath, result);
                    }
                }, 
                () => ActiveDocument?.Selections.Count > 0);
                return saveSelectionCommand;
            }
        }

        ICommand clearSelectionsCommand;
        public ICommand ClearSelectionsCommand
        {
            get
            {
                clearSelectionsCommand = clearSelectionsCommand ?? new RelayCommand(() => ActiveDocument.Selections.Clear(), () => ActiveDocument?.Selections.Count > 0);
                return clearSelectionsCommand;
            }
        }

        #endregion

        ICommand aboutCommand;
        public ICommand AboutCommand => aboutCommand = aboutCommand ?? new RelayCommand(() => new AboutWindow().ShowDialog());

        #endregion
    }
}