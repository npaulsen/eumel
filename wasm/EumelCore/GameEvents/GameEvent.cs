namespace EumelCore
{
    public abstract class GameEvent
    {
        public readonly PlayerIndex Player;

        protected GameEvent(PlayerIndex player)
        {
            this.Player = player;
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

        public override string ToString() => $"P{Value}";
    }
}