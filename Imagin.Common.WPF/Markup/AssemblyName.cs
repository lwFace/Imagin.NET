using System;
using System.Reflection;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public sealed class AssemblyName : MarkupExtension
    {
        public AssemblyName() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => Assembly.GetEntryAssembly().FullName;
    }
}