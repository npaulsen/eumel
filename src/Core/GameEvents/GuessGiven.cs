namespace Eumel.Core
{
    public record GuessGiven : Move
    {
        public readonly int Count;

        public GuessGiven(GameEventContext context, PlayerIndex player, int count) : base(context, player)
        {
            Count = count;
        }
        public override string ToString() => $"[{nameof(GuessGiven)} {Player} {Count}]";

    }

}