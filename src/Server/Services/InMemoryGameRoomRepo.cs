using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Microsoft.Extensions.Logging;

namespace Eumel.Server.Services
{
    public class InMemoryGameRoomRepo : IGameRoomRepo
    {
        private readonly Dictionary<string, EumelGameRoomDefinition> _rooms;

        public InMemoryGameRoomRepo()
        {
            _rooms = new();
        }

        public void Insert(EumelGameRoomDefinition room)
        {
            _rooms.Add(room.Name.ToLowerInvariant(), room);
        }

        public bool ExistsWithName(string roomName) => FindByName(roomName) is not null;

        public IEnumerable<EumelGameRoomDefinition> FindAll()
            => _rooms.Values;

        public EumelGameRoomDefinition FindByName(string roomName)
        {
            var name = roomName.ToLowerInvariant();
            return _rooms.ContainsKey(name) ? _rooms[name] : null;
        }
    }
}