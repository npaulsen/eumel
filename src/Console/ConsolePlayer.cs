using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;

namespace EumelConsole
{
    public class ConsolePlayer : IInvokablePlayer
    {
        public Card GetMove(GameState state)
        {
            PrintPlayerState(state.Players.ToList());
            PrintCurrentTrick(state.CurrentTrick);
            PrintOwnCards(state);
            var validCards = state.Players[state.Turn.PlayerIndex].Hand as KnownHand;
            var enteredIndex = ConsoleUi.PromptInt("Which card to play? Enter #: ", 1, validCards.NumberOfCards);
            return validCards[enteredIndex - 1];
        }

        public int GetGuess(GameState state)
        {
            PrintOwnCards(state);
            var otherPlayerIndices = Enumerable.Range(state.Turn.PlayerIndex + 1, state.Players.Count - 1)
                .Select(index => index % state.Players.Count);
            foreach (var otherPlayerIndex in otherPlayerIndices)
            {
                var guess = state.Players[otherPlayerIndex].Guess;
                if (guess.HasValue)
                {
                    System.Console.WriteLine($"P{otherPlayerIndex+1}: {guess}");
                }
            }
            return ConsoleUi.PromptInt("Enter your guess: ", 0, 100);
        }

        private void PrintPlayerState(IEnumerable<PlayerState> players)
        {
            Console.WriteLine(string.Join("   ", players.Select((PlayerState state, int index) => $"P{index+1}: {state.TricksWon} / {state.Guess}")));
        }
        private void PrintCurrentTrick(TrickState currentTrick)
        {
            if (!currentTrick.AnyPlayed)
            {
                Console.WriteLine("You're first to play a card.");
            }
            else
            {
                Console.WriteLine(string.Join("  ", currentTrick.Moves.Select(m => $"{m.Card} (P{m.Player+1})")));
            }
        }
        private static void PrintOwnCards(GameState state)
        {
            Console.WriteLine("Your cards: " + state.Players[state.Turn.PlayerIndex].Hand);
        }

        public void NoteSeriesStart(GameSeriesStarted seriesStarted) { }
    }
}