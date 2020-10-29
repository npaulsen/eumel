using System;

namespace Eumel.Core
{
    public partial class GameState
    {
        public class PlayerState
        {
            public readonly int PlayerIndex;
            public readonly int? Guess;
            public readonly int TricksWon;
            public readonly Hand Hand;

            private PlayerState(int playerIndex, int? guess, int tricks, Hand hand)
            {
                PlayerIndex = playerIndex;
                Guess = guess;
                TricksWon = tricks;
                Hand = hand;
            }

            public static PlayerState Initial(int playerIndex) => new PlayerState(new PlayerIndex(playerIndex), null, 0, null);

            internal PlayerState Dispatch(GameEvent gameEvent)
            {
                if (gameEvent.Player != PlayerIndex)
                {
                    return this;
                }
                switch (gameEvent)
                {
                    case HandReceived receivedHand:
                        return new PlayerState(PlayerIndex, null, 0, receivedHand.Hand);
                    case GuessGiven guess:
                        return new PlayerState(PlayerIndex, guess.Count, TricksWon, Hand);
                    case CardPlayed move:
                        return new PlayerState(PlayerIndex, Guess, TricksWon, Hand.Play(move.Card));
                    case TrickWon won:
                        return new PlayerState(PlayerIndex, Guess, TricksWon + 1, Hand);
                    default:
                        throw new NotImplementedException();
                }
            }

            public override string ToString()
            {
                var guess = Guess.HasValue? Guess.ToString(): "?";
                return $"Player {PlayerIndex+1}: {TricksWon} / {guess} \t{Hand}";
            }
        }
    }
}