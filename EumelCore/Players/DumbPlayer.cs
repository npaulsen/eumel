using System;
using System.Collections.Generic;
using System.Linq;

namespace EumelCore.Players
{
    public class DumbPlayer : Player
    {

        private static List<Suit> AllSuits = System.Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList();
        private static List<Rank> AllRanks = System.Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();

        private static Random Rand = new Random();

        public override int GetGuess()
        {
            var myHand = CurrentRound.State.Players[CurrentRound.State.TurnOfPlayerIndex].Hand;
            return Math.Min(Rand.Next(myHand.NumberOfCards + 1), Rand.Next(myHand.NumberOfCards + 1));
        }

        public override Card GetMove()
        {
            var suit = AllSuits.Skip(Rand.Next(AllSuits.Count)).First();
            var rank = AllRanks.Skip(Rand.Next(AllRanks.Count)).First();
            return new Card(suit, rank);
        }

    }
}