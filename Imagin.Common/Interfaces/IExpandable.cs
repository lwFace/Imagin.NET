namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> capable of expanding.
    /// </summary>
    public interface IExpandable
    {
        bool IsExpanded
        {
            get; set;
        }
    }
}
