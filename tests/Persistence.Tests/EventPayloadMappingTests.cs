using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using Eumel.Persistance.GameEvents;

namespace Persistence
{
    public class EventPayloadMappingTests
    {
        [Fact]
        public void HandReceivedIsMappedAndBack()
        {
            var ctx = new GameEventContext("g", 12);
            var handReceivedEvent = new HandReceived(ctx, new PlayerIndex(4), new KnownHand(new[] { new Card(Suit.Club, Rank.Four), new Card(Suit.Hearts, Rank.Ace)}));

            var persistable = GameEventSerializer.Convert(handReceivedEvent);
            var recreated = GameEventSerializer.Convert(persistable) as HandReceived;

            Assert.Equal(4, recreated.Player.Value);
            Assert.Equal(handReceivedEvent.Hand, recreated.Hand);
        }

        [Fact]
        public void GuessGivenIsMappedAndBack()
        {
            var ctx = new GameEventContext("g", 12);
            var guessGiven = new GuessGiven(ctx, new PlayerIndex(1), 23);

            var persistable = GameEventSerializer.Convert(guessGiven);
            var recreated = GameEventSerializer.Convert(persistable) as GuessGiven;

            Assert.Equal(1, recreated.Player.Value);
            Assert.Equal(23, recreated.Count);
        }

        [Fact]
        public void CardPlayedIsMappedAndBack()
        {
            var ctx = new GameEventContext("g", 12);
            var aCard = new Card(Suit.Diamonds, Rank.King);
            var cardPlayed = new CardPlayed(ctx, new PlayerIndex(2), aCard);

            var persistable = GameEventSerializer.Convert(cardPlayed);
            var recreated = GameEventSerializer.Convert(persistable) as CardPlayed;

            Assert.Equal(2, recreated.Player.Value);
            Assert.Equal(aCard, recreated.Card);
        }

        [Fact]
        public void TrickWonIsMappedAndBack()
        {
            var ctx = new GameEventContext("g", 12);
            var trickWon = new TrickWon(ctx, new PlayerIndex(1));

            var persistable = GameEventSerializer.Convert(trickWon);
            var recreated = GameEventSerializer.Convert(persistable) as TrickWon;

            Assert.Equal(1, recreated.Player.Value);
        }

        [Fact]
        public void GameSeriesStartIsMappedAndBack()
        {
            var playerInfos = new List<PlayerInfo> { 
                new PlayerInfo("a", "t1"), 
                new PlayerInfo("b", "t2"),
                new PlayerInfo("c", "t1")
            };
            var plan = EumelGamePlan.For(3);
            var seriesStarted = new GameSeriesStarted("uuid", playerInfos, plan);

            var persistable = GameSeriesEventSerializer.Convert(seriesStarted);
            var recreated = GameSeriesEventSerializer.Convert(persistable) as GameSeriesStarted;

            Assert.Equal(seriesStarted.Plan, recreated.Plan);
            Assert.Equal(seriesStarted.Players, recreated.Players);
            Assert.Equal(seriesStarted, recreated);
        }

        [Fact]
        public void RoundStartedIsMappedAndBack()
        {
            var roundStarted = new RoundStarted("uuid", new (2, 10));

            var persistable = GameSeriesEventSerializer.Convert(roundStarted);
            var recreated = GameSeriesEventSerializer.Convert(persistable) as RoundStarted;

            Assert.Equal(roundStarted, recreated);
        }

        [Fact]
        public void RoundEndedIsMappedAndBack()
        {
            var result = new RoundResult(
                new PlayerRoundResult[] { new (1, 2, 3), new (10, 0, 1)}
                .ToImmutableList());
            var roundEnded = new RoundEnded("uuid", new (2, 10), result);

            var persistable = GameSeriesEventSerializer.Convert(roundEnded);
            var recreated = GameSeriesEventSerializer.Convert(persistable) as RoundEnded;

            Assert.Equal(roundEnded, recreated);
        }
    }
}
