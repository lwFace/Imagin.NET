

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> capable of being edited.
    /// </summary>
    public interface IEditable
    {
        bool IsEditable
        {
            get; set;
        }
    }
}
