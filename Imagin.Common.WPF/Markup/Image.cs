using System;
using System.Windows.Media;

namespace Imagin.Common.Markup
{
    public class Image : Uri
    {
        public Image(string assembly, string relativePath) : base(assembly, relativePath) { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var result = (System.Uri)base.ProvideValue(serviceProvider);
            return new ImageSourceConverter().ConvertFromString(result.OriginalString);
        }
    }
}