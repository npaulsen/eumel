using Server.Hubs;

namespace Server.Models
{
    public class GameRoom
    {
        public string Id;
        public int PlayerCount;

        public GameContext GameContext { get; internal set; }
    }
}