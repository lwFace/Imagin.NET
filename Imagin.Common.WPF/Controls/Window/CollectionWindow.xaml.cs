using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public partial class CollectionWindow : BaseWindow
    {
        public const string DefaultLabel = "Items";

        public const string DefaultTitle = "Manage items";

        public static readonly DependencyProperty CollectionProperty = DependencyProperty.Register(nameof(Collection), typeof(IList), typeof(CollectionWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public IList Collection
        {
            get => (IList)GetValue(CollectionProperty);
            private set => SetValue(CollectionProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(CollectionWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(CollectionWindow), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(CollectionWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public CollectionWindow(IList collection)
        {
            SetCurrentValue(CollectionProperty, collection);
            SetCurrentValue(LabelProperty, DefaultLabel);
            SetCurrentValue(TitleProperty, DefaultTitle);
            InitializeComponent();
        }

        void Move(Func<int, int> change, Func<int, int> coerce)
        {
            var item = SelectedItem;
            var index = Collection.IndexOf(SelectedItem);
            index = change(index);
            index = coerce(index);
            Collection.Remove(item);
            Collection.Insert(index, item);
        }

        ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                deleteCommand = deleteCommand ?? new RelayCommand(() => Collection.Remove(SelectedItem), () => Collection != null && SelectedItem != null);
                return deleteCommand;
            }
        }

        ICommand downCommand;
        public ICommand DownCommand
        {
            get
            {
                downCommand = downCommand ?? new RelayCommand(() => Move(i => i + 1, i => i > Collection.Count - 1 ? 0 : i), () => Collection != null && SelectedItem != null);
                return downCommand;
            }
        }

        ICommand renameCommand;
        public ICommand RenameCommand
        {
            get
            {
                renameCommand = renameCommand ?? new RelayCommand(() => 
                {
                    var window = new InputWindow();
                    window.Placeholder = "Name";
                    window.Result = (SelectedItem as INamable).Name;
                    window.SaveButtonLabel = "Rename";
                    window.ShowDialog();

                    if (window.Result.NullOrEmpty())
                        (SelectedItem as INamable).Name = window.Result;
                }, 
                () => SelectedItem is INamable);
                return renameCommand;
            }
        }

        ICommand upCommand;
        public ICommand UpCommand
        {
            get
            {
                upCommand = upCommand ?? new RelayCommand(() => Move(i => i - 1, i => i < 0 ? Collection.Count - 1 : i), () => Collection != null && SelectedItem != null);
                return upCommand;
            }
        }
    }
}