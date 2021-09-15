using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using Imagin.Common.Media.Models;

namespace Imagin.Common.Media.Conversion
{
    public class VisualModelCollection : Collection<VisualModel>
    {
        public VisualModelCollection(ColorConverter converter)
        {
            foreach (var i in typeof(VisualModel).GetTypes($"{nameof(Imagin)}.{nameof(Common)}.{nameof(Media)}.{nameof(Models)}"))
            {
                var j = i.Create<VisualModel>();
                j.Converter = converter;
                Add(j);
            }
        }
    }
}