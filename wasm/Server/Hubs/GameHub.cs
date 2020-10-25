using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorSignalRApp.Shared.HubInterface;
using EumelCore;
using EumelCore.GameSeriesEvents;
using EumelCore.Players;
using Microsoft.AspNetCore.SignalR;

namespace BlazorSignalRApp.Server.Hubs
{
    public class GameHub : Hub<IGameClient>, IGameHub
    {
        private readonly ConnectionManager _connectionManager;
        public GameHub(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public override Task OnConnectedAsync()
        {
            System.Console.WriteLine($"Client {Context.ConnectionId} connected");
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var conn = Context.ConnectionId;
            System.Console.WriteLine($"Client {conn} disconnected");
            _connectionManager.Unsubscribe(conn);
            return Task.CompletedTask;
        }

        public Task PlayCard(CardPlayedDto data)
        {
            System.Console.WriteLine("Received move: " + data.CardIndex);
            var(room, playerIndex) = _connectionManager.GetPlayerConnection(Context.ConnectionId);
            var res = room.TryPlayCard(playerIndex, data.CardIndex);
            if (!res) System.Console.WriteLine("INVALID");
            return Task.CompletedTask;
        }

        public Task MakeGuess(GuessGivenDto data)
        {
            System.Console.WriteLine("Received guess: " + data);
            var(room, playerIndex) = _connectionManager.GetPlayerConnection(Context.ConnectionId);
            var res = room.TryGiveGuess(playerIndex, data.Count);
            if (!res) System.Console.WriteLine("INVALID");
            return Task.CompletedTask;
        }

        public Task StartNextRound()
        {
            System.Console.WriteLine("Received command to start round");
            var(room, _) = _connectionManager.GetPlayerConnection(Context.ConnectionId);
            room.StartNextRound();
            return Task.CompletedTask;
        }

        public Task Join(JoinData data)
        {
            Console.WriteLine($"Got join request of {Context.ConnectionId} for room {data.Room}");
            _connectionManager.AddConnection(Context.ConnectionId, Clients.Caller, data);
            Console.WriteLine("Client subscribed!");
            return Task.CompletedTask;
        }
    }
}