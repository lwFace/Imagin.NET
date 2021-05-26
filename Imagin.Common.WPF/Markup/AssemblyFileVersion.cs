using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public sealed class AssemblyFileVersion : MarkupExtension
    {
        public AssemblyFileVersion() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute)).OfType<AssemblyFileVersionAttribute>().FirstOrDefault()?.Version;
    }
}