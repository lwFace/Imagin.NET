using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using Imagin.Common.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    [DisplayName("Color picker")]
    [Serializable]
    public class ColorPickerOptions : IColorPickerOptions, ISerialize
    {
        [Hidden]
        public string FilePath { get; private set; }

        [Category(nameof(Window))]
        public PanelCollection Panels => default;

        internal Dictionary<string, bool> PanelVisibility { get; private set; }

        public override string ToString() => "Color picker";

        public ColorPickerOptions() : base() { }

        public ColorPickerOptions(string filePath) : this()
        {
            FilePath = filePath;
        }

        void OnPanelChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Models.Panel.IsVisible):
                    PanelVisibility[(sender as Models.Panel).Name] = (sender as Models.Panel).IsVisible;
                    break;
            }
        }

        public static Result Load(string filePath, out ColorPickerOptions data)
        {
            var result = BinarySerializer.Deserialize(filePath, out object options);
            data = options as ColorPickerOptions ?? new ColorPickerOptions(filePath);
            return result;
        }

        public Result Deserialize(string filePath, out object data) => BinarySerializer.Deserialize(filePath, out data);

        public Result Save() => Serialize(this);

        public Result Serialize(object data) => Serialize(FilePath, data);

        public Result Serialize(string filePath, object data) => BinarySerializer.Serialize(filePath, data);

        public void Initialize(ColorPicker colorPicker)
        {
            var panels = colorPicker.Panels;

            if (panels?.Count > 0)
            {
                if (PanelVisibility == null)
                    PanelVisibility = new Dictionary<string, bool>();

                foreach (var i in panels)
                {
                    if (!PanelVisibility.ContainsKey(i.Name))
                        PanelVisibility.Add(i.Name, i.IsVisible);
                }
                for (var i = PanelVisibility.Count - 1; i >= 0; i--)
                {
                    var item = PanelVisibility.ElementAt(i);
                    if (!panels.Contains(j => j.Name.Equals(item.Key)))
                    {
                        PanelVisibility.Remove(item.Key);
                        continue;
                    }
                    panels[item.Key].IsVisible = item.Value;
                    panels[item.Key].PropertyChanged += OnPanelChanged;
                }
            }
        }
    }

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