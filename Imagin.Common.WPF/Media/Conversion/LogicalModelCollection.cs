using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using Imagin.Common.Media.Models;

namespace Imagin.Common.Media.Conversion
{
    public class LogicalModelCollection : Collection<LogicalModel>
    {
        public LogicalModelCollection(ColorConverter converter)
        {
            foreach (var i in typeof(LogicalModel).GetTypes($"{nameof(Imagin)}.{nameof(Common)}.{nameof(Media)}.{nameof(Models)}"))
            {
                var j = i.Create<LogicalModel>();
                j.Converter = converter;
                Add(j);
            }
        }
    }
}