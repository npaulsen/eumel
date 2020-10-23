using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSignalRApp.Shared.HubInterface;
using EumelCore;
using EumelCore.GameSeriesEvents;
using Microsoft.AspNetCore.SignalR;

namespace BlazorSignalRApp.Server.Hubs
{
    class GameEventSender : IObserver<GameEvent>, IObserver<GameSeriesEvent>
    {
        private IGameClient _client;
        private Dictionary<Card, int> _cardIndices;
        public GameEventSender(IGameClient client)
        {
            _client = client;
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
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
            }
        }

        public void OnNext(GameSeriesStarted started)
        {
            var deck = started.Deck;
            _cardIndices = deck.AllCards
                .Select((Card, Index) => (Card, Index))
                .ToDictionary(pair => pair.Card, pair => pair.Index);
            var minCardRank = (int) deck[0].Rank;
            _client.GameSeriesStarted(new GameSeriesDto(minCardRank, started.PlayerNames.ToList()));
        }
        public void OnNext(RoundStarted started)
        {
            var settings = started.Settings;
            _client.GameRoundStarted(new GameRoundDto(settings.StartingPlayerIndex, settings.TricksToPlay));
        }

        private int GetIndex(Card c) => _cardIndices[c];
    }
}