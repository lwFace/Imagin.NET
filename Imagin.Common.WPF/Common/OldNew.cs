using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an old and new value. When used with <see cref="DependencyObject"/>, avoids <see cref="System.NullReferenceException"/> by capturing <see cref="DependencyPropertyChangedEventArgs"/> instead of casting the value that changed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OldNew<T>
    {
        public readonly T Old = default;

        public readonly T New = default;

        public OldNew(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is T i)
                Old = i;

            if (e.NewValue is T j)
                New = j;
        }

        public OldNew(T @new) : this(default, @new) { }

        public OldNew(T old, T @new)
        {
            Old = old;
            New = @new;
        }
    }

    /// <inheritdoc />
    public class OldNew : OldNew<object>
    {
        public OldNew(DependencyPropertyChangedEventArgs e) : base(e) { }

        public OldNew(object old, object @new) : base(old, @new) { }
    }
}