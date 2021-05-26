namespace Imagin.Common.Media
{
    public enum ComponentType
    {
        /// <summary>
        /// The visual representation of the <see cref="Models.Component"/> changes when a sibling changes.
        /// </summary>
        Dynamic,
        /// <summary>
        /// The visual representation of the <see cref="Models.Component"/> does not change when a sibling changes.
        /// </summary>
        Static
    }
}