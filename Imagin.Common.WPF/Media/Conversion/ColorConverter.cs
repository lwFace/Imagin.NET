using Imagin.Common.Controls;
using Imagin.Common.Media.Conversion;
using Imagin.Common.Media.Models;
using System.Linq;
using System.Windows.Media;

namespace Imagin.Common.Media
{
    public sealed class ColorConverter : Base
    {
        public readonly IColorDocument Document;

        Color color = Colors.White;
        public Color Color
        {
            get => color;
            set
            {
                this.Change(ref color, value);
                Document.Color = value;
                LogicalModel.Update(value);
                VisualModel.Update(value);
            }
        }

        LogicalModels logicalModel = Conversion.LogicalModels.CMYK;
        public LogicalModel LogicalModel
        {
            get => logicalModels?.FirstOrDefault(i => (LogicalModels)i == logicalModel);
            set
            {
                this.Change(ref logicalModel, (LogicalModels)value);
                Document.LogicalModel = logicalModel;
            }
        }

        readonly LogicalModelCollection logicalModels;
        public LogicalModelCollection LogicalModels => logicalModels;

        VisualModels visualModel = Media.VisualModels.HSB;
        public VisualModel VisualModel
        {
            get => visualModels.FirstOrDefault(i => (VisualModels)i == visualModel);
            set
            {
                this.Change(ref visualModel, (VisualModels)value);
                Document.VisualModel = visualModel;
            }
        }

        readonly VisualModelCollection visualModels;
        public VisualModelCollection VisualModels => visualModels;

        public ColorConverter(IColorDocument document) : base()
        {
            Document = document;

            logicalModel = document.LogicalModel;
            logicalModels = new LogicalModelCollection(this);

            visualModel = document.VisualModel;
            visualModels = new VisualModelCollection(this);
        }

        internal void Update(Model sender, Color input)
        {
            color = input;
            this.Changed(() => Color);

            Document.Color = input;
            if (sender is LogicalModel)
                VisualModel.Update(input);

            if (sender is VisualModel)
                LogicalModel.Update(input);
        }
    }
}