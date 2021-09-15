using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public class Styles : MarkupExtension
    {
        readonly string Assembly = string.Empty;

        public Styles(string assembly) : base()
        {
            Assembly = assembly;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => Resources.Uri(Assembly, $"Styles/Generic.xaml");
    }
}