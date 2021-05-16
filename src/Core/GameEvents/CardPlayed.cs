namespace Eumel.Core
{
    public record CardPlayed : Move
    {
        public readonly Card Card;

        public CardPlayed(GameEventContext context, PlayerIndex player, Card card) : base(context, player)
        {
            Card = card;
        }

        public override string ToString() => $"[{nameof(CardPlayed)} {Player} {Card}]";
    }

}