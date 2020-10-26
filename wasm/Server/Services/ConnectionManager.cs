using System;
using System.Collections.Generic;
using BlazorSignalRApp.Server.Hubs;
using BlazorSignalRApp.Shared.HubInterface;
using Server.Models;

namespace BlazorSignalRApp.Server.Services
{
    class PlayerConnection
    {
        public readonly GameRoom RoomId;
        public readonly int PlayerIndex;
        public readonly GameEventForwarder EventSender;

        public PlayerConnection(GameRoom room, int playerIndex, GameEventForwarder eventSender)
        {
            RoomId = room;
            PlayerIndex = playerIndex;
            EventSender = eventSender;
        }
    }

    public class ConnectionManager
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

        public(GameContext, int) GetPlayerConnection(string conn)
        {
            if (!_playerConnections.ContainsKey(conn))
            {
                throw new ArgumentException("Client not registered for a room.");
            }
            var playerConnection = _playerConnections[conn];
            var room = playerConnection.RoomId;
            return (room.GameContext, playerConnection.PlayerIndex);
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

            var sender = new GameEventForwarder(client);
            sender.SubscribeTo(room.GameContext);
            _playerConnections.Add(connectionId, new PlayerConnection(room, data.PlayerIndex, sender));
        }
    }
}