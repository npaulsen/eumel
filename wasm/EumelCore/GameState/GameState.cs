using System;
using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace EumelCore
{
    public partial class GameState
    {
        public readonly IReadOnlyList<PlayerState> Players;

        public readonly TrickState CurrentTrick;

        public readonly TurnState Turn;
        public struct TurnState
        {
            public readonly PlayerIndex PlayerIndex;
            public readonly Type NextEventType;

            public TurnState(int turnOfPlayerIndex, Type nextEventType)
            {
                PlayerIndex = new PlayerIndex(turnOfPlayerIndex);
                NextEventType = nextEventType;
            }
            public static TurnState RoundIsOver => new TurnState(0, null);

            public bool IsPreparing => NextEventType == typeof(HandReceived);
            public bool IsGuess => NextEventType == typeof(GuessGiven);
            public bool IsPlay => NextEventType == typeof(CardPlayed);
            public bool IsRoundOver => NextEventType == null;

            public override bool Equals(object obj)
            {
                return obj is TurnState state &&
                    PlayerIndex == state.PlayerIndex &&
                    EqualityComparer<Type>.Default.Equals(NextEventType, state.NextEventType);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(PlayerIndex, NextEventType);
            }

            public override string ToString() =>
                IsRoundOver ?
                "Round is over." :
                $"Waiting for {NextEventType.Name} of {PlayerIndex}";
        }

        private GameState(IReadOnlyList<PlayerState> players, TrickState currentTrick, TurnState turn)
        {
            Players = players;
            CurrentTrick = currentTrick;
            Turn = turn;
        }

        public int TricksPlayed => Players.Sum(p => p.TricksWon);
        public bool AllTricksPlayed => Players[0].Hand.IsEmpty;

        public static GameState Initial(int players, EumelRoundSettings settings) =>
            new GameState(
                Enumerable.Range(0, players).Select(PlayerState.Initial).ToList(),
                TrickState.Initial,
                new TurnState(settings.StartingPlayerIndex, typeof(HandReceived)));

        public GameState Dispatch(GameEvent gameEvent) =>
            new GameState(Players.Select(p => p.Dispatch(gameEvent)).ToList(),
                CurrentTrick.Dispatch(gameEvent),
                GetNextTurn(gameEvent));

        private TurnState GetNextTurn(GameEvent gameEvent) => gameEvent
        switch
        {
            HandReceived hand => Next(hand),
            GuessGiven guess => Next(guess),
            CardPlayed move => Next(move),
            TrickWon won => Next(won),
            _ =>
            throw new InvalidOperationException(),
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
            if (Players[nextPlayer].Hand.IsEmpty)
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
            var currentSuit = CurrentTrick.Suit;
            var switchesSuit = currentSuit.HasValue && card.Suit != currentSuit;
            if (switchesSuit && playersHand.MustFollow(currentSuit.Value))
            {
                return false;
            }
            return playersHand.Has(card);
        }

        private bool IsMoveAllowed(Move move) => move.Player == Turn.PlayerIndex && move.GetType() == Turn.NextEventType;

        public override string ToString() => $"{string.Join(NewLine, Players)}{NewLine}{CurrentTrick}{NewLine}{Turn}";

    }
}