using System;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that can be locked and unlocked.
    /// </summary>
    public interface ILockable
    {
        event EventHandler<EventArgs> Locked;

        event EventHandler<EventArgs> Unlocked;

        bool IsLocked
        {
            get; set;
        }
    }
}
