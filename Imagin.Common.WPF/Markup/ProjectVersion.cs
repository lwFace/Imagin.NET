using System;
using System.Windows.Markup;
using Imagin.Common;

namespace Imagin.Common.Markup
{
    public sealed class ProjectVersion : MarkupExtension
    {
        public ProjectVersion() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var assembly = typeof(ProjectVersion).Assembly;

            object[] attributes = assembly.GetCustomAttributes(typeof(ProjectVersionAttribute), false);

            ProjectVersionAttribute attribute = null;
            if (attributes.Length > 0)
                attribute = attributes[0] as ProjectVersionAttribute;

            return attribute.Value.ToString();
        }
    }
}