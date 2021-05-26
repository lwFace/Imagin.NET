using Imagin.Common.Configuration;
using Imagin.Common.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public partial class ColorWindow : BaseWindow
    {
        public bool Result { get; private set; } = false;

        public static DependencyProperty ActiveDocumentProperty = DependencyProperty.Register(nameof(ActiveDocument), typeof(ColorDocument), typeof(ColorWindow), new PropertyMetadata(null));
        public ColorDocument ActiveDocument
        {
            get => (ColorDocument)GetValue(ActiveDocumentProperty);
            set => SetValue(ActiveDocumentProperty, value);
        }

        public static DependencyProperty DocumentsProperty = DependencyProperty.Register(nameof(Documents), typeof(DocumentCollection), typeof(ColorWindow), new PropertyMetadata(null));
        public DocumentCollection Documents
        {
            get => (DocumentCollection)GetValue(DocumentsProperty);
            set => SetValue(DocumentsProperty, value);
        }

        public static DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(ColorPickerOptions), typeof(ColorWindow), new PropertyMetadata(null));
        public ColorPickerOptions Options
        {
            get => (ColorPickerOptions)GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }

        public Color Color
        {
            get => ColorPicker.ActiveDocument.Color;
            set => ColorPicker.ActiveDocument.Color = value;
        }

        ColorPickerOptions options = null;

        public ColorWindow(Color color) : base()
        {
            SetCurrentValue(DocumentsProperty, new DocumentCollection());

            ColorPickerOptions.Load($@"{DataProperties.GetFolderPath(DataFolders.Documents)}\{nameof(ColorWindow)}\Options.data", out options);
            SetCurrentValue(OptionsProperty, options);

            var document = new ColorDocument(color);
            document.CanClose = false;
            document.CanFloat = false;

            Documents.Add(document);
            SetCurrentValue(ActiveDocumentProperty, document);

            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            options.Save();
        }

        void OnSave(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }

        void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}