using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class FileBox : TextBox
    {
        #region Properties

        public static readonly IValidate DefaultValidateHandler = new LocalValidateHandler();

        public event EventHandler DialogOpened;

        public static DependencyProperty BrowseButtonTemplateProperty = DependencyProperty.Register(nameof(BrowseButtonTemplate), typeof(DataTemplate), typeof(FileBox), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate BrowseButtonTemplate
        {
            get => (DataTemplate)GetValue(BrowseButtonTemplateProperty);
            set => SetValue(BrowseButtonTemplateProperty, value);
        }

        public static DependencyProperty BrowseButtonToolTipProperty = DependencyProperty.Register(nameof(BrowseButtonToolTip), typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string BrowseButtonToolTip
        {
            get => (string)GetValue(BrowseButtonToolTipProperty);
            set => SetValue(BrowseButtonToolTipProperty, value);
        }

        public static DependencyProperty BrowseButtonVisibilityProperty = DependencyProperty.Register(nameof(BrowseButtonVisibility), typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool BrowseButtonVisibility
        {
            get => (bool)GetValue(BrowseButtonVisibilityProperty);
            set => SetValue(BrowseButtonVisibilityProperty, value);
        }

        public static DependencyProperty BrowseModeProperty = DependencyProperty.Register(nameof(BrowseMode), typeof(ExplorerWindow.Modes), typeof(FileBox), new FrameworkPropertyMetadata(ExplorerWindow.Modes.OpenFolder, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBrowseModeChanged));
        /// <summary>
        /// Gets or sets the type of objects to browse.
        /// </summary>
        public ExplorerWindow.Modes BrowseMode
        {
            get => (ExplorerWindow.Modes)GetValue(BrowseModeProperty);
            set => SetValue(BrowseModeProperty, value);
        }
        static void OnBrowseModeChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<FileBox>().OnBrowseModeChanged(new OldNew<ExplorerWindow.Modes>(e));

        public static DependencyProperty BrowseTitleProperty = DependencyProperty.Register(nameof(BrowseTitle), typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the title of the dialog used to browse objects.
        /// </summary>
        public string BrowseTitle
        {
            get => (string)GetValue(BrowseTitleProperty);
            set => SetValue(BrowseTitleProperty, value);
        }

        public static DependencyProperty CanBrowseProperty = DependencyProperty.Register(nameof(CanBrowse), typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not browsing objects is enabled.
        /// </summary>
        public bool CanBrowse
        {
            get => (bool)GetValue(CanBrowseProperty);
            set => SetValue(CanBrowseProperty, value);
        }

        public static DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(null));
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static DependencyProperty IconVisibilityProperty = DependencyProperty.Register(nameof(IconVisibility), typeof(Visibility), typeof(FileBox), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.None));
        public Visibility IconVisibility
        {
            get => (Visibility)GetValue(IconVisibilityProperty);
            set => SetValue(IconVisibilityProperty, value);
        }

        public static DependencyProperty IsEditableProperty = DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        public static DependencyProperty IsValidProperty = DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(false));
        /// <summary>
        /// If validation is enabled, gets whether or not the input (or file or folder path) is valid.
        /// </summary>
        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidProperty, value);
        }

        public static DependencyProperty FileExtensionsProperty = DependencyProperty.Register(nameof(FileExtensions), typeof(IList<string>), typeof(FileBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public IList<string> FileExtensions
        {
            get => (IList<string>)GetValue(FileExtensionsProperty);
            set => SetValue(FileExtensionsProperty, value);
        }

        public static DependencyProperty ValidateProperty = DependencyProperty.Register(nameof(CanValidate), typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCanValidateChanged));
        /// <summary>
        /// Gets or sets whether or not the input (or file or folder path) should be validated.
        /// </summary>
        public bool CanValidate
        {
            get => (bool)GetValue(ValidateProperty);
            set => SetValue(ValidateProperty, value);
        }
        static void OnCanValidateChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<FileBox>().OnValidateChanged(new OldNew<bool>(e));

        public static DependencyProperty ValidateHandlerProperty = DependencyProperty.Register(nameof(ValidateHandler), typeof(IValidate), typeof(FileBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnValidateHandlerChanged));
        /// <summary>
        /// If validation is enabled, gets or sets an object that implements <see cref="IValidate{T}"/>, which is used to validate the input (or file or folder path).
        /// </summary>
        public IValidate ValidateHandler
        {
            get => (IValidate)GetValue(ValidateHandlerProperty);
            set => SetValue(ValidateHandlerProperty, value);
        }
        static void OnValidateHandlerChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<FileBox>().OnValidateChanged(new OldNew<bool>(i.As<FileBox>().CanValidate));

        public static DependencyProperty ValidateTemplateProperty = DependencyProperty.Register(nameof(ValidateTemplate), typeof(DataTemplate), typeof(FileBox), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ValidateTemplate
        {
            get => (DataTemplate)GetValue(ValidateTemplateProperty);
            set => SetValue(ValidateTemplateProperty, value);
        }
        
        public static DependencyProperty ValidateToolTipProperty = DependencyProperty.Register(nameof(ValidateToolTip), typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string ValidateToolTip
        {
            get => (string)GetValue(ValidateToolTipProperty);
            set => SetValue(ValidateToolTipProperty, value);
        }

        #endregion

        #region FileBox

        public FileBox()
        {
            DefaultStyleKey = typeof(FileBox);
        }

        #endregion

        #region Methods

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            OnValidateChanged(new OldNew<bool>(CanValidate));
        }

        protected virtual void OnBrowseModeChanged(OldNew<ExplorerWindow.Modes> input) => OnValidateChanged(new OldNew<bool>(CanValidate));

        protected virtual void OnValidateChanged(OldNew<bool> input)
        {
            SetCurrentValue(IsValidProperty, input.New ? (ValidateHandler ?? DefaultValidateHandler).Validate(BrowseMode, Text) : false);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!IsEditable)
            {
                if (!IsReadOnly)
                    SetCurrentValue(IsEditableProperty, true);
            }
            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (IsEditable)
                SetCurrentValue(IsEditableProperty, false);
        }

        public void Browse()
        {
            DialogOpened?.Invoke(this, new EventArgs());
            Focus();

            var path = string.Empty;
            if (ExplorerWindow.Show(out path, BrowseTitle, BrowseMode, FileExtensions, Text))
                SetCurrentValue(TextProperty, path);

            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        ICommand browseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                browseCommand = browseCommand ?? new RelayCommand(Browse, () => CanBrowse);
                return browseCommand;
            }
        }

        #endregion
    }
}