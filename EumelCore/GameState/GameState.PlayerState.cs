using System;

namespace EumelCore
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

            public static PlayerState Initial(PlayerInfo player) => new PlayerState(player.Index, null, 0, null);

            internal PlayerState Dispatch(GameEvent gameEvent)
            {
                if (gameEvent.Player.Index != PlayerIndex)
                {
                    return this;
                }
                switch (gameEvent)
                {
                    case ReceivedHand receivedHand:
                        return new PlayerState(PlayerIndex, Guess, TricksWon, receivedHand.Hand);
                    case MadeGuess guess:
                        return new PlayerState(PlayerIndex, guess.Count, TricksWon, Hand);
                    case PlayedCard move:
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