using Imagin.Common.Input;
using System;
using System.Collections;

namespace Imagin.Common.Collections
{
    public interface ICollect
    {
        event EventHandler<EventArgs<object>> Added;

        event ChangedEventHandler Changed;

        event EventHandler<EventArgs> Cleared;

        event EventHandler<EventArgs> Clearing;

        event EventHandler<EventArgs<object>> Inserted;

        event EventHandler<EventArgs<object>> Removed;

        /// ......................................................................................................................

        object this[int index] { get; }

        int Count { get; }

        bool IsEmpty { get; }

        /// ......................................................................................................................

        void Add(object i);

        void Clear();

        bool Contains(object i);
        
        int IndexOf(object i);

        void Insert(int index, object i);

        bool Remove(object i);

        void RemoveAt(int i);

        /// ......................................................................................................................

        IEnumerator GetEnumerator();
    }
}