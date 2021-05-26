using System;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// Whether or not members must implicitly or explicitly specify visibility with <see cref="HiddenAttribute"/> or <see cref="System.ComponentModel.BrowsableAttribute"/>.
    /// </summary>
    public enum MemberVisibility
    {
        Explicit,
        Implicit
    }

    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MemberVisibilityAttribute : Attribute
    {
        public readonly MemberVisibility Visibility;

        public MemberVisibilityAttribute(MemberVisibility visibility) : base()
        {
            Visibility = visibility;
        }
    }
}