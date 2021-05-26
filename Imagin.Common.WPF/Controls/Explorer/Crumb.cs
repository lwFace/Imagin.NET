using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class Crumb : MaskedButton
    {
        public static DependencyProperty CollectionProperty = DependencyProperty.Register(nameof(Collection), typeof(ItemCollection), typeof(Crumb), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ItemCollection Collection
        {
            get => (ItemCollection)GetValue(CollectionProperty);
            set => SetValue(CollectionProperty, value);
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(Crumb), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPathChanged));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static void OnPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as Crumb).OnPathChanged(new OldNew<string>(e));

        public Crumb() : base() { }

        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
            var point = PointToScreen(e.GetPosition(this));
            ShellContextMenu.Show(new System.Drawing.Point(point.X.Int32(), point.Y.Int32()), new System.IO.DirectoryInfo(Path));
        }

        protected virtual void OnPathChanged(OldNew<string> input)
        {
            var collection = Collection;
            if (Collection == null)
                collection = new ItemCollection(Path, new Filter(ItemType.Drive | ItemType.Folder));

            collection?.Refresh(input.New);

            if (Collection == null)
                SetCurrentValue(CollectionProperty, collection);
        }
    }
}