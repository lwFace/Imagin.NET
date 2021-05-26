using System;

namespace Imagin.Common
{
    /// <summary>
    /// An alternative for <see langword="System.ComponentModel.DescriptionAttribute"/>, which isn't available in some frameworks.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class DescriptionAttribute : Attribute
    {
        public readonly string Description;

        public readonly string Name;

        public DescriptionAttribute(string description = "") : base()
        {
            Description = description;
        }

        public DescriptionAttribute(string name, string description) : this(description)
        {
            Name = name;
        }
    }
}