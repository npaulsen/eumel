using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using Eumel.Shared.HubInterface;

namespace Eumel.Server.Hubs
{
    public class GameEventForwarder : IObserver<GameEvent>, IObserver<GameSeriesEvent>, IDisposable
    {
        private readonly IGameClient _client;
        private readonly int _playerIndex;
        private Dictionary<Card, int> _cardIndices;
        private IDisposable _unsub1;
        private IDisposable _unsub2;
        public GameEventForwarder(IGameClient client, int playerIndex)
        {
            _client = client;
            _playerIndex = playerIndex;
        }
        public void SubscribeTo(ActiveLobby room)
        {
            if (_unsub1 != null) throw new InvalidOperationException("already subsribed");
            _unsub1 = room.SubscribeWithPreviousEvents(this);
            _unsub2 = room.GameContext.SubscribeWithPreviousEvents(this);
        }

        public void OnNext(GameEvent e)
        {
            var task = e
            switch
            {
                HandReceived hand => _client.HandReceived(GetHandReceivedData(e.Context, hand)),
                CardPlayed move => _client.CardPlayed(new CardPlayedDto(e.Context.GameId, e.Context.RoundIndex, move.Player, GetIndex(move.Card))),
                GuessGiven guess => _client.GuessGiven(new GuessGivenDto(e.Context.GameId, e.Context.RoundIndex, guess.Player, guess.Count)),
                TrickWon trick => _client.TrickWon(new TrickWonDto(e.Context.GameId, e.Context.RoundIndex, trick.Player)),
                _ => Task.CompletedTask,
            };
            task.GetAwaiter().GetResult();
            System.Console.WriteLine("sent " + e);
        }

        private HandReceivedDto GetHandReceivedData(GameEventContext ctx, HandReceived hand)
        {
            if (hand.Player == _playerIndex)
            {
                var knownHand = hand.Hand as KnownHand;
                return HandReceivedDto.ForKnownHand(ctx.GameId, ctx.RoundIndex, hand.Player, knownHand.Select(GetIndex).ToList());
            }
            return HandReceivedDto.ForSecretHand(ctx.GameId, ctx.RoundIndex, hand.Player, hand.Hand.NumberOfCards);
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
            var deck = started.Plan.Deck;
            _cardIndices = deck.AllCards
                .Select((Card, Index) => (Card, Index))
                .ToDictionary(pair => pair.Card, pair => pair.Index);
            var minCardRank = (int)deck[0].Rank;
            var plannedRounds = started.Plan.Rounds.Select(ConvertRoundSettingsToDto);
            var data = new GameSeriesDto(started.GameUuid, minCardRank, started.Players.Select(p => p.Name), plannedRounds);
            _client.GameSeriesStarted(data);
        }
        public void OnNext(RoundStarted started)
        {
            var settings = started.Settings;
            var dto = new RoundStartedDto(started.GameUuid, settings.StartingPlayerIndex, settings.TricksToPlay);
            _client.GameRoundStarted(dto);
        }
        public void OnNext(RoundEnded ended)
        {
            var roundData = ConvertRoundSettingsToDto(ended.Settings);
            var resultData = new RoundResultDto(ended.GameUuid, roundData, ended.Result.PlayerResults.Select(
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