using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public sealed class ProjectVersion : MarkupExtension
    {
        public ProjectVersion() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => ProjectData.Version.ToString();
    }
}