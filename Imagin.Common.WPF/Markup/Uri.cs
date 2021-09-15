using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public class Uri : MarkupExtension
    {
        readonly string Assembly = string.Empty;

        readonly string RelativePath = string.Empty;

        public Uri(string assembly, string relativePath) : base()
        {
            Assembly = assembly;
            RelativePath = relativePath;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => Resources.Uri(Assembly, RelativePath);
    }
}