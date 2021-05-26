using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Media.Conversion;
using Imagin.Common.Media.Models;
using Imagin.Common.Models;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Imagin.Common.Controls
{
    public interface IColorDocument
    {
        System.Windows.Media.Color Color { get; set; }

        LogicalModels LogicalModel { get; set; }

        VisualModels VisualModel { get; set; }

        void Update(System.Windows.Media.Color input);
    }

    [PropertyVisibility(MemberVisibility.Explicit)]
    [Serializable]
    public class ColorDocument : Document, IColorDocument
    {
        [field: NonSerialized]
        ColorConverter converter = null;
        public ColorConverter Converter
        {
            get => converter;
            set => this.Change(ref converter, value);
        }

        //...............................................................................

        LogicalModels logicalModel = Media.Conversion.LogicalModels.CMYK;
        public LogicalModels LogicalModel
        {
            get => logicalModel;
            set => this.Change(ref logicalModel, value);
        }

        [Hidden(false)]
        [Index(1)]
        public LogicalModelCollection LogicalModels => converter.LogicalModels;

        VisualModels visualModel = Media.VisualModels.HSB;
        public VisualModels VisualModel
        {
            get => visualModel;
            set => this.Change(ref visualModel, value);
        }

        [Hidden(false)]
        [Index(0)]
        public VisualModelCollection VisualModels => converter.VisualModels;

        //...............................................................................

        byte alpha = 255;
        public byte Alpha
        {
            get => alpha;
            set => this.Change(ref alpha, value);
        }

        StringColor color;
        [Featured]
        [Hidden(false)]
        public System.Windows.Media.Color Color
        {
            get => color.Value;
            set => this.Change(ref color, new StringColor(value.A(alpha)));
        }

        //...............................................................................

        public override string Title => $"#{Color.Hexadecimal()}";

        public override object ToolTip => $"#{Color.Hexadecimal()}";

        //...............................................................................

        public ColorDocument(System.Windows.Media.Color color) : base()
        {
            Color = color;
        }

        //...............................................................................

        public override void Save() { }

        //...............................................................................

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Alpha):
                    Converter.Color = Color.A(alpha);
                    break;

                case nameof(Color):
                    this.Changed(() => Title);
                    this.Changed(() => ToolTip);
                    break;
            }
        }

        //...............................................................................

        public override void OnAdded()
        {
            Converter = new ColorConverter(this);
            Converter.Color = color;
        }

        //...............................................................................

        public void Update(System.Windows.Media.Color input)
        {
            Color = input;
            Converter.Color = input;
        }
    }
}