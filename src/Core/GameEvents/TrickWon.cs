namespace Eumel.Core
{
    public class TrickWon : GameEvent
    {
        public TrickWon(PlayerIndex player) : base(player) { }

        public override string ToString() => $"[{nameof(TrickWon)} {Player}]";

    }

}