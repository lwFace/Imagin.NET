using Imagin.Common.Analytics;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Web;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// Updates a WPF <see cref="Application"/> using a local <see cref="AppManifest"/> and a remote <see cref="AppManifest"/>.
    /// </summary>
    public sealed class Updater : Base
    {
        #region Properties

        #region Events

        public event EventHandler<EventArgs<Result>> Checked;

        public event EventHandler<EventArgs<Error>> CheckFailed;
        
        public event EventHandler<EventArgs> Downloaded;

        public event EventHandler<EventArgs<Error>> DownloadFailed;

        public event EventHandler<EventArgs<Error>> InstallFailed;

        public event EventHandler<EventArgs> Installed;
        
        public event EventHandler<EventArgs> VersionCurrent;

        public event EventHandler<EventArgs> VersionNewer;

        public event EventHandler<EventArgs> VersionOlder;

        #endregion

        #region Private

        readonly string localManifestPath = string.Empty;

        readonly string localWorkPath;

        readonly Uri manifestPrototype = default(Uri);

        readonly string remoteManifestPath = string.Empty;

        readonly string remoteSourcePath;

        Timer timer = new Timer();

        #endregion

        #region Public

        public const double DefaultInterval = 60 * 60 * 1000;

        double interval = DefaultInterval;
        /// <summary>
        /// The interval (in milliseconds) between checking for updates (default = 60 * 60 seconds = 60 minutes = 1 hour).
        /// </summary>
        public double Interval
        {
            get => timer.Interval;
            set
            {
                this.Change(ref interval, value);

                if (isMonitoring)
                    StopMonitoring();

                timer.Interval = interval;

                if (isMonitoring)
                    StartMonitoring();
            }
        }

        bool isChecking = false;
        /// <summary>
        /// Whether or not we're currently checking for updates.
        /// </summary>
        public bool IsChecking
        {
            get => isChecking;
            private set => isChecking = value;
        }

        bool isMonitoring = false;
        /// <summary>
        /// Whether or not we can monitor (or check every so often) for updates.
        /// </summary>
        public bool IsMonitoring
        {
            get => isMonitoring;
            private set => this.Change(ref isMonitoring, value);
        }

        bool isMonitoringEnabled = false;
        /// <summary>
        /// Whether or not we can monitor (or check every so often) for updates.
        /// </summary>
        public bool IsMonitoringEnabled
        {
            get => isMonitoringEnabled;
            set
            {
                if (isMonitoring && !value)
                {
                    StopMonitoring();
                }
                else if (!isMonitoring && value)
                    StartMonitoring();

                this.Change(ref isMonitoringEnabled, value);
            }
        }

        #endregion

        #endregion

        #region Updater

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ManifestPrototype"></param>
        /// <param name="LocalPath">The path to the local manifest.</param>
        /// <param name="RemotePath">The path to the remote manifest.</param>
        /// <param name="RemoteSourcePath">The remote path to download files from.</param>
        public Updater(Uri manifestPrototype, string localManifestPath, string remoteManifestPath, string remoteSourcePath)
        {
            this.manifestPrototype = manifestPrototype;

            this.localManifestPath = localManifestPath;
            localWorkPath = $@"{Path.GetDirectoryName(this.localManifestPath)}\Update";

            this.remoteManifestPath = remoteManifestPath;
            this.remoteSourcePath = remoteSourcePath;

            timer.Elapsed += OnMonitoring;
        }

        #endregion

        #region Methods

        #region Private

        async Task<Result> DownloadAsync()
        {
            var download = default(Result);
            var downloader = new Downloader(remoteSourcePath, localWorkPath);

            var result = await Dialog.Show("Update", downloader, DialogImage.Globe, async i => download = await downloader.DownloadAsync(), DialogButtons.Cancel);
            switch (result)
            {
                case -1:
                    //The dialog was closed
                    break;
                case 0:
                    //The download was cancelled
                    break;
                case 1:
                    //The download was cancelled
                    break;
            }

            if (download is Error)
            {
                OnDownloadFailed(download as Error);
            }
            else OnDownloaded();

            return download;
        }

        async Task InstallAsync(AppManifestPair Manifests)
        {
            var Path = string.Concat(localWorkPath, @"\", System.IO.Path.GetFileName(remoteSourcePath));

            if (!File.Exists(Path))
            {
                OnInstallFailed(new Error(new FileNotFoundException("Installer file cannot be found.")));
            }
            else
            {
                await RestoreLocalManifestAsync(Manifests.Local.Path, manifestPrototype, Manifests.Remote.Version);

                try
                {
                    Storage.File.Long.Open(Path);
                }
                catch (Exception e)
                {
                    OnInstallFailed(new Error(new FileNotFoundException($"Unable to run installer file: {e.Message}")));
                    return;
                }

                OnInstalled();
                Environment.Exit(0);
            }
        }

        //---------------------------------------------------------

        async Task RestoreLocalManifestAsync(AppManifest LocalManifest, string Message)
        {
            var result = Dialog.Show("Update", "{0} Would you like to restore the local manifest?".F(Message), DialogImage.Warning, DialogButtons.YesNo);

            if (result == 0)
            {
                if (await RestoreLocalManifestAsync(LocalManifest.Path, manifestPrototype))
                {
                    await CheckAsync();
                }
                else OnCheckFailed(new Error(new FileLoadException("Manifest creation failed.")));
            }
        }

        async Task<Result> RestoreLocalManifestAsync(string Path, Uri ResourceUri, Version NewVersion = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Storage.File.Long.Delete(Path);
                }
                catch { }
                SaveManifest(Path, ResourceUri, NewVersion);

                return new Success();
            });
        }

        void SaveManifest(string Path, Uri ResourceUri)
        {
            SaveManifest(Path, ResourceUri, default(Version));
        }

        void SaveManifest(string Path, Uri ResourceUri, Version NewVersion)
        {
            using (var Resource = Application.GetResourceStream(ResourceUri).Stream)
            {
                if (Resource == null)
                    throw new InvalidDataException("Resource is null");

                if (NewVersion != null)
                {
                    using (var Reader = new StreamReader(Resource, Encoding.UTF8))
                    {
                        var Xml = XDocument.Parse(Reader.ReadToEnd());
                        if (Xml.Root.Name.LocalName != "App")
                            return;
                        Xml.Root.Element("Version").Value = NewVersion.ToString();
                        System.IO.File.WriteAllText(Path, Xml.ToString(), Encoding.ASCII);
                    }
                }
                else
                {
                    using (Stream Output = System.IO.File.OpenWrite(Path))
                        Resource.CopyTo(Output);
                }
            }
        }

        //---------------------------------------------------------

        async void OnChecked(Result<AppManifestPair> result)
        {
            Checked?.Invoke(this, new EventArgs<Result>(result));

            if (result)
            {
                Version a = result.Data.Local.Version, b = result.Data.Remote.Version;

                if (a == b)
                {
                    OnVersionCurrent(result.Data);
                }
                else if (a < b)
                {
                    OnVersionOlder(result.Data);
                }
                else if (a > b)
                    OnVersionNewer(result.Data);
            }
            else
            {
                var error = result as Error<AppManifestPair>;

                if (error.Exception is ManifestNotFoundException)
                {
                    var f = error.Exception as ManifestFormatException;

                    if (f.IsRemote)
                    {
                        OnCheckFailed(new Error(error.Exception));
                    }
                    else await RestoreLocalManifestAsync(result.Data.Local, f.Message);
                }
                else if (error.Exception is ManifestFormatException)
                {
                    var f = error.Exception as ManifestFormatException;

                    if (f.IsRemote)
                    {
                        OnCheckFailed(new Error(error.Exception));
                    }
                    else await RestoreLocalManifestAsync(result.Data.Local, f.Message);
                }
                else OnCheckFailed(new Error(error.Exception));
            }
        }

        void OnCheckFailed(Error Error)
        {
            Dialog.Show("Update", Error.Message, DialogImage.Error, DialogButtons.Done);
            CheckFailed?.Invoke(this, new EventArgs<Error>(Error));
        }
        
        void OnDownloaded()
        {
            Downloaded?.Invoke(this, new EventArgs());
        }

        void OnDownloadFailed(Error Error)
        {
            Dialog.Show("Update", Error.Message, DialogImage.Error, DialogButtons.Done);
            DownloadFailed?.Invoke(this, new EventArgs<Error>(Error));
        }

        void OnInstallFailed(Error Error)
        {
            InstallFailed?.Invoke(this, new EventArgs<Error>(Error));
        }

        void OnInstalled()
        {
            Installed?.Invoke(this, new EventArgs());
        }

        async void OnMonitoring(object sender, ElapsedEventArgs e)
        {
            var result = await CheckAsync(null);
            OnChecked(result);
        }

        //---------------------------------------------------------

        /// <summary>
        /// Occurs when the local version is newer than the remote version.
        /// </summary>
        void OnVersionNewer(AppManifestPair manifests)
        {
            Dialog.Show("Update", "Your version ({0}) is invalid.".F(manifests.Local.Version), DialogImage.Error, DialogButtons.Done);
            VersionNewer?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when the local version is older than the remote version.
        /// </summary>
        async void OnVersionOlder(AppManifestPair manifests)
        {
            VersionOlder?.Invoke(this, new EventArgs());

            var result = Dialog.Show("Update", "A new version is available ({0}).".F(manifests.Remote.Version), default(Uri), new DialogButton("Download & Install", 0), new DialogButton("Download", 1), new DialogButton("Cancel", 2));

            var download = default(Result);

            if (result == 0 || result == 1)
                download = await DownloadAsync();

            if (result == 1 && download is Success)
                await InstallAsync(manifests);
        }

        /// <summary>
        /// Occurs when the local version is the same as the remote version.
        /// </summary>
        void OnVersionCurrent(AppManifestPair Manifests)
        {
            Dialog.Show("Update", "You have the latest version ({0}).".F(Manifests.Local.Version), DialogImage.Success, DialogButtons.Done);
            VersionCurrent?.Invoke(this, new EventArgs());
        }

        //---------------------------------------------------------

        void StartMonitoring()
        {
            IsMonitoring = true;
            timer.Start();
        }

        void StopMonitoring()
        {
            timer.Stop();
            IsMonitoring = false;
        }

        #endregion

        #region Public

        public async Task<Result> CheckAsync()
        {
            var result = default(Result<AppManifestPair>);

            var text = new StackPanel();
            text.Children.Add(new TextBlock() { Text = "Checking for updates..." });
            text.Children.Add(new Controls.ProgressBar() { IsIndeterminate = true });

            await Dialog.Show("Update", text, null, async i => result = await CheckAsync(null), DialogButtons.Cancel);
            OnChecked(result);

            return result;
        }

        public async Task<Result<AppManifestPair>> CheckAsync(Action OnChecked)
        {
            var result = default(Result<AppManifestPair>);
            IsChecking = true;

            var localManifest = new AppManifest(localManifestPath);
            var remoteManifest = new AppManifest(remoteManifestPath);

            await Task.Run(() =>
            {
                try
                {
                    if (!new FileInfo(localManifest.Path).Exists)
                        throw new ManifestNotFoundException(false, "Local manifest doesn't exist.");

                    if (!localManifest.Load(File.ReadAllText(localManifest.Path)))
                        throw new ManifestFormatException(false, "Local manifest is invalid.");

                    var remoteUri = new Uri(remoteManifest.Path);

                    var http = new Fetch
                    {
                        Retries = 5,
                        Timeout = 30000
                    };
                    http.Load(remoteUri.AbsoluteUri);

                    if (!http.Success)
                        throw new ManifestNotFoundException(true, "Remote app manifest is unavailable.");

                    if (!remoteManifest.Load(Encoding.UTF8.GetString(http.ResponseData)))
                        throw new ManifestFormatException(true, "Remote app manifest is unavailable.");

                    if (localManifest.SecurityToken != remoteManifest.SecurityToken)
                        throw new InvalidTokenException("Security tokens do not match.");
                }
                catch (Exception e)
                {
                    result = new Error<AppManifestPair>(e, new AppManifestPair(localManifest, remoteManifest));
                }

                result = new Success<AppManifestPair>(new AppManifestPair(localManifest, remoteManifest));
            });

            OnChecked?.Invoke();

            IsChecking = false;
            return result;
        }

        #endregion

        #endregion
    }
}