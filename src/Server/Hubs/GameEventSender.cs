using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using Eumel.Shared.HubInterface;

namespace Eumel.Server.Hubs
{
    class GameEventForwarder : IObserver<GameEvent>, IObserver<GameSeriesEvent>, IDisposable
    {
        private IGameClient _client;
        private Dictionary<Card, int> _cardIndices;
        private IDisposable _unsub1;
        private IDisposable _unsub2;
        public GameEventForwarder(IGameClient client)
        {
            _client = client;
        }
        public void SubscribeTo(GameRoom room)
        {
            if (_unsub1 != null) throw new InvalidOperationException("already subsribed");
            _unsub1 = room.Subscribe((IObserver<GameSeriesEvent>) this);
            _unsub2 = room.GameContext.Subscribe((IObserver<GameEvent>) this);
        }

        public void OnNext(GameEvent e)
        {
            var task = e
            switch
            {
                HandReceived hand => _client.HandReceived(new HandReceivedDto(hand.Player, hand.Hand.Select(GetIndex).ToList())),
                CardPlayed move => _client.CardPlayed(new CardPlayedDto(move.Player, GetIndex(move.Card))),
                GuessGiven guess => _client.GuessGiven(new GuessGivenDto(guess.Player, guess.Count)),
                TrickWon trick => _client.TrickWon(new TrickWonDto(trick.Player)),
                _ => Task.CompletedTask,
            };
            task.GetAwaiter().GetResult();
            System.Console.WriteLine("sent " + e);
        }

        public void OnNext(GameSeriesEvent value)
        {
            switch (value)
            {
                case GameSeriesStarted gameSeriesStarted:
                    OnNext(gameSeriesStarted);
                    break;
                case RoundStarted roundStarted:
                    OnNext(roundStarted);
                    break;
                case RoundEnded ended:
                    OnNext(ended);
                    break;
            }
        }

        public void OnNext(GameSeriesStarted started)
        {
            var deck = started.Deck;
            _cardIndices = deck.AllCards
                .Select((Card, Index) => (Card, Index))
                .ToDictionary(pair => pair.Card, pair => pair.Index);
            var minCardRank = (int) deck[0].Rank;
            var plannedRounds = started.PlannedRounds.Select(ConvertRoundSettingsToDto);
            var data = new GameSeriesDto(minCardRank, started.PlayerNames, plannedRounds);
            _client.GameSeriesStarted(data);
        }
        public void OnNext(RoundStarted started)
        {
            var settings = started.Settings;
            _client.GameRoundStarted(ConvertRoundSettingsToDto(settings));
        }
        public void OnNext(RoundEnded ended)
        {
            var roundData = ConvertRoundSettingsToDto(ended.Settings);
            var resultData = new RoundResultDto(roundData, ended.Result.PlayerResults.Select(
                pres => new PlayerRoundResultDto(pres.Guesses, pres.TricksWon, pres.Score)
            ));
            _client.GameRoundEnded(resultData);
        }

        public static GameRoundDto ConvertRoundSettingsToDto(EumelRoundSettings settings) =>
            new GameRoundDto(settings.StartingPlayerIndex, settings.TricksToPlay);

        private int GetIndex(Card c) => _cardIndices[c];

        public void OnCompleted() =>
            throw new NotImplementedException();

        public void OnError(Exception error) =>
            throw new NotImplementedException();

        public void Dispose()
        {
            _unsub1?.Dispose();
            _unsub2?.Dispose();
        }
    }
}