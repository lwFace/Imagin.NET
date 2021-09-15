namespace Imagin.Common.Media.Conversion
{
    /// <summary>
    /// Specifies a color model that consists of [3, 4] components.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Gets the value for each of the components.
        /// </summary>
        Vector Value { get; }
    }
}