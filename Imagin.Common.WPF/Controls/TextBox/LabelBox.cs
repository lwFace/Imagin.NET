using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = "PART_Button", Type = typeof(MaskedButton))]
    public class LabelBox : TextBox
    {
        #region Properties

        int mouseDown = 0;

        MaskedButton PART_Button;

        public event EventHandler<EventArgs<string>> Edited;

        public static DependencyProperty ButtonHorizontalAlignmentProperty = DependencyProperty.Register(nameof(ButtonHorizontalAlignment), typeof(HorizontalAlignment), typeof(LabelBox), new FrameworkPropertyMetadata(HorizontalAlignment.Left, FrameworkPropertyMetadataOptions.None));
        public HorizontalAlignment ButtonHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(ButtonHorizontalAlignmentProperty);
            set => SetValue(ButtonHorizontalAlignmentProperty, value);
        }

        public static DependencyProperty IsEditableProperty = DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(LabelBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        public static DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(LabelBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static DependencyProperty MouseEventProperty = DependencyProperty.Register(nameof(MouseEvent), typeof(MouseEvent), typeof(LabelBox), new FrameworkPropertyMetadata(MouseEvent.MouseUp, FrameworkPropertyMetadataOptions.None));
        public MouseEvent MouseEvent
        {
            get => (MouseEvent)GetValue(MouseEventProperty);
            set => SetValue(MouseEventProperty, value);
        }

        public static DependencyProperty ShowButtonProperty = DependencyProperty.Register(nameof(ShowButton), typeof(bool), typeof(LabelBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public bool ShowButton
        {
            get => (bool)GetValue(ShowButtonProperty);
            set => SetValue(ShowButtonProperty, value);
        }

        #endregion

        #region LabelBox

        public LabelBox() : base() => DefaultStyleKey = typeof(LabelBox);

        #endregion

        #region Methods

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (!IsEditable)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (MouseEvent == MouseEvent.MouseDoubleClick)
                    {
                        OnEdited(true);
                        e.Handled = true;
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (!IsEditable)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    var handle = false;
                    switch (MouseEvent)
                    {
                        case MouseEvent.DelayedMouseDown:
                            if (mouseDown == 1)
                            {
                                mouseDown = 0;
                                handle = true;
                            }
                            else
                            {
                                mouseDown++;
                            }
                            break;

                        case MouseEvent.MouseDown:
                            handle = true;
                            break;
                    }

                    if (handle)
                        OnEdited(true);
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (IsEditable)
                base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (!IsEditable)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    if (MouseEvent == MouseEvent.MouseUp)
                    {
                        OnEdited(true);
                        e.Handled = true;
                    }
                }
            }
        }

        protected override void OnEntered(string text)
        {
            base.OnEntered(text);
            OnEdited(false);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            OnEdited(false);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Button = Template.FindName(nameof(PART_Button), this) as MaskedButton;
            PART_Button.Click += (s, e) => OnEdited(true);
        }

        protected virtual void OnEdited(bool Value)
        {
            IsEditable = Value;
            if (Value)
            {
                Focus();
            }
            else Edited?.Invoke(this, new EventArgs<string>(Text));
        }

        #endregion
    }
}
