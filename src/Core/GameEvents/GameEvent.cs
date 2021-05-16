namespace Eumel.Core
{
    public record GameEventContext(string GameId, int RoundIndex);
    public abstract record GameEvent
    {
        public readonly GameEventContext Context;
        public readonly PlayerIndex Player;

        protected GameEvent(GameEventContext ctx, PlayerIndex player)
        {
            Context = ctx;
            Player = player;
        }
    }

    public struct PlayerIndex
    {
        public readonly int Value;

        public PlayerIndex(int value)
        {
            Value = value;
        }
        public static implicit operator int(PlayerIndex pi) => pi.Value;

        public override string ToString() => $"Player {Value + 1}";
    }
}