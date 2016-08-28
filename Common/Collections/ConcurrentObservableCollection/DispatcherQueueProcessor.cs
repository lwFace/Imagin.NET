﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Imagin.Common.Collections
{
    /// <summary>
    /// Executes a stream on actions on the dispatcher thread
    /// </summary>
    internal class DispatcherQueueProcessor
    {
        #region Properties

        #region Private

        /// <summary>
        /// This class is a singleton class. Below is the instance.
        /// </summary>
        private static readonly Lazy<DispatcherQueueProcessor> _instance = new Lazy<DispatcherQueueProcessor>(() => new DispatcherQueueProcessor(), true);
        
        /// <summary>
        /// A queue of actions to be called on the dispatcher thread
        /// </summary>
        private BlockingCollection<Action> _actionQueue;
        
        /// <summary>
        /// A list of pending subscribers, processed once the Dispather is created.
        /// Using a ConcurrentDictionary because this is the only framework collection
        /// that has a remove method.
        /// </summary>
        private ConcurrentDictionary<WeakReference, object> _subscriberQueue;
        
        /// <summary>
        /// The Application Dispatcher
        /// </summary>
        private Dispatcher _dispatcher = null;
        
        /// <summary>
        /// The current action that is awaiting processing on the Dispatcher thread
        /// </summary>
        private Action _actionWaiting = null;
        
        /// <summary>
        /// Semaphore used to prevent a race condition on _actionWaiting 
        /// </summary>
        private Semaphore _actionWaitingSemaphore = new Semaphore(0, 1);

        private object _startQueueLock = new object();

        #endregion Private Fields

        #region Properties

        /// <summary>
        /// Gets the instance of the singleton class
        /// </summary>
        public static DispatcherQueueProcessor Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        /// <summary>
        /// Tests if the calling thread is the same as the dispatcher thread
        /// </summary>
        public bool IsDispatcherThread
        {
            get
            {
                if (!CheckIfDispatcherCreated())
                {
                    return false;
                }
                else
                {
                    return _dispatcher.CheckAccess();
                }
            }
        }

        #endregion Properties

        #endregion

        #region DispatcherQueueProcessor

        /// <summary>
        /// Private constructor for this singleton class. Use the Instance property to
        /// get an instance of this class.
        /// </summary>
        DispatcherQueueProcessor()
        {
            _actionQueue = new BlockingCollection<Action>();
            _subscriberQueue = new ConcurrentDictionary<WeakReference, object>();
            if (!CheckIfDispatcherCreated())
            {
                // The application domain hasn't been created yet, poll at 10Hz until it's created
                var timer = new System.Timers.Timer(100);
                timer.Elapsed += (sender, e) => {
                    if (CheckIfDispatcherCreated())
                    {
                        timer.Enabled = false;
                    }
                };
                timer.Enabled = true;
            }
        }

        #endregion

        #region Methods

        #region Public

        object _actionWaitingLock = new object();

        /// <summary>
        /// Adds an action to the processing queue
        /// </summary>
        /// <param name="action"></param>
        public void Add(Action action) {

            //// If we are running on the dispatcher thread, we could call the
            //// action directly, but then we've got the problem with queue
            //// jumping. It's desirable to immediately update the view model,
            //// as if we don't the code that added the item won't see it
            //// if an iteration is done over the collection, which would confuse
            //// the person using this collection.
            ////
            //// So, we need to add it to the queue and then process the queue
            //// so the view is consistent with the Add action.

            // We're on a background thread
            if (!IsDispatcherThread)
            {
            // Just add action to queue.
            _actionQueue.Add(action);
            }
            else // We're on UI thread
            {
            lock (_actionWaitingLock)
            {
                // Add action to queue
                _actionQueue.Add(action);

                // Background thread might have set next action to execute.
                if (_actionWaiting != null)
                {
                _actionWaiting();
                _actionWaiting = null;
                }

                // Clear the more of the action queue, up to 100 items at a time.
                // Batch up processing into lots of 100 so as to give some
                // responsiveness if the collection is being bombarded.
                int countDown = 300;
                Action nextCommand = null;

                // Note that countDown must be tested first, otherwise we throw away a queue item
                while (countDown > 0 && _actionQueue.TryTake(out nextCommand))
                {
                --countDown;
                nextCommand();
                }
            }
            }
        }

        /// <summary>
        /// Adds a subscribe action to the subscriber queue
        /// </summary>
        /// <param name="subscribeRef"></param>
        public IDisposable QueueSubscribe(Action subscribeAction) {

            // Subscriber queue is set to null after the Dispatcher has been created.
            // So subscriptions can be handled directly once the dispatcher queue is
            // being processed.

            if(_subscriberQueue != null) {
            try {
                WeakReference weakRef = new WeakReference(subscribeAction);
                _subscriberQueue[weakRef] = null;

                // Return a disposable for removing subscriber from the queue
                return new DoDispose(subscribeAction, () => {
                // Copy to avoid race condition
                var subscriberQueue = _subscriberQueue;
                if(subscriberQueue != null) {
                    object dummy;
                    subscriberQueue.TryRemove(weakRef, out dummy);
                }
                });

            } catch {
                // _subscriberQueue may have been set to null on another thread
                if(_subscriberQueue == null) {
                subscribeAction();
                }
            }
            } else {
            subscribeAction();
            }

            return new DoDispose();
        }

        #endregion

        #region Private

        /// <summary>
        /// Checks if the dispatcher has been created yet
        /// </summary>
        /// <returns></returns>
        bool CheckIfDispatcherCreated()
        {
            if(_dispatcher != null)
                return true;
            else
            {
                lock(_startQueueLock)
                {
                    if(_dispatcher == null)
                    {
                        if(Application.Current != null)
                        {
                            _dispatcher = Application.Current.Dispatcher;
                            _dispatcher.ShutdownStarted += (s, e) => _dispatcher = null;
                            if(_dispatcher != null)
                                StartQueueProcessing();
                        }
                    }
                    return _dispatcher != null;
                }
            }
        }

        /// <summary>
        /// Starts the thread that processes the queue of actions that are to be
        /// executed on the dispatcher thread.
        /// </summary>
        void StartQueueProcessing()
        {
            var keys = _subscriberQueue.Keys;
            _subscriberQueue = null;

            foreach(WeakReference subscribeRef in keys)
            {
                Action subscribe = subscribeRef.Target as Action;
                if(subscribe != null)
                    subscribe();
            }
            keys = null;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    while (!_actionQueue.IsCompleted)
                    {
                        // Get action from queue in this background thread.
                        while (true)
                        {
                            lock (_actionWaitingLock)
                            {
                                try
                                {
                                    Action action = null;
                                    if (_actionQueue.TryTake(out action, 1))
                                    {
                                        _actionWaiting = action;
                                        break;
                                    }
                                }
                                catch (InvalidOperationException)
                                {
                                }
                            }
                            try
                            {
                                Thread.Sleep(1);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        // Wait to join to the dispatcher thread
                        _dispatcher?.Invoke((Action)(() =>
                        {
                            lock (_actionWaitingLock)
                            {
                                // Action might have already been executed by UI thread in Add method.
                                if (_actionWaiting != null)
                                {
                                    _actionWaiting();
                                    _actionWaiting = null;
                                }
                                // Clear the more of the action queue, up to 100 items at a time.
                                // Batch up processing into lots of 100 so as to give some
                                // responsiveness if the collection is being bombarded.
                                int countDown = 100;
                                Action nextCommand = null;
                                // Note that countDown must be tested first, otherwise we throw away a queue item
                                while (countDown > 0 && _actionQueue.TryTake(out nextCommand))
                                {
                                    --countDown;
                                    nextCommand();
                                }
                            }
                        }));
                    }
                }
                catch (Exception)
                {
                    // TODO: Some diagnostics
                    // Assume render thread is dead, so exit
                }
            });
        }

        #endregion

        #region Classes

        /// <summary>
        /// Used for passing back an IDisposable from the QueueSubscribe method
        /// </summary>
        class DoDispose : IDisposable
        {
            // We create a subscribe action, which has a reference to it's parent view model object.
            // If the DispatcherQueueProcessor isn't started (because the Dispatcher hasn't been
            // created), then the subscriber action will sit in the queue forever, hence will
            // never be garbage collection, hence the view model will never be garbage
            // collected. To get around this the DispatcherQueueProcessor stores a weak
            // reference. As such the _subscribeAction needs to be referenced somewhere in the view model
            // otherwise it will be garbage collected once we leave the scope of this constructor.
            // We do that by storing it in the disposable that we pass back.
            public Action _subscribeAction = null;
            private Action _doDispose = null;

            public DoDispose()
            {
            }

            public DoDispose(Action subscribeAction, Action doDispose)
            {
                _subscribeAction = subscribeAction;
                _doDispose = doDispose;
            }

            public void Dispose()
            {
                if (_doDispose != null)
                {
                    _doDispose();
                }
                _doDispose = null;
            }
        }

        #endregion

        #endregion
    }
}
