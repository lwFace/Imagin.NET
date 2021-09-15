using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace Imagin.Common.Linq
{
    public static class RichTextBoxExtensions
    {
        async static Task Load(RichTextBox richTextBox)
        {
            richTextBox.TextChanged
               -= Extends_OnTextChanged;
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                try
                {
                    using (var fileStream = new System.IO.FileStream(GetSource(richTextBox), System.IO.FileMode.OpenOrCreate))
                    {
                        var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                        textRange.Load(fileStream, DataFormats.Rtf);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }));
            if (GetExtends(richTextBox))
            {
                richTextBox.TextChanged
                   += Extends_OnTextChanged;
            }
        }

        async static Task Save(RichTextBox richTextBox)
        {
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                try
                {
                    using (var fileStream = new System.IO.FileStream(GetSource(richTextBox), System.IO.FileMode.Create))
                    {
                        var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                        textRange.Save(fileStream, DataFormats.Rtf);
                    }
                    SetIsModified(richTextBox, false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }));
        }

        #region Extends

        public static readonly DependencyProperty ExtendsProperty = DependencyProperty.RegisterAttached("Extends", typeof(bool), typeof(RichTextBoxExtensions), new PropertyMetadata(false, OnExtendsChanged));
        public static bool GetExtends(RichTextBox d)
            => (bool)d.GetValue(ExtendsProperty);
        public static void SetExtends(RichTextBox d, bool value)
            => d.SetValue(ExtendsProperty, value);
        static void OnExtendsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var richTextBox = sender as RichTextBox;
            richTextBox.PreviewKeyDown
                -= Extends_OnPreviewKeyDown;
            richTextBox.TextChanged
                -= Extends_OnTextChanged;

            if ((bool)e.NewValue)
            {
                richTextBox.PreviewKeyDown
                    += Extends_OnPreviewKeyDown;
                richTextBox.TextChanged
                    += Extends_OnTextChanged;
            }
        }

        static void Extends_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys.Control.Pressed())
            {
                if (e.Key == Key.S)
                    _ = Save((RichTextBox)sender);
            }
        }

        static void Extends_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SetIsModified((RichTextBox)sender, true);
            if (GetAutoSave((RichTextBox)sender))
                _ = Save((RichTextBox)sender);
        }

        #endregion

        #region AutoSave

        public static readonly DependencyProperty AutoSaveProperty = DependencyProperty.RegisterAttached("AutoSave", typeof(bool), typeof(RichTextBoxExtensions), new PropertyMetadata(false));
        public static bool GetAutoSave(RichTextBox d)
            => (bool)d.GetValue(AutoSaveProperty);
        public static void SetAutoSave(RichTextBox d, bool value)
            => d.SetValue(AutoSaveProperty, value);

        #endregion

        #region IsModified

        public static readonly DependencyProperty IsModifiedProperty = DependencyProperty.RegisterAttached("IsModified", typeof(bool), typeof(RichTextBoxExtensions), new PropertyMetadata(false));
        public static bool GetIsModified(RichTextBox d)
            => (bool)d.GetValue(IsModifiedProperty);
        public static void SetIsModified(RichTextBox d, bool value)
            => d.SetValue(IsModifiedProperty, value);

        #endregion

        #region Source

        public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source", typeof(string), typeof(RichTextBoxExtensions), new PropertyMetadata(null, OnSourceChanged));
        public static string GetSource(RichTextBox d)
            => (string)d.GetValue(SourceProperty);
        public static void SetSource(RichTextBox d, string value)
            => d.SetValue(SourceProperty, value);
        async static void OnSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            await Load((RichTextBox)sender);
            SetIsModified((RichTextBox)sender, false);
        }

        #endregion
    }

    public static class TextBoxExtensions
    {
        #region Tab

        public static readonly DependencyProperty TabProperty = DependencyProperty.RegisterAttached("Tab", typeof(bool), typeof(TextBoxExtensions), new PropertyMetadata(false, OnTabChanged));
        public static bool GetTab(TextBox i) => (bool)i.GetValue(TabProperty);
        public static void SetTab(TextBox i, bool value) => i.SetValue(TabProperty, value);
        static void OnTabChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var i = sender as TextBox;
            i.PreviewKeyDown -= Tab_OnPreviewKeyDown;
            if ((bool)e.NewValue)
                i.PreviewKeyDown += Tab_OnPreviewKeyDown;
        }

        static void Tab_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox)sender;

            var insert = string.Empty;
            for (var i = 0; i < GetTabSpace(textBox); i++)
                insert += " ";

            if (e.Key == Key.Tab)
            {
                if (ModifierKeys.Shift.Pressed())
                {
                    var a = textBox.CaretIndex - 1;
                    var b = a + 1;

                    for (var i = a; i > a - GetTabSpace(textBox); i--)
                    {
                        if (i < 0)
                            break;

                        if ($"{textBox.Text[i]}".NullOrWhitespace())
                        {
                            b = i;
                            continue;
                        }

                        break;
                    }

                    if (a >= 0 && b >= 0)
                    {
                        var x = textBox.Text.Substring(0, b);
                        var y = textBox.Text.Substring(a + 1);
                        textBox.Text = $"{x}{y}";
                        textBox.CaretIndex = b;
                    }
                }
                else
                {
                    var caretIndex = textBox.CaretIndex + (int)GetTabSpace(textBox);
                    textBox.Text = textBox.Text.Insert(textBox.CaretIndex, insert);
                    textBox.CaretIndex = caretIndex;
                }
                e.Handled = true;
            }
        }

        #endregion

        #region TabSpace

        public static readonly DependencyProperty TabSpaceProperty = DependencyProperty.RegisterAttached("TabSpace", typeof(uint), typeof(TextBoxExtensions), new PropertyMetadata((uint)4));
        public static uint GetTabSpace(TextBox i) => (uint)i.GetValue(TabSpaceProperty);
        public static void SetTabSpace(TextBox i, uint value) => i.SetValue(TabSpaceProperty, value);

        #endregion

        #region CaretIndex

        public static readonly DependencyProperty CaretIndexProperty = DependencyProperty.RegisterAttached("CaretIndex", typeof(int), typeof(TextBoxExtensions), new PropertyMetadata(0, OnCaretIndexChanged));
        public static int GetCaretIndex(TextBox i) => (int)i.GetValue(CaretIndexProperty);
        public static void SetCaretIndex(TextBox i, int value) => i.SetValue(CaretIndexProperty, value);
        static void OnCaretIndexChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var i = sender as TextBox;
            i.CaretIndex = (int)e.NewValue;
            i.Focus();
        }

        #endregion

        static void OnEnableCommandChanged(UIElement sender, ExecutedRoutedEventHandler handler, bool enable)
        {
            if (sender != null)
            {
                if (enable)
                    CommandManager.RemovePreviewExecutedHandler(sender, handler);

                else CommandManager.AddPreviewExecutedHandler(sender, handler);
            }
        }

        #region EnableCopyCommand

        public static readonly DependencyProperty EnableCopyCommandProperty = DependencyProperty.RegisterAttached("EnableCopyCommand", typeof(bool), typeof(TextBoxExtensions), new PropertyMetadata(true, OnEnableCopyCommandChanged));
        public static void SetEnableCopyCommand(TextBoxBase i, bool value) => i.SetValue(EnableCopyCommandProperty, value);
        public static bool GetEnableCopyCommand(TextBoxBase i) => (bool)i.GetValue(EnableCopyCommandProperty);
        static void OnEnableCopyCommandChanged(object sender, DependencyPropertyChangedEventArgs e) => OnEnableCommandChanged(sender as UIElement, OnPreviewCopyExecuted, (bool)e.NewValue);

        static void OnPreviewCopyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy)
                e.Handled = true;
        }

        #endregion

        #region EnableCutCommand

        public static readonly DependencyProperty EnableCutCommandProperty = DependencyProperty.RegisterAttached("EnableCutCommand", typeof(bool), typeof(TextBoxExtensions), new PropertyMetadata(true, OnEnableCutCommandChanged));
        public static void SetEnableCutCommand(TextBoxBase i, bool value) => i.SetValue(EnableCutCommandProperty, value);
        public static bool GetEnableCutCommand(TextBoxBase i) => (bool)i.GetValue(EnableCutCommandProperty);
        static void OnEnableCutCommandChanged(object sender, DependencyPropertyChangedEventArgs e) => OnEnableCommandChanged(sender as UIElement, OnPreviewCutExecuted, (bool)e.NewValue);

        static void OnPreviewCutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut)
                e.Handled = true;
        }

        #endregion

        #region EnablePasteCommand

        public static readonly DependencyProperty EnablePasteCommandProperty = DependencyProperty.RegisterAttached("EnablePasteCommand", typeof(bool), typeof(TextBoxExtensions), new PropertyMetadata(true, OnEnablePasteCommandChanged));
        public static void SetEnablePasteCommand(TextBoxBase i, bool value) => i.SetValue(EnablePasteCommandProperty, value);
        public static bool GetEnablePasteCommand(TextBoxBase i) => (bool)i.GetValue(EnablePasteCommandProperty);
        static void OnEnablePasteCommandChanged(object sender, DependencyPropertyChangedEventArgs e) => OnEnableCommandChanged(sender as UIElement, OnPreviewPasteExecuted, (bool)e.NewValue);

        static void OnPreviewPasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
                e.Handled = true;
        }

        #endregion

        #region Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof(string), typeof(TextBoxExtensions), new PropertyMetadata(string.Empty));
        public static string GetPlaceholder(TextBox i) => (string)i.GetValue(PlaceholderProperty);
        public static void SetPlaceholder(TextBox i, string value) => i.SetValue(PlaceholderProperty, value);

        #endregion

        static void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var i = sender as TextBox;
            GetHandleSelection(i).Invoke(() =>
            {
                var textBox = (TextBox)sender;
                SetCaretIndex(textBox, textBox.CaretIndex);
                SetSelectionLength(textBox, textBox.SelectionLength);
                SetSelectionStart(textBox, textBox.SelectionStart);
            });
        }

        #region HandleSelection

        internal static readonly DependencyProperty HandleSelectionProperty = DependencyProperty.RegisterAttached("HandleSelection", typeof(Handle), typeof(TextBoxExtensions), new PropertyMetadata(null));
        internal static Handle GetHandleSelection(TextBox i)
        {
            var result = i.GetValue(HandleSelectionProperty) as Handle;
            if (result == null)
            {
                result = false;
                SetHandleSelection(i, result);
            }
            return result;
        }
        internal static void SetHandleSelection(TextBox i, Handle value) => i.SetValue(HandleSelectionProperty, value);

        #endregion

        #region SelectionLength

        public static readonly DependencyProperty SelectionLengthProperty = DependencyProperty.RegisterAttached("SelectionLength", typeof(int), typeof(TextBoxExtensions), new PropertyMetadata(0, OnSelectionLengthChanged));
        public static int GetSelectionLength(TextBox d)
            => (int)d.GetValue(SelectionLengthProperty);
        public static void SetSelectionLength(TextBox d, int value)
            => d.SetValue(SelectionLengthProperty, value);
        static void OnSelectionLengthChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var i = sender as TextBox;
            GetHandleSelection(i).InvokeIfFalse(() =>
            {
                i.SelectionChanged -= OnSelectionChanged;
                i.Select(GetSelectionStart(i), (int)e.NewValue);
                i.Focus();
                i.SelectionChanged += OnSelectionChanged;
            });
        }

        #endregion

        #region SelectionStart

        public static readonly DependencyProperty SelectionStartProperty = DependencyProperty.RegisterAttached("SelectionStart", typeof(int), typeof(TextBoxExtensions), new PropertyMetadata(0, OnSelectionStartChanged));
        public static int GetSelectionStart(TextBox d)
            => (int)d.GetValue(SelectionStartProperty);
        public static void SetSelectionStart(TextBox d, int value)
            => d.SetValue(SelectionStartProperty, value);
        static void OnSelectionStartChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var i = sender as TextBox;
            GetHandleSelection(i).InvokeIfFalse(() =>
            {
                i.SelectionChanged -= OnSelectionChanged;
                i.Select((int)e.NewValue, GetSelectionLength(i));
                i.Focus();
                i.SelectionChanged += OnSelectionChanged;
            });
        }

        #endregion
    }
}