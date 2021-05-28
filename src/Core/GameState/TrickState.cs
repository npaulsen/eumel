using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Eumel.Core
{
    public record TrickState
    {
        private readonly int _highestMoveIndex;

        public readonly ImmutableListWithValueSemantics<CardPlayed> Moves;

        public PlayerIndex PlayerWithHighestCard =>
            _highestMoveIndex >= 0 ? Moves[_highestMoveIndex].Player :
            throw new InvalidOperationException("no moves yet");

        public Card HighestCard => _highestMoveIndex >= 0 ? Moves[_highestMoveIndex].Card :
            throw new InvalidOperationException("no moves yet");

        public bool AnyPlayed => Moves.Any();
        public bool NonePlayedYet => !AnyPlayed;
        public Suit? Suit => Moves.FirstOrDefault()?.Card?.Suit;

        public static TrickState Initial => new(ImmutableList.Create<CardPlayed>().WithValueSemantics(), -1);

        private TrickState(ImmutableListWithValueSemantics<CardPlayed> moves, int highestMoveIndex)
        {
            Moves = moves;
            _highestMoveIndex = highestMoveIndex;
        }

        public TrickState Next(CardPlayed move)
        {
            var nextMoves = Moves.Concat(new[] { move }).ToImmutableList().WithValueSemantics();
            var isHighest = !Moves.Any() || move.Card > Moves[_highestMoveIndex].Card;
            var bestMove = isHighest ? Moves.Count : _highestMoveIndex;
            return new TrickState(nextMoves, bestMove);
        }

        internal TrickState Dispatch(GameEvent gameEvent)
            => gameEvent switch
            {
                TrickWon => TrickState.Initial,
                CardPlayed move => Next(move),
                _ => this,
            };


        public override string ToString() =>
            string.Join("  ", Moves.Select(m => $"{m.Card} ({m.Player})"));
    }
}