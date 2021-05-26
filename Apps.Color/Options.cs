using Imagin.Common;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Linq;

namespace Color
{
    [Serializable]
    public class Options : Data, IColorPickerOptions
    {
        ColorPickerOptions colorPickerOptions = new ColorPickerOptions();
        [Category("Color picker")]
        [DisplayName("Options")]
        public ColorPickerOptions ColorPickerOptions
        {
            get => colorPickerOptions;
            set => this.Change(ref colorPickerOptions, value);
        }

        Document[] documents = null;
        [Hidden]
        public Document[] Documents
        {
            get => documents;
            set => this.Change(ref documents, value);
        }

        public override void OnApplicationExit()
        {
            base.OnApplicationExit();
            Documents = Get.Current<MainViewModel>().Documents.Cast<Document>().ToArray();
        }

        public override void OnApplicationReady()
        {
            base.OnApplicationReady();
            if (Documents?.Length > 0)
                0.For(Documents.Length, i => Get.Current<MainViewModel>().Documents.Add(Documents[i]));
        }

        public void Initialize(ColorPicker colorPicker) => colorPickerOptions?.Initialize(colorPicker);
    }
}