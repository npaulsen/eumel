using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Eumel.Core
{
    public record PlayerRoundResult(int Guesses, int TricksWon, int Score);

    public record RoundResult
    {
        public readonly ImmutableListWithValueSemantics<PlayerRoundResult> PlayerResults;

        public RoundResult(IEnumerable<PlayerRoundResult> playerResults)
        {
            PlayerResults = playerResults.ToImmutableList().WithValueSemantics();
        }

        public static RoundResult From(GameState state)
        {
            if (state.Players.Any(p => p.Hand.NumberOfCards > 0))
            {
                throw new ArgumentException("GameState not finished");
            }
            var playerResults = state.Players.Select(playerState => GetResult(playerState.Guess.Value, playerState.TricksWon));
            return new RoundResult(playerResults.ToImmutableList());
        }

        private static PlayerRoundResult GetResult(int guesses, int tricksWon)
        {
            var bonus = guesses == tricksWon ? 10 : 0;
            return new PlayerRoundResult(guesses, tricksWon, bonus + tricksWon);
        }
    }
}