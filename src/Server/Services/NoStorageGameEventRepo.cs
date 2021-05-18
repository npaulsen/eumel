using System.Collections.Generic;
using Eumel.Core;
using Microsoft.Extensions.Logging;

namespace Eumel.Server
{
    public class NoStorageGameEventRepo : IGameEventRepo
    {
        private readonly ILogger<NoStorageGameEventRepo> _logger;

        public NoStorageGameEventRepo(ILogger<NoStorageGameEventRepo> logger)
        {
            _logger = logger;
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
    }
}