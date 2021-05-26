using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public partial class Downloader : UserControl
    {
        #region Properties

        Stopwatch stopwatch = new Stopwatch();

        public event EventHandler<EventArgs> Downloaded;

        public static DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(string), typeof(Downloader), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static DependencyProperty DestinationProperty = DependencyProperty.Register(nameof(Destination), typeof(string), typeof(Downloader), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
        public string Destination
        {
            get => (string)GetValue(DestinationProperty);
            set => SetValue(DestinationProperty, value);
        }

        public static DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(double), typeof(Downloader), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.None));
        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        public static DependencyProperty SpeedProperty = DependencyProperty.Register(nameof(Speed), typeof(double), typeof(Downloader), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.None));
        public double Speed
        {
            get => (double)GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
        }

        public static DependencyProperty ProcessedProperty = DependencyProperty.Register(nameof(Processed), typeof(string), typeof(Downloader), new FrameworkPropertyMetadata("0 MB / 0 MB", FrameworkPropertyMetadataOptions.None));
        public string Processed
        {
            get => (string)GetValue(ProcessedProperty);
            set => SetValue(ProcessedProperty, value);
        }

        public static DependencyProperty RemainingProperty = DependencyProperty.Register(nameof(Remaining), typeof(long), typeof(Downloader), new FrameworkPropertyMetadata(0L, FrameworkPropertyMetadataOptions.None));
        public long Remaining
        {
            get => (long)GetValue(RemainingProperty);
            set => SetValue(RemainingProperty, value);
        }

        #endregion

        #region Downloader

        public Downloader()
        {
            InitializeComponent();
        }

        public Downloader(string source, string destination) : this()
        {
            SetCurrentValue(SourceProperty, source);
            SetCurrentValue(DestinationProperty, destination);
        }

        #endregion

        #region Methods

        public async Task<Result> DownloadAsync()
        {
            Result result = null;

            Stopwatch stopwatch = this.stopwatch;

            using (WebClient client = new WebClient())
            {
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnDownloadProgressChanged);

                Uri uri = Source.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(Source) : new Uri("http://" + Source);
                await Task.Run(new Action(() =>
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(Destination);
                    }
                    catch (Exception e)
                    {
                        result = new Error($"Download failed: {e.Message}");
                        return;
                    }
                }));
                try
                {
                    stopwatch.Start();
                    await client.DownloadFileTaskAsync(uri, string.Concat(Destination, @"\", System.IO.Path.GetFileName(Source)));
                    stopwatch.Reset();
                    result = new Success();
                }
                catch (Exception e)
                {
                    result = new Error($"Download failed: {e.Message}");
                }
            }
            return result;
        }

        /// <summary>
        /// The event that will fire whenever the progress of the WebClient is changed
        /// </summary>
        void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Speed = System.Math.Round(e.BytesReceived / 1024d / stopwatch.Elapsed.TotalSeconds, 3); 
            Progress = Convert.ToDouble(e.ProgressPercentage);
            Processed = $"{(e.BytesReceived / 1024d / 1024d).ToString("0.00")} MB / {(e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00")} MB"; 
            Remaining = stopwatch.Elapsed.Left(e.TotalBytesToReceive, e.BytesReceived).TotalSeconds.Int64();
        }

        /// <summary>
        ///  The event that will trigger when the WebClient is completed. At this point, our required payloads (or zips) have been downloaded. Now we must extract them.
        /// </summary>
        void OnDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Downloaded?.Invoke(this, new EventArgs());
        }

        #endregion
    }
}