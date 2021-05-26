using Imagin.Common.Input;
using System;

namespace Imagin.Common.Collections.Generic
{
    public interface ICollect<T>
    {
        event EventHandler<EventArgs<T>> Added;

        event ChangedEventHandler Changed;

        event EventHandler<EventArgs> Cleared;

        event EventHandler<EventArgs> Clearing;

        event EventHandler<EventArgs<T>> Inserted;

        event EventHandler<EventArgs<T>> Removed;

        bool IsEmpty { get; }

        void Add(T Item);

        void Clear();

        void Insert(int i, T Item);

        bool Remove(T Item);

        void RemoveAt(int i);
    }
}