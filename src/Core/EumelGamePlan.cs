using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Eumel.Core
{

    public record EumelGamePlan
    {
        public readonly ImmutableListWithValueSemantics<EumelRoundSettings> Rounds;
        public readonly GameCardDeck Deck;

        public EumelGamePlan(IEnumerable<EumelRoundSettings> plannedRounds, GameCardDeck deck)
        {
            Rounds = plannedRounds.ToImmutableList().WithValueSemantics();
            Deck = deck;
        }

        public static EumelGamePlan For(int players)
        {
            var deck = PrepareDeck(players);
            var plannedRounds = PrepareRounds(deck, players);
            return new EumelGamePlan(plannedRounds, deck);
        }
        private static List<EumelRoundSettings> PrepareRounds(GameCardDeck deck, int playerCount)
        {
            var maxTricks = deck.Count / playerCount;
            var rounds = Enumerable.Range(1, maxTricks)
                .Concat(Enumerable.Range(1, maxTricks - 1).Reverse());
            return rounds.Select((r, i) => new EumelRoundSettings(i % playerCount, r)).ToList();
        }

        private static GameCardDeck PrepareDeck(int playerCount)
        {
            var minRank = GetMinRankForPlayerCount(playerCount);
            return new GameCardDeck(minRank);
        }

        public static Rank GetMinRankForPlayerCount(int playerCount)
            => playerCount switch
            {
                3 => Rank.Six,
                4 => Rank.Two,
                5 => Rank.Five,
                6 => Rank.Three,
                _ => throw new ArgumentException("PlayerCount must be between 3 and 6")
            };
    }
}