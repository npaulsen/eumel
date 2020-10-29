using System;
using System.Collections.Generic;
using System.Linq;

namespace EumelCore
{
    public class RoundResult
    {
        public readonly IReadOnlyList<PlayerRoundResult> PlayerResults;

        public RoundResult(IReadOnlyList<PlayerRoundResult> playerResults)
        {
            PlayerResults = playerResults;
        }

        public static RoundResult From(GameState state)
        {
            if (state.Players.Any(p => p.Hand.NumberOfCards > 0))
            {
                throw new ArgumentException("GameState not finished");
            }
            var playerResults = state.Players.Select(playerState => GetResult(playerState.Guess.Value, playerState.TricksWon));
            return new RoundResult(playerResults.ToList());
        }

        private static PlayerRoundResult GetResult(int guesses, int tricksWon)
        {
            var bonus = guesses == tricksWon ? 10 : 0;
            return new PlayerRoundResult(guesses, tricksWon, bonus + tricksWon);
        }
        public class PlayerRoundResult
        {
            public readonly int Guesses;
            public readonly int TricksWon;
            public readonly int Score;

            public PlayerRoundResult(int guesses, int tricksWon, int score)
            {
                Guesses = guesses;
                TricksWon = tricksWon;
                Score = score;
            }
        }
    }
}