using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorSignalRApp.Shared.HubInterface;
using EumelCore;
using EumelCore.GameSeriesEvents;
using Microsoft.AspNetCore.SignalR.Client;

namespace wasm.Client.Pages
{
    class GameClient : IGameClient
    {
        private HubConnection _connection;

        private GameCardDeck _deck;
        private List<string> _playerNames;

        private Action<GameEvent> _gameEventCallback;
        private Action<GameSeriesEvent> _gameSeriesEventCallback;
        private int _playerIndex = 2;

        public HubConnectionState ConnectionState => _connection.State;
        public GameClient(Action<GameSeriesEvent> gameSeriesEventCallback, Action<GameEvent> gameEventCallback)
        {
            _gameEventCallback = gameEventCallback;
            _gameSeriesEventCallback = gameSeriesEventCallback;

            _connection = new HubConnectionBuilder()
                .WithUrl(GameHubInterface.HubUrl)
                .Build();

            _connection.On<string>(GameHubInterface.Events.Test, Test);
            _connection.On<GameSeriesDto>(GameHubInterface.Events.GameSeriesStarted, GameSeriesStarted);
            _connection.On<GameRoundDto>(GameHubInterface.Events.GameRoundStarted, GameRoundStarted);
            _connection.On<HandReceivedDto>(GameHubInterface.Events.CardsReceived, HandReceived);
            _connection.On<CardPlayedDto>(GameHubInterface.Events.CardPlayed, CardPlayed);
            _connection.On<GuessGivenDto>(GameHubInterface.Events.GuessGiven, GuessGiven);
            _connection.On<TrickWonDto>(GameHubInterface.Events.TrickWon, TrickWon);
            System.Console.WriteLine("Game client configured.");
        }

        public Task Test(string msg)
        {
            System.Console.WriteLine(msg);
            return Task.CompletedTask;
        }
        public Task StartAsync() => _connection.StartAsync();
        public Task DisposeAsync() => _connection.DisposeAsync();

        public Task GameSeriesStarted(GameSeriesDto data)
        {
            _deck = new GameCardDeck((Rank) data.MinCardRank);
            _playerNames = data.PlayerNames;
            var plannedRoundsMock = new List<EumelRoundSettings>
            {
                new EumelRoundSettings(0, 1),
                new EumelRoundSettings(1, 2)
            };
            var e = new GameSeriesStarted(_playerNames, plannedRoundsMock, _deck);
            _gameSeriesEventCallback(e);
            return Task.CompletedTask;
        }
        public Task GameRoundStarted(GameRoundDto data)
        {
            var settings = new EumelRoundSettings(data.StartingPlayer, data.TricksToPlay);
            _gameSeriesEventCallback(new RoundStarted(settings));
            return Task.CompletedTask;
        }
        public Task CardPlayed(CardPlayedDto data)
        {
            var e = new CardPlayed(new PlayerIndex(data.PlayerIndex), _deck[data.CardIndex]);
            _gameEventCallback(e);
            return Task.CompletedTask;
        }

        public Task HandReceived(HandReceivedDto data)
        {
            var hand = new Hand(data.CardIndices.Select(i => _deck[i]));
            var e = new HandReceived(new PlayerIndex(data.PlayerIndex), hand);
            System.Console.WriteLine(e);
            _gameEventCallback(e);
            return Task.CompletedTask;
        }

        public Task GuessGiven(GuessGivenDto data)
        {
            var e = new GuessGiven(new PlayerIndex(data.PlayerIndex), data.Count);
            System.Console.WriteLine(e);
            _gameEventCallback(e);
            return Task.CompletedTask;
        }

        public Task TrickWon(TrickWonDto data)
        {
            var e = new TrickWon(new PlayerIndex(data.PlayerIndex));
            _gameEventCallback(e);
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
            var data = new CardPlayedDto(_playerIndex, cardIndex);
            return _connection.SendAsync(nameof(IGameHub.PlayCard), data);
        }
        public Task MakeGuess(int count)
        {
            System.Console.WriteLine("Making guess");
            var data = new GuessGivenDto(_playerIndex, count);
            return _connection.SendAsync(nameof(IGameHub.MakeGuess), data);
        }
    }
}