using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class CropTool : Tool
    {
        public override Cursor Cursor => Cursors.Cross;

        public override string Icon => Resources.Uri(nameof(Paint), "Images/Crop.png").OriginalString;

        string background = $"255,0,0,0";
        public virtual SolidColorBrush Background
        {
            get
            {
                var result = background.Split(',');
                var actualResult = default(SolidColorBrush);
                App.Current.Dispatcher.Invoke(() => actualResult = new SolidColorBrush(System.Windows.Media.Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte())));
                return actualResult;
            }
            set => this.Change(ref background, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        double opacity = 0.8;
        public double Opacity
        {
            get => opacity;
            set => this.Change(ref opacity, value);
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Background):
                case nameof(Opacity):
                    Documents.ForEach(i => i.UpdateCropPreview(this));
                    break;
            }
        }

        public override string ToString() => "Crop";

        ICommand cropCommand;
        public ICommand CropCommand
        {
            get
            {
                cropCommand = cropCommand ?? new RelayCommand(() =>
                {
                    Document.Crop(Document.CropRegion);
                    Document.CropRegion.X = 0;
                    Document.CropRegion.Y = 0;
                    Document.CropRegion.Height = Document.Height;
                    Document.CropRegion.Width = Document.Width;
                }, 
                () => Document != null);
                return cropCommand;
            }
        }
    }
}