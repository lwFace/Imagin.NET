﻿namespace Imagin.Controls.Extended
{
    public sealed class MultiLinePropertyItem : StringPropertyItem
    {
        public MultiLinePropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsPrimary = false) : base(SelectedObject, Name, Value, Category, IsReadOnly)
        {
            this.Type = PropertyType.MultiLine;
        }
    }
}
