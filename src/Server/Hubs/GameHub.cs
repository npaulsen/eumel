using System;
using System.Linq;
using System.Threading.Tasks;
using Eumel.Core;
using Eumel.Server.Services;
using Eumel.Shared.HubInterface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Eumel.Server.Hubs
{
    public class GameHub : Hub<IGameClient>, IGameHub
    {
        private readonly ConnectionManager _connectionManager;
        private readonly ILogger<GameHub> _logger;
        public GameHub(ConnectionManager connectionManager, ILogger<GameHub> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("Client {connectionId} connected", Context.ConnectionId);
            return Task.CompletedTask;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            _logger.LogInformation("Client {connectionId} disconnected", Context.ConnectionId);
            if (_connectionManager.HasJoinedLobby(connectionId))
            {
                var lobby = _connectionManager.GetPlayerConnection(connectionId).Lobby;
                _connectionManager.ResetPreviousLobbyAssignment(connectionId);
                await BroadCastCurrentPlayersAsync(lobby.Room.Name);
            }
        }

        private async Task BroadCastCurrentPlayersAsync(string lobby)
        {
            var assignments = _connectionManager.GetConnectionsFor(lobby).ToList();
            var msg = new CurrentLobbyPlayersDto
            {
                PlayerConnections = assignments
                    .Select(a => a.PlayerIndex).ToArray()
            };
            // Parallel?
            // TODO use SignalR Groups and Users maybe?
            foreach (var assigment in assignments)
            {
                await Clients.Client(assigment.ConnectionId).PlayerUpdate(msg);
            }
        }

        public Task PlayCard(CardPlayedDto data)
        {
            _logger.LogInformation("Received move for card {cardIndex}", data.CardIndex);
            var (room, playerIndex) = _connectionManager.GetPlayerConnection(Context.ConnectionId);
            var res = room.TryPlayCard(playerIndex, data.CardIndex);
            if (!res)
            {
                _logger.LogInformation("move is currently INVALID");
            }
            return Task.CompletedTask;
        }

        public Task MakeGuess(GuessGivenDto data)
        {
            _logger.LogInformation("Received guess: {guess}", data.Count);
            var (room, playerIndex) = _connectionManager.GetPlayerConnection(Context.ConnectionId);
            var res = room.TryGiveGuess(playerIndex, data.Count);
            if (!res)
            {
                _logger.LogInformation("guess is currently INVALID");
            }
            return Task.CompletedTask;
        }

        public Task StartNextRound()
        {
            _logger.LogInformation("Received command to start round");
            var (room, _) = _connectionManager.GetPlayerConnection(Context.ConnectionId);
            room.StartNextRound();
            return Task.CompletedTask;
        }

        public async Task Join(JoinData data)
        {
            var role = data.PlayerIndex >= 0 ? "player " + (data.PlayerIndex + 1) : "watcher";
            _logger.LogInformation($"Got join request of {Context.ConnectionId} as {role} for room {data.RoomId}");
            _connectionManager.AddConnection(Context.ConnectionId, Clients.Caller, data);
            await BroadCastCurrentPlayersAsync(data.RoomId);
            _logger.LogInformation("Client subscribed!");
        }
    }
}