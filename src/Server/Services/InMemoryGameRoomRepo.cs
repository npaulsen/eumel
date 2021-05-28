using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Microsoft.Extensions.Logging;

namespace Eumel.Server.Services
{
    public class InMemoryGameRoomRepo : IGameRoomRepo
    {
        private readonly Dictionary<string, EumelGameRoom> _rooms;

        public InMemoryGameRoomRepo()
        {
            _rooms = new();
        }

        public void Insert(EumelGameRoomDefinition room)
        {
            _rooms.Add(room.Name.ToLowerInvariant(), new(room, GameStatus.Prepared));
        }

        public bool ExistsWithName(string roomName) => FindByName(roomName) is not null;

        public IEnumerable<EumelGameRoom> FindAll()
            => _rooms.Values;

        public EumelGameRoom FindByName(string roomName)
        {
            var name = roomName.ToLowerInvariant();
            return _rooms.ContainsKey(name) ? _rooms[name] : null;
        }

        public void UpdateStatus(string roomName, GameStatus newStatus)
        {
            if (!ExistsWithName(roomName))
            {
                throw new InvalidOperationException(nameof(roomName));
            }
            var room = FindByName(roomName);
            _rooms[roomName.ToLowerInvariant()] = room with { Status = newStatus };
        }
    }
}