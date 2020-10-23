using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EumelCore
{
    public class Hand : IEnumerable<Card>
    {
        private readonly IReadOnlyList<Card> _cards;

        public int NumberOfCards => _cards.Count;

        public bool IsEmpty => NumberOfCards == 0;

        public Card this[int index] => _cards[index];

        public Hand(IEnumerable<Card> cards)
        {
            _cards = cards.ToList();
        }

        public Hand Play(Card card)
        {
            if (!_cards.Contains(card))
            {
                throw new InvalidOperationException("Cannot play " + card);
            }
            return new Hand(_cards.Where(c => c != card));
        }
        public bool MustFollow(Suit currentSuit) => _cards.Any(c => c.Suit == currentSuit);
        public bool Has(Card card) => _cards.Contains(card);

        public override string ToString() => $"[{string.Join(", ", _cards)}]";

        public IEnumerator<Card> GetEnumerator() => _cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _cards.GetEnumerator();

    }
}