using System.Windows.Data;
using Imagin.Common.Converters;

namespace Imagin.Common.Markup
{
    public class Options : Binding
    {
        public Options() : this(string.Empty) { }

        public Options(string path) : base(path)
        {
            Converter = new DefaultConverter<object, object>(i => i ?? Nothing.Do, i => i);
            Source = Get.Where<Configuration.Data>();
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }
    }
}