using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EumelCore.Players
{
    public class DumbPlayer : Player
    {

        private static List<Suit> AllSuits = System.Enum.GetValues(typeof(Suit)).Cast<Suit>().ToList();
        private static List<Rank> AllRanks = System.Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();

        private static Random Rand = new Random();

        private readonly int _delay;

        public DumbPlayer(int delay)
        {
            _delay = delay;
        }

        public override int GetGuess(GameState state)
        {
            Thread.Sleep(_delay);
            var myHand = state.Players[state.Turn.PlayerIndex].Hand;
            return Math.Min(Rand.Next(myHand.NumberOfCards + 1), Rand.Next(myHand.NumberOfCards + 1));
        }

        public override Card GetMove(GameState state)
        {
            Thread.Sleep(_delay);
            var myHand = state.Players[state.Turn.PlayerIndex].Hand;
            return myHand[Rand.Next(myHand.NumberOfCards)];
        }

    }
}