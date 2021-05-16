using System.Collections.Generic;
using Eumel.Core;

namespace Eumel.Core
{
    public interface IGameRoomRepo
    {
        IEnumerable<EumelGameRoomDefinition> FindAll();
        void Insert(EumelGameRoomDefinition room);
        bool ExistsWithName(string roomName);
        EumelGameRoomDefinition FindByName(string roomName);
    }

    public interface IActiveLobbyRepo
    {
        ActiveLobby GetLobbyFor(EumelGameRoomDefinition room);
    }
}