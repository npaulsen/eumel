using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Eumel.Server.Hubs;
using Eumel.Shared.HubInterface;

namespace Eumel.Server.Services
{
    public class ConnectionManager
    {
        private readonly IGameRoomRepo _roomRepo;
        private readonly IActiveLobbyRepo _lobbyRepo;
        private readonly IClientToLobbyAssignmentStore _lobbyAssignments;

        public ConnectionManager(IGameRoomRepo roomRepo, IActiveLobbyRepo lobbyRepo, IClientToLobbyAssignmentStore lobbyAssignmentStore)
        {
            _roomRepo = roomRepo ?? throw new ArgumentNullException(nameof(roomRepo));
            _lobbyRepo = lobbyRepo ?? throw new ArgumentNullException(nameof(lobbyRepo));
            _lobbyAssignments = lobbyAssignmentStore ?? throw new ArgumentNullException(nameof(lobbyAssignmentStore));
        }

        public (ActiveLobby Lobby, int PlayerIndex) GetPlayerConnection(string conn)
        {
            if (!_lobbyAssignments.IsAssigned(conn))
            {
                throw new ArgumentException("Client not registered for a room.");
            }
            return _lobbyAssignments.Get(conn);
        }

        internal bool HasJoinedLobby(string connectionId)
            => _lobbyAssignments.IsAssigned(connectionId);

        // TOOD: return result and handle errors in caller.
        public void AddConnection(string connectionId, IGameClient client, JoinData data)
        {
            if (string.IsNullOrWhiteSpace(data.RoomId))
            {
                return;
            }
            var room = _roomRepo.FindByName(data.RoomId);

            if (room == null)
            {
                Console.WriteLine($"Room '{data.RoomId}' not found, ignoring join.");
                return;
            }

            var lobby = _lobbyRepo.GetLobbyFor(room.Definition);
            ResetPreviousLobbyAssignment(connectionId);

            var sender = new GameEventForwarder(client, data.PlayerIndex);
            sender.SubscribeTo(lobby);
            _lobbyAssignments.Add(connectionId, new ClientToLobbyAssignment(lobby, data.PlayerIndex, sender));
        }

        internal IEnumerable<(string ConnectionId, int PlayerIndex)> GetConnectionsFor(string lobby)
            => _lobbyAssignments.ForLobby(lobby);

        public void ResetPreviousLobbyAssignment(string connectionId)
        {
            var oldAssignment = _lobbyAssignments.Remove(connectionId);
            oldAssignment?.Dispose();
        }
    }
}