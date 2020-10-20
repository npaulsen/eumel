namespace EumelCore
{
    public class ReceivedHand : GameEvent
    {
        public readonly Hand Hand;

        public ReceivedHand(PlayerInfo player, Hand hand) : base(player)
        {
            Hand = hand;
        }
    }

}