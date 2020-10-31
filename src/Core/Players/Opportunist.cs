using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core.Players
{
    public class Opportunist : BasePlayer
    {
        public override int GetGuess(GameState state)
        {
            var myHand = state.Players[state.Turn.PlayerIndex].Hand as KnownHand;
            double estimate = SimpleEstimate(state, myHand);

            return GetGuessFromEstimate(state, estimate);
        }

        private double SimpleEstimate(GameState state, KnownHand myHand)
        {
            return myHand.Sum(card =>
            {
                var relativePositionInDeck = 1.0 * _cardIndices[card] / (Deck.Count - 1);
                return Math.Pow(relativePositionInDeck, state.Players.Count);

            });
        }

        public override Card GetMove(GameState state)
        {
            var trick = state.CurrentTrick;
            var playerState = state.Players[state.Turn.PlayerIndex];
            var hand = playerState.Hand as KnownHand;

            var currentGuessOnHand = SimpleEstimate(state, hand);
            var remainingToTarget = playerState.Guess - playerState.TricksWon;
            var getAsManyAsPossible = remainingToTarget < 0 || currentGuessOnHand <= remainingToTarget;

            if (trick.NonePlayedYet)
            {
                // TODO: free mid colors where few in hand?
                return hand.First();
            }

            var suitToFollow = hand.Any(c => c.Suit == trick.Suit);
            var playableCards = suitToFollow? hand.Where(c => c.Suit == trick.Suit) : hand;
            var lowToHigh = playableCards.OrderBy(c => c).ToList();
            var highestInTrick = trick.HighestCard;

            if (getAsManyAsPossible)
            {
                var lowestWinningCard = lowToHigh.FirstOrDefault(c => c > highestInTrick);
                return lowestWinningCard ?? lowToHigh.First();
            }
            else
            {
                var highestNonWinningCard = lowToHigh.LastOrDefault(c => c < highestInTrick);
                return highestNonWinningCard ?? lowToHigh.Last();
            }
        }
    }
}