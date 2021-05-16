using System;

namespace Eumel.Core
{
    public interface IObservableWithHistory<T> : IObservable<T>
    {
        IDisposable SubscribeWithPreviousEvents(IObserver<T> observer);
    }
}