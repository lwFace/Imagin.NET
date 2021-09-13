using ImageMagick;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Imagin.Common.Controls
{
    public class Slideshow : UserControl
    {
        public enum Types
        {
            None,
            File,
            Folder
        }

        #region Properties

        DispatcherTimer timer;

        readonly Storage.ItemCollection items = new Storage.ItemCollection(string.Empty, new Filter(ItemType.File));

        public static DependencyProperty BackgroundBlurProperty = DependencyProperty.Register(nameof(BackgroundBlur), typeof(bool), typeof(Slideshow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool BackgroundBlur
        {
            get => (bool)GetValue(BackgroundBlurProperty);
            set => SetValue(BackgroundBlurProperty, value);
        }

        public static DependencyProperty BackgroundBlurRadiusProperty = DependencyProperty.Register(nameof(BackgroundBlurRadius), typeof(double), typeof(Slideshow), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.None));
        public double BackgroundBlurRadius
        {
            get => (double)GetValue(BackgroundBlurRadiusProperty);
            set => SetValue(BackgroundBlurRadiusProperty, value);
        }

        public static DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register(nameof(BackgroundOpacity), typeof(double), typeof(Slideshow), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None));
        public double BackgroundOpacity
        {
            get => (double)GetValue(BackgroundOpacityProperty);
            set => SetValue(BackgroundOpacityProperty, value);
        }

        public static DependencyProperty IntervalProperty = DependencyProperty.Register(nameof(Interval), typeof(TimeSpan), typeof(Slideshow), new FrameworkPropertyMetadata(TimeSpan.FromSeconds(3), FrameworkPropertyMetadataOptions.None, OnIntervalChanged));
        public TimeSpan Interval
        {
            get => (TimeSpan)GetValue(IntervalProperty);
            set => SetValue(IntervalProperty, value);
        }
        static void OnIntervalChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<Slideshow>().OnIntervalChanged(new OldNew<TimeSpan>(e));

        public static DependencyProperty PauseOnMouseOverProperty = DependencyProperty.Register(nameof(PauseOnMouseOver), typeof(bool), typeof(Slideshow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));
        public bool PauseOnMouseOver
        {
            get => (bool)GetValue(PauseOnMouseOverProperty);
            set => SetValue(PauseOnMouseOverProperty, value);
        }

        public static DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(Slideshow), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, OnPathChanged));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static void OnPathChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<Slideshow>().OnPathChanged(new OldNew<string>(e));

        public static DependencyProperty ScalingModeProperty = DependencyProperty.Register(nameof(ScalingMode), typeof(BitmapScalingMode), typeof(Slideshow), new FrameworkPropertyMetadata(BitmapScalingMode.HighQuality, FrameworkPropertyMetadataOptions.None));
        public BitmapScalingMode ScalingMode
        {
            get => (BitmapScalingMode)GetValue(ScalingModeProperty);
            set => SetValue(ScalingModeProperty, value);
        }

        public static DependencyProperty SelectedImageProperty = DependencyProperty.Register(nameof(SelectedImage), typeof(string), typeof(Slideshow), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.None, OnSelectedImageChanged));
        public string SelectedImage
        {
            get => (string)GetValue(SelectedImageProperty); 
            private set => SetValue(SelectedImageProperty, value); 
        }
        static void OnSelectedImageChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<Slideshow>().OnSelectedImageChanged(new OldNew<string>(e));

        public static DependencyProperty SelectedImageSourceProperty = DependencyProperty.Register(nameof(SelectedImageSource), typeof(ImageSource), typeof(Slideshow), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.None));
        public ImageSource SelectedImageSource
        {
            get => (ImageSource)GetValue(SelectedImageSourceProperty);
            private set => SetValue(SelectedImageSourceProperty, value);
        }

        public static DependencyProperty TransitionProperty = DependencyProperty.Register(nameof(Transition), typeof(Transitions), typeof(Slideshow), new FrameworkPropertyMetadata(Transitions.LeftReplace, FrameworkPropertyMetadataOptions.None));
        public Transitions Transition
        {
            get => (Transitions)GetValue(TransitionProperty);
            set => SetValue(TransitionProperty, value);
        }

        public static DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(Types), typeof(Slideshow), new FrameworkPropertyMetadata(Types.None, FrameworkPropertyMetadataOptions.None));
        public Types Type
        {
            get => (Types)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        #endregion

        #region Slideshow

        public Slideshow()
        {
            DefaultStyleKey = typeof(Slideshow);

            timer = new DispatcherTimer();
            timer.Interval = Interval;
            timer.Tick += OnTick;
        }

        #endregion

        #region Methods

        void OnTick(object sender, object e)
        {
            if (Type != Types.Folder)
            {
                timer.Stop();
                return;
            }

            NextCommand.Execute(null);
        }

        /// -----------------------------------------------------------------------------

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (PauseOnMouseOver)
                timer.Stop();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (PauseOnMouseOver)
                timer.Start();
        }

        /// -----------------------------------------------------------------------------

        protected virtual void OnIntervalChanged(OldNew<TimeSpan> input)
        {
            timer.Interval = input.New.Coerce(TimeSpan.MaxValue, TimeSpan.FromSeconds(3));
        }

        protected virtual void OnPathChanged(OldNew<string> input)
        {
            timer.Stop();
            if (Storage.File.Long.Exists(input.New))
            {
                SetCurrentValue(TypeProperty, Types.File);
                //Single image (file)
                SetCurrentValue(SelectedImageProperty, input.New);
            }
            else if (Folder.Long.Exists(input.New))
            {
                SetCurrentValue(TypeProperty, Types.Folder);
                //Multiple images (folder)
                _ = items.RefreshAsync(input.New);
                timer.Start();
            }
            else SetCurrentValue(TypeProperty, Types.None);
        }

        protected async virtual void OnSelectedImageChanged(OldNew<string> input)
        {
            var result = await Open(input.New);
            SetCurrentValue(SelectedImageSourceProperty, result);
        }

        /// -----------------------------------------------------------------------------

        async Task<BitmapSource> Open(string filePath)
        {
            var result = await Task.Run(() =>
            {
                try
                {
                    return new MagickImage(filePath);
                }
                catch (Exception)
                {
                    return null;
                }
            });
            return result?.ToBitmapSource();
        }

        /// -----------------------------------------------------------------------------

        void Next()
        {
            if (items.Count <= 1)
                return;

            var item = items.FirstOrDefault(i => i.Path == SelectedImage) ?? items[0];
            var index = items.IndexOf(item);

            index++;
            index = index > items.Count - 1 ? 0 : index;

            SetCurrentValue(SelectedImageProperty, items[index].Path);
        }

        void Previous()
        {
            if (items.Count <= 1)
                return;

            var item = items.FirstOrDefault(i => i.Path == SelectedImage) ?? items[0];
            var index = items.IndexOf(item);

            index--;
            index = index < 0 ? items.Count - 1 : index;

            SetCurrentValue(SelectedImageProperty, items[index].Path);
        }

        ICommand nextCommand;
        public ICommand NextCommand
        {
            get
            {
                nextCommand = nextCommand ?? new RelayCommand(Next, () => Type == Types.Folder && items.Count > 1);
                return nextCommand;
            }
        }

        ICommand previousCommand;
        public ICommand PreviousCommand
        {
            get
            {
                previousCommand = previousCommand ?? new RelayCommand(Previous, () => Type == Types.Folder && items.Count > 1);
                return previousCommand;
            }
        }

        ICommand speedCommand;
        public ICommand SpeedCommand
        {
            get
            {
                speedCommand = speedCommand ?? new RelayCommand(() =>
                {
                    /*
                    var dialog = Imagin.Common.Dialog.ShowInput("Speed", "Enter speed in seconds", Interval.TotalSeconds.ToString(), default(Uri), DialogButtons.Done);

                    double seconds;
                    double.TryParse(dialog.Input, out seconds);
                    SetCurrentValue(IntervalProperty, TimeSpan.FromSeconds(seconds));
                    */
                }, () => true);
                return speedCommand;
            }
        }

        #endregion
    }
}