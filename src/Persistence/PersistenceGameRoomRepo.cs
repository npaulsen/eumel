using System.Collections.Generic;
using Eumel.Core;
using Eumel.Persistance.Games;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Eumel.Persistance
{
    public class PersistenceGameRoomRepo : IGameRoomRepo
    {
        private readonly EumelGameContext _context;

        public PersistenceGameRoomRepo(EumelGameContext context)
        {
            _context = context;
        }

        public bool ExistsWithName(string roomName)
            => _context.Games.Any(g => g.Name == roomName);

        public IEnumerable<EumelGameRoom> FindAll()
            => _context.Games
                .OrderByDescending(g => g.Id)
                .Include(global => global.Players)
                .Select(persistedGame => persistedGame.ToEumelGameRoom());

        public void Insert(EumelGameRoomDefinition roomDefinition)
        {
            var room = new EumelGameRoom(roomDefinition, GameStatus.Prepared);
            _context.Games.Add(PersistedEumelGame.CreateFrom(room));
            _context.SaveChanges();
        }

        public EumelGameRoom FindByName(string roomName)
            => _context.Games
                .Include(g => g.Players)
                .Single(g => g.Name == roomName)
                .ToEumelGameRoom();

        public void UpdateStatus(string roomName, GameStatus newStatus)
        {
            var room = _context.Games
                .Include(g => g.Players)
                .Single(g => g.Name == roomName);
            room.Status = newStatus;
            _context.SaveChanges();
        }

    }
}