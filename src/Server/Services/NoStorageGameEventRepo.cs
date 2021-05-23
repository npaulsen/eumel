using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using Microsoft.Extensions.Logging;

namespace Eumel.Server
{
    // TODO: noop implementation indicating bad abstraction.
    public class NoStorageGameEventRepo : IGameEventRepo
    {
        private readonly ILogger<NoStorageGameEventRepo> _logger;

        public NoStorageGameEventRepo(ILogger<NoStorageGameEventRepo> logger)
        {
            _logger = logger;
        }

        public void DeleteOutdatedEvents(string gameUuid)
        {
            throw new System.NotImplementedException();
        }

        public GameProgress GetGameProgress(string gameUuid)
        {
            _logger.LogWarning("no state of games can be loaded, assuming game {gameUuid} has not started.", gameUuid);
            return GameProgress.NotStarted;
        }

        public void StoreEvent(GameEvent gameEvent)
        {
            throw new System.NotImplementedException();
        }

        public void StoreSeriesEvent(GameSeriesEvent gameEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}