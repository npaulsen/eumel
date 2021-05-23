using System;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using Microsoft.Extensions.Logging;

namespace Eumel.Persistance
{
    public class GameEventPersister : IGameEventPersister
    {
        private readonly ILogger<GameEventPersister> _logger;
        private readonly IGameEventRepo _repo;

        public GameEventPersister(ILogger<GameEventPersister> logger, IGameEventRepo repo)
        {
            _logger = logger;
            _repo = repo;
            _logger.LogInformation("created");
        }

        public void OnNext(GameEvent value)
        {
            _logger.LogInformation(value.ToString());
            _repo.StoreEvent(value);
        }

        public void OnNext(GameSeriesEvent value)
        {
            _logger.LogInformation("persisting {gameSeriesEvent}", value);
            // HACK: want to stay in 10k rows. Therefore delete previous rounds events for now.
            // TODO: old events can be used for stats, delete only when series is finished.
            if (value is RoundStarted roundStarted)
            {
                _repo.DeleteOutdatedEvents(roundStarted.GameUuid);
            }
            _repo.StoreSeriesEvent(value);
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