using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Eumel.Shared.Rooms;
using Microsoft.Extensions.Logging;

namespace Eumel.Server.Services
{
    public class InMemoryGameRoomService : IGameRoomService
    {
        private readonly Dictionary<string, GameRoom> _rooms;
        private readonly ILogger<InMemoryGameRoomService> _logger;

        public InMemoryGameRoomService(ILogger<InMemoryGameRoomService> logger)
        {
            _rooms = new Dictionary<string, GameRoom>();
            _logger = logger;
        }

        public void Create(GameRoomData roomData)
        {
            var id = roomData.Id;
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new System.ArgumentException("invalid room Id given");
            }
            if (Find(id) != null)
            {
                throw new System.ArgumentException("room id taken");
            }

            var players = roomData.Players.Select(CreatePlayer);
            var room = new GameRoom(id, players, GameRoomSettings.Default);
            _rooms.Add(id.ToLowerInvariant(), room);
        }
        private static PlayerInfo CreatePlayer(GamePlayerData data) =>
            data.IsHuman? PlayerInfo.CreateHuman(data.Name) : PlayerInfo.CreateBot(data.Name);

        public GameRoom Find(string roomId)
        {
            var id = roomId.ToLowerInvariant();
            return _rooms.ContainsKey(id) ? _rooms[id] : null;
        }

        public IEnumerable<GameRoom> FindAll() => _rooms.Values;
    }
}