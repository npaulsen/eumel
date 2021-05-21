using System.Collections.Generic;
using Eumel.Core;

namespace Eumel.Server.Services
{
    public interface IClientToLobbyAssignmentStore
    {
        ClientToLobbyAssignment Remove(string connectionId);
        bool IsAssigned(string connectionId);
        (ActiveLobby, int) Get(string connectionId);
        void Add(string connectionId, ClientToLobbyAssignment assignment);
        IEnumerable<(string ConnectionId, int PlayerIndex)> ForLobby(string lobby);
    }
}