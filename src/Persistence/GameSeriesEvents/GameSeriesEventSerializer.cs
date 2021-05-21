using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Persistance.GameEvents
{
    public class GameSeriesEventSerializer
    {

        public static PersistedSeriesEvent Convert(GameSeriesEvent ev)
        {
            return new PersistedSeriesEvent
            {
                GameUuid = ev.GameUuid,
                Payload = ConvertPayload(ev),
                Type = ConvertType(ev)
            };
        }

        public static GameSeriesEvent Convert(PersistedSeriesEvent persisted)
        {
            var gameUuid = persisted.GameUuid;
            return persisted.Type switch
            {
                PersistedSeriesEventType.SeriesStarted =>
                    RecreateSeriesStarted(gameUuid, JsonSerializer.Deserialize<SeriesStartedPayload>(persisted.Payload)),
                PersistedSeriesEventType.RoundStarted =>
                    RecreateRoundStarted(gameUuid, JsonSerializer.Deserialize<RoundStartedPayload>(persisted.Payload)),
                PersistedSeriesEventType.RoundEnded =>
                    RecreateRoundEnded(gameUuid, JsonSerializer.Deserialize<RoundEndedPayload>(persisted.Payload)),
                _ => throw new NotImplementedException()
            };
        }

        private static RoundEnded RecreateRoundEnded(string gameUuid, RoundEndedPayload roundEndedPayload)
            => new RoundEnded(gameUuid, roundEndedPayload.Settings, new(roundEndedPayload.PlayerResults.ToImmutableList()));

        private static RoundStarted RecreateRoundStarted(string gameUuid, RoundStartedPayload roundStartedPayload)
            => new RoundStarted(gameUuid, roundStartedPayload.Settings);

        private static GameSeriesStarted RecreateSeriesStarted(string gameUuid, SeriesStartedPayload payload)
        {
            var players = payload.Players;
            var plan = EumelGamePlan.For(players.Count);
            return new GameSeriesStarted(gameUuid, players, plan);
        }

        private static PersistedSeriesEventType ConvertType(GameSeriesEvent ev)
        => ev switch
        {
            GameSeriesStarted _ => PersistedSeriesEventType.SeriesStarted,
            RoundStarted _ => PersistedSeriesEventType.RoundStarted,
            RoundEnded _ => PersistedSeriesEventType.RoundEnded,
            _ => throw new NotImplementedException()
        };

        private static string ConvertPayload(GameSeriesEvent ev)
        {
            var mapped = MapPayload(ev);
            return JsonSerializer.Serialize(mapped, mapped.GetType());
        }

        private static GameSeriesEventPayload MapPayload(GameSeriesEvent ev)
        => ev switch
        {
            GameSeriesStarted seriesStarted => new SeriesStartedPayload(seriesStarted),
            RoundStarted roundStarted => new RoundStartedPayload(roundStarted),
            RoundEnded roundEnded => new RoundEndedPayload(roundEnded),
            _ => throw new NotImplementedException()
        };

    }
}