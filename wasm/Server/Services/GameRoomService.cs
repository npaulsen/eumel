using System.Collections.Generic;
using BlazorSignalRApp.Shared.Rooms;
using Microsoft.Extensions.Logging;
using Server.Hubs;
using Server.Models;

namespace BlazorSignalRApp.Server.Services
{
    public interface IGameRoomService
    {
        GameRoom Find(string roomId);
        IEnumerable<GameRoom> FindAll();
        void Create(GameRoomData room);
    }
    public class GameRoomService : IGameRoomService
    {
        private readonly Dictionary<string, GameRoom> _rooms;
        private readonly ILogger<GameRoomService> _logger;

        public GameRoomService(ILogger<GameRoomService> logger)
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
            _rooms.Add(id.ToLowerInvariant(), new GameRoom
            {
                Id = id,
                    PlayerCount = room.PlayerCount,
                    GameContext = new GameContext(room.PlayerCount),
            });

        }

        public GameRoom Find(string roomId)
        {
            var id = roomId.ToLowerInvariant();
            return _rooms.ContainsKey(id) ? _rooms[id] : null;
        }

        public IEnumerable<GameRoom> FindAll() => _rooms.Values;
    }
}