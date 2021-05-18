using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core.Players
{
    public abstract class BasePlayer : IInvokablePlayer
    {
        protected GameCardDeck Deck { get; private set; }
        protected Dictionary<Card, int> _cardIndices;

        public abstract int GetGuess(GameState state);

        public abstract Card GetMove(GameState state);

        public void NoteSeriesStart(GameSeriesStarted seriesStarted)
        {
            Deck = seriesStarted.Plan.Deck;
            _cardIndices = new Dictionary<Card, int>();
            foreach (var (card, index) in Deck.AllCards.Select((c, i) => (c, i)))
            {
                _cardIndices.Add(card, index);
            }
        }

        protected static int GetGuessFromEstimate(GameState state, double estimate)
        {
            var roundedGuess = (int)Math.Round(estimate);
            int forbiddenGuess = GetForbiddenGuess(state);
            if (roundedGuess == forbiddenGuess)
            {
                if (roundedGuess <= estimate) return roundedGuess + 1;
                else if (roundedGuess > estimate) return roundedGuess - 1;
            }
            return roundedGuess;
        }

        protected static int GetForbiddenGuess(GameState state)
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
    }
}