using Imagin.Common.Linq;
using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public sealed class Enumerate : MarkupExtension
    {
        public Type Type { get; set; }

        public int Sort { get; set; } = 0;

        public Enumerate(Type type) : base() => Type = type;

        public override object ProvideValue(IServiceProvider serviceProvider) => Type.Enumerate(Appearance.Visible, Sort == 1);
    }
}