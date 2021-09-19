using Imagin.Common.Analytics;
using Imagin.Common.Globalization;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using Imagin.Common.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Imagin.Common.Controls
{
    [DisplayName("Color picker")]
    [Serializable]
    public class ColorPickerOptions : IColorPickerOptions, ISerialize
    {
        [Hidden]
        public string FilePath { get; private set; }

        [Category(nameof(Window))]
        [DisplayName(Localizer.Prefix + "Panels")]
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
}