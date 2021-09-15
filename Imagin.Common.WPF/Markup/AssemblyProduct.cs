using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public sealed class AssemblyProduct : MarkupExtension
    {
        public AssemblyProduct() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute)).OfType<AssemblyProductAttribute>().FirstOrDefault().Product;
    }
}