using System.Collections.Generic;
using BlazorSignalRApp.Shared.Rooms;
using EumelCore;

namespace BlazorSignalRApp.Server.Services
{
    public interface IGameRoomService
    {
        GameRoom Find(string roomId);
        IEnumerable<GameRoom> FindAll();
        void Create(GameRoomData room);
    }
}