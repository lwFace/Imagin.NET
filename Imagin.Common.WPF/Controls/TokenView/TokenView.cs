using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Imagin.Common.Controls
{
    [ContentProperty(nameof(TokensSource))]
    public class TokenView : RichTextBox
    {
        #region Properties

        bool TextChangeHandled = false;

        bool TokensChangeHandled = false;

        BlockCollection Blocks => Document.Blocks;
        
        Run CurrentRun
        {
            get
            {
                var Paragraph = CaretPosition.Paragraph;
                return Paragraph.Inlines.FirstOrDefault(Inline =>
                {
                    var Run = Inline.As<Run>();
                    var Text = CurrentText;

                    if (Run != null && (Run.Text.StartsWith(Text) || Run.Text.EndsWith(Text)))
                        return true;

                    return false;
                }) as Run;
            }
        }

        /// <summary>
        /// Gets the current input text.
        /// </summary>
        string CurrentText => CaretPosition?.GetTextInRun(LogicalDirection.Backward);

        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(TokenView), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static DependencyProperty TokenDelimiterProperty = DependencyProperty.Register(nameof(TokenDelimiter), typeof(char), typeof(TokenView), new FrameworkPropertyMetadata(';', FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTokenDelimiterChanged));
        /// <summary>
        /// The <see cref="char"/> used to delimit tokens.
        /// </summary>
        public char TokenDelimiter
        {
            get => (char)GetValue(TokenDelimiterProperty);
            set => SetValue(TokenDelimiterProperty, value);
        }
        static void OnTokenDelimiterChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TokenView>().OnTokenDelimiterChanged(new OldNew<char>(e));

        public static DependencyProperty TokenizerProperty = DependencyProperty.Register(nameof(Tokenizer), typeof(ITokenizer), typeof(TokenView), new FrameworkPropertyMetadata(default(ITokenizer), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// The <see cref="ITokenizer"/> that handles tokenizing.
        /// </summary>
        public ITokenizer Tokenizer
        {
            get => (ITokenizer)GetValue(TokenizerProperty);
            set => SetValue(TokenizerProperty, value);
        }

        public static DependencyProperty TokenTriggersProperty = DependencyProperty.Register(nameof(TokenTriggers), typeof(TokenTriggerKey), typeof(TokenView), new FrameworkPropertyMetadata(TokenTriggerKey.Return | TokenTriggerKey.Tab, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Keys used to generate tokens when pressed.
        /// </summary>
        public TokenTriggerKey TokenTriggers
        {
            get => (TokenTriggerKey)GetValue(TokenTriggersProperty);
            set => SetValue(TokenTriggersProperty, value);
        }

        public static DependencyProperty TokenMouseDownActionProperty = DependencyProperty.Register(nameof(TokenMouseDownAction), typeof(TokenMouseAction), typeof(TokenView), new FrameworkPropertyMetadata(TokenMouseAction.Edit, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the action to perform when the mouse is down on token.
        /// </summary>
        public TokenMouseAction TokenMouseDownAction
        {
            get => (TokenMouseAction)GetValue(TokenMouseDownActionProperty);
            set => SetValue(TokenMouseDownActionProperty, value);
        }

        public static DependencyProperty TokensProperty = DependencyProperty.Register(nameof(Tokens), typeof(ObservableCollection<object>), typeof(TokenView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets a collection of all instances of tokens.
        /// </summary>
        public ObservableCollection<object> Tokens
        {
            get => (ObservableCollection<object>)GetValue(TokensProperty);
            private set => SetValue(TokensProperty, value);
        }

        public static DependencyProperty TokensSourceProperty = DependencyProperty.Register(nameof(TokensSource), typeof(string), typeof(TokenView), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTokensSourceChanged));
        /// <summary>
        /// Gets or sets a tokenized string.
        /// </summary>
        public string TokensSource
        {
            get => (string)GetValue(TokensSourceProperty);
            set => SetValue(TokensSourceProperty, value);
        }
        static void OnTokensSourceChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TokenView>().OnTokensSourceChanged(new OldNew<string>(e));

        public static DependencyProperty TokenStyleProperty = DependencyProperty.Register(nameof(TokenStyle), typeof(Style), typeof(TokenView), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTokenStyleChanged));
        public Style TokenStyle
        {
            get => (Style)GetValue(TokenStyleProperty);
            set => SetValue(TokenStyleProperty, value);
        }
        static void OnTokenStyleChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<TokenView>().OnTokenStyleChanged(new OldNew<Style>(e));

        #endregion

        #region TokenView

        public TokenView() : base()
        {
            SetCurrentValue(IsDocumentEnabledProperty, true);
            SetCurrentValue(TokenizerProperty, new StringTokenizer());
            SetCurrentValue(TokensProperty, new ObservableCollection<object>());
        }

        #endregion

        #region Methods

        #region Private

        /// <summary>
        /// For each token, perform action exposing corresponding <see cref="TokenButton"/>.
        /// </summary>
        /// <typeparam name="TButton"></typeparam>
        /// <param name="Action"></param>
        void Enumerate<TButton>(Func<TButton, bool> Action) where TButton : TokenButton
        {
            Enumerate<InlineUIContainer, TButton>((i, j) => Action(j));
        }

        /// <summary>
        /// For each token, perform action exposing corresponding <see cref="TokenButton"/> and <see cref="Inline"/>.
        /// </summary>
        /// <typeparam name="TInline"></typeparam>
        /// <typeparam name="TButton"></typeparam>
        /// <param name="Action"></param>
        void Enumerate<TInline, TButton>(Func<TInline, TButton, bool> Action) where TInline : Inline where TButton : TokenButton
        {
            Enumerate<Paragraph, TInline, TButton>((p, i, b) => Action(i, b));
        }

        /// <summary>
        /// For each token, perform action exposing corresponding <see cref="TokenButton"/>, <see cref="Inline"/>, and <see cref="Paragraph"/>.
        /// </summary>
        /// <typeparam name="TParagraph"></typeparam>
        /// <typeparam name="TInline"></typeparam>
        /// <typeparam name="TButton"></typeparam>
        /// <param name="Action"></param>
        void Enumerate<TParagraph, TInline, TButton>(Func<TParagraph, TInline, TButton, bool> Action) where TParagraph : Paragraph where TInline : Inline where TButton : TokenButton
        {
            foreach (var i in Blocks)
            {
                if (i is TParagraph)
                {
                    var Continue = true;
                    foreach (var j in i.As<TParagraph>().Inlines)
                    {
                        if (j is TInline)
                        {
                            if (!Action(i as TParagraph, j as TInline, j.As<InlineUIContainer>()?.Child?.As<TButton>() ?? default(TButton)))
                                Continue = false;
                        }
                        if (!Continue)
                            break;
                    }
                    if (!Continue)
                        break;
                }
            }
        }

        /// <summary>
        /// Generates an <see cref="Inline"/> element to host the given token.
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        InlineUIContainer GenerateInline(object Token)
        {
            OnTokenLoaded(Token);

            var Button = new TokenButton(Token);

            //BaselineAlignment is needed to align with run
            var Result = new InlineUIContainer(Button)
            {
                BaselineAlignment = BaselineAlignment.Center
            };

            return Result;
        }

        /// <summary>
        /// Generates a <see cref="Run"/> to host the <see cref="string"/> representation of the given token.
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        Run GenerateRun(object Token)
        {
            OnTokenUnloaded(Token);
            return new Run(Tokenizer.ToString(Token));
        }

        string GetTokensSource()
        {
            var Result = new StringBuilder();
            Enumerate<Inline, TokenButton>((i, b) =>
            {
                if (i is InlineUIContainer)
                {
                    Result.Append("{0}{1}".F(Tokenizer.ToString(b?.Content), TokenDelimiter));
                }
                else if (i is Run)
                    Result.Append(i.As<Run>().Text);

                return true;
            });
            return Result.ToString();
        }

        void IntersectTokens()
        {
            var ActualTokens = new List<object>();
            Enumerate<TokenButton>((b) =>
            {
                ActualTokens.Add(b.Content);
                return true;
            });

            var Result = Tokens.Intersect(ActualTokens).ToList();

            Tokens.Clear();
            while (Result.Any<object>())
            {
                foreach (var i in Result)
                {
                    Result.Remove(i);
                    Tokens.Add(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Replaces the given input text with the given token.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Token"></param>
        void ReplaceWithToken(string Text, object Token)
        {
            var Paragraph = CaretPosition.Paragraph;

            var currentRun = CurrentRun;
            if (currentRun != null)
            {
                Paragraph.Inlines.InsertBefore(currentRun, GenerateInline(Token));

                if (currentRun.Text == Text)
                {
                    Paragraph.Inlines.Remove(currentRun);
                }
                else
                {
                    var Tail = new Run(currentRun.Text.Substring(currentRun.Text.IndexOf(Text) + Text.Length));
                    Paragraph.Inlines.InsertAfter(currentRun, Tail);
                    Paragraph.Inlines.Remove(currentRun);
                }
            }
        }

        #endregion

        #region Overrides

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            var Field = e.Key.ToString();
            if (e.Key.ToChar() == TokenDelimiter)
            {
                OnTokenTriggered();
                e.Handled = true;
                return;
            }

            if (Field.TryParse(out TokenTriggerKey TriggerKey) && TokenTriggers.HasAll(TriggerKey))
            {
                OnTokenTriggered(); 
                e.Handled = true;
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            var button = e.Source as TokenButton;

            if (button == null) return;

            switch (TokenMouseDownAction)
            {
                case TokenMouseAction.Edit:
                    EditToken(button);
                    e.Handled = true;
                    break;
                case TokenMouseAction.Remove:
                    RemoveToken(button);
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Occurs when the current text changes.
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (!TextChangeHandled)
            {
                await Dispatcher.BeginInvoke(() => IntersectTokens());

                TokensChangeHandled = true;
                TokensSource = GetTokensSource();
                TokensChangeHandled = false;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Converts the element that hosts the given token to a <see cref="Run"/>.
        /// </summary>
        /// <param name="Token"></param>
        public void EditToken(object Token)
        {
            var Inline = default(Inline);
            Enumerate<Paragraph, InlineUIContainer, TokenButton>((p, i, b) =>
            {
                if (b?.Content == Token)
                {
                    Inline = GenerateRun(Token);

                    p.Inlines.InsertBefore(i, Inline);
                    p.Inlines.Remove(i);

                    return false;
                }
                return true;
            });
            Focus();

            if (Inline != null)
                Selection.Select(Inline.ContentStart, Inline.ContentEnd);
        }

        /// <summary>
        /// Converts the given token element to a <see cref="Run"/>.
        /// </summary>
        /// <param name="Button"></param>
        public void EditToken(TokenButton Button)
        {
            var Inline = default(Inline);
            Enumerate<Paragraph, InlineUIContainer, TokenButton>((p, i, b) =>
            {
                if (b == Button)
                {
                    Inline = GenerateRun(b.Content);

                    p.Inlines.InsertBefore(i, Inline);
                    p.Inlines.Remove(i);

                    return false;
                }
                return true;
            });
            Focus();

            if (Inline != null)
                Selection.Select(Inline.ContentStart, Inline.ContentEnd);
        }

        /// <summary>
        /// Removes the <see cref="Inline"/> that hosts the <see cref="TokenButton"/> corresponding to the given token.
        /// </summary>
        /// <param name="Token"></param>
        public void RemoveToken(object Token)
        {
            Enumerate<Paragraph, InlineUIContainer, TokenButton>((p, i, b) =>
            {
                if (b?.Content == Token)
                {
                    p.Inlines.Remove(i);
                    return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Removes the <see cref="Inline"/> that hosts the given <see cref="TokenButton"/>.
        /// </summary>
        /// <param name="Button"></param>
        public void RemoveToken(TokenButton Button)
        {
            Enumerate<Paragraph, InlineUIContainer, TokenButton>((p, i, b) =>
            {
                if (b == Button)
                {
                    p.Inlines.Remove(i);
                    return false;
                }
                return true;
            });
        }

        #endregion

        #region Virtual

        /// <summary>
        /// Occurs when <see cref="TokenDelimiter"/> property changes.
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected virtual void OnTokenDelimiterChanged(OldNew<char> input) => SetCurrentValue(TokensSourceProperty, TokensSource.Replace(input.Old, input.New));

        /// <summary>
        /// Occurs when a token has been loaded (or added both logically and visually).
        /// </summary>
        /// <param name="token"></param>
        protected virtual void OnTokenLoaded(object token)
        {
            Tokens.Add(token);
        }

        /// <summary>
        /// Occurs when a token has been unloaded (or removed both logically and visually).
        /// </summary>
        /// <param name="token"></param>
        protected virtual void OnTokenUnloaded(object token)
        {
            Tokens.Remove(token);
        }
        
        /// <summary>
        /// Occurs when <see cref="TokensSource"/> property changes.
        /// </summary>
        protected virtual async void OnTokensSourceChanged(OldNew<string> input)
        {
            if (!TokensChangeHandled)
            {
                TextChangeHandled = true;

                await Dispatcher.BeginInvoke(() =>
                {
                    if (input.New != null)
                    {
                        var d = TokenDelimiter.ToString();

                        //Check to see if delimiter occurs more than once in any one place.
                        var Temp = Regex.Replace(input.New, d + "+", d);

                        //If so, correct it
                        if (Temp != input.New)
                        {
                            TokensChangeHandled = true;
                            TokensSource = Temp;
                            TokensChangeHandled = false;
                        }
                    }

                    Tokens.Clear();
                    Blocks.Clear();

                    if (input.New?.ToString().Empty() == false)
                    {
                        var p = new Paragraph();
                        Tokenizer?.Tokenize(input.New, TokenDelimiter)?.ForEach(Token => p.Inlines.Add(GenerateInline(Token)));
                        Blocks.Add(p);
                    }
                });

                TextChangeHandled = false;
            }
        }

        /// <summary>
        /// Occurs when <see cref="TokenStyle"/> property changes; if the style is null, the global style for <see cref="TokenButton"/> (if present) is used instead.
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected virtual void OnTokenStyleChanged(OldNew<Style> input)
        {
            if (input.Old != null)
                Resources.Remove(input.Old.TargetType);

            if (input.New != null)
                Resources.Add(input.New.TargetType, input.New);
        }

        /// <summary>
        /// Occurs when some event triggers the creation of a token.
        /// </summary>
        protected virtual void OnTokenTriggered()
        {
            if (!TextChangeHandled)
            {
                var currentText = CurrentText;

                //Attempt to get a token from the current text
                var Token = Tokenizer?.ParseToken(currentText);

                //If a token was created, replace the current text with it
                if (Token != null)
                {
                    TextChangeHandled = true;
                    ReplaceWithToken(currentText, Token);
                    TextChangeHandled = false;
                }

                //Ensure source reflects changes
                TokensChangeHandled = true;
                TokensSource = GetTokensSource();
                TokensChangeHandled = false;
            }
        }

        #endregion

        #endregion
    }
}