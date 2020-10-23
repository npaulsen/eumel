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

        public void Insert(T newEvent)
        {
            _events.Add(newEvent);
            _observers.ForEach(o => o.OnNext(newEvent));
        }

        public IEnumerator<T> GetEnumerator() => _events.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _events.GetEnumerator();

    }
}