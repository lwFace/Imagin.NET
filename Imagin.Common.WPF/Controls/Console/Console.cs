using Imagin.Common.Analytics;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class Console : UserControl
    {
        #region Properties

        TextBox PART_TextBox;

        //..................................................................

        public readonly List<BaseCommand> Commands = new List<BaseCommand>();

        //..................................................................

        internal Handle HandleFolder = false;

        public static DependencyProperty FolderProperty = DependencyProperty.Register(nameof(Folder), typeof(string), typeof(Console), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, OnFolderChanged));
        public string Folder
        {
            get => (string)GetValue(FolderProperty);
            set => SetValue(FolderProperty, value);
        }
        static void OnFolderChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as Console).OnFolderChanged(new OldNew<string>(e));

        public static DependencyProperty HelpButtonTemplateProperty = DependencyProperty.Register(nameof(HelpButtonTemplate), typeof(DataTemplate), typeof(Console), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate HelpButtonTemplate
        {
            get => (DataTemplate)GetValue(HelpButtonTemplateProperty);
            set => SetValue(HelpButtonTemplateProperty, value);
        }

        public static DependencyProperty HistoryProperty = DependencyProperty.Register(nameof(History), typeof(History), typeof(Console), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public History History
        {
            get => (History)GetValue(HistoryProperty);
            set => SetValue(HistoryProperty, value);
        }

        public static DependencyProperty LineProperty = DependencyProperty.Register(nameof(Line), typeof(string), typeof(Console), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string Line
        {
            get => (string)GetValue(LineProperty);
            set => SetValue(LineProperty, value);
        }

        public static DependencyProperty LinePaddingProperty = DependencyProperty.Register(nameof(LinePadding), typeof(int), typeof(Console), new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.None));
        public int LinePadding
        {
            get => (int)GetValue(LinePaddingProperty);
            set => SetValue(LinePaddingProperty, value);
        }

        public static DependencyProperty FontSizeIntervalProperty = DependencyProperty.Register(nameof(FontSizeInterval), typeof(double), typeof(Console), new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.None));
        public double FontSizeInterval
        {
            get => (double)GetValue(FontSizeIntervalProperty);
            set => SetValue(FontSizeIntervalProperty, value);
        }

        public static DependencyProperty MaximumFontSizeProperty = DependencyProperty.Register(nameof(MaximumFontSize), typeof(double), typeof(Console), new FrameworkPropertyMetadata(36.0, FrameworkPropertyMetadataOptions.None));
        public double MaximumFontSize
        {
            get => (double)GetValue(MaximumFontSizeProperty);
            set => SetValue(MaximumFontSizeProperty, value);
        }

        public static DependencyProperty MinimumFontSizeProperty = DependencyProperty.Register(nameof(MinimumFontSize), typeof(double), typeof(Console), new FrameworkPropertyMetadata(8.0, FrameworkPropertyMetadataOptions.None));
        public double MinimumFontSize
        {
            get => (double)GetValue(MinimumFontSizeProperty);
            set => SetValue(MinimumFontSizeProperty, value);
        }

        public static DependencyProperty OutputProperty = DependencyProperty.Register(nameof(Output), typeof(string), typeof(Console), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string Output
        {
            get => (string)GetValue(OutputProperty);
            set => SetValue(OutputProperty, value);
        }

        public static DependencyProperty OutputBackgroundProperty = DependencyProperty.Register(nameof(OutputBackground), typeof(Brush), typeof(Console), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.None));
        public Brush OutputBackground
        {
            get => (Brush)GetValue(OutputBackgroundProperty);
            set => SetValue(OutputBackgroundProperty, value);
        }

        public static DependencyProperty OutputFontFamilyProperty = DependencyProperty.Register(nameof(OutputFontFamily), typeof(FontFamily), typeof(Console), new FrameworkPropertyMetadata(default(FontFamily), FrameworkPropertyMetadataOptions.None));
        public FontFamily OutputFontFamily
        {
            get => (FontFamily)GetValue(OutputFontFamilyProperty);
            set => SetValue(OutputFontFamilyProperty, value);
        }

        public static DependencyProperty OutputFontSizeProperty = DependencyProperty.Register(nameof(OutputFontSize), typeof(double), typeof(Console), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.None));
        public double OutputFontSize
        {
            get => (double)GetValue(OutputFontSizeProperty);
            set => SetValue(OutputFontSizeProperty, value);
        }

        public static DependencyProperty OutputFontStyleProperty = DependencyProperty.Register(nameof(OutputFontStyle), typeof(FontStyle), typeof(Console), new FrameworkPropertyMetadata(FontStyles.Normal, FrameworkPropertyMetadataOptions.None));
        public FontStyle OutputFontStyle
        {
            get => (FontStyle)GetValue(OutputFontStyleProperty);
            set => SetValue(OutputFontStyleProperty, value);
        }

        public static DependencyProperty OutputForegroundProperty = DependencyProperty.Register(nameof(OutputForeground), typeof(Brush), typeof(Console), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));
        public Brush OutputForeground
        {
            get => (Brush)GetValue(OutputForegroundProperty);
            set => SetValue(OutputForegroundProperty, value);
        }

        public static DependencyProperty OutputTextWrappingProperty = DependencyProperty.Register(nameof(OutputTextWrapping), typeof(TextWrapping), typeof(Console), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.None));
        public TextWrapping OutputTextWrapping
        {
            get => (TextWrapping)GetValue(OutputTextWrappingProperty);
            set => SetValue(OutputTextWrappingProperty, value);
        }

        #endregion

        #region Console

        public Console() : base()
        {
            SetCurrentValue(HistoryProperty, new History(Explorer.DefaultLimit));

            var @namespace = $"{nameof(Imagin)}.{nameof(Common)}.{nameof(Controls)}";

            var result = from type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
            where
            (
                type.IsClass
                &&
                type.Namespace == @namespace
                &&
                type.Inherits<BaseCommand>()
                &&
                !type.IsAbstract
                &&
                type.GetAttribute<HiddenAttribute>()?.Hidden != true
            )
            select type;
            result.ForEach(i =>
            {
                var command = i.Create<BaseCommand>();
                command.Console = this;
                Commands.Add(command);
            });

            OutputFontFamily = new FontFamily("Consolas");
            PreviewMouseWheel += OnPreviewMouseWheel;

            DefaultStyleKey = typeof(Console);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calls <see cref="System.Console.WriteLine"/>.
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLine(string message) => System.Console.WriteLine(message);

        //..................................................................

        void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ModifierKeys.Control.Pressed())
            {
                if (e.Delta > 0)
                {
                    if (OutputFontSize + FontSizeInterval <= MaximumFontSize)
                        OutputFontSize += FontSizeInterval;
                }
                else
                {
                    if (OutputFontSize - FontSizeInterval >= MinimumFontSize)
                        OutputFontSize -= FontSizeInterval;
                }
            }
        }

        //..................................................................

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_TextBox = Template.FindName(nameof(PART_TextBox), this) as TextBox;
        }

        //..................................................................

        ICommand executeCommand;
        public ICommand ProcessCommand
        {
            get
            {
                executeCommand = executeCommand ?? new RelayCommand<string>(i =>
                {
                    Execute(i ?? Line);
                    SetCurrentValue(LineProperty, string.Empty);
                }, 
                i => true);
                return executeCommand;
            }
        }

        //..................................................................

        Result Parse(string line, out BaseCommand command, out string name)
        {
            command = null;

            name = line?.Split(new char[0])?.FirstOrDefault();
            if (name.NullOrEmpty())
                return new Error($"Cannot parse '{line}'.");

            foreach (var i in Commands)
            {
                foreach (var j in i.Names())
                {
                    if (j.ToLower().Equals(name.ToLower()))
                    {
                        command = i;
                        return new Success();
                    }
                }
            }
            return new Error($"Command '{name}' not found.");
        }

        void Execute(string line)
        {
            var result = Parse(line, out BaseCommand command, out string name);
            if (result is Error error)
            {
                Write($"{Folder}> {error.Exception.Message}");
                return;
            }

            try
            {
                command.Execute(line);
            }
            catch (Exception e)
            {
                command.Write($"Command '{name}' failed: {e.Message}");
            }
        }

        //..................................................................

        public void Write(string input)
        {
            StringBuilder result = new StringBuilder();
            result.Append(Output);
            result.AppendLine(input);

            SetCurrentValue(OutputProperty, result.ToString());
            PART_TextBox?.ScrollToEnd();
        }

        public void WriteBlock(string input)
        {
            StringBuilder result = new StringBuilder();

            result.Append(Output);
            result.AppendLine(string.Empty);
            result.AppendLine(input);

            SetCurrentValue(OutputProperty, result.ToString());
            PART_TextBox?.ScrollToEnd();
        }

        //..................................................................

        protected virtual void OnFolderChanged(OldNew<string> input)
        {
            if (!HandleFolder)
            {
                if (Storage.Folder.Long.Exists(input.New))
                    History?.Add(input.New);
            }
        }

        #endregion
    }
}