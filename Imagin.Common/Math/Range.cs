﻿using Imagin.Common.Linq;
using System;

namespace Imagin.Common
{
    [Serializable]
    public struct Range<T> : IEquatable<Range<T>>
    {
        public static Range<T> Default = new Range<T>(default, default);

        //.....................................................................................

        public readonly T Minimum;

        public readonly T Maximum;

        //.....................................................................................

        public Range(T minimum, T maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public override string ToString() => $"{nameof(Minimum)} = {Minimum}, {nameof(Maximum)} = {Maximum}";

        //.....................................................................................

        public static bool operator ==(Range<T> left, Range<T> right) => left.EqualsOverload(right);

        public static bool operator !=(Range<T> left, Range<T> right) => !(left == right);

        public bool Equals(Range<T> i) => this.Equals<Range<T>>(i) && Minimum.Equals(i.Minimum) && Maximum.Equals(i.Maximum);

        public override bool Equals(object i) => Equals((Range<T>)i);

        public override int GetHashCode() => new { Minimum, Maximum }.GetHashCode();
    }
}