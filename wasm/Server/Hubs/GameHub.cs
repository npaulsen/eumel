using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSignalRApp.Shared.HubInterface;
using EumelCore;
using EumelCore.GameSeriesEvents;
using EumelCore.Players;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;

namespace BlazorSignalRApp.Server.Hubs
{
    public class GameHub : Hub<IGameClient>, IGameHub
    {
        public GameHub() { }

        public override Task OnConnectedAsync()
        {
            System.Console.WriteLine("Client connected");

            var sender = new GameEventSender(Clients.Caller);
            GameContext.Singleton.Subscribe((IObserver<GameSeriesEvent>) sender);
            GameContext.Singleton.Subscribe((IObserver<GameEvent>) sender);
            System.Console.WriteLine("Client subscribed");
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            System.Console.WriteLine("Client disconnected");
            return Task.CompletedTask;
        }

        public Task PlayCard(CardPlayedDto data)
        {
            System.Console.WriteLine("Received move: " + data);
            GameContext.Singleton.TryPlayCard(data.PlayerIndex, data.CardIndex);
            return Task.CompletedTask;
        }
        public Task MakeGuess(GuessGivenDto data)
        {
            System.Console.WriteLine("Received guess: " + data);
            GameContext.Singleton.TryGiveGuess(data.PlayerIndex, data.Count);
            return Task.CompletedTask;
        }

        public Task StartNextRound()
        {
            System.Console.WriteLine("Received command to start round");
            GameContext.Singleton.StartNextRound();
            return Task.CompletedTask;
        }
    }
}