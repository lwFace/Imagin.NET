using System;
using System.Timers;
using System.Xml.Serialization;

namespace Imagin.Common
{
    /// <inheritdoc/>
    public class Notify : DisposableObject
    {
        /// <summary>
        /// The default interval to use.
        /// </summary>
        public readonly static TimeSpan DefaultInterval = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Occurs when the timer elapses.
        /// </summary>
        public event ElapsedEventHandler Elapsed;

        Timer timer;

        [XmlIgnore]
        public bool Enabled
        {
            get => timer.Enabled;
            set => timer.Enabled = value;
        }

        [XmlIgnore]
        public TimeSpan Interval
        {
            get => TimeSpan.FromMilliseconds(timer.Interval);
            set => timer.Interval = value.TotalMilliseconds;
        }

        /// <summary>
        /// Initializes an instance of <see cref="Notify"/>.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="enabled"></param>
        public Notify(TimeSpan? interval = null) : base() => Reset(interval, false);

        void OnElapsed(object sender, ElapsedEventArgs e) => OnElapsed(e);

        /// <summary>
        /// Occurs when the timer elapses.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnElapsed(ElapsedEventArgs e) => Elapsed?.Invoke(this, e);

        /// <inheritdoc/>
        protected override void OnUnmanagedDisposed()
        {
            base.OnUnmanagedDisposed();
            timer.Enabled = false;
            timer.Elapsed -= OnElapsed;
            timer.Dispose();
        }

        public void Reset(TimeSpan? interval = null, bool enabled = false)
        {
            if (timer != null)
            {
                timer.Enabled = false;
                timer.Elapsed -= OnElapsed;
                timer.Dispose();
            }

            timer = new Timer();
            timer.Elapsed += OnElapsed;

            Interval = interval ?? DefaultInterval;
            Enabled = enabled;
        }
    }
}
