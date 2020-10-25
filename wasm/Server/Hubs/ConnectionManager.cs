using System;
using System.Collections.Generic;
using BlazorSignalRApp.Shared.HubInterface;
using Server.Hubs;

namespace BlazorSignalRApp.Server.Hubs
{
    class PlayerConnection
    {
        public readonly string Room;
        public readonly int PlayerIndex;
        public readonly GameEventSender EventSender;

        public PlayerConnection(string room, int playerIndex, GameEventSender eventSender)
        {
            Room = room;
            PlayerIndex = playerIndex;
            EventSender = eventSender;
        }
    }

    public class ConnectionManager
    {
        private Dictionary<string, GameContext> _rooms;
        private Dictionary<string, PlayerConnection> _playerConnections;

        public ConnectionManager()
        {
            _rooms = new Dictionary<string, GameContext>();
            _rooms.Add("a", GameContext.Singleton);
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
            var room = _rooms[playerConnection.Room];
            return (room, playerConnection.PlayerIndex);
        }

        public void AddConnection(string connectionId, IGameClient client, JoinData data)
        {
            if (string.IsNullOrWhiteSpace(data.Room))
            {
                return;
            }
            if (!_rooms.ContainsKey(data.Room))
            {
                Console.WriteLine($"Room '{data.Room}' not found, ignoring join.");
                return;
            }

            Unsubscribe(connectionId);

            var room = _rooms[data.Room];
            var sender = new GameEventSender(client);
            sender.SubscribeTo(room);
            _playerConnections.Add(connectionId, new PlayerConnection(data.Room, data.PlayerIndex, sender));
        }
    }
}