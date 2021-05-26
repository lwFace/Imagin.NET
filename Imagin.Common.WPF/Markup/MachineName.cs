using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public sealed class MachineName : MarkupExtension
    {
        public MachineName() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => Environment.MachineName;
    }
}