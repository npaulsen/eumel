namespace EumelCore
{
    public abstract class Move : GameEvent
    {
        protected Move(PlayerIndex player) : base(player) { }
    }

}