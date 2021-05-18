using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Eumel.Core.Players;
using Microsoft.Extensions.Logging;

namespace Eumel.Server.Services
{
    public class InMemoryLobbyManager : IActiveLobbyRepo
    {
        private readonly ILogger<InMemoryLobbyManager> _logger;
        private readonly IPlayerFactory _playerFactory;
        private readonly IGameEventPersister _eventPersister;
        private readonly IGameEventRepo _eventRepo;

        // TODO use cache here to evict inactive lobbies automatically?
        // Would it work with GC if no more event handlers for a lobby are registered??
        private readonly Dictionary<string, ActiveLobby> _lobbies;


        public InMemoryLobbyManager(ILogger<InMemoryLobbyManager> logger,
            IPlayerFactory playerFactory,
            IGameEventPersister eventPersister,
            IGameEventRepo eventRepo)
        {
            _logger = logger;
            _lobbies = new Dictionary<string, ActiveLobby>();
            _eventPersister = eventPersister;
            _eventRepo = eventRepo;
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
        }

        public ActiveLobby GetLobbyFor(EumelGameRoomDefinition room)
        {
            var roomName = room.Name;
            lock (_lobbies)
            {
                if (!_lobbies.ContainsKey(roomName))
                {
                    _lobbies[roomName] = ActivateLobby(room);
                }
                return _lobbies[roomName];
            }
        }

        private ActiveLobby ActivateLobby(EumelGameRoomDefinition room)
        {
            var roomName = room.Name;
            _logger.LogInformation("activating lobby for {roomName}", roomName);
            var progress = _eventRepo.GetGameProgress(roomName);
            var botController = CreateBotControllerFor(room);
            var newLobby = new ActiveLobby(botController, room, progress);
            newLobby.Subscribe(_eventPersister);
            newLobby.GameContext.Subscribe(_eventPersister);
            return newLobby;
        }

        private BotController CreateBotControllerFor(EumelGameRoomDefinition def)
        {
            var players = def.Players
                .Select(playerInfo => _playerFactory.GetNewPlayerOfType(playerInfo.Type));
            return new BotController(players, def);
        }
    }
}