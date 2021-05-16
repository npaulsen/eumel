using Eumel.Core.Players;

namespace Eumel.Core
{
    public record PlayerInfo(string Name, string Type)
    {

    }

    public class PlayerType {
        public const string Human = "human";
        public const string Bot = "bot";
    }
}