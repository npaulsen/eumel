using System;
using System.Collections.Generic;
using System.Linq;

namespace Eumel.Core
{
    public class TrickState
    {
        public readonly IReadOnlyList<CardPlayed> Moves;

        public PlayerIndex PlayerWithHighestCard =>
            _highestMoveIndex >= 0 ? Moves[_highestMoveIndex].Player :
            throw new InvalidOperationException("no moves yet");

        public Card HighestCard => _highestMoveIndex >= 0 ? Moves[_highestMoveIndex].Card :
            throw new InvalidOperationException("no moves yet");
        private readonly int _highestMoveIndex;

        public static TrickState Initial => new TrickState(new List<CardPlayed>(), -1);

        private TrickState(List<CardPlayed> moves, int highestMoveIndex)
        {
            Moves = moves;
            _highestMoveIndex = highestMoveIndex;
        }

        public bool AnyPlayed => Moves.Any();
        public bool NonePlayedYet => !AnyPlayed;
        public Suit? Suit => Moves.FirstOrDefault()?.Card?.Suit;

        internal TrickState Next(CardPlayed move)
        {
            var nextMoves = Moves.Concat(new [] { move }).ToList();
            var isHighest = !Moves.Any() || move.Card > Moves[_highestMoveIndex].Card;
            var bestMove = isHighest? Moves.Count : _highestMoveIndex;
            return new TrickState(nextMoves, bestMove);
        }

        internal TrickState Dispatch(GameEvent gameEvent)
        {
            switch (gameEvent)
            {
                case TrickWon won:
                    return TrickState.Initial;
                case CardPlayed move:
                    return Next(move);
                default:
                    return this;
            }
        }

        public override string ToString() =>
            string.Join("  ", Moves.Select(m => $"{m.Card} ({m.Player})"));
    }
}