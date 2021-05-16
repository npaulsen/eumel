using System;

namespace Eumel.Core
{
    public record Card(Suit Suit, Rank Rank) : IComparable<Card>
    {
        public static bool operator >(Card a, Card b) => a.Suit > b.Suit || (a.Suit == b.Suit && a.Rank > b.Rank);
        public static bool operator <(Card a, Card b) => a.Suit < b.Suit || (a.Suit == b.Suit && a.Rank < b.Rank);

        public override string ToString() => SuitString + RankString;

        public int CompareTo(Card other)
        {
            if (Suit == other.Suit) return Rank.CompareTo(other.Rank);
            return Suit < other.Suit ? -1 : 1;
        }

        public string RankString => Rank
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

        public string SuitString => Suit
        switch
        {
            Suit.Club => "♣",
            Suit.Diamonds => "♦",
            Suit.Hearts => "♥",
            Suit.Spade => "♠",
            _ =>
            throw new ArgumentException("unknown suit: " + Suit)
        };
    }
}