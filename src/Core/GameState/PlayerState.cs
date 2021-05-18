using System;

namespace Eumel.Core
{
    public record PlayerState(int PlayerIndex, int? Guess, int TricksWon, IHand Hand)
    {
        public static PlayerState Initial(int playerIndex) => new(new PlayerIndex(playerIndex), null, 0, null);

        internal PlayerState Dispatch(GameEvent gameEvent)
        {
            if (gameEvent.Player != PlayerIndex)
            {
                return this;
            }
            return gameEvent switch
            {
                HandReceived receivedHand => this with { Guess = null, TricksWon = 0, Hand = receivedHand.Hand },
                GuessGiven guess => this with { Guess = guess.Count },
                CardPlayed move => this with { Hand = Hand.Play(move.Card) },
                TrickWon => this with { TricksWon = TricksWon + 1 },
                _ => throw new NotImplementedException()
            };
        }

        public override string ToString()
        {
            var guess = Guess.HasValue ? Guess.ToString() : "?";
            return $"Player {PlayerIndex + 1}: {TricksWon} / {guess} \t{Hand}";
        }
    }
}