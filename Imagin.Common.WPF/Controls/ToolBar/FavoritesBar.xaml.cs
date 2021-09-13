using Imagin.Common.Input;
using Imagin.Common.Storage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace Imagin.Common.Controls
{
    public partial class FavoritesBar : UserControl
    {
        public static DependencyProperty FavoritesProperty = DependencyProperty.Register(nameof(Favorites), typeof(Favorites), typeof(FavoritesBar), new FrameworkPropertyMetadata(null));
        public Favorites Favorites
        {
            get => (Favorites)GetValue(FavoritesProperty);
            set => SetValue(FavoritesProperty, value);
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(FavoritesBar), new FrameworkPropertyMetadata(string.Empty));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        public FavoritesBar()
        {
            InitializeComponent();
        }

        ICommand goCommand;
        public ICommand GoCommand => goCommand = goCommand ?? new RelayCommand<string>(i => SetCurrentValue(PathProperty, i), i =>
        {
            var result = false;
            Try.Invoke(() => result = Folder.Long.Exists(i));
            return result;
        });
    }
}