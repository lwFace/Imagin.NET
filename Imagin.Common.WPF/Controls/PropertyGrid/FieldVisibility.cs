using System;

namespace Imagin.Common.Controls
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FieldVisibilityAttribute : MemberVisibilityAttribute
    {
        public FieldVisibilityAttribute(MemberVisibility visibility) : base(visibility) { }
    }
}