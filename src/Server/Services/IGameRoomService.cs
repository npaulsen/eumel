using System.Collections.Generic;
using Eumel.Core;
using Eumel.Shared.Rooms;

namespace Eumel.Server.Services
{
    public interface IGameRoomService
    {
        GameRoom Find(string roomId);
        IEnumerable<GameRoom> FindAll();
        void Create(GameRoomData room);
    }
}