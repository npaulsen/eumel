using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static System.Environment;

namespace Eumel.Core
{
    public record GameState
    {
        public readonly ImmutableListWithValueSemantics<PlayerState> Players;

        public readonly TrickState CurrentTrick;

        public readonly TurnState Turn;

        private GameState(ImmutableList<PlayerState> players, TrickState currentTrick, TurnState turn)
        {
            Players = players.WithValueSemantics();
            CurrentTrick = currentTrick;
            Turn = turn;
        }

        public int TricksPlayed => Players.Sum(p => p.TricksWon);
        public bool AllTricksPlayed => Players[0].Hand.IsEmpty();

        public static GameState Initial(int players, EumelRoundSettings settings) =>
            new GameState(
                Enumerable.Range(0, players).Select(PlayerState.Initial).ToImmutableList(),
                TrickState.Initial,
                new TurnState(settings.StartingPlayerIndex, typeof(HandReceived)));

        public GameState Dispatch(GameEvent gameEvent) =>
            new GameState(Players.Select(p => p.Dispatch(gameEvent)).ToImmutableList(),
                CurrentTrick.Dispatch(gameEvent),
                GetNextTurn(gameEvent));

        private TurnState GetNextTurn(GameEvent gameEvent) => gameEvent
        switch
        {
            HandReceived hand => Next(hand),
            GuessGiven guess => Next(guess),
            CardPlayed move => Next(move),
            TrickWon won => Next(won),
            _ => throw new InvalidOperationException(),
        };

        private TurnState Next(GuessGiven guess)
        {
            var nextPlayer = (Turn.PlayerIndex + 1) % Players.Count;
            var isLastGuess = Players[nextPlayer].Guess.HasValue;
            var nextType = isLastGuess? typeof(CardPlayed) : typeof(GuessGiven);
            return new TurnState(nextPlayer, nextType);
        }
        private TurnState Next(CardPlayed move)
        {
            var nextPlayer = (Turn.PlayerIndex + 1) % Players.Count;
            var isLastMove = CurrentTrick.Moves.Count + 1 == Players.Count;
            var nextType = isLastMove? typeof(TrickWon) : typeof(CardPlayed);
            return new TurnState(nextPlayer, nextType);
        }
        private TurnState Next(HandReceived hand)
        {
            var nextPlayer = (Turn.PlayerIndex + 1) % Players.Count;
            var isLastHand = Players[nextPlayer].Hand != null;
            var nextType = isLastHand? typeof(GuessGiven) : typeof(HandReceived);
            return new TurnState(nextPlayer, nextType);
        }
        private TurnState Next(TrickWon won)
        {
            var nextPlayer = won.Player;
            if (Players[nextPlayer].Hand.IsEmpty())
            {
                return TurnState.RoundIsOver;
            }
            return new TurnState(nextPlayer, typeof(CardPlayed));
        }

        public bool IsValid(GuessGiven guess)
        {
            if (!IsMoveAllowed(guess))
            {
                System.Console.WriteLine("Not your turn!");
                return false;
            }
            var isLastGuess = Players.Count(p => !p.Guess.HasValue) == 1;
            if (!isLastGuess)
            {
                return true;
            }
            var totalGuessed = guess.Count + Players.Select(p => p.Guess).Where(g => g.HasValue).Sum();
            return totalGuessed != Players.First().Hand.NumberOfCards;
        }

        public bool IsValid(CardPlayed move)
        {
            if (!IsMoveAllowed(move))
            {
                System.Console.WriteLine("Not your turn!");
                return false;
            }
            var card = move.Card;
            var playersHand = Players[move.Player].Hand;
            if (playersHand is KnownHand knownHand)
            {
                var currentSuit = CurrentTrick.Suit;
                var switchesSuit = currentSuit.HasValue && card.Suit != currentSuit;
                if (switchesSuit && knownHand.MustFollow(currentSuit.Value))
                {
                    return false;
                }
                return knownHand.Has(card);
            }
            return true;
        }

        private bool IsMoveAllowed(Move move) => move.Player == Turn.PlayerIndex && move.GetType() == Turn.NextEventType;

        public override string ToString() => $"{string.Join(NewLine, Players)}{NewLine}{CurrentTrick}{NewLine}{Turn}";

    }
}