using System;

namespace Imagin.Common.Controls
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyPropertyVisibilityAttribute : MemberVisibilityAttribute
    {
        public DependencyPropertyVisibilityAttribute(MemberVisibility visibility) : base(visibility) { }
    }
}