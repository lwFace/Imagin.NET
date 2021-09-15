using Imagin.Common.Media.Conversion;

namespace Imagin.Common.Media.Models
{
    public abstract class LogicalModel : Model
    {
        public abstract LogicalModels Name { get; }

        public static explicit operator LogicalModels(LogicalModel input) => input.Name;

        public LogicalModel(params Component[] components) : base(components) { }
    }
}