using System;
using System.Collections.Generic;
using System.Linq;

namespace Eumel.Core
{
    public class GameCardDeck
    {
        private readonly List<Card> _allCards;
        private readonly Random _random;

        public int Count => _allCards.Count;

        public Card this[int index] => _allCards[index];

        public IEnumerable<Card> AllCards => _allCards;

        public GameCardDeck(Rank minRank)
        {
            var allSuits = Enum.GetValues(typeof(Suit)).Cast<Suit>();
            var allRanks = Enum.GetValues(typeof(Rank)).Cast<Rank>();
            var ranksToUse = allRanks.Where(rank => rank >= minRank);
            _allCards = allSuits
                .SelectMany(suit => ranksToUse
                    .Select(rank => new Card(suit, rank)))
                .ToList();
            _random = new Random();
        }

        public List<KnownHand> DrawXTimesY(int x, int y)
        {
            if (x * y > _allCards.Count)
            {
                throw new ArgumentException();
            }
            var shuffled = _allCards.OrderBy(_ => _random.Next()).Take(x * y).ToList();
            return Enumerable.Range(0, x)
                .Select(offset =>
                    new KnownHand(shuffled.Skip(offset * y).Take(y)))
                .ToList();
        }

        public override bool Equals(object obj)
        => obj is GameCardDeck deck && Enumerable.SequenceEqual(AllCards, deck.AllCards);

        // Hashcode is based on creation rule (by minRank)
        public override int GetHashCode() => _allCards.First().GetHashCode();
    }
}