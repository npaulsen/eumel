namespace EumelCore
{
    public class PlayedCard : Move
    {
        public readonly Card Card;

        public PlayedCard(PlayerInfo player, Card card) : base(player)
        {
            Card = card;
        }
    }

}