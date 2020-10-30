namespace Eumel.Core
{
    public class GameRoomSettings
    {
        public readonly int BotDelayMs;

        public static GameRoomSettings Default = new GameRoomSettings(2000);

        public GameRoomSettings(int botDelayMs)
        {
            BotDelayMs = botDelayMs;
        }
    }
}