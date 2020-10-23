using System;

namespace EumelCore
{
    public class Card
    {
        public readonly Suit Suit;
        public readonly Rank Rank;

        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public static bool operator >(Card a, Card b) => a.Suit > b.Suit || (a.Suit == b.Suit && a.Rank > b.Rank);
        public static bool operator <(Card a, Card b) => a.Suit < b.Suit || (a.Suit == b.Suit && a.Rank < b.Rank);

        public static bool operator ==(Card a, Card b) => a.Suit == b.Suit && a.Rank == b.Rank;
        public static bool operator !=(Card a, Card b) => a.Suit != b.Suit || a.Rank != b.Rank;

        public override bool Equals(object obj) =>
            obj is Card card && Suit == card.Suit && Rank == card.Rank;

        public override int GetHashCode() => HashCode.Combine(Suit, Rank);

        public override string ToString()
        {
            var rank = Rank
            switch
            {
                Rank.Two => "2",
                Rank.Three => "3",
                Rank.Four => "4",
                Rank.Five => "5",
                Rank.Six => "6",
                Rank.Seven => "7",
                Rank.Eight => "8",
                Rank.Nine => "9",
                Rank.Ten => "10",
                Rank.Jack => "J",
                Rank.Queen => "Q",
                Rank.King => "K",
                Rank.Ace => "A",
                _ =>
                throw new ArgumentException("unknown rank: " + Rank)
            };
            var suit = Suit
            switch
            {
                Suit.Club => "♣",
                Suit.Diamonds => "♦",
                Suit.Hearts => "♥",
                Suit.Spade => "♠",
                _ =>
                throw new ArgumentException("unknown suit: " + Suit)
            };
            return suit + rank;
        }
    }
}