namespace Imagin.Common.Input
{
    /// <summary>
    /// Specifies a changed value.
    /// </summary>
    public class ChangedValue : ChangedValue<object>
    {
        public ChangedValue(object OldValue, object NewValue) : base(OldValue, NewValue) { }
    }
}
