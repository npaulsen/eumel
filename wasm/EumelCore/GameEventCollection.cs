using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EumelCore
{
    public class EventCollection<T> : IObservable<T>, IEnumerable<T>
    {
        private List<IObserver<T>> _observers;
        private List<T> _events;

        public IEnumerable<IObserver<S>> GetObservers<S>() => _observers.OfType<IObserver<S>>();

        public EventCollection()
        {
            _observers = new List<IObserver<T>>();
            _events = new List<T>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                foreach (var item in _events)
                    observer.OnNext(item);
            }
            return new Unsubscriber<T>(_observers, observer);
        }

        private bool _isNotifying = false;
        private Queue<T> _queue = new Queue<T>();
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