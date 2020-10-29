using Eumel.Core;
using Eumel.Server.Hubs;

namespace Eumel.Server.Services
{
    public partial class ConnectionManager
    {
        class PlayerConnection
        {
            public readonly GameRoom Room;
            public readonly int PlayerIndex;
            public readonly GameEventForwarder EventSender;

            public PlayerConnection(GameRoom room, int playerIndex, GameEventForwarder eventSender)
            {
                Room = room;
                PlayerIndex = playerIndex;
                EventSender = eventSender;
            }
        }
    }
}