using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class Thumbnail : Image
    {
        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(Thumbnail), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, OnPathChanged));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static void OnPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<Thumbnail>().OnPathChanged(new OldNew<string>(e));

        public Thumbnail() : base() { }

        async Task<ImageSource> GetResult(string path)
        {
            return await Task.Run(() =>
            {
                var type = Machine.GetType(path);
                switch (type)
                {
                    case ItemType.File:

                        if (Shortcut.Is(path))
                            return Machine.Icon.GetLarge(path);

                        ImageSource i = File.Long.Thumbnail(path);
                        return i ?? Machine.Icon.GetLarge(path);

                    case ItemType.Drive:
                    case ItemType.Folder:
                        return Machine.Icon.GetLarge(path == Folder.Long.Root || path == Folder.Long.RecycleBin ? string.Empty : path);
                }
                return default;
            });
        }

        protected virtual async void OnPathChanged(OldNew<string> input)
        {
            var result = await GetResult(input.New);
            SetCurrentValue(SourceProperty, result);
        }
    }
}