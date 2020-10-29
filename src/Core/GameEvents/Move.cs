namespace Eumel.Core
{
    public abstract class Move : GameEvent
    {
        protected Move(PlayerIndex player) : base(player) { }
    }

}