using Imagin.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Media;

namespace Alarm
{
    public class Media : Base
    {
        public event EventHandler<EventArgs> Failed;

        public enum VolumeUnit
        {
            Decibel,
            Scalar
        }

        [DllImport("MasterVolume", CallingConvention = CallingConvention.Cdecl)]
        public static extern float GetVolume(VolumeUnit vUnit);

        [DllImport("MasterVolume", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetVolume(double newVolume, VolumeUnit vUnit);

        [DllImport("MasterVolume", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Muted();

        [DllImport("MasterVolume", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mute();

        [DllImport("MasterVolume", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Unmute();

        /// ------------------------------------------------------------------

        public const string DefaultFormat = ".mp3";

        public static List<string> Formats = new List<string>()
        {
            "mp3",
            "wma",
            "wav",
            "ogg",
            "aac",
        };

        /// ------------------------------------------------------------------

        readonly MediaPlayer mediaPlayer = new MediaPlayer();

        /// ------------------------------------------------------------------

        public double Volume
        {
            get => GetVolume(VolumeUnit.Scalar);
            set => SetVolume(value, VolumeUnit.Scalar);
        }

        public bool IsMuted
        {
            get => Muted();
            set
            {
                if (value)
                {
                    Mute();
                }
                else Unmute();
            }
        }

        TimeSpan? mediaLength = null;

        TimeSpan mediaElapsed = TimeSpan.Zero;

        readonly Timer timer = new Timer();

        /// ------------------------------------------------------------------

        bool playing = false;
        public bool Playing
        {
            get => playing;
            set
            {
                this.Change(ref playing, value);
                Get.Current<MainViewModel>().Changed(() => Get.Current<MainViewModel>().CanPlay);
                Get.Current<MainViewModel>().Changed(() => Get.Current<MainViewModel>().CanStop);
            }
        }

        /// ------------------------------------------------------------------

        public Media()
        {
            mediaPlayer.MediaEnded += OnMediaEnded;
            mediaPlayer.MediaOpened += OnMediaOpened;

            timer.Elapsed += OnTimerElapsed;
            timer.Interval = 500;
            timer.Start();
        }

        /// ------------------------------------------------------------------

        void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (mediaLength != null)
            {
                mediaElapsed = mediaElapsed.Add(TimeSpan.FromSeconds(1));
                if (mediaElapsed < mediaLength)
                    Playing = true;
            }
            else Playing = false;
        }

        void OnMediaEnded(object sender, EventArgs e)
        {
            Reset();
        }

        /// ------------------------------------------------------------------

        public void Play()
        {
            try
            {
                mediaPlayer.Close();
                mediaPlayer.Open(new Uri(Get.Current<Options>().AudioFilePath));
            }
            catch (Exception)
            {
                OnMediaFailed();
            }
        }

        /// ------------------------------------------------------------------

        void OnMediaFailed()
        {
            Failed?.Invoke(this, EventArgs.Empty);
        }

        void OnMediaOpened(object sender, EventArgs e)
        {
            mediaLength = mediaPlayer.NaturalDuration.TimeSpan;

            if (mediaLength.Value.TotalSeconds < 5)
            {
                OnMediaFailed();
                return;
            }

            try
            {
                mediaPlayer.Play();
            }
            catch
            {
                OnMediaFailed();
            }
        }

        /// ------------------------------------------------------------------

        void Reset()
        {
            mediaElapsed = TimeSpan.Zero;
            mediaLength = null;
        }

        public void Stop()
        {
            mediaPlayer.Stop();
            Reset();
        }
    }
}