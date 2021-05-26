using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class TextBox : System.Windows.Controls.TextBox
    {
        #region Properties

        public event EventHandler<TextEnteredEventArgs> Entered;

        public event EventHandler<RoutedEventArgs> TripleClick;

        //.........................................................................................................................

        public static DependencyProperty ActualFontFamilyProperty = DependencyProperty.Register(nameof(ActualFontFamily), typeof(FontFamily), typeof(TextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public FontFamily ActualFontFamily
        {
            get => (FontFamily)GetValue(ActualFontFamilyProperty);
            set => SetValue(ActualFontFamilyProperty, value);
        }

        public static DependencyProperty ClearButtonTemplateProperty = DependencyProperty.Register(nameof(ClearButtonTemplate), typeof(DataTemplate), typeof(TextBox), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ClearButtonTemplate
        {
            get => (DataTemplate)GetValue(ClearButtonTemplateProperty);
            set => SetValue(ClearButtonTemplateProperty, value);
        }

        public static DependencyProperty EnterButtonTemplateProperty = DependencyProperty.Register(nameof(EnterButtonTemplate), typeof(DataTemplate), typeof(TextBox), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate EnterButtonTemplate
        {
            get => (DataTemplate)GetValue(EnterButtonTemplateProperty);
            set => SetValue(EnterButtonTemplateProperty, value);
        }

        public static DependencyProperty EnteredCommandProperty = DependencyProperty.Register(nameof(EnteredCommand), typeof(ICommand), typeof(TextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public ICommand EnteredCommand
        {
            get => (ICommand)GetValue(EnteredCommandProperty);
            set => SetValue(EnteredCommandProperty, value);
        }
        
        public static DependencyProperty IsClearEnabledProperty = DependencyProperty.Register(nameof(IsClearEnabled), typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsClearEnabled
        {
            get => (bool)GetValue(IsClearEnabledProperty);
            set => SetValue(IsClearEnabledProperty, value);
        }

        public static DependencyProperty MaskCharactersProperty = DependencyProperty.Register(nameof(MaskCharacters), typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool MaskCharacters
        {
            get => (bool)GetValue(MaskCharactersProperty);
            set => SetValue(MaskCharactersProperty, value);
        }

        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(TextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static DependencyProperty PlaceholderTemplateProperty = DependencyProperty.Register(nameof(PlaceholderTemplate), typeof(DataTemplate), typeof(TextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate PlaceholderTemplate
        {
            get => (DataTemplate)GetValue(PlaceholderTemplateProperty);
            set => SetValue(PlaceholderTemplateProperty, value);
        }

        public static DependencyProperty SelectAllOnFocusProperty = DependencyProperty.Register(nameof(SelectAllOnFocus), typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool SelectAllOnFocus
        {
            get => (bool)GetValue(SelectAllOnFocusProperty);
            set => SetValue(SelectAllOnFocusProperty, value);
        }

        public static DependencyProperty SelectAllOnTripleClickProperty = DependencyProperty.Register(nameof(SelectAllOnTripleClick), typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool SelectAllOnTripleClick
        {
            get => (bool)GetValue(SelectAllOnTripleClickProperty);
            set => SetValue(SelectAllOnTripleClickProperty, value);
        }

        public static DependencyProperty ShowEnterButtonProperty = DependencyProperty.Register(nameof(ShowEnterButton), typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowEnterButton
        {
            get => (bool)GetValue(ShowEnterButtonProperty);
            set => SetValue(ShowEnterButtonProperty, value);
        }

        public static DependencyProperty ShowToggleButtonProperty = DependencyProperty.Register(nameof(ShowToggleButton), typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowToggleButton
        {
            get => (bool)GetValue(ShowToggleButtonProperty);
            set => SetValue(ShowToggleButtonProperty, value);
        }

        public static DependencyProperty ToggleButtonTemplateProperty = DependencyProperty.Register(nameof(ToggleButtonTemplate), typeof(DataTemplate), typeof(TextBox), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ToggleButtonTemplate
        {
            get => (DataTemplate)GetValue(ToggleButtonTemplateProperty);
            set => SetValue(ToggleButtonTemplateProperty, value);
        }

        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register(nameof(TextTrimming), typeof(TextTrimming), typeof(TextBox), new FrameworkPropertyMetadata(TextTrimming.CharacterEllipsis, FrameworkPropertyMetadataOptions.None));
        public TextTrimming TextTrimming
        {
            get => (TextTrimming)GetValue(TextTrimmingProperty);
            set => SetValue(TextTrimmingProperty, value);
        }
        
        #endregion

        #region TextBox

        public TextBox() : base()
        {
            DefaultStyleKey = typeof(TextBox);
        }

        #endregion

        #region Methods

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            if (SelectAllOnFocus)
                SelectAll();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Enter)
                OnEntered(Text);
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            if (SelectAllOnFocus)
            {
                if (!IsKeyboardFocusWithin)
                {
                    //It would seem this is internal :(
                    const string targetType = "System.Windows.Controls.TextBoxView";
                    if (e.OriginalSource?.GetType().FullName == targetType)
                    {
                        Focus();
                        e.Handled = true;
                    }
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            switch (e.ClickCount)
            {
                case 3:
                    OnTripleClick();
                    SelectAllOnTripleClick.If(true, SelectAll);
                    break;
            }
        }

        //.........................................................................................................................

        public sealed override string ToString() => GetType().FullName;

        //.........................................................................................................................

        protected virtual void OnEntered(string Text)
        {
            EnteredCommand?.Execute(null);
            Entered?.Invoke(this, new TextEnteredEventArgs(Text));
        }

        protected virtual void OnTripleClick(RoutedEventArgs e = null)
        {
            TripleClick?.Invoke(this, e ?? new RoutedEventArgs());
        }

        //.........................................................................................................................

        ICommand clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                clearCommand = clearCommand ?? new RelayCommand(() => Text = string.Empty, () => IsFocused && Text.Length > 0 && !IsReadOnly);
                return clearCommand;
            }
        }

        ICommand enterCommand;
        public ICommand EnterCommand
        {
            get
            {
                enterCommand = enterCommand ?? new RelayCommand(() => OnEntered(Text), () => true);
                return enterCommand;
            }
        }
        
        #endregion
    }
}