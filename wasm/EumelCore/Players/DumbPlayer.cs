using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EumelCore.Players
{
    public class DumbPlayer : IInvocablePlayer
    {
        private static Random Rand = new Random();

        public int GetGuess(GameState state)
        {
            var myHand = state.Players[state.Turn.PlayerIndex].Hand;
            return Math.Min(Rand.Next(myHand.NumberOfCards + 1), Rand.Next(myHand.NumberOfCards + 1));
        }

        public Card GetMove(GameState state)
        {
            var myHand = state.Players[state.Turn.PlayerIndex].Hand;
            return myHand[Rand.Next(myHand.NumberOfCards)];
        }

    }
}