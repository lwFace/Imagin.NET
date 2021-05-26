using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Converters;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public partial class ExplorerWindow : BaseWindow
    {
        struct Constants
        {
            public const string Open = "Select";

            public const string OpenWithDots = "Open...";

            public const string Save = "Save";

            public const string SaveWithDots = "Save...";
        }

        //....................................................................................

        public enum Modes
        {
            Open,
            OpenFile,
            OpenFolder,
            SaveFile
        }

        public enum SelectionModes
        {
            Single,
            Multiple
        }

        //....................................................................................

        Handle handleClosing = false;

        Handle handleFileNames = false;

        //....................................................................................

        string SaveFilePath => $@"{Path.TrimEnd('\\')}\{FileNames}";

        //....................................................................................

        public readonly List<string> Paths = new List<string>();

        //....................................................................................

        public static DependencyProperty ActualFileExtensionsProperty = DependencyProperty.Register(nameof(ActualFileExtensions), typeof(IList<string>), typeof(ExplorerWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public IList<string> ActualFileExtensions
        {
            get => (IList<string>)GetValue(ActualFileExtensionsProperty);
            private set => SetValue(ActualFileExtensionsProperty, value);
        }

        public static DependencyProperty FileExtensionProperty = DependencyProperty.Register(nameof(FileExtension), typeof(int), typeof(ExplorerWindow), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnFileExtensionChanged));
        public int FileExtension
        {
            get => (int)GetValue(FileExtensionProperty);
            set => SetValue(FileExtensionProperty, value);
        }
        static void OnFileExtensionChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ExplorerWindow).OnFileExtensionChanged(new OldNew<int>(e));

        public static DependencyProperty FileExtensionsProperty = DependencyProperty.Register(nameof(FileExtensions), typeof(IList<string>), typeof(ExplorerWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnFileExtensionsChanged));
        public IList<string> FileExtensions
        {
            get => (IList<string>)GetValue(FileExtensionsProperty);
            set => SetValue(FileExtensionsProperty, value);
        }
        static void OnFileExtensionsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ExplorerWindow).OnFileExtensionsChanged(new OldNew<IList<string>>(e));
        
        public static DependencyProperty FileExtensionGroupsProperty = DependencyProperty.Register(nameof(FileExtensionGroups), typeof(FileExtensionGroups), typeof(ExplorerWindow), new FrameworkPropertyMetadata(default(FileExtensionGroups), FrameworkPropertyMetadataOptions.None));
        public FileExtensionGroups FileExtensionGroups
        {
            get => (FileExtensionGroups)GetValue(FileExtensionGroupsProperty);
            private set => SetValue(FileExtensionGroupsProperty, value);
        }

        public static DependencyProperty FileNamesProperty = DependencyProperty.Register(nameof(FileNames), typeof(string), typeof(ExplorerWindow), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, OnFileNamesChanged));
        public string FileNames
        {
            get => (string)GetValue(FileNamesProperty);
            set => SetValue(FileNamesProperty, value);
        }
        static void OnFileNamesChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ExplorerWindow).OnFileNamesChanged(new OldNew<string>(e));

        public static DependencyProperty HistoryProperty = DependencyProperty.Register(nameof(History), typeof(History), typeof(ExplorerWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public History History
        {
            get => (History)GetValue(HistoryProperty);
            set => SetValue(HistoryProperty, value);
        }

        public static DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(Modes), typeof(ExplorerWindow), new FrameworkPropertyMetadata(Modes.Open, FrameworkPropertyMetadataOptions.None, OnModeChanged));
        public Modes Mode
        {
            get => (Modes)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        static void OnModeChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ExplorerWindow).OnModeChanged(new OldNew<Modes>(e));

        public static DependencyProperty OverwritePromptProperty = DependencyProperty.Register(nameof(OverwritePrompt), typeof(bool), typeof(ExplorerWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool OverwritePrompt
        {
            get => (bool)GetValue(OverwritePromptProperty);
            set => SetValue(OverwritePromptProperty, value);
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(ExplorerWindow), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, OnPathChanged, OnPathCoerced));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static void OnPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ExplorerWindow).OnPathChanged(new OldNew<string>(e));
        static object OnPathCoerced(DependencyObject i, object value) => Explorer.Validate(i, value?.ToString());

        public static DependencyProperty SelectionProperty = DependencyProperty.Register(nameof(Selection), typeof(IList<Item>), typeof(ExplorerWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnSelectionChanged));
        public IList<Item> Selection
        {
            get => (IList<Item>)GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }
        static void OnSelectionChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ExplorerWindow).OnSelectionChanged(new OldNew<IList<Item>>(e));

        public static DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(SelectionModes), typeof(ExplorerWindow), new FrameworkPropertyMetadata(SelectionModes.Single, FrameworkPropertyMetadataOptions.None, null, OnSelectionModeCoerced));
        public SelectionModes SelectionMode
        {
            get => (SelectionModes)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }
        static object OnSelectionModeCoerced(DependencyObject i, object value)
        {
            switch ((i as ExplorerWindow).Mode)
            {
                case Modes.Open:
                case Modes.OpenFile:
                    return value;

                case Modes.OpenFolder:
                case Modes.SaveFile:
                    return SelectionModes.Single;
            }
            throw new NotSupportedException();
        }

        public static DependencyProperty ViewFileExtensionsProperty = DependencyProperty.Register(nameof(ViewFileExtensions), typeof(bool), typeof(ExplorerWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool ViewFileExtensions
        {
            get => (bool)GetValue(ViewFileExtensionsProperty);
            set => SetValue(ViewFileExtensionsProperty, value);
        }

        public static DependencyProperty ViewHiddenItemsProperty = DependencyProperty.Register(nameof(ViewHiddenItems), typeof(bool), typeof(ExplorerWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool ViewHiddenItems
        {
            get => (bool)GetValue(ViewHiddenItemsProperty);
            set => SetValue(ViewHiddenItemsProperty, value);
        }

        //....................................................................................

        public ExplorerWindow(string title, Modes mode, SelectionModes selectionMode)
        {
            InitializeComponent();

            SetCurrentValue(FileExtensionGroupsProperty, new FileExtensionGroups());
            FileExtensionGroups.Add(FileExtensionGroup.Wild);

            SetCurrentValue(TitleProperty, title);
            SetCurrentValue(ModeProperty, mode);
            SetCurrentValue(SelectionModeProperty, selectionMode);

            PART_Explorer.Bind(Explorer.SelectionModeProperty, nameof(SelectionMode), this, BindingMode.OneWay, new DefaultConverter<SelectionModes, SelectionMode>(i =>
            {
                switch (i)
                {
                    case SelectionModes.Multiple:
                        return Controls.SelectionMode.Multiple;
                    case SelectionModes.Single:
                        return Controls.SelectionMode.Single;
                }
                throw new NotSupportedException();
            }, null));
        }

        //....................................................................................

        void Update()
        {
            handleFileNames = true;
            void update()
            {
                Paths.Clear();
                if (Selection == null || Selection.Count == 0)
                {
                    switch (Mode)
                    {
                        case Modes.Open:
                        case Modes.OpenFolder:
                            SetCurrentValue(FileNamesProperty, $"<{Machine.FriendlyName(Path)}>");
                            Paths.Add(Path);
                            return;

                        case Modes.SaveFile:
                            if (Valid(FileNames))
                                Paths.Add(SaveFilePath);
                            return;

                        default:
                            SetCurrentValue(FileNamesProperty, string.Empty);
                            return;
                    }
                }

                if (SelectionMode == SelectionModes.Single || Selection.Count == 1)
                {
                    switch (Mode)
                    {
                        case Modes.Open:
                            goto default;

                        case Modes.OpenFile:
                            if (Selection[0] is File)
                                goto default;

                            break;

                        case Modes.SaveFile:
                            if (Selection[0] is File)
                            {
                                SetCurrentValue(FileNamesProperty, $"{Machine.FriendlyName(Selection[0].Path)}");
                                if (Valid(FileNames))
                                    Paths.Add(Selection[0].Path);

                                break;
                            }

                            if (Valid(FileNames))
                                Paths.Add(SaveFilePath);
                            break;

                        case Modes.OpenFolder:
                            if (Selection[0] is Storage.Container)
                                goto default;

                            if (Selection[0] is Shortcut)
                            {
                                if (Shortcut.TargetsFolder(Selection[0].Path))
                                    goto default;
                            }

                            break;

                        default:
                            SetCurrentValue(FileNamesProperty, $"{Machine.FriendlyName(Selection[0].Path)}");
                            Paths.Add(Selection[0].Path);
                            break;
                    }
                    return;
                }

                if (SelectionMode == SelectionModes.Multiple && Selection.Count > 1)
                {
                    var result = string.Empty;
                    foreach (var i in Selection)
                    {
                        switch (Mode)
                        {
                            case Modes.Open:
                                goto default;

                            case Modes.OpenFile:
                                if (i is File)
                                    goto default;

                                break;

                            case Modes.SaveFile:
                                break;

                            case Modes.OpenFolder:
                                if (i is Storage.Container)
                                    goto default;

                                if (i is Shortcut)
                                {
                                    if (Shortcut.TargetsFolder(i.Path))
                                        goto default;
                                }

                                break;

                            default:
                                result += $"\"{Machine.FriendlyName(i.Path)}\" ";
                                Paths.Add(i.Path);
                                break;
                        }
                    }
                    SetCurrentValue(FileNamesProperty, result);
                }
            }
            update();
            handleFileNames = false;
        }

        //....................................................................................

        void OnFileOpened(object sender, EventArgs<string> e)
        {
            Close();
        }

        void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != File.Long.CleanName(e.Text))
                e.Handled = true;
        }

        //....................................................................................

        bool Valid(string input)
        {
            if (FileExtensions == null || FileExtensions.Count == 0 || FileExtensionGroups[FileExtension].FileExtension == FileExtensionGroup.Wild)
                return !Storage.Path.GetFirstExtension(input).NullOrEmpty();

            return Storage.Path.GetFirstExtension(input) == FileExtensionGroups[FileExtension].FileExtension;
        }

        //....................................................................................

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!handleClosing)
                Paths.Clear();
        }

        //....................................................................................

        protected virtual void OnFileExtensionChanged(OldNew<int> input)
        {
            SetCurrentValue(ActualFileExtensionsProperty, FileExtensionGroups[input.New].FileExtension == FileExtensionGroup.Wild ? null : Array<string>.New(FileExtensionGroups[input.New].FileExtension));
        }

        protected virtual void OnFileExtensionsChanged(OldNew<IList<string>> input)
        {
            FileExtensionGroups.Clear();
            if (Mode != Modes.OpenFolder)
            {
                if (input != null)
                {
                    foreach (var i in input.New)
                        FileExtensionGroups.Add(i);
                }

                FileExtensionGroups.Add(FileExtensionGroup.Wild);
                OnFileExtensionChanged(new OldNew<int>(0));
            }
        }

        protected virtual void OnFileNamesChanged(OldNew<string> input)
        {
            if (!handleFileNames)
            {
                if (Mode == Modes.SaveFile)
                {
                    Paths.Clear();
                    if (Valid(input.New))
                        Paths.Add(SaveFilePath);
                }
            }
        }

        protected virtual void OnModeChanged(OldNew<Modes> input)
        {
            switch (input.New)
            {
                case Modes.Open:
                case Modes.OpenFile:
                    PART_Explorer.ViewFiles = true;
                    PART_ComboBox.Visibility = Visibility.Visible;
                    PART_OpenButton.Content = Constants.Open;
                    PART_TextBox.IsReadOnly = true;
                    break;

                case Modes.SaveFile:
                    PART_Explorer.ViewFiles = true;
                    PART_ComboBox.Visibility = Visibility.Visible;
                    PART_OpenButton.Content = Constants.Save;
                    PART_TextBox.IsReadOnly = false;
                    break;

                case Modes.OpenFolder:
                    PART_Explorer.ViewFiles = false;
                    PART_ComboBox.Visibility = Visibility.Collapsed;
                    PART_OpenButton.Content = Constants.Open;
                    PART_TextBox.IsReadOnly = true;
                    break;
            }
            SetCurrentValue(SelectionModeProperty, SelectionMode);
        }

        protected virtual void OnPathChanged(OldNew<string> input)
        {
            Update();
        }

        protected virtual void OnSelectionChanged(OldNew<IList<Item>> input)
        {
            Update();
        }

        //....................................................................................

        ICommand openCommand;
        public ICommand OpenCommand
        {
            get
            {
                openCommand = openCommand ?? new RelayCommand<object>(x =>
                {
                    if (Mode == Modes.SaveFile)
                    {
                        if (OverwritePrompt)
                        {
                            var filePath = Paths.First<string>();
                            if (File.Long.Exists(filePath))
                            {
                                var result = Dialog.Show("Confirm", $"'{filePath}' already exists. Do you want to replace it?", DialogImage.Warning, DialogButtons.YesNo);
                                if (result == 1)
                                    return;
                            }
                        }
                    }

                    handleClosing = true;
                    Close();
                }, 
                x => Paths.Count > 0);
                return openCommand;
            }
        }

        ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                cancelCommand = cancelCommand ?? new RelayCommand<object>(i => Close(), i => true);
                return cancelCommand;
            }
        }

        //....................................................................................

        static ExplorerWindow New(string title, Modes mode, SelectionModes selectionMode, IEnumerable<string> fileExtensions, string initialFolderPath)
        {
            var result = new ExplorerWindow(title, mode, selectionMode);
            //If the initial folder path points to a file, get the folder path of the file.
            result.SetCurrentValue(PathProperty, File.Long.Exists(initialFolderPath) ? System.IO.Path.GetDirectoryName(initialFolderPath) : initialFolderPath);
            result.SetCurrentValue(ViewFileExtensionsProperty, true);
            result.SetCurrentValue(ViewHiddenItemsProperty, true);

            switch (mode)
            {
                case Modes.Open:
                case Modes.OpenFile:
                case Modes.OpenFolder:
                    result.Title = title.NullOrEmpty() ? Constants.OpenWithDots : title;
                    break;

                case Modes.SaveFile:
                    result.Title = title.NullOrEmpty() ? Constants.SaveWithDots : title;
                    break;
            }

            if (mode != Modes.OpenFolder)
                result.FileExtensions = fileExtensions?.ToList();

            return result;
        }

        //....................................................................................

        public static bool Show 
        #region <parameters>
        (
            out string[] paths,
            string title
                = "",
            Modes mode
                = Modes.OpenFile,
            IEnumerable<string> extensions
                = null,
            string initialFolderPath
                = ""
        )
        #endregion
        #region <body>
        {
            if (mode == Modes.SaveFile)
                throw new NotSupportedException();

            paths = new string[0];

            var dialog = New(title, mode, SelectionModes.Multiple, extensions, initialFolderPath);
            dialog.ShowDialog();

            if (dialog.Paths.Count > 0)
            {
                paths = dialog.Paths.ToArray();
                return true;
            }
            return false;
        }
        #endregion

        public static bool Show
        #region <parameters>
        (
            out string path,
            string title
                = "",
            Modes dialogMode
                = Modes.OpenFile,
            IEnumerable<string> fileExtensions
                = null,
            string initialFolderPath
                = ""
        )
        #endregion
        #region <body>
        {
            path = string.Empty;

            var dialog = New(title, dialogMode, SelectionModes.Single, fileExtensions, initialFolderPath);
            dialog.ShowDialog();

            if (dialog.Paths.Count > 0)
            {
                path = dialog.Paths.First<string>();
                return true;
            }

            return false;
        }
        #endregion
    }
}