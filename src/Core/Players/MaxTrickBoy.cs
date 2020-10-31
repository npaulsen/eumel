using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core.Players
{
    public class MaxTrickBoy : IInvocablePlayer
    {
        private static Random Rand = new Random();

        private GameCardDeck _hardcodedDeck;
        private Dictionary<Card, int> _cardIndices;

        public void NoteSeriesStart(GameSeriesStarted seriesStarted)
        {
            _hardcodedDeck = seriesStarted.Deck;
            _cardIndices = new Dictionary<Card, int>();
            foreach (var(card, index) in _hardcodedDeck.AllCards.Select((c, i) => (c, i)))
            {
                _cardIndices.Add(card, index);
            }
        }

        public int GetGuess(GameState state)
        {
            var myHand = state.Players[state.Turn.PlayerIndex].Hand as KnownHand;
            var estimate = myHand.Sum(card =>
            {
                var relativePositionInDeck = 1.0 * _cardIndices[card] / (_hardcodedDeck.Count - 1);
                return Math.Pow(relativePositionInDeck, state.Players.Count + 1);
            });
            return GetGuessFromEstimate(state, estimate);
        }

        private static int GetGuessFromEstimate(GameState state, double estimate)
        {
            var roundedGuess = (int) Math.Round(estimate);
            int forbiddenGuess = GetForbiddenGuess(state);
            if (roundedGuess == forbiddenGuess)
            {
                if (roundedGuess <= estimate) return roundedGuess + 1;
                else if (roundedGuess > estimate) return roundedGuess - 1;
            }
            return roundedGuess;
        }

        private static int GetForbiddenGuess(GameState state)
        {
            var playersThatGuessed = state.Players.Count(p => p.Guess.HasValue);
            var isLastGuess = playersThatGuessed == state.Players.Count - 1;
            if (!isLastGuess)
                return -1;
            var tricks = state.Players[0].Hand.NumberOfCards;
            var totalGuessed = state.Players.Sum(p => p.Guess ?? 0);
            var forbiddenGuess = tricks - totalGuessed;
            return forbiddenGuess;
        }

        public Card GetMove(GameState state)
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