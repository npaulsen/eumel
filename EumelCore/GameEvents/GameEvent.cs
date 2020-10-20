namespace EumelCore
{
    public abstract class GameEvent
    {
        public readonly PlayerInfo Player;

        protected GameEvent(PlayerInfo player)
        {
            this.Player = player;
        }
    }
}