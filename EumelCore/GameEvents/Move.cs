namespace EumelCore
{
    public abstract class Move : GameEvent
    {
        protected Move(PlayerInfo player) : base(player) { }
    }

}