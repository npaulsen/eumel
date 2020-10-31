using System;
using System.Linq;

namespace Eumel.Core.Players
{
    public class MaxTrickBoy : BasePlayer
    {

        public override int GetGuess(GameState state)
        {
            var myHand = state.Players[state.Turn.PlayerIndex].Hand as KnownHand;
            var estimate = myHand.Sum(card =>
            {
                var relativePositionInDeck = 1.0 * _cardIndices[card] / (Deck.Count - 1);
                return Math.Pow(relativePositionInDeck, state.Players.Count + 1);
            });
            return GetGuessFromEstimate(state, estimate);
        }

        public override Card GetMove(GameState state)
        {
            var trick = state.CurrentTrick;
            var hand = state.Players[state.Turn.PlayerIndex].Hand as KnownHand;
            if (trick.NonePlayedYet)
            {
                // TODO: free mid colors where few in hand?
                return hand.First();
            }

            var suitToFollow = hand.Any(c => c.Suit == trick.Suit);
            var playableCards = suitToFollow? hand.Where(c => c.Suit == trick.Suit) : hand;
            var lowToHigh = playableCards.OrderBy(c => c).ToList();
            var highestInTrick = trick.HighestCard;
            var lowestWinningCard = lowToHigh.FirstOrDefault(c => c > highestInTrick);
            return lowestWinningCard ?? lowToHigh.First();
        }
    }
}