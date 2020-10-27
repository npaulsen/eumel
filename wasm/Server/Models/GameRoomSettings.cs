namespace Server.Models
{
    public class GameRoomSettings
    {
        public readonly int BotDelayMs = 2000;

        public static GameRoomSettings Default = new GameRoomSettings();
    }
}