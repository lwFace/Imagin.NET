using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    [Serializable]
    public sealed class DefaultPalette : Palette
    {
        public DefaultPalette() : base("<default>")
        {
            var result = new List<Color>();
            foreach (var i in typeof(Colors).GetProperties())
            {
                if (i.PropertyType.Equals(typeof(Color)) && !i.Name.Equals("Transparent"))
                    result.Add((Color)i.GetValue(null));
            }

            var newResult = result.OrderBy(i => i.GetHue()).ThenBy(i => i.R * 3 + i.G * 2 + i.B * 1);
            foreach (var i in newResult)
                Add(new StringColor(i));
        }
    }
}