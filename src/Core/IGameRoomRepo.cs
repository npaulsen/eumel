using System.Collections.Generic;
using Eumel.Core;

namespace Eumel.Core
{
    public interface IGameRoomRepo
    {
        IEnumerable<EumelGameRoom> FindAll();
        void Insert(EumelGameRoomDefinition room);
        bool ExistsWithName(string roomName);
        void UpdateStatus(string roomName, GameStatus newStatus);
        EumelGameRoom FindByName(string roomName);
    }

    public interface IActiveLobbyRepo
    {
        ActiveLobby GetLobbyFor(EumelGameRoomDefinition room);
    }
}