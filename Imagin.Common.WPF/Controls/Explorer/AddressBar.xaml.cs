using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public partial class AddressBar : UserControl, IExplorer, IPropertyChanged
    {
        Handle handle = false;

        //...........................................................................................

        public event EventHandler<EventArgs<string>> PathChanged;

        public event RoutedEventHandler Refreshed;

        //...........................................................................................

        public static DependencyProperty FavoritesProperty = DependencyProperty.Register(nameof(Favorites), typeof(Favorites), typeof(AddressBar), new FrameworkPropertyMetadata(null));
        public Favorites Favorites
        {
            get => (Favorites)GetValue(FavoritesProperty);
            set => SetValue(FavoritesProperty, value);
        }

        public static DependencyProperty HistoryProperty = DependencyProperty.Register(nameof(History), typeof(History), typeof(AddressBar), new FrameworkPropertyMetadata(null));
        public History History
        {
            get => (History)GetValue(HistoryProperty);
            set => SetValue(HistoryProperty, value);
        }

        public static DependencyProperty IsEditableProperty = DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(AddressBar), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        public bool IsPathFavorite
        {
            get => Favorites.FirstOrDefault(i => i.Path == Path) != null;
            set
            {
                if (Favorites != null)
                {
                    var found = Favorites.FirstOrDefault(i => i.Path == Path);
                    if (!value)
                    {
                        found.If(i => i != null, i => Favorites.Remove(i));
                    }
                    else
                    {
                        if (found == null)
                            Favorites.Add(new Favorite(Path));
                    }
                }
            }
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(AddressBar), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPathChanged, OnPathCoerced));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static void OnPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<AddressBar>().OnPathChanged(new OldNew<string>(e));
        static object OnPathCoerced(DependencyObject i, object value) => Explorer.Validate(i, value?.ToString());
        
        public static DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), typeof(AddressBar), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.None));
        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        //...........................................................................................

        public AddressBar()
        {
            SetCurrentValue(HistoryProperty, new History(Explorer.DefaultLimit));
            InitializeComponent();
        }

        #region IPropertyChanged

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        //...........................................................................................

        protected virtual void OnPathChanged(OldNew<string> input)
        {
            if (!handle)
                History?.Add(input.New);

            this.Changed(() => IsPathFavorite);
            PathChanged?.Invoke(this, new EventArgs<string>(input.New));
        }

        //...........................................................................................

        ICommand backCommand;
        public ICommand BackCommand => backCommand = backCommand ?? 
            new RelayCommand<object>(i => History.Undo(j => handle.Invoke(() => SetCurrentValue(PathProperty, j))), i => History?.CanUndo() == true);

        ICommand changePathCommand;
        public ICommand ChangePathCommand => changePathCommand = changePathCommand ?? 
            new RelayCommand<object>(i => SetCurrentValue(PathProperty, i.ToString()), i => i != null);

        ICommand clearHistoryCommand;
        public ICommand ClearHistoryCommand => clearHistoryCommand = clearHistoryCommand ?? 
            new RelayCommand<object>(i => History.Clear(), i => History?.Count > 0);

        ICommand forwardCommand;
        public ICommand ForwardCommand => forwardCommand = forwardCommand ?? 
            new RelayCommand<object>(i => History.Redo(j => handle.Invoke(() => SetCurrentValue(PathProperty, j))), x => History?.CanRedo() == true);

        ICommand goCommand;
        public ICommand GoCommand => goCommand = goCommand ?? 
            new RelayCommand<object>(i => SetCurrentValue(PathProperty, PART_CrumbBar.EditableAddress), i => true);

        ICommand goUpCommand;
        public ICommand GoUpCommand => goUpCommand = goUpCommand ?? 
            new RelayCommand<object>(i => SetCurrentValue(PathProperty, Storage.Folder.Long.Parent(i.ToString())), i => i?.ToString().Empty() == false && i.ToString() != Storage.Folder.Long.Root);

        ICommand refreshCommand;
        public ICommand RefreshCommand => refreshCommand = refreshCommand ?? 
            new RelayCommand<object>(i => Refreshed?.Invoke(this, new RoutedEventArgs()), i => true);
    }
}