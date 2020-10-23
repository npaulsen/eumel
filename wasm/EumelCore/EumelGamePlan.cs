using System;
using System.Collections.Generic;
using System.Linq;

namespace EumelCore
{
    public class EumelGamePlan
    {
        public readonly IReadOnlyList<EumelRoundSettings> PlannedRounds;
        public readonly GameCardDeck Deck;

        public EumelGamePlan(IReadOnlyList<EumelRoundSettings> plannedRounds, GameCardDeck deck)
        {
            PlannedRounds = plannedRounds;
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
            var minRank = Rank.Two;
            if (playerCount < 3 || playerCount > 6)
            {
                throw new ArgumentException("PlayerCount must be between 3 and 6");
            }
            if (playerCount == 3)
            {
                minRank = Rank.Six;
            }
            else if (playerCount == 4)
            {
                minRank = Rank.Two;
            }
            else if (playerCount == 5)
            {
                minRank = Rank.Five;
            }
            else
            {
                minRank = Rank.Three;
            }
            return new GameCardDeck(minRank);
        }
    }
}