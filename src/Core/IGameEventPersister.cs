using System;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core
{

    public interface IGameEventPersister : IObserver<GameEvent>, IObserver<GameSeriesEvent>
    {
    }

    public class NoopGameEventPersister : IGameEventPersister
    {
        public void OnNext(GameEvent gameEvent)
        {
            System.Console.WriteLine(gameEvent);
        }

        public void OnNext(GameSeriesEvent value)
        {
            System.Console.WriteLine(value);
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }
    }
}