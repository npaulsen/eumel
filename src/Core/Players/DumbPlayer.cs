using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core.Players
{
    public class DumbPlayer : IInvokablePlayer
    {
        private static readonly Random Rand = new();

        public int GetGuess(GameState state)
        {
            var myHand = state.Players[state.Turn.PlayerIndex].Hand;
            return Math.Min(Rand.Next(myHand.NumberOfCards + 1), Rand.Next(myHand.NumberOfCards + 1));
        }

        public Card GetMove(GameState state)
        {
            var myHand = state.Players[state.Turn.PlayerIndex].Hand as KnownHand;
            return myHand[Rand.Next(myHand.NumberOfCards)];
        }

        public void NoteSeriesStart(GameSeriesStarted seriesStarted) { }
    }
}