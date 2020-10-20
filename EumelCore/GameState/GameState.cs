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
        public readonly int TurnOfPlayerIndex;

        private GameState(IReadOnlyList<PlayerState> players, TrickState currentTrick, int turnOf)
        {
            Players = players;
            CurrentTrick = currentTrick;
            TurnOfPlayerIndex = turnOf;
        }

        public int TricksPlayed => Players.Sum(p => p.TricksWon);

        public static GameState Initial(PlayerCollection players, EumelRoundSettings settings) =>
            new GameState(players.Select(PlayerState.Initial).ToList(), TrickState.Initial, settings.StartingPlayerIndex);

        internal GameState Dispatch(GameEvent gameEvent) =>
            new GameState(Players.Select(p => p.Dispatch(gameEvent)).ToList(), CurrentTrick.Dispatch(gameEvent), GetNextStateTurnNumber(gameEvent));

        private int GetNextStateTurnNumber(GameEvent gameEvent)
        {
            switch (gameEvent)
            {
                case Move anyMove:
                    return (TurnOfPlayerIndex + 1) % Players.Count;
                case TrickWon won:
                    return won.Player.Index;
                default:
                    return TurnOfPlayerIndex;
            }
        }

        public override string ToString() => $"{string.Join(NewLine, Players)}{NewLine}{CurrentTrick}";
    }
}