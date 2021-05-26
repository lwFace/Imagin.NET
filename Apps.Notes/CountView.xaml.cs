using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Notes
{
    public partial class CountView : UserControl
    {
        public struct Values
        {
            public bool Checks;

            public int A;

            public int B;

            public Values(List input)
            {
                Checks = input.Attributes.HasFlag(Attributes.Check);
                A = input.Lines.Count(i => i.Checked);
                B = input.Lines.Count;
            }

            public override string ToString() => Checks ? $"{A}/{B}" : $"{B}";
        }

        public static readonly DependencyProperty CountTemplateProperty = DependencyProperty.Register(nameof(CountTemplate), typeof(DataTemplate), typeof(CountView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
        public DataTemplate CountTemplate
        {
            get => (DataTemplate)GetValue(CountTemplateProperty);
            set => SetValue(CountTemplateProperty, value);
        }

        public static readonly DependencyProperty FileProperty = DependencyProperty.Register(nameof(File), typeof(File), typeof(CountView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnFileChanged));
        public File File
        {
            get => (File)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }
        static void OnFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as CountView).OnFileChanged((File)e.NewValue);

        static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(Values), typeof(CountView), new FrameworkPropertyMetadata(default(Values), FrameworkPropertyMetadataOptions.None));
        public Values Value
        {
            get => (Values)GetValue(ValueProperty);
            private set => SetValue(ValueProperty, value);
        }

        public CountView()
        {
            update = new HandleTask(Update);
            SetCurrentValue(ValueProperty, new Values());
            SetCurrentValue(VisibilityProperty, Visibility.Collapsed);

            Unloaded += OnUnloaded;
            InitializeComponent();
        }

        HandleTask update;
        async Task Update()
        {
            if (Get.Current<MainViewModel>().Documents.First<List>(i => i.Path == File.Path) is List list)
            {
                SetCurrentValue(ValueProperty, new Values(list));
                SetCurrentValue(VisibilityProperty, Visibility.Visible);
            }
            else
            {
                var encoding = Get.Current<Options>().Encoding.Convert();
                var filePath = File.Path;

                List result = null;
                await Task.Run(() => result = List.Open(filePath, encoding));
                if (result != null)
                {
                    SetCurrentValue(ValueProperty, new Values(result));
                    SetCurrentValue(VisibilityProperty, Visibility.Visible);
                }
                else SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
        }

        void OnFileChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => _ = update.Invoke();

        void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (File != null)
                File.PropertyChanged -= OnFileChanged;
        }

        protected virtual void OnFileChanged(File input)
        {
            if (System.IO.Path.GetExtension(input.Path).TrimStart('.') == MainViewModel.FileExtensions[typeof(List)])
            {
                input.PropertyChanged -= OnFileChanged;
                input.PropertyChanged += OnFileChanged;
                _ = update.Invoke();
            }
        }
    }
}