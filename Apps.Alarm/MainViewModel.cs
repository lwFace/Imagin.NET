using Alarm.Linq;
using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Models;
using Imagin.Common.Storage;
using Imagin.Common.Threading;
using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Alarm
{
    public class MainViewModel : MainViewModel<MainWindow>
    {
        #region Properties

        readonly ObservableCollection<DateTime> alarms = new ObservableCollection<DateTime>();

        readonly BackgroundQueue queue = new BackgroundQueue();

        readonly Timer timer = new Timer() { Interval = 500 };

        //----------------------------------------------------------------
        
        bool viewOptions = false;
        public bool ViewOptions
        {
            get => viewOptions;
            set => this.Change(ref viewOptions, value);
        }

        //----------------------------------------------------------------

        Media media = null;
        public Media Media
        {
            get => media;
            set => this.Change(ref media, value);
        }

        public bool CanPlay 
            => !set && !media.Playing;

        public bool CanStop 
            => !set && media.Playing;

        //----------------------------------------------------------------

        public PathCollection Sounds { get; private set; } = new PathCollection(new Filter(ItemType.File, Media.Formats.ToArray()));

        //----------------------------------------------------------------

        public string MathDifficulty
        {
            get
            {
                var result = $"{(MathParser.Difficulties)Get.Current<Options>().MathDifficulty.Int32()}";
                return result == nameof(MathParser.Difficulties.None) ? "Disable" : result;
            }
        }

        //----------------------------------------------------------------

        public int Count
        {
            get
            {
                var result = 0;
                var interval = Interval();

                var j = interval.From;
                while (j <= interval.To)
                {
                    result++;
                    j = j.AddSeconds(Get.Current<Options>().Increment * 60d);
                }
                return result;
            }
        }

        public string Difference
            => (Set ? ((Next() ?? DateTime.Now) - DateTime.Now) : From - DateTime.Now).ShortTime(false);

        public DateTime From
            => Interval().From;

        public DateTime To
            => Interval().To;

        public string NextAlarm
        {
            get
            {
                var next = Next();
                if (next != null)
                    return $"Next alarm starts {next.Value.Relative("in")}";

                return $"Unable to determine next alarm";
            }
        }

        //----------------------------------------------------------------

        bool bypass = false;

        bool set = false;
        public bool Set
        {
            get => set;
            set
            {
                if (!bypass)
                {
                    if (!value)
                    {
                        if (!Solve())
                        {
                            return;
                        }
                    }
                    if (value)
                    {
                        if (!Imagin.Common.Storage.File.Long.Exists(Get.Current<Options>().AudioFilePath))
                        {
                            return;
                        }
                    }
                }

                this.As<IPropertyChanged>().Change(ref set, value);
                OnSet();
            }
        }

        #endregion

        #region MainViewModel

        public MainViewModel() : base()
        {
            media = new Media();
            media.Failed += OnMediaFailed;
            Sounds.Refresh(Get.Current<Options>().AudioFolderPath);

            timer.Elapsed += OnUpdate;
            timer.Start();
        }

        #endregion

        #region Methods

        #region Commands

        ICommand aboutCommand;
        public ICommand AboutCommand => aboutCommand = aboutCommand ?? new RelayCommand(() => new AboutWindow().ShowDialog());

        ICommand playCommand;
        public ICommand PlayCommand => playCommand = playCommand ?? new RelayCommand(() => media.Play());

        ICommand stopCommand;
        public ICommand StopCommand => stopCommand = stopCommand ?? new RelayCommand(() => media.Stop());

        ICommand snoozeCommand;
        public ICommand SnoozeCommand => snoozeCommand = snoozeCommand ?? new RelayCommand(() => Snooze(), () => Set);

        #endregion

        #region Other

        Interval Interval()
        {
            DateTime from = DateTime.Now.Date.AddSeconds(Get.Current<Options>().StartTime.TotalSeconds), to = DateTime.Now.Date.AddSeconds(Get.Current<Options>().EndTime.TotalSeconds);
            //If the time has already passed today, look at the next day
            if (DateTime.Now > from)
            {
                from = from.AddDays(1);
                to = to.AddDays(1);

                if (to < from)
                {
                    to = to.AddDays(1);
                }
            }
            else if (DateTime.Now > to)
            {
                to = to.AddDays(1);
            }
            return new Interval(from, to);
        }

        void Snooze()
        {
            if (Solve())
            {
                media.Stop();
            }
        }

        DateTime? Next()
        {
            for (var i = 0; i < alarms.Count; i++)
            {
                var alarm = alarms[i];
                if (alarm >= DateTime.Now)
                    return alarm;
            }
            return null;
        }

        void OnMediaFailed(object sender, EventArgs e)
        {
            if (set)
            {
                bypass = true;
                Set = false;
                bypass = false;
            }
        }

        void OnSet()
        {
            OnPropertyChanged(nameof(CanPlay));
            OnPropertyChanged(nameof(CanStop));

            media.Stop();
            alarms.Clear();

            if (set)
            {
                var interval = Interval();

                var j = interval.From;
                while (j <= interval.To)
                {
                    alarms.Add(j);
                    j = j.AddSeconds(Get.Current<Options>().Increment * 60d);
                }

                return;
            }
        }

        void OnUpdate(object sender, ElapsedEventArgs e)
        {
            if (media.Volume != Get.Current<Options>().AudioVolume)
                media.Volume = Get.Current<Options>().AudioVolume;

            if (media.IsMuted)
                media.IsMuted = false;

            queue.Add(() => OnPropertyChanged(nameof(Difference)));

            if (!set)
            {
                queue.Add(() =>
                {
                    OnPropertyChanged(nameof(Count));
                    OnPropertyChanged(nameof(From));
                    OnPropertyChanged(nameof(To));
                });
                return;
            }

            if (alarms.Count == 0)
            {
                if (!media.Playing)
                {
                    //Automatically disable the alarm if all alarms have stopped playing
                    Set = false;
                }
                return;
            }

            if (alarms.First<DateTime>() <= DateTime.Now)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => media.Play()));
                alarms.RemoveAt(0);
            }

            OnPropertyChanged(nameof(NextAlarm));
        }

        public bool Solve()
        {
            if (Set)
            {
                if (Get.Current<Options>().MathDifficulty > 0)
                {
                    var mathWindow = new MathWindow((MathParser.Difficulties)Get.Current<Options>().MathDifficulty.Int32());
                    mathWindow.ShowDialog();

                    return mathWindow.CorrectAnswer == mathWindow.Answer;
                }
            }
            return true;
        }

        #endregion

        #endregion
    }
}