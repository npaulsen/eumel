namespace Eumel.Core
{
    public abstract record Move : GameEvent
    {
        protected Move(GameEventContext context, PlayerIndex player) : base(context, player) { }
    }

}