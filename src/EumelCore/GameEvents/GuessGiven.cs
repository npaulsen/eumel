namespace EumelCore
{
    public class GuessGiven : Move
    {
        public readonly int Count;

        public GuessGiven(PlayerIndex player, int count) : base(player)
        {
            Count = count;
        }
        public override string ToString() => $"[{nameof(GuessGiven)} {Player} {Count}]";

    }

}