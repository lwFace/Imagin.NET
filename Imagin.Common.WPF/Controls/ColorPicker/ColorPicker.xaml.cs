using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public partial class ColorPicker : UserControl
    {
        public static DependencyProperty ActiveDocumentProperty = DependencyProperty.Register(nameof(ActiveDocument), typeof(ColorDocument), typeof(ColorPicker), new PropertyMetadata(null));
        public ColorDocument ActiveDocument
        {
            get => (ColorDocument)GetValue(ActiveDocumentProperty);
            set => SetValue(ActiveDocumentProperty, value);
        }

        public static DependencyProperty AlphaPanelProperty = DependencyProperty.Register(nameof(AlphaPanel), typeof(AlphaPanel), typeof(ColorPicker), new PropertyMetadata(null));
        public AlphaPanel AlphaPanel
        {
            get => (AlphaPanel)GetValue(AlphaPanelProperty);
            private set => SetValue(AlphaPanelProperty, value);
        }

        public static DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(IColorPickerOptions), typeof(ColorPicker), new PropertyMetadata(null, OnOptionsChanged));
        public IColorPickerOptions Options
        {
            get => (IColorPickerOptions)GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }
        static void OnOptionsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ColorPicker>().OnOptionsChanged(e.NewValue as IColorPickerOptions);

        public static DependencyProperty OptionsPanelProperty = DependencyProperty.Register(nameof(OptionsPanel), typeof(OptionsPanel), typeof(ColorPicker), new PropertyMetadata(null));
        public OptionsPanel OptionsPanel
        {
            get => (OptionsPanel)GetValue(OptionsPanelProperty);
            private set => SetValue(OptionsPanelProperty, value);
        }

        public static DependencyProperty DocumentsProperty = DependencyProperty.Register(nameof(Documents), typeof(DocumentCollection), typeof(ColorPicker), new PropertyMetadata(null));
        public DocumentCollection Documents
        {
            get => (DocumentCollection)GetValue(DocumentsProperty);
            set => SetValue(DocumentsProperty, value);
        }

        public static DependencyProperty PanelsProperty = DependencyProperty.Register(nameof(Panels), typeof(PanelCollection), typeof(ColorPicker), new PropertyMetadata(null));
        public PanelCollection Panels
        {
            get => (PanelCollection)GetValue(PanelsProperty);
            set => SetValue(PanelsProperty, value);
        }

        public ColorPicker()
        {
            SetCurrentValue(OptionsProperty, new ColorPickerOptions());

            SetCurrentValue(DocumentsProperty, 
                new DocumentCollection());
            SetCurrentValue(PanelsProperty, 
                new PanelCollection());

            SetCurrentValue(AlphaPanelProperty, new AlphaPanel());

            Panels.Add
                (AlphaPanel);
            Panels.Add
                (new ColorsPanel(this));
            Panels.Add
                (new ComponentPanel());

            SetCurrentValue(OptionsPanelProperty, new OptionsPanel());

            Panels.Add
                (OptionsPanel);
            Panels.Add
                (new PropertiesPanel());

            InitializeComponent();
        }

        void OnColorMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement i)
            {
                if (i.DataContext is StringColor j)
                {
                    if (ActiveDocument is ColorDocument k)
                        k.Update(j.Value);
                }
            }
        }

        protected virtual void OnOptionsChanged(IColorPickerOptions input) => input?.Initialize(this);
    }
}