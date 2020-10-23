using System;
using System.Collections.Generic;
using System.Linq;

namespace EumelCore
{
    public class TrickState
    {
        public readonly IReadOnlyCollection<CardPlayed> Moves;

        public static TrickState Initial => new TrickState(new List<CardPlayed>());

        private TrickState(List<CardPlayed> moves)
        {
            Moves = moves;
        }

        public PlayerIndex Winner
        {
            get
            {
                var firstMove = Moves.First();
                var(playerWithHighestCard, highestCard) = (firstMove.Player, firstMove.Card);
                foreach (var move in Moves.Skip(1))
                {
                    if (move.Card > highestCard)
                    {
                        (playerWithHighestCard, highestCard) = (move.Player, move.Card);
                    }
                }
                return playerWithHighestCard;
            }
        }

        public bool AnyPlayed => Moves.Any();
        public Suit? Suit => Moves.FirstOrDefault()?.Card?.Suit;

        internal TrickState Next(CardPlayed move) => new TrickState(Moves.Concat(new [] { move }).ToList());

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