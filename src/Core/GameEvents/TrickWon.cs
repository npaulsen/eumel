namespace Eumel.Core
{
    public record TrickWon : GameEvent
    {
        public TrickWon(GameEventContext context, PlayerIndex player) : base(context, player) { }

        public override string ToString() => $"[{nameof(TrickWon)} {Player}]";

    }

}