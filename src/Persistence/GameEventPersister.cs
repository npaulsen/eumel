using System;
using System.Linq;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using Eumel.Persistance.GameEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eumel.Persistance
{
    public class GameEventPersister : IGameEventPersister
    {
        private readonly ILogger<GameEventPersister> _logger;
        private readonly IDbContextFactory<EumelGameContext> _contextFactory;

        public GameEventPersister(ILogger<GameEventPersister> logger, IDbContextFactory<EumelGameContext> contextFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;

            _logger.LogInformation("created");
        }

        public void OnNext(GameEvent value)
        {
            _logger.LogInformation(value.ToString());

            var persistableEvent = GameEventSerializer.Convert(value);

            using var ctx = _contextFactory.CreateDbContext();
            ctx.Events.Add(persistableEvent);
            ctx.SaveChanges();
        }

        public void OnNext(GameSeriesEvent value)
        {
            _logger.LogInformation("persisting {gameSeriesEvent}", value);

            var persistableEvent = GameSeriesEventSerializer.Convert(value);

            using var ctx = _contextFactory.CreateDbContext();
            ctx.SeriesEvents.Add(persistableEvent);
            ctx.SaveChanges();
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