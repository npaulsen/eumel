namespace Eumel.Core
{
    public class CardPlayed : Move
    {
        public readonly Card Card;

        public CardPlayed(PlayerIndex player, Card card) : base(player)
        {
            Card = card;
        }

        public override string ToString() => $"[{nameof(CardPlayed)} {Player} {Card}]";
    }

}