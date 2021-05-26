using Imagin.Common.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Imagin.Common.Collections.Generic
{
    [Serializable]
    public class Collection<T> : ICollect, ICollect<T>, IList<T>, IList, IReadOnlyList<T>, INotifyCollectionChanged, IPropertyChanged
    {
        #region Classes

        /// <summary>
        /// This helps prevent reentrant calls.
        /// </summary>
        class SimpleMonitor : IDisposable
        {
            int _busyCount;

            public bool Busy => _busyCount > 0;

            public void Enter() => ++_busyCount;

            public void Dispose() => --_busyCount;
        }

        #endregion

        #region Events

        [field: NonSerialized()]
        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => PropertyChanged += value;
            remove => PropertyChanged -= value;
        }
        [field: NonSerialized()]
        protected virtual event PropertyChangedEventHandler PropertyChanged;

        event EventHandler<EventArgs<object>> added;
        event EventHandler<EventArgs<object>> ICollect.Added
        {
            add => added += value;
            remove => added -= value;
        }
        [field: NonSerialized()]
        public event EventHandler<EventArgs<T>> Added;

        event ChangedEventHandler ICollect.Changed
        {
            add => Changed += value;
            remove => Changed -= value;
        }
        [field: NonSerialized()]
        public event ChangedEventHandler Changed;

        event EventHandler<EventArgs> ICollect.Cleared
        {
            add => Cleared += value;
            remove => Cleared -= value;
        }
        [field: NonSerialized()]
        public event EventHandler<EventArgs> Cleared;

        event EventHandler<EventArgs> ICollect.Clearing
        {
            add => Clearing += value;
            remove => Clearing -= value;
        }
        [field: NonSerialized()]
        public event EventHandler<EventArgs> Clearing;

        event EventHandler<EventArgs<object>> inserted;
        event EventHandler<EventArgs<object>> ICollect.Inserted
        {
            add => inserted += value;
            remove => inserted -= value;
        }
        [field: NonSerialized()]
        public event EventHandler<EventArgs<T>> Inserted;

        event EventHandler<EventArgs<object>> removed;
        event EventHandler<EventArgs<object>> ICollect.Removed
        {
            add => removed += value;
            remove => removed -= value;
        }
        [field: NonSerialized()]
        public event EventHandler<EventArgs<T>> Removed;

        #endregion

        #region Fields

        const string CountString = "Count";

        /// <summary>
        /// This must agree with Binding.IndexerName. It is declared separately here so as to avoid a dependency on PresentationFramework.dll.
        /// </summary>
        const string IndexerName = "Item[]";

        IList<T> _items;

        [NonSerialized]
        Object _syncRoot;

        SimpleMonitor _monitor = new SimpleMonitor();

        #endregion

        #region Properties

        public int Count => _items.Count;

        public bool IsEmpty => Count == 0;

        protected IList<T> Items => _items;

        #endregion

        #region Collection

        public T this[int index]
        {
            get => _items[index];
            set
            {
                if (_items.IsReadOnly)
                    throw new NotSupportedException();

                if (index < 0 || index >= _items.Count)
                    throw new ArgumentOutOfRangeException();

                SetItem(index, value);
            }
        }

        public Collection() => _items = new List<T>();

        public Collection(IList<T> input) : this()
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            CopyFrom(input);
        }

        public Collection(IEnumerable<T> input) : this()
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            CopyFrom(input);
        }

        #endregion

        #region Methods

        #region Private

        /// <summary>
        /// Non-null values are fine. Only accept nulls if <see langword="T"/> is a class or <see cref="Nullable{T}"/>. Note that default(T) is not equal to <see langword="null"/> for value types except when <see langword="T"/> is <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static bool IsCompatibleObject(object value) => ((value is T) || (value == null && default(T) == null));

        void CopyFrom(IEnumerable<T> collection)
        {
            IList<T> items = Items;
            if (collection != null && items != null)
            {
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                        items.Add(enumerator.Current);
                }
            }
        }

        /// <summary>
        /// Called by base class Collection&lt;T&gt; when the list is being cleared;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        void ClearItems()
        {
            CheckReentrancy();
            _items.Clear();
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionReset();
        }

        /// <summary>
        /// Called by base class Collection&lt;T&gt; when an item is removed from list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        void RemoveItem(int index)
        {
            CheckReentrancy();
            T removedItem = this[index];

            _items.RemoveAt(index);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
        }

        /// <summary>
        /// Called by base class Collection&lt;T&gt; when an item is added to list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        void InsertItem(int index, T item)
        {
            CheckReentrancy();
            _items.Insert(index, item);

            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// Called by base class Collection&lt;T&gt; when an item is set in list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        void SetItem(int index, T item)
        {
            CheckReentrancy();
            T originalItem = this[index];
            _items[index] = item;

            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, originalItem, item, index);
        }

        void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion

        #region Protected

        /// <summary>
        /// Called by base class ObservableCollection&lt;T&gt; when an item is to be moved within the list;
        /// raises a CollectionChanged event to any listeners.
        /// </summary>
        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            T removedItem = this[oldIndex];

            RemoveItem(oldIndex);
            InsertItem(newIndex, removedItem);

            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Move, removedItem, newIndex, oldIndex);
        }

        /// <summary>
        /// Raises a PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);

        /// <summary>
        /// Raise CollectionChanged event to any listeners.
        /// Properties/methods modifying this ObservableCollection will raise
        /// a collection changed event through this virtual method.
        /// </summary>
        /// <remarks>
        /// When overriding this method, either call its base implementation
        /// or call <see cref="BlockReentrancy"/> to guard against reentrant collection changes.
        /// </remarks>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                using (BlockReentrancy())
                {
                    CollectionChanged(this, e);
                }
            }
        }

        /// <summary>
        /// Disallow reentrant attempts to change this collection. E.g. a event handler
        /// of the CollectionChanged event is not allowed to make changes to this collection.
        /// </summary>
        /// <remarks>
        /// typical usage is to wrap e.g. a OnCollectionChanged call with a using() scope:
        /// <code>
        ///         using (BlockReentrancy())
        ///         {
        ///             CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
        ///         }
        /// </code>
        /// </remarks>
        protected IDisposable BlockReentrancy()
        {
            _monitor.Enter();
            return _monitor;
        }

        /// <summary> Check and assert for reentrant attempts to change this collection. </summary>
        /// <exception cref="InvalidOperationException"> raised when changing the collection
        /// while another collection change is still being notified to other listeners </exception>
        protected void CheckReentrancy()
        {
            if (_monitor.Busy)
            {
                // we can allow changes if there's only one listener - the problem
                // only arises if reentrant changes make the original event args
                // invalid for later listeners.  This keeps existing code working
                // (e.g. Selector.SelectedItems).
                if ((CollectionChanged != null) && (CollectionChanged.GetInvocationList().Length > 1))
                    throw new InvalidOperationException();
            }
        }

        #endregion

        #region Public

        public void CopyTo(T[] array, int index) => _items.CopyTo(array, index);

        public bool Contains(T item) => _items.Contains(item);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        public int IndexOf(T item) => _items.IndexOf(item);

        /// <summary>
        /// Move item at oldIndex to newIndex.
        /// </summary>
        public void Move(int oldIndex, int newIndex)
        {
            MoveItem(oldIndex, newIndex);
        }

        /// ...................................................

        public virtual void Add(T i)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            int index = _items.Count;
            InsertItem(index, i);
            OnAdded(i);
        }

        public virtual void Clear()
        {
            OnClearing();
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            ClearItems();
            OnCleared();
        }

        public virtual void Insert(int i, T Item)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            if (i < 0 || i > _items.Count)
                throw new ArgumentOutOfRangeException();

            InsertItem(i, Item);
            OnInserted(Item, i);
        }

        public virtual bool Remove(T Item)
        {
            bool remove()
            {
                if (_items.IsReadOnly)
                    throw new NotSupportedException();

                int index = _items.IndexOf(Item);
                if (index < 0)
                    return false;

                RemoveItem(index);
                return true;
            }
            var result = remove();
            OnRemoved(Item);
            return result;
        }

        public virtual void RemoveAt(int i)
        {
            var Item = this[i];
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            if (i < 0 || i >= _items.Count)
                throw new ArgumentOutOfRangeException();

            RemoveItem(i);
            OnRemoved(Item);
        }

        /// ...................................................

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Virtual

        protected virtual void OnAdded(T item)
        {
            added?.Invoke(this, new EventArgs<object>(item));
            Added?.Invoke(this, new EventArgs<T>(item));
            OnChanged();
        }

        protected virtual void OnChanged()
        {
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(IsEmpty));
            Changed?.Invoke(this);
        }

        protected virtual void OnCleared()
        {
            Cleared?.Invoke(this, new EventArgs());
            OnChanged();
        }

        protected virtual void OnClearing() => Clearing?.Invoke(this, new EventArgs());

        protected virtual void OnInserted(T item, int index)
        {
            inserted?.Invoke(this, new EventArgs<object>(item, index));
            Inserted?.Invoke(this, new EventArgs<T>(item, index));
            OnChanged();
        }

        protected virtual void OnRemoved(T item)
        {
            removed?.Invoke(this, new EventArgs<object>(item));
            Removed?.Invoke(this, new EventArgs<T>(item));
            OnChanged();
        }

        #endregion

        #endregion

        #region ICollect

        object ICollect.this[int index] => this[index];

        void ICollect.Add(object i) => Add((T)i);

        void ICollect.Clear() => Clear();

        IEnumerator ICollect.GetEnumerator() => GetEnumerator();

        bool ICollect.Contains(object i) => i is T ? Contains((T)i) : false;

        int ICollect.IndexOf(object i) => i is T ? IndexOf((T)i) : -1;

        void ICollect.Insert(int index, object i) => Insert(index, (T)i);

        bool ICollect.Remove(object i) => Remove((T)i);

        void ICollect.RemoveAt(int i) => RemoveAt(i);

        #endregion

        #region ICollection

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    ICollection c = _items as ICollection;
                    if (c != null)
                    {
                        _syncRoot = c.SyncRoot;
                    }
                    else
                    {
                        System.Threading.Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
                    }
                }
                return _syncRoot;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Rank != 1)
                throw new ArgumentException();

            if (array.GetLowerBound(0) != 0)
                throw new ArgumentException();

            if (index < 0)
                throw new ArgumentOutOfRangeException();

            if (array.Length - index < Count)
                throw new ArgumentException();

            T[] tArray = array as T[];
            if (tArray != null)
            {
                _items.CopyTo(tArray, index);
            }
            else
            {
                //
                // Catch the obvious case assignment will fail.
                // We can found all possible problems by doing the check though.
                // For example, if the element type of the Array is derived from T,
                // we can't figure out if we can successfully copy the element beforehand.
                //
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                    throw new ArgumentException();

                //
                // We can't cast array of value type to object[], so we don't support 
                // widening of primitive types here.
                //
                object[] objects = array as object[];
                if (objects == null)
                    throw new ArgumentException();

                int count = _items.Count;
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        objects[index++] = _items[i];
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException();
                }
            }
        }

        #endregion

        #region ICollection<T>

        bool ICollection<T>.IsReadOnly => _items.IsReadOnly;

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_items).GetEnumerator();

        #endregion

        #region IList

        object IList.this[int index]
        {
            get { return _items[index]; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                try
                {
                    this[index] = (T)value;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException();
                }

            }
        }

        bool IList.IsReadOnly => _items.IsReadOnly;

        bool IList.IsFixedSize
        {
            get
            {
                // There is no IList<T>.IsFixedSize, so we must assume that only
                // readonly collections are fixed size, if our internal item 
                // collection does not implement IList.  Note that Array implements
                // IList, and therefore T[] and U[] will be fixed-size.
                IList list = _items as IList;
                if (list != null)
                {
                    return list.IsFixedSize;
                }
                return _items.IsReadOnly;
            }
        }

        int IList.Add(object value)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            try
            {
                Add((T)value);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException();
            }

            return this.Count - 1;
        }

        bool IList.Contains(object value)
        {
            if (IsCompatibleObject(value))
            {
                return Contains((T)value);
            }
            return false;
        }

        int IList.IndexOf(object value)
        {
            if (IsCompatibleObject(value))
            {
                return IndexOf((T)value);
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            try
            {
                Insert(index, (T)value);
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException();
            }

        }

        void IList.Remove(object value)
        {
            if (_items.IsReadOnly)
                throw new NotSupportedException();

            if (IsCompatibleObject(value))
                Remove((T)value);
        }

        #endregion
    }
}