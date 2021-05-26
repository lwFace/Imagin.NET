namespace Imagin.Common.Controls
{
    public sealed class LayoutDocumentGroupControl : LayoutContentGroupControl
    {
        public bool Default;

        public LayoutDocumentGroupControl(LayoutRootControl root, bool @default) : base(root)
        {
            Default = @default;
        }

        protected override void OnItemsChanged()
        {
            base.OnItemsChanged();
            SetCurrentValue(SelectedIndexProperty, (Source?.Count ?? 0) - 1);
        }
    }
}