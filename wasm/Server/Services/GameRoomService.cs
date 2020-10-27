using System.Collections.Generic;
using System.Linq;
using BlazorSignalRApp.Shared.Rooms;
using Microsoft.Extensions.Logging;
using Server.Models;

namespace BlazorSignalRApp.Server.Services
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

        public void Create(GameRoomData room)
        {
            var id = room.Id;
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new System.ArgumentException("invalid room Id given");
            }
            if (Find(id) != null)
            {
                throw new System.ArgumentException("room id taken");
            }

            _rooms.Add(id.ToLowerInvariant(), new GameRoom(id, room.Players));
        }

        public GameRoom Find(string roomId)
        {
            var id = roomId.ToLowerInvariant();
            return _rooms.ContainsKey(id) ? _rooms[id] : null;
        }

        public IEnumerable<GameRoom> FindAll() => _rooms.Values;
    }
}