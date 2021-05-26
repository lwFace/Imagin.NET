﻿using Imagin.Common.Input;
    
namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that can be selected.
    /// </summary>
    public interface ISelectable
    {
        event SelectedEventHandler Selected;

        bool IsSelected
        {
            get; set;
        }
    }
}
