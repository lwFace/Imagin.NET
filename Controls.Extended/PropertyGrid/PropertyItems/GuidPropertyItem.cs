﻿using System;

namespace Imagin.Controls.Extended
{
    public sealed class GuidPropertyItem : PropertyItem
    {
        public override void SetValue(object NewValue)
        {
            if (this.Info != null) 
                this.Info.SetValue(SelectedObject, NewValue == null ? default(Guid) : (Guid)NewValue, null);
        }

        public GuidPropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsPrimary = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsPrimary)
        {
            this.Type = PropertyType.Guid;
        }
    }
}
