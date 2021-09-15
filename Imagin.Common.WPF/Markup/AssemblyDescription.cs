using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public sealed class AssemblyDescription : MarkupExtension
    {
        public AssemblyDescription() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute)).OfType<AssemblyDescriptionAttribute>().FirstOrDefault().Description;
    }
}