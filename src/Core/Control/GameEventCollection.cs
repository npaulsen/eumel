using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Eumel.Core
{
    public class EventCollection<T> : IObservableWithHistory<T>, IEnumerable<T>
    {
        private readonly List<IObserver<T>> _observers;
        private readonly List<T> _events;

        private bool _isNotifying = false;
        private readonly Queue<T> _queue;

        public EventCollection()
        {
            _observers = new List<IObserver<T>>();
            _events = new List<T>();
            _queue = new Queue<T>();
        }

        public IDisposable SubscribeWithPreviousEvents(IObserver<T> observer)
        {
            var disposer = Subscribe(observer);
            _events.ForEach(observer.OnNext);
            return disposer;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new Unsubscriber<T>(_observers, observer);
        }

        public void Insert(T newEvent)
        {
            _queue.Enqueue(newEvent);
            if (!_isNotifying)
            {
                _isNotifying = true;
                NotifyEnqueuedEvents();
                _isNotifying = false;
            }
        }

        private void NotifyEnqueuedEvents()
        {
            while (_queue.Any())
            {
                var e = _queue.Dequeue();
                _observers.ForEach(o => o.OnNext(e));
                _events.Add(e);
            }
        }

        // TODO: rather end observable and start new one?!
        public void Clear() => _events.Clear();

        public IEnumerator<T> GetEnumerator() => _events.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _events.GetEnumerator();

    }
}