namespace Eumel.Core
{
    public record HandReceived : GameEvent
    {
        public readonly IHand Hand;

        public HandReceived(GameEventContext context, PlayerIndex player, IHand hand) : base(context, player)
        {
            Hand = hand;
        }
        public override string ToString() => $"[{nameof(HandReceived)} {Player} {Hand}]";

    }
}