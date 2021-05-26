using System;

namespace Imagin.Common.Controls
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PropertyVisibilityAttribute : MemberVisibilityAttribute
    {
        public PropertyVisibilityAttribute(MemberVisibility visibility) : base(visibility) { }
    }
}