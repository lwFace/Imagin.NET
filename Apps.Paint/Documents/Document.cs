using ImageMagick;
using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Media;
using Imagin.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class Document : Imagin.Common.Models.Document
    {
        #region Fields

        /// <summary>
        /// The default name for untitled files.
        /// </summary>
        public const string DefaultName = "Untitled";

        /// <summary>
        /// The default extension for document files.
        /// </summary>
        public const string DefaultExtension = "phd";

        #endregion

        #region Properties

        [Hidden]
        public override string Title
        {
            get
            {
                var result = new StringBuilder();
                if (FilePath.NullOrEmpty())
                {
                    result.Append("{0}.{1}".F(Name, DefaultExtension));
                }
                else result.Append(FileName);

                if (IsModified)
                    result.Append("*");

                return result.ToString();
            }
        }

        [Hidden]
        public override object ToolTip => Title;

        /// ---------------------------------------------------------------------------

        WriteableBitmap cropPreview;
        public WriteableBitmap CropPreview
        {
            get => cropPreview;
            set => this.Change(ref cropPreview, value);
        }

        /// ---------------------------------------------------------------------------

        [Hidden]
        public Int32Region Region => new Int32Region(0, 0, Width, Height);

        /// ---------------------------------------------------------------------------

        double angle = 0;
        [Hidden]
        public double Angle
        {
            get => angle;
            set => this.Change(ref angle, value);
        }

        [Category("Format")]
        [DisplayName("Bit depth")]
        [ReadOnly]
        public int BitDepth
        {
            get => default(int);
        }

        ChannelCollection channels = new ChannelCollection();
        public ChannelCollection Channels
        {
            get => channels;
            set => this.Change(ref channels, value);
        }

        ObservableCollection<Count> counts = new ObservableCollection<Count>();
        [Hidden]
        public ObservableCollection<Count> Counts
        {
            get => counts;
            set => this.Change(ref counts, value);
        }

        DoubleRegion cropRegion = new DoubleRegion();
        [Hidden]
        public DoubleRegion CropRegion
        {
            get => cropRegion;
            set => this.Change(ref cropRegion, value); 
        }

        string filePath = string.Empty;
        [Category(nameof(System.IO.File))]
        [DisplayName("Path")]
        [ReadOnly]
        public string FilePath
        {
            get => filePath;
            set
            {
                this.Change(ref filePath, value);
                this.Changed(() => FolderPath);
                this.Changed(() => FileName);
                this.Changed(() => Title);
                this.Changed(() => ToolTip);
            }
        }

        [Category(nameof(System.IO.File))]
        [DisplayName("Name")]
        [ReadOnly]
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(this.FilePath))
                    return this.Name + "." + DefaultExtension;

                return System.IO.Path.GetFileName(this.FilePath);
            }
        }

        [Hidden]
        public string FileNameWithoutExtension
        {
            get
            {
                if (string.IsNullOrEmpty(this.FilePath))
                    return DefaultName;

                return System.IO.Path.GetFileNameWithoutExtension(this.FilePath);
            }
        }

        [Category(nameof(System.IO.File))]
        [DisplayName("Extension")]
        [ReadOnly]
        public string FileExtension
        {
            get
            {
                if (string.IsNullOrEmpty(this.FilePath))
                    return "." + DefaultExtension;

                return System.IO.Path.GetExtension(this.FilePath);
            }
        }

        [Hidden]
        public string FolderPath
        {
            get => string.IsNullOrEmpty(filePath) ? string.Empty : System.IO.Path.GetDirectoryName(filePath);
        }

        int height;
        [Category(nameof(System.Windows.Size))]
        [ReadOnly]
        public int Height
        {
            get => height;
            private set
            {
                this.Change(ref height, value);
                this.Changed(() => MegaPixels);
                this.Changed(() => FileSize);
            }
        }

        ActionCollection history = new ActionCollection();
        [Hidden]
        public ActionCollection History
        {
            get => history;
            set => this.Change(ref history, value);
        }

        LayerCollection layers;
        [Hidden]
        public LayerCollection Layers
        {
            get => layers;
            set => this.Change(ref layers, value);
        }

        [Category("Format")]
        [DisplayName("Mega pixels")]
        [ReadOnly]
        public double MegaPixels
        {
            get => (Width * Height) / 1.0.Shift(6);
        }

        string name;
        [Hidden]
        public string Name
        {
            get => name;
            set
            {
                this.Change(ref name, value);
                this.Changed(() => Title);
                this.Changed(() => ToolTip);
            }
        }

        ObservableCollection<Note> notes = new ObservableCollection<Note>();
        [Hidden]
        public ObservableCollection<Note> Notes
        {
            get => notes;
            set => this.Change(ref notes, value);
        }

        string password = string.Empty;
        [Category(nameof(System.IO.File))]
        [StringFormat(Imagin.Common.Data.StringFormat.Password)]
        public string Password
        {
            get => password;
            set => this.Change(ref password, value);
        }

        Imagin.Common.Media.PixelFormat pixelFormat = Imagin.Common.Media.PixelFormat.Rgba128Float;
        /// <summary>
        /// RGB
        ///     -> Rgba128Float (32 bits / channel)
        ///     -> Rgba64       (16 bits / channel)
        ///     -> Bgr32        (8  bits / channel)
        /// CMYK
        ///     -> Cmyk32       (8  bits / channel)
        /// Grayscale
        ///     -> Gray32Float  (8  bits / channel)
        ///     -> Gray16       (4  bits / channel)
        ///     -> Gray8        (2  bits / channel)
        /// Black/White
        ///     -> BlackWhite   (1  bit  / pixel)
        /// </summary>
        [Category("Format")]
        [DisplayName("Pixel format")]
        [ReadOnly]
        public Imagin.Common.Media.PixelFormat PixelFormat
        {
            get => pixelFormat;
            set => this.Change(ref pixelFormat, value);
        }

        WriteableBitmap preview;
        [Hidden]
        public WriteableBitmap Preview
        {
            get => preview;
            set => this.Change(ref preview, value);
        }

        float resolution = 72;
        [Category("Format")]
        public float Resolution
        {
            get => resolution;
            set => this.Change(ref resolution, value);
        }

        [field: NonSerialized]
        ScrollViewer scrollViewer = default(ScrollViewer);
        [Hidden]
        public ScrollViewer ScrollViewer
        {
            get => scrollViewer;
            set => this.Change(ref scrollViewer, value);
        }

        ObservableCollection<CustomPath> selections = new ObservableCollection<CustomPath>();
        [Hidden]
        public ObservableCollection<CustomPath> Selections
        {
            get => selections;
            set => this.Change(ref selections, value);
        }

        [Category(nameof(System.IO.File))]
        [LongFormat(LongFormat.FileSize)]
        [ReadOnly]
        public long FileSize => (height * width) * 4;

        [Hidden]
        public Int32Size Size => new Int32Size(Height, Width);

        int width;
        [Category(nameof(System.Windows.Size))]
        [ReadOnly]
        public int Width
        {
            get => width;
            private set
            {
                this.Change(ref width, value);
                this.Changed(() => MegaPixels);
                this.Changed(() => FileSize);
            }
        }

        double zoomValue = 1;
        [Category(nameof(System.IO.File))]
        [DisplayName("Zoom")]
        [ReadOnly]
        public double ZoomValue
        {
            get => zoomValue;
            set => this.Change(ref zoomValue, value);
        }

        #endregion

        #region Document

        public Document() : base()
        {
            Channels.Add(new RChannel());
            Channels.Add(new GChannel());
            Channels.Add(new BChannel());

            Layers = new LayerCollection(this);
            CropRegion.Changed += OnCropRegionChanged;
        }

        public Document(int height, int width) : this()
        {
            Height = height;
            Width = width;
        }

        public Document(DocumentPreset preset) : this()
        {
            Height = preset.Height;
            Name = preset.Name;
            PixelFormat = preset.PixelFormat;
            Resolution = preset.Resolution;
            Width = preset.Width;

            var layer = new PixelLayer("Layer 0", new Size(preset.Width, preset.Height));
            layer.Pixels.Clear(preset.Background);
            Layers.Add(layer);

            History.Add(new NewLayerAction(layer, Layers, 0));
        }

        #endregion

        #region Methods

        public void Draw()
        {
            /*
            if (Preview == null || Preview.PixelHeight != Height || Preview.PixelWidth != Width)
                Preview = BitmapFactory.WriteableBitmap(Height, Width);

            Layers.Render(Preview);
            */
        }

        /// ---------------------------------------------------------------------------

        public void Preserve()
        {
            Selections.ForEach(i => i.Preserve());
            Layers.Each<VisualLayer>(i => i.Preserve());
        }

        public void Restore()
        {
            Selections.ForEach(i => i.Restore());
            Layers.Each<VisualLayer>(i => i.Restore());
        }

        /// ---------------------------------------------------------------------------

        /*
        public static int GetColorByte(System.Windows.Media.Color Color, System.Windows.Media.PixelFormat Format)
        {
            int Byte = 0;
            if (Format == PixelFormats.BlackWhite)
            {

            }
            else if (Format == PixelFormats.Cmyk32)
            {

            }
            else if (Format == PixelFormats.Rgba128Float)
            {
                Byte = Color.A << 96; // A
                Byte |= Color.B << 64; // R
                Byte |= Color.G << 32; // G
                Byte |= Color.R << 0; // B
            }
            else if (Format == PixelFormats.Rgba64)
            {
                Byte = Color.A << 48; // A
                Byte |= Color.B << 32; // R
                Byte |= Color.G << 16; // G
                Byte |= Color.R << 0; // B
            }
            else if (Format == PixelFormats.Bgra32)
            {
                Byte = Color.A << 24; // A
                Byte |= Color.R << 16; // R
                Byte |= Color.G << 8; // G
                Byte |= Color.B << 0; // B
            }
            else if (Format == PixelFormats.Gray32Float)
            {
            }
            else if (Format == PixelFormats.Gray16)
            {
            }
            else if (Format == PixelFormats.Gray8)
            {
            }
            return Byte;
        }

        public void GetInitialHistory()
        {
            if (this.FilePath.Length == 0)
            {
                this.History.Push(new NewLayerHistoryState("New", "Document.png", true));
            } else
            {
                this.History.Push(new NewLayerHistoryState("Open", "Document.png", true));
            }
        }

        /// <summary>
        /// Creates inital layer from file (dubbed "Layer 0").
        /// </summary>
        public static Layer LayerFromFile(string FilePath, out float Resolution)
        {
            Resolution = 0;
            using (ImageMagick.MagickImage image = new ImageMagick.MagickImage(FilePath))
            {
                Bitmap Bitmap = image == null ? null : image.ToBitmap() as Bitmap;
                Resolution = Bitmap == null ? 0f : Bitmap.HorizontalResolution;
                return new PixelLayer()
                {
                    IsVisible = true,
                    IsLocked = true,
                    Title = "Layer 0",
                    Bitmap = new FastBitmap(Bitmap)
                };
            }          
        }

        public void PrintSize()
        {
            int dpiX, dpiY;
            //Utility.GetDpi(out dpiX, out dpiY);
            //this.ZoomValue = Math.Round((float)dpiX / this.Resolution, 2);
        }
        */

        public override async void Save()
        {
            if (filePath.NullOrEmpty())
            {
                await SaveAs();
                return;
            }

            switch (System.IO.Path.GetExtension(filePath).Replace(".", string.Empty))
            {
                case DefaultExtension:
                    await Save(filePath);
                    break;

                default:
                    await SaveAs();
                    break;
            }
        }

        public async Task Save(string filePath)
        {
            //void error() => System.Windows.Dialog.Show($"Unable to save '{filePath}'.", "Save", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

            switch (System.IO.Path.GetExtension(filePath).Replace(".", string.Empty))
            {
                case DefaultExtension:
                    Preserve();
                    BinarySerializer.Serialize(filePath, this);
                    break;

                default:
                    var result = await Render();
                    var image = new MagickImage(result);
                    image.Write(filePath);
                    break;
            }
            FilePath = filePath;
        }

        public async Task SaveAs()
        {
            var path = string.Empty;
            var extensions = ImageFormats.Writable.Select(i => i.Extension).ToArray();

            if (ExplorerWindow.Show(out path, "Save...", ExplorerWindow.Modes.SaveFile, extensions))
                await Save(path);
        }

        /// ---------------------------------------------------------------------------

        #region Commands

        [field: NonSerialized]
        ICommand saveCommand;
        [Hidden]
        public ICommand SaveCommand
        {
            get
            {
                saveCommand = saveCommand ?? new RelayCommand(() => Save(), () => true);
                return saveCommand;
            }
        }

        [field: NonSerialized]
        ICommand saveAsCommand;
        [Hidden]
        public ICommand SaveAsCommand
        {
            get
            {
                saveAsCommand = saveAsCommand ?? new RelayCommand(() => _ = SaveAs(), () => true);
                return saveAsCommand;
            }
        }

        #endregion

        /// ---------------------------------------------------------------------------

        public void FitScreen()
        {
            var resulta = (ScrollViewer.ActualWidth - (0.025 * ScrollViewer.ActualWidth)) / Width;
            var resultb = (ScrollViewer.ActualHeight - (0.05 * ScrollViewer.ActualHeight)) / Height;

            if (resulta < resultb)
            {
                ZoomValue = Math.Round(resulta, 2);
            }
            else ZoomValue = Math.Round(resultb, 2);
        }

        /*
        public void FitScreen()
        {
            double ActualWidth = this.ObservedWidth, ActualHeight = this.ObservedHeight;
            double ResultA = ActualWidth / this.Width;
            double ResultB = ActualHeight / this.Height;

            if (ResultA < ResultB)
            {
                this.ZoomValue = Math.Round(ResultA, 2);
            }
            else
            {
                this.ZoomValue = Math.Round(ResultB, 2);
            }
        }
        */

        public void FillScreen()
        {
            var resulta = (ScrollViewer.ActualWidth - (0.025 * ScrollViewer.ActualWidth)) / Width;
            var resultb = (ScrollViewer.ActualHeight - (0.05 * ScrollViewer.ActualHeight)) / Height;

            if (resulta > resultb)
            {
                ZoomValue = Math.Round(resulta, 2);
            }
            else ZoomValue = Math.Round(resultb, 2);
        }

        /*
        public void FillScreen()
        {
            double ActualWidth = this.ObservedWidth;
            double Result = ActualWidth / this.Width;
            this.ZoomValue = Math.Round(Result, 2);
        }
        */

        /// ---------------------------------------------------------------------------

        public async Task<Bitmap> Render()
        {
            var size = new Size(Width, Height);
            var result = new Bitmap(size.Width, size.Height);

            var bitmaps = new List<Bitmap>();
            Layers.Each<VisualLayer>(i =>
            {
                var bitmap = i.Render(size);
                bitmaps.Add(bitmap);
            });

            await Task.Run(() => 
            {
                using (var g = Graphics.FromImage(result))
                    bitmaps.ForEach(i => g.DrawImage(i, 0, 0));
            });

            return result;
        }

        /// ---------------------------------------------------------------------------

        class LayerIndex
        {
            public readonly int Index;

            public readonly VisualLayer Layer;

            public readonly LayerCollection Layers;

            public LayerIndex(LayerCollection parent, int index, VisualLayer layer)
            {
                Layers = parent;
                Index = index;
                Layer = layer;
            }
        }

        /// ---------------------------------------------------------------------------

        public void Flatten()
        {
            var queue = new Queue<VisualLayer>();
            foreach (var i in Flatten(Layers))
                queue.Enqueue(i);

            if (queue.Count == 0)
                return;

            string name = null;
            var size = new Size(Width, Height);

            var bitmaps = new List<Bitmap>();
            foreach (var i in queue)
            {
                if (name == null)
                    name = i.Name;

                bitmaps.Add(i.Render(size));
            }

            var result = new Bitmap(Width, Height);
            using (var g = Graphics.FromImage(result))
            {
                foreach (var i in bitmaps)
                {
                    if (i != null)
                        g.DrawImage(i, 0, 0);
                }
            }

            var newLayer = new PixelLayer(name, result.WriteableBitmap());
            Layers.Insert(0, newLayer);
        }

        public IEnumerable<VisualLayer> Flatten(LayerCollection layers)
        {
            for (var i = layers.Count - 1; i >= 0; i--)
            {
                if (layers[i] is GroupLayer)
                {
                    foreach (var j in Flatten(((GroupLayer)layers[i]).Layers))
                        yield return j;
                }
                else if (layers[i] is VisualLayer)
                    yield return (VisualLayer)layers[i];

                layers.RemoveAt(i);
            }
        }

        /// ---------------------------------------------------------------------------

        public void MergeVisible()
        {
            var queue = new Queue<LayerIndex>();
            foreach (var i in MergeVisible(Layers))
                queue.Enqueue(i);

            if (queue.Count == 0)
                return;

            var firstLayer = queue.First().Layer;
            var firstLayers = queue.First().Layers;
            var firstIndex = queue.First().Index;

            var size = new Size(Width, Height);

            var bitmaps = new List<Bitmap>();
            foreach (var i in queue)
                bitmaps.Add(i.Layer.Render(size));

            var result = new Bitmap(Width, Height);
            using (var g = Graphics.FromImage(result))
            {
                foreach (var i in bitmaps)
                {
                    if (i != null)
                        g.DrawImage(i, 0, 0);
                }
            }

            var newLayer = new PixelLayer(firstLayer.Name, result.WriteableBitmap()) { Parent = firstLayer.Parent };
            firstLayers.Insert(firstIndex, newLayer);
        }

        IEnumerable<LayerIndex> MergeVisible(LayerCollection layers)
        {
            for (var i = layers.Count - 1; i >= 0; i--)
            {
                if (layers[i] is GroupLayer)
                {
                    foreach (var j in MergeVisible(((GroupLayer)layers[i]).Layers))
                        yield return j;
                }
                else if (layers[i] is VisualLayer && layers[i].IsVisible)
                {
                    yield return new LayerIndex(layers, i, (VisualLayer)layers[i]);
                    layers.RemoveAt(i);
                }
            }
        }

        /// ---------------------------------------------------------------------------

        /// <summary>
        /// Sets the pixel format of the document (all layers are altered accordingly).
        /// </summary>
        public void SetFormat()
        {
        }

        /// ---------------------------------------------------------------------------

        public void Crop(DoubleRegion selection) => Crop(selection.X.Int32(), selection.Y.Int32(), selection.Height.Int32(), selection.Width.Int32());

        public void Crop(int x, int y, int height, int width)
        {
            Height = height;
            Width = width;

            Layers.Each<VisualLayer>(i => i.Crop(x, y, height, width));
        }

        public void UpdateCropPreview(CropTool cropTool)
        {
            if (CropPreview == null || CropPreview.PixelHeight != Height || CropPreview.PixelWidth != Width)
                CropPreview = BitmapFactory.WriteableBitmap(Height, Width);

            var x
                = CropRegion.X.Int32();
            var y
                = CropRegion.Y.Int32();
            var height
                = CropRegion.Height.Int32();
            var width
                = CropRegion.Width.Int32();

            CropPreview.FillRectangle(0, 0, Width, Height, cropTool.Background.Color.A((cropTool.Opacity * 255.0).Round().Byte()));
            CropPreview.FillRectangle(x, y, x + width, y + height, System.Windows.Media.Colors.Transparent);
        }

        void OnCropRegionChanged(object sender)
        {
            UpdateCropPreview(Get.Current<Options>().Tools.Get<CropTool>());
        }

        /// ---------------------------------------------------------------------------

        public void Resize(int height, int width, CardinalDirection direction, bool stretch, Interpolations interpolation)
        {
            Int32Size oldSize = new Int32Size(Height, Width), newSize = new Int32Size(height, width);

            Height = newSize.Height;
            Width = newSize.Width;

            if (stretch)
            {
                Layers.Each<VisualLayer>(i => i.Resize(oldSize, newSize, interpolation));
                return;
            }

            Layers.Each<VisualLayer>(i => i.Crop(oldSize, newSize, direction));
        }

        /// ---------------------------------------------------------------------------

        public static async Task<WriteableBitmap> Open(string path)
        {
            var result = default(WriteableBitmap);

            try
            {
                var image = default(MagickImage);
                await Task.Run(new Action(() => image = new MagickImage(path)));
                result = image.ToBitmap().WriteableBitmap();
            }
            catch (Exception)
            {
                return default(WriteableBitmap);
            }

            return result;
        }

        /// ---------------------------------------------------------------------------

        public override string ToString() => Imagin.Common.Storage.File.Long.Description(FilePath);

        #endregion
    }
}