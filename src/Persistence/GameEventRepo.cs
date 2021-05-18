using System;
using System.Collections.Immutable;
using System.Linq;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using Eumel.Persistance.GameEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eumel.Persistance
{
    public class GameEventRepo : IGameEventRepo
    {
        private readonly ILogger<GameEventRepo> _logger;
        private readonly IDbContextFactory<EumelGameContext> _contextFactory;

        public GameEventRepo(ILogger<GameEventRepo> logger, IDbContextFactory<EumelGameContext> contextFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;
        }

        public void OnNext(GameEvent value)
        {
            _logger.LogInformation(value.ToString());

            var persistableEvent = GameEventSerializer.Convert(value);

            using var ctx = _contextFactory.CreateDbContext();
            ctx.Events.Add(persistableEvent);
            ctx.SaveChanges();

            _logger.LogInformation("stored");
        }

        public void StoreEvent(GameEvent gameEvent)
        {
            throw new NotImplementedException();
        }

        // TODO separate humble accessor to dbcontext and logic.
        public GameProgress GetGameProgress(string gameUuid)
        {
            if (string.IsNullOrWhiteSpace(gameUuid))
            {
                throw new ArgumentException(nameof(gameUuid));
            }
            _logger.LogInformation("Start retrieving events of game {gameUuid}", gameUuid);
            using var ctx = _contextFactory.CreateDbContext();
            var seriesEvents = ctx.SeriesEvents
                .Where(ev => ev.GameUuid == gameUuid)
                .Select(GameSeriesEventSerializer.Convert)
                .ToImmutableList()
                .WithValueSemantics();
            var roundsStarted = seriesEvents.Count(e => e is RoundStarted);
            // can be -1, feels hacky.
            var lastRoundIndex = roundsStarted - 1;
            var lastRoundEvents = ctx.Events
                .Where(ev => ev.GameUuid == gameUuid && ev.RoundIndex == lastRoundIndex)
                .Select(GameEventSerializer.Convert)
                .ToImmutableList()
                .WithValueSemantics();
            return new(seriesEvents, lastRoundEvents);
        }
    }
}