using System;

namespace Alarm
{
    public class Interval
    {
        public readonly DateTime From;

        public readonly DateTime To;

        public Interval(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public bool Invalid() => !Valid();

        public bool Valid() => From < To;
    }
}