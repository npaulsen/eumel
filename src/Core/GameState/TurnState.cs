using System;

namespace Eumel.Core
{
    public record TurnState(PlayerIndex PlayerIndex, Type NextEventType)
    {
        public TurnState(int turnOfPlayerIndex, Type nextEventType) 
            : this(new PlayerIndex(turnOfPlayerIndex), nextEventType)
        {
        }

        public static TurnState RoundIsOver => new TurnState(0, null);

        public bool IsPreparing => NextEventType == typeof(HandReceived);
        public bool IsGuess => NextEventType == typeof(GuessGiven);
        public bool IsPlay => NextEventType == typeof(CardPlayed);
        public bool IsRoundOver => NextEventType == null;

        public override string ToString() =>
            IsRoundOver ?
            "Round is over." :
            $"Waiting for {NextEventType.Name} of {PlayerIndex}";
    }
}