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
                .OrderBy(ev => ev.Id)
                .Select(GameSeriesEventSerializer.Convert)
                .ToImmutableList()
                .WithValueSemantics();
            var roundsStarted = seriesEvents.Count(e => e is RoundStarted);
            // can be -1, feels hacky.
            var lastRoundIndex = roundsStarted - 1;
            var lastRoundEvents = ctx.Events
                .Where(ev => ev.GameUuid == gameUuid && ev.RoundIndex == lastRoundIndex)
                .OrderBy(ev => ev.Id)
                .Select(GameEventSerializer.Convert)
                .ToImmutableList()
                .WithValueSemantics();
            return new(seriesEvents, lastRoundEvents);
        }

        public void StoreEvent(GameEvent gameEvent)
        {
            var persistableEvent = GameEventSerializer.Convert(gameEvent);

            using var ctx = _contextFactory.CreateDbContext();
            ctx.Events.Add(persistableEvent);
            ctx.SaveChanges();
        }

        public void StoreSeriesEvent(GameSeriesEvent gameEvent)
        {
            var persistableEvent = GameSeriesEventSerializer.Convert(gameEvent);

            using var ctx = _contextFactory.CreateDbContext();

            ctx.SeriesEvents.Add(persistableEvent);
            ctx.SaveChanges();
        }

        public void DeleteOutdatedEvents(string gameUuid)
        {
            if (string.IsNullOrWhiteSpace(gameUuid))
            {
                throw new ArgumentException(nameof(gameUuid));
            }
            _logger.LogInformation("Deleting outdated events for {gameUuid}", gameUuid);
            using var ctx = _contextFactory.CreateDbContext();
            var roundsStarted = ctx.SeriesEvents
                .Where(ev => ev.GameUuid == gameUuid)
                .Select(GameSeriesEventSerializer.Convert)
                .Count(e => e is RoundStarted);
            var lastRoundIndex = roundsStarted - 1;
            ctx.Events
                .RemoveRange(ctx.Events
                    .Where(ev => ev.GameUuid == gameUuid && ev.RoundIndex < lastRoundIndex)
                );
            ctx.SaveChanges();
        }
    }
}