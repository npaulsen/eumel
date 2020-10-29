namespace EumelCore
{
    public class HandReceived : GameEvent
    {
        public readonly Hand Hand;

        public HandReceived(PlayerIndex player, Hand hand) : base(player)
        {
            Hand = hand;
        }
        public override string ToString() => $"[{nameof(HandReceived)} {Player} {Hand}]";

    }

}