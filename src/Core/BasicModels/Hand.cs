using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Eumel.Core
{
    public interface IHand
    {
        int NumberOfCards { get; }
        IHand Play(Card card);
    }

    public static class IHandExtensions
    {
        public static bool IsEmpty(this IHand self) => self.NumberOfCards == 0;
    }

    public class KnownHand : IHand, IEnumerable<Card>
    {
        private readonly IReadOnlyList<Card> _cards;

        public int NumberOfCards => _cards.Count;

        public Card this[int index] => _cards[index];

        public KnownHand(IEnumerable<Card> cards)
        {
            _cards = cards.ToList();
        }

        public IHand Play(Card card)
        {
            if (!_cards.Contains(card))
            {
                throw new InvalidOperationException("Cannot play " + card);
            }
            return new KnownHand(_cards.Where(c => c != card));
        }
        public bool MustFollow(Suit currentSuit) => _cards.Any(c => c.Suit == currentSuit);
        public bool Has(Card card) => _cards.Contains(card);

        public override string ToString() => $"[{string.Join(", ", _cards)}]";

        public IEnumerator<Card> GetEnumerator() => _cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _cards.GetEnumerator();

    }

    public class UnknownHand : IHand
    {
        public int NumberOfCards { get; }

        public UnknownHand(int numberOfCards)
        {
            if (numberOfCards < 0)
            {
                throw new ArgumentOutOfRangeException("Hand cannot have negative number of cards.");
            }
            NumberOfCards = numberOfCards;
        }
        public IHand Play(Card card) => new UnknownHand(NumberOfCards - 1);

        public override string ToString() => $"[{string.Join(", ", Enumerable.Repeat("?", NumberOfCards))}]";

    }
}