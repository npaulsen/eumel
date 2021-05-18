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

        public IEnumerable<EumelGameRoomDefinition> FindAll()
            => _context.Games
                .Include(global => global.Players)
                .Select(persistedGame => persistedGame.ToEumelGameRoomDef());

        public void Insert(EumelGameRoomDefinition room)
        {
            _context.Games.Add(PersistedEumelGame.CreateFrom(room));
            _context.SaveChanges();
        }

        public EumelGameRoomDefinition FindByName(string roomName)
            => _context.Games
                .Include(g => g.Players)
                .Include(g => g.Rounds)
                .Single(g => g.Name == roomName)
                .ToEumelGameRoomDef();
    }
}