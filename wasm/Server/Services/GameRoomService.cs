using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Server.Hubs;
using Server.Models;

namespace BlazorSignalRApp.Server.Services
{
    public interface IGameRoomService
    {
        GameRoom Find(string roomId);
        IEnumerable<GameRoom> FindAll();
    }
    public class GameRoomService : IGameRoomService
    {
        private readonly Dictionary<string, GameRoom> _rooms;
        private readonly ILogger<GameRoomService> _logger;

        public GameRoomService(ILogger<GameRoomService> logger)
        {
            _rooms = new Dictionary<string, GameRoom>();
            _rooms.Add("a", new GameRoom { Id = "a", PlayerCount = 3, GameContext = GameContext.Singleton });
            _logger = logger;
        }
        public GameRoom Find(string roomId)
        {
            var id = roomId.ToLowerInvariant();
            return _rooms.ContainsKey(id) ? _rooms[id] : null;
        }

        public IEnumerable<GameRoom> FindAll() => _rooms.Values;
    }
}