using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EumelCore;
using Microsoft.AspNetCore.SignalR;

namespace BlazorSignalRApp.Server.Hubs
{
    public class GameHub : Hub
    {
        public async Task CardMove(int playerIndex, Card card)
        {
            await Clients.All.SendAsync("CardMove", playerIndex, card);
        }

        public async Task TrickGuess(int playerIndex, int guess)
        {
            await Clients.All.SendAsync("CardMove", playerIndex, guess);
        }
    }
}