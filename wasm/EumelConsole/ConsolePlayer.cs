using System;
using System.Collections.Generic;
using System.Linq;
using EumelCore;

namespace EumelConsole
{
    public class ConsolePlayer : IInvocablePlayer
    {
        private readonly IReadOnlyList<PlayerInfo> _players;

        public ConsolePlayer(List<PlayerInfo> players)
        {
            _players = players;
        }

        public Card GetMove(GameState state)
        {
            PrintPlayerState(state.Players);
            PrintCurrentTrick(state.CurrentTrick);
            PrintOwnCards(state);
            var validCards = CurrentRound.State.Players[state.TurnOfPlayerIndex].Hand;
            var enteredIndex = PromptInt("Which card to play? Enter #: ", 1, validCards.NumberOfCards);
            return validCards[enteredIndex - 1];
        }

        public int GetGuess(GameState state)
        {
            PrintOwnCards(state);
            var otherPlayerIndices = Enumerable.Range(state.TurnOfPlayerIndex + 1, state.Players.Count - 1)
                .Select(index => index % state.Players.Count);
            foreach (var otherPlayerIndex in otherPlayerIndices)
            {
                var guess = state.Players[otherPlayerIndex].Guess;
                if (guess.HasValue)
                {
                    System.Console.WriteLine(_players[otherPlayerIndex].Name + ": " + guess);
                }
            }
            return PromptInt("Enter your guess: ", 0, 100);
        }

        private void PrintPlayerState(IEnumerable<GameState.PlayerState> players)
        {
            Console.WriteLine(string.Join("   ", players.Select((GameState.PlayerState state, int index) => $"{_players[index].Name}: {state.TricksWon} / {state.Guess}")));
        }
        private void PrintCurrentTrick(TrickState currentTrick)
        {
            if (!currentTrick.AnyPlayed)
            {
                Console.WriteLine("You're first to play a card.");
            }
            else
            {
                Console.WriteLine(string.Join("  ", currentTrick.Moves.Select(m => $"{m.Card} ({m.Player.Name})")));
            }
        }
        private static void PrintOwnCards(GameState state)
        {
            Console.WriteLine("Your cards: " + state.Players[state.TurnOfPlayerIndex].Hand);
        }

        private int PromptInt(string prompt, int min, int max)
        {
            do
            {
                var oldFc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(prompt);
                Console.ForegroundColor = oldFc;
                var isInteger = int.TryParse(System.Console.ReadLine(), out int enteredInt);

                if (!isInteger || enteredInt < min || enteredInt > max)
                {
                    Console.WriteLine($" -> Enter an integer number between {min} and {max}.");
                    continue;
                }
                return enteredInt;
            } while (true);
        }
    }
}