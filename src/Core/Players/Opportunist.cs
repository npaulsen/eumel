using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core.Players
{
    public class Opportunist : IInvocablePlayer
    {
        private static Random Rand = new Random();

        private GameCardDeck _hardcodedDeck;
        private Dictionary<Card, int> _cardIndices;

        private double _param;

        public Opportunist(double param)
        {
            _param = param;
        }

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
                var suitCorrection = .05 * _param;
                var minRank = _hardcodedDeck[0].Rank;
                var rankRatio = 1.0 * (card.Rank - minRank) / (Rank.Ace - minRank);
                var relativePositionInDeck2 = (rankRatio * (.25 + suitCorrection)) + ((int) card.Suit / 4.0) / (1 + suitCorrection);

                // var relativePositionInDeck = 1.0 * _cardIndices[card] / (_hardcodedDeck.Count - 1);
                return Math.Pow(relativePositionInDeck2, state.Players.Count);

            });

            // var averageTricksPerPlayer = myHand.NumberOfCards * 1.0 / state.Players.Count;
            // var numberOPlayersThatGuessed = state.Players.Count(p => p.Guess.HasValue);
            // var numberOPlayersThatDidntGuess = state.Players.Count - numberOPlayersThatGuessed;
            // var sumOfPreviousGuesses = state.Players.Sum(p => p.Guess ?? 0);
            // var averageTricksRemainingPerPlayer = (myHand.NumberOfCards - sumOfPreviousGuesses) / numberOPlayersThatDidntGuess;

            // var guessCorrectionFactor = averageTricksRemainingPerPlayer / averageTricksPerPlayer;
            // if (guessCorrectionFactor > 0 && myHand.NumberOfCards < 4)
            //     estimate *= ((1 - _param) + guessCorrectionFactor * _param);

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
            var playerState = state.Players[state.Turn.PlayerIndex];
            var hand = playerState.Hand as KnownHand;
            if (trick.NonePlayedYet)
            {

                // TODO: free mid colors where few in hand?
                return hand.First();
            }

            var suitToFollow = hand.Any(c => c.Suit == trick.Suit);
            var playableCards = suitToFollow? hand.Where(c => c.Suit == trick.Suit) : hand;
            var lowToHigh = playableCards.OrderBy(c => c).ToList();
            var highestInTrick = trick.HighestCard;

            var currentGuessOnHand = 4;
            var remainingToTarget = playerState.Guess - playerState.TricksWon;
            var getAsManyAsPossible = remainingToTarget < 0 || currentGuessOnHand <= remainingToTarget;

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