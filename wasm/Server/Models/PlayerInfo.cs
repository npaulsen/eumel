using EumelCore;
using EumelCore.Players;

namespace Server.Models
{
    public class PlayerInfo
    {
        public readonly string Name;

        public readonly IInvocablePlayer Player;

        public bool IsBot => Player != null;
        public bool IsHuman => Player == null;

        private PlayerInfo(string name, IInvocablePlayer player)
        {
            Name = name;
            Player = player;
        }

        public static PlayerInfo CreateHuman(string name) => new PlayerInfo(name, null);
        public static PlayerInfo CreateBot(string name) => new PlayerInfo(name, new DumbPlayer());

    }
}