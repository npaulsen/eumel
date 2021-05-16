using System;
using Eumel.Core;
using Eumel.Server.Hubs;

namespace Eumel.Server.Services
{
    public record ClientToLobbyAssignment(ActiveLobby Room, int PlayerIndex, GameEventForwarder EventSender) : IDisposable
    {
        public void Dispose()
        {
            EventSender.Dispose();
        }
    }
}