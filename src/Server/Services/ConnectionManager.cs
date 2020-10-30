using System;
using System.Collections.Generic;
using Eumel.Core;
using Eumel.Server.Hubs;
using Eumel.Shared.HubInterface;

namespace Eumel.Server.Services
{
    public partial class ConnectionManager
    {
        private readonly Dictionary<string, PlayerConnection> _playerConnections;
        private readonly IGameRoomService _roomService;

        public ConnectionManager(IGameRoomService roomService)
        {
            _roomService = roomService;
            _playerConnections = new Dictionary<string, PlayerConnection>();
        }

        public void Unsubscribe(string conn)
        {
            if (_playerConnections.ContainsKey(conn))
            {
                var player = _playerConnections[conn];
                player.EventSender.Dispose();
                _playerConnections.Remove(conn);
            }
        }

        public(GameRoom, int) GetPlayerConnection(string conn)
        {
            if (!_playerConnections.ContainsKey(conn))
            {
                throw new ArgumentException("Client not registered for a room.");
            }
            var playerConnection = _playerConnections[conn];
            var room = playerConnection.Room;
            return (room, playerConnection.PlayerIndex);
        }

        public void AddConnection(string connectionId, IGameClient client, JoinData data)
        {
            if (string.IsNullOrWhiteSpace(data.RoomId))
            {
                return;
            }
            var room = _roomService.Find(data.RoomId);

            if (room == null)
            {
                Console.WriteLine($"Room '{data.RoomId}' not found, ignoring join.");
                return;
            }

            Unsubscribe(connectionId);

            var sender = new GameEventForwarder(client, data.PlayerIndex);
            sender.SubscribeTo(room);
            _playerConnections.Add(connectionId, new PlayerConnection(room, data.PlayerIndex, sender));
        }
    }
}