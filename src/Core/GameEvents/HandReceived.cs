namespace Eumel.Core
{
    public class HandReceived : GameEvent
    {
        public readonly IHand Hand;

        public HandReceived(PlayerIndex player, IHand hand) : base(player)
        {
            Hand = hand;
        }
        public override string ToString() => $"[{nameof(HandReceived)} {Player} {Hand}]";

    }

}