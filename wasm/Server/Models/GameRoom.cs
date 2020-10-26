using System.Collections.Generic;
using System.Linq;
using Server.Models;

namespace Server.Models
{
    public class GameRoom
    {
        public readonly string Id;
        public readonly IReadOnlyList<PlayerInfo> Players;
        public readonly GameContext GameContext;

        public GameRoom(string id, IEnumerable<PlayerInfo> players, GameContext gameContext)
        {
            Id = id;
            Players = players.ToList();
            GameContext = gameContext;
        }
    }
}