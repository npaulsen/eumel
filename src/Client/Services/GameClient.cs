using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using Eumel.Shared.HubInterface;
using Microsoft.AspNetCore.SignalR.Client;

namespace Eumel.Client.Services
{
    public class GameClient : IGameClient
    {
        private readonly HubConnection _connection;
        private readonly Action<GameEvent> _gameEventCallback;
        private readonly Action<GameSeriesEvent> _gameSeriesEventCallback;
        private readonly Action<CurrentLobbyPlayersDto> _playerUpdateCallback;
        private readonly Action<ConnectionState> _connectionStateChangedCallback;
        public readonly int PlayerIndex;
        public readonly string Room;

        private GameCardDeck _deck;

        public GameClient(string baseUri, string room, int playerIndex,
            Action<GameSeriesEvent> gameSeriesEventCallback,
            Action<GameEvent> gameEventCallback,
            Action<CurrentLobbyPlayersDto> playerUpdateCallback,
            Action<ConnectionState> connectionStateChangedCallback)
        {
            PlayerIndex = playerIndex;
            Room = room;
            _gameEventCallback = gameEventCallback;
            _gameSeriesEventCallback = gameSeriesEventCallback;
            _playerUpdateCallback = playerUpdateCallback;
            _connectionStateChangedCallback = connectionStateChangedCallback;

            _connection = new HubConnectionBuilder()
                .WithUrl(baseUri + GameHubInterface.HubUrl)
                // .WithAutomaticReconnect()
                .Build();

            SetupConnectionStateHandlers();

            _connection.On<GameSeriesDto>(nameof(GameSeriesStarted), GameSeriesStarted);
            _connection.On<RoundStartedDto>(nameof(GameRoundStarted), GameRoundStarted);
            _connection.On<RoundResultDto>(nameof(GameRoundEnded), GameRoundEnded);
            _connection.On<HandReceivedDto>(nameof(HandReceived), HandReceived);
            _connection.On<CardPlayedDto>(nameof(CardPlayed), CardPlayed);
            _connection.On<GuessGivenDto>(nameof(GuessGiven), GuessGiven);
            _connection.On<TrickWonDto>(nameof(TrickWon), TrickWon);
            _connection.On<CurrentLobbyPlayersDto>(nameof(PlayerUpdate), PlayerUpdate);
            System.Console.WriteLine("Game client configured.");
        }

        private void SetupConnectionStateHandlers()
        {
            _connection.Closed += (exception) =>
            {
                if (exception == null)
                {
                    Console.WriteLine("Connection closed without error.");
                }
                else
                {
                    Console.WriteLine($"Connection closed due to an error: {exception}");
                }
                _connectionStateChangedCallback(ConnectionState.Disconnected);
                return Task.CompletedTask;
            };
            _connection.Reconnected += (connectionId) =>
            {
                Console.WriteLine($"Connection successfully reconnected. The ConnectionId is now: {connectionId}");
                _connectionStateChangedCallback(ConnectionState.Connected);
                return Task.CompletedTask;
            };
            _connection.Reconnecting += (exception) =>
            {
                Console.WriteLine($"Connection started reconnecting due to an error: {exception}");
                _connectionStateChangedCallback(ConnectionState.Connecting);
                return Task.CompletedTask;
            };
        }

        public async Task StartAsync()
        {
            try
            {
                Console.WriteLine($"StartAsync :)");
                _connectionStateChangedCallback(ConnectionState.Connecting);
                Console.WriteLine($"StartAsync 2 :)");
                await _connection.StartAsync();
                _connectionStateChangedCallback(ConnectionState.Connected);
                var data = new JoinData { RoomId = Room, PlayerIndex = PlayerIndex };
                await _connection.SendAsync(nameof(IGameHub.Join), data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting {nameof(GameClient)}: {ex}");
                _connectionStateChangedCallback(ConnectionState.Disconnected);
            }
        }
        public ValueTask DisposeAsync() => _connection.DisposeAsync();

        public Task GameSeriesStarted(GameSeriesDto data)
        {
            _deck = new GameCardDeck((Rank)data.MinCardRank);
            var plannedRounds = data.PlannedRounds
                .Select(setting => new EumelRoundSettings(setting.StartingPlayer, setting.TricksToPlay));
            var plan = new EumelGamePlan(plannedRounds, _deck);
            var players = data.PlayerInfos.Select(p => new PlayerInfo(p.Name, p.Type)).ToList();
            var e = new GameSeriesStarted(data.GameId, players, plan);
            _gameSeriesEventCallback(e);
            return Task.CompletedTask;
        }
        public Task GameRoundStarted(RoundStartedDto data)
        {
            var settings = new EumelRoundSettings(data.StartingPlayer, data.TricksToPlay);
            _gameSeriesEventCallback(new RoundStarted(data.GameId, settings));
            return Task.CompletedTask;
        }

        public Task CardPlayed(CardPlayedDto data)
        {
            var ctx = new GameEventContext(data.GameId, data.RoundIndex);
            var e = new CardPlayed(ctx, new PlayerIndex(data.PlayerIndex), _deck[data.CardIndex]);
            _gameEventCallback(e);
            return Task.CompletedTask;
        }

        public Task HandReceived(HandReceivedDto data)
        {
            System.Console.WriteLine(data);
            var hand = data.CardIndices == null ?
                (IHand)new UnknownHand(data.NumberOfCards) :
                new KnownHand(data.CardIndices.Select(i => _deck[i]));
            var ctx = new GameEventContext(data.GameId, data.RoundIndex);
            var e = new HandReceived(ctx, new PlayerIndex(data.PlayerIndex), hand);
            System.Console.WriteLine(e);
            _gameEventCallback(e);
            return Task.CompletedTask;
        }

        public Task GuessGiven(GuessGivenDto data)
        {
            var ctx = new GameEventContext(data.GameId, data.RoundIndex);
            var e = new GuessGiven(ctx, new PlayerIndex(data.PlayerIndex), data.Count);
            System.Console.WriteLine(e);
            _gameEventCallback(e);
            return Task.CompletedTask;
        }

        public Task TrickWon(TrickWonDto data)
        {
            var ctx = new GameEventContext(data.GameId, data.RoundIndex);
            var e = new TrickWon(ctx, new PlayerIndex(data.PlayerIndex));
            _gameEventCallback(e);
            return Task.CompletedTask;
        }

        public Task GameRoundEnded(RoundResultDto data)
        {
            var settings = new EumelRoundSettings(data.GameRound.StartingPlayer, data.GameRound.TricksToPlay);
            var res = new RoundResult(data.PlayerResults.Select(
                player => new PlayerRoundResult(player.Guesses, player.TricksWon, player.Score)
            ).ToList());
            var e = new RoundEnded(data.GameId, settings, res);
            _gameSeriesEventCallback(e);
            return Task.CompletedTask;
        }

        public Task PlayerUpdate(CurrentLobbyPlayersDto data)
        {
            System.Console.WriteLine("Current lobby players: " + string.Join(",", data.PlayerConnections));
            _playerUpdateCallback(data);
            return Task.CompletedTask;
        }

        public Task StartNextRound()
        {
            return _connection.SendAsync(nameof(IGameHub.StartNextRound));
        }
        public Task PlayCard(Card card)
        {
            System.Console.WriteLine("Playing card");
            var cardIndex = _deck.AllCards.ToList().IndexOf(card);
            // TODO dont reuse the event dto
            var data = new CardPlayedDto(null, -1, PlayerIndex, cardIndex);
            return _connection.SendAsync(nameof(IGameHub.PlayCard), data);
        }
        public Task MakeGuess(int count)
        {
            System.Console.WriteLine("Making guess");
            var data = new GuessGivenDto(null, -1, PlayerIndex, count);
            return _connection.SendAsync(nameof(IGameHub.MakeGuess), data);
        }
    }
}