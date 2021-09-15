using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public sealed class AssemblyCopyright : MarkupExtension
    {
        public AssemblyCopyright() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute)).OfType<AssemblyCopyrightAttribute>().FirstOrDefault().Copyright;
    }
}