using System;
using System.Linq;
using System.Text.Json;
using Eumel.Core;

namespace Eumel.Persistance.GameEvents
{
    public class GameEventSerializer
    {

        public static PersistedEvent Convert(GameEvent ev)
        {
            return new PersistedEvent
            {
                GameUuid = ev.Context.GameId,
                RoundIndex = ev.Context.RoundIndex,
                Payload = ConvertPayload(ev),
                Type = ConvertType(ev)
            };
        }

        public static GameEvent Convert(PersistedEvent persisted)
        {
            var context = new GameEventContext(persisted.GameUuid, persisted.RoundIndex);
            return persisted.Type switch
            {
                PersistedEventType.HandReceived =>
                    RecreateHandReceived(context, JsonSerializer.Deserialize<HandReceivedPayload>(persisted.Payload)),
                PersistedEventType.GuessGiven =>
                    RecreateGuessGiven(context, JsonSerializer.Deserialize<GuessPayload>(persisted.Payload)),
                PersistedEventType.CardPlayed =>
                    RecreateCardPlayed(context, JsonSerializer.Deserialize<CardPlayedPayload>(persisted.Payload)),
                PersistedEventType.TrickWon =>
                    RecreateTrickWon(context, JsonSerializer.Deserialize<TrickWonPayload>(persisted.Payload)),
                _ => throw new NotImplementedException()
            };
        }

        private static GuessGiven RecreateGuessGiven(GameEventContext context, GuessPayload payload)
        => new GuessGiven(context, new PlayerIndex(payload.PlayerIndex), payload.Guess);

        private static CardPlayed RecreateCardPlayed(GameEventContext context, CardPlayedPayload payload)
        => new CardPlayed(
            context,
            new PlayerIndex(payload.PlayerIndex),
            new Card(payload.Card.Suit, payload.Card.Rank)
        );

        private static TrickWon RecreateTrickWon(GameEventContext context, TrickWonPayload payload)
        => new TrickWon(context, new PlayerIndex(payload.PlayerIndex));

        private static HandReceived RecreateHandReceived(GameEventContext context, HandReceivedPayload payload)
        {
            var cards = payload.Cards.Select(c => new Card(c.Suit, c.Rank));
            var hand = new KnownHand(cards);
            return new HandReceived(context, new PlayerIndex(payload.PlayerIndex), hand);
        }

        private static PersistedEventType ConvertType(GameEvent ev)
        => ev switch
        {
            HandReceived _ => PersistedEventType.HandReceived,
            GuessGiven _ => PersistedEventType.GuessGiven,
            CardPlayed _ => PersistedEventType.CardPlayed,
            TrickWon _ => PersistedEventType.TrickWon,
            _ => throw new NotImplementedException()
        };

        private static string ConvertPayload(GameEvent ev)
        {
            var mapped = MapPayload(ev);
            return JsonSerializer.Serialize(mapped, mapped.GetType());
        }

        private static GameEventPayload MapPayload(GameEvent ev)
        => ev switch
        {
            HandReceived hand => new HandReceivedPayload(hand),
            GuessGiven guess => new GuessPayload(guess),
            CardPlayed card => new CardPlayedPayload(card),
            TrickWon trick => new TrickWonPayload(trick),
            _ => throw new NotImplementedException()
        };

    }
}