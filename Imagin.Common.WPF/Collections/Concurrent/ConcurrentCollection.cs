using Imagin.Common.Collections.Generic;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Imagin.Common.Collections.Concurrent
{
    /// <summary>
    /// Provides a collection that can be modified safely on other threads. The notify event is thrown using the dispatcher from the event listener(s).
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    [Serializable]
    public class ConcurrentCollection<T> : BaseConcurrentCollection<T>, ICollect, ICollect<T>, ICollection, ICollection<T>, IList, IList<T>, IPropertyChanged
    {
        #region Properties

        event EventHandler<EventArgs<object>> added;
        event EventHandler<EventArgs<object>> ICollect.Added
        {
            add => added += value;
            remove => added -= value;
        }
        public event EventHandler<EventArgs<T>> Added;

        event ChangedEventHandler ICollect.Changed
        {
            add => Changed += value;
            remove => Changed -= value;
        }
        public event ChangedEventHandler Changed;

        event EventHandler<EventArgs> ICollect.Cleared
        {
            add => Cleared += value;
            remove => Cleared -= value;
        }
        public event EventHandler<EventArgs> Cleared;

        event EventHandler<EventArgs> ICollect.Clearing
        {
            add => Clearing += value;
            remove => Clearing -= value;
        }
        public event EventHandler<EventArgs> Clearing;

        event EventHandler<EventArgs<object>> inserted;
        event EventHandler<EventArgs<object>> ICollect.Inserted
        {
            add => inserted += value;
            remove => inserted -= value;
        }
        public event EventHandler<EventArgs<T>> Inserted;

        event EventHandler<EventArgs<object>> removed;
        event EventHandler<EventArgs<object>> ICollect.Removed
        {
            add => removed += value;
            remove => removed -= value;
        }
        public event EventHandler<EventArgs<T>> Removed;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Count => DoBaseRead(() => ReadCollection.Count);

        bool isEmpty = true;
        public bool IsEmpty
        {
            get => isEmpty;
            set => this.Change(ref isEmpty, value);
        }

        public bool IsReadOnly
        {
            get
            {
                return DoBaseRead(() => ((ICollection<T>)ReadCollection).IsReadOnly);
            }
        }

        #endregion

        #region ConcurrentCollection

        /// <summary>
        /// 
        /// </summary>
        public ConcurrentCollection() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        public ConcurrentCollection(T Item) : base()
        {
            Add(Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public ConcurrentCollection(params T[] Items) : base(Items)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public ConcurrentCollection(IEnumerable<T> Items) : base(Items)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Items"></param>
        public ConcurrentCollection(params IEnumerable<T>[] Items) : base()
        {
            Items.ForEach<IEnumerable<T>>(i => i.ForEach(j => Add(j)));
        }

        #endregion

        #region Methods

        #region ICollect

        object ICollect.this[int index] => this[index];

        void ICollect.Add(object i) => Add((T)i);

        void ICollect.Clear() => Clear();

        bool ICollect.Contains(object i) => i is T ? Contains((T)i) : false;

        IEnumerator ICollect.GetEnumerator() => GetEnumerator();

        int ICollect.IndexOf(object i) => i is T ? IndexOf((T)i) : -1;

        void ICollect.Insert(int index, object i) => Insert(index, (T)i);

        bool ICollect.Remove(object i) => Remove((T)i);

        void ICollect.RemoveAt(int i) => RemoveAt(i);

        #endregion

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            DoBaseRead(() => ((ICollection)ReadCollection).CopyTo(array, index));
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return DoBaseRead(() => ((ICollection)ReadCollection).IsSynchronized);
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return DoBaseRead(() => ((ICollection)ReadCollection).SyncRoot);
            }
        }

        #endregion 

        #region IList 

        int IList.Add(object Item)
        {
            var Result = DoBaseWrite(() => ((IList)WriteCollection).Add(Item));
            OnAdded((T)Item);
            OnChanged();
            return Result;
        }

        bool IList.Contains(object value)
        {
            return DoBaseRead(() => ((IList)ReadCollection).Contains(value));
        }

        bool IList.IsFixedSize
        {
            get
            {
                return DoBaseRead(() => ((IList)ReadCollection).IsFixedSize);
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return DoBaseRead(() => ((IList)ReadCollection).IsReadOnly);
            }
        }

        int IList.IndexOf(object Item)
        {
            return DoBaseRead(() => ((IList)ReadCollection).IndexOf(Item));
        }

        object IList.this[int index]
        {
            get
            {
                return DoBaseRead(() => {
                    return ((IList)ReadCollection)[index];
                });
            }
            set
            {
                DoBaseWrite(() => {
                    ((IList)WriteCollection)[index] = value;
                });
            }
        }

        void IList.Insert(int i, object Item)
        {
            DoBaseWrite(() => ((IList)WriteCollection).Insert(i, Item));
            OnInserted((T)Item, i);
            OnChanged();
        }

        void IList.Remove(object Item)
        {
            DoBaseWrite(() => ((IList)WriteCollection).Remove(Item));
            OnRemoved((T)Item);
            OnChanged();
        }

        void IList.RemoveAt(int i)
        {
            var Item = this[i];
            DoBaseWrite(() => ((IList)WriteCollection).RemoveAt(i));
            OnRemoved(Item);
            OnChanged();
        }

        #endregion

        #region Public

        public T this[int index]
        {
            get
            {
                return DoBaseRead(() => ReadCollection[index]);
            }
            set
            {
                DoBaseWrite(() => WriteCollection[index] = value);
            }
        }

        public void Add(T Item)
        {
            DoBaseWrite(() => WriteCollection.Add(Item));
            OnAdded(Item);
            OnChanged();
        }

        public void Clear()
        {
            OnClearing();
            DoBaseClear(null);
            OnCleared();
            OnChanged();
        }

        public bool Contains(T item) => DoBaseRead(() => ReadCollection.Contains(item));

        public void CopyTo(T[] array, int arrayIndex)
        {
            DoBaseRead(() =>
            {
                if (array.Count() >= ReadCollection.Count)
                {
                    ReadCollection.CopyTo(array, arrayIndex);
                }
                else Console.Out.WriteLine("ConcurrentObservableCollection attempting to copy into wrong sized array (array.Count < ReadCollection.Count)");
            });
        }

        public T FirstOrDefault() => Count > 0 ? this[0] : default(T);

        public T LastOrDefault() => Count > 0 ? this[Count - 1] : default(T);

        public int IndexOf(T item)
        {
            return DoBaseRead(() => ReadCollection.IndexOf(item));
        }

        public void Insert(int i, T Item)
        {
            DoBaseWrite(() => WriteCollection.Insert(i, Item));
            OnInserted(Item, i);
            OnChanged();
        }

        public bool Remove(T Item)
        {
            var Result = DoBaseWrite(() => WriteCollection.Remove(Item));
            OnRemoved(Item);
            OnChanged();
            return Result;
        }

        public void RemoveAt(int i)
        {
            var Item = this[i];
            DoBaseWrite(() => WriteCollection.RemoveAt(i));
            OnRemoved(Item);
            OnChanged();
        }

        #endregion

        #region Public (Async)

        public async Task BeginClear() => await Task.Run(() => Clear());

        #endregion

        #region Virtual

        protected virtual void OnAdded(T Item)
        {
            added?.Invoke(this, new EventArgs<object>(Item));
            Added?.Invoke(this, new EventArgs<T>(Item));
        }

        protected virtual void OnChanged()
        {
            OnPropertyChanged(nameof(Count));
            IsEmpty = Count == 0;
            Changed?.Invoke(this);
        }

        protected virtual void OnCleared()
        {
            Cleared?.Invoke(this, new EventArgs());
        }

        protected virtual void OnClearing() => Clearing?.Invoke(this, new EventArgs());

        protected virtual void OnInserted(T Item, int Index)
        {
            inserted?.Invoke(this, new EventArgs<object>(Item, Index));
            Inserted?.Invoke(this, new EventArgs<T>(Item, Index));
        }

        protected virtual void OnRemoved(T Item)
        {
            removed?.Invoke(this, new EventArgs<object>(Item));
            Removed?.Invoke(this, new EventArgs<T>(Item));
        }

        public virtual void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }

        #endregion

        #endregion
    }
}