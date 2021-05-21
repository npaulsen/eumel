using System.Collections.Generic;
using System.Linq;
using Eumel.Core;

namespace Eumel.Server.Services
{
    public class ClientToLobbyAssignmentStore : IClientToLobbyAssignmentStore
    {
        private readonly Dictionary<string, ClientToLobbyAssignment> _assignments;

        public ClientToLobbyAssignmentStore()
        {
            _assignments = new();
        }

        public void Add(string connectionId, ClientToLobbyAssignment assignment)
            => _assignments.Add(connectionId, assignment);

        // TODO this is ugly.
        public IEnumerable<(string ConnectionId, int PlayerIndex)> ForLobby(string lobby)
            => _assignments
                .Where(kvp => kvp.Value.Room.Room.Name == lobby)
                .Select(kvp => (kvp.Key, kvp.Value.PlayerIndex));

        public (ActiveLobby, int) Get(string connectionId)
        {
            var (room, playerIndex, _) = _assignments[connectionId];
            return (room, playerIndex);
        }

        public bool IsAssigned(string connectionId)
            => _assignments.ContainsKey(connectionId);

        public ClientToLobbyAssignment Remove(string connectionId)
        {
            if (_assignments.TryGetValue(connectionId, out var assignment))
            {
                _assignments.Remove(connectionId);
                return assignment;
            }
            return null;
        }
    }
}