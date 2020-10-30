using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core
{
    public class GameRoom : IObservable<GameSeriesEvent>
    {
        public readonly string Id;
        public readonly IReadOnlyList<PlayerInfo> Players;
        public readonly GameContext GameContext;

        public GameRoomSettings Settings { get; private set; }

        private readonly EventCollection<GameSeriesEvent> _events;

        public GameRoom(string id, IEnumerable<PlayerInfo> players, GameRoomSettings settings)
        {
            Id = id;
            Players = players.ToList();
            Settings = settings;

            _events = new EventCollection<GameSeriesEvent>();

            var plan = EumelGamePlan.For(Players.Count);

            GameContext = new GameContext(plan, Players.Count);
            GameContext.OnGameEvent += async(s, e) => await GameEventHandler(s, e);
            var seriesStartEvent = new GameSeriesStarted(
                playerNames: players.Select(p => p.Name).ToArray(),
                plannedRounds: plan.PlannedRounds.ToList(),
                deck: plan.Deck);
            _events.Insert(seriesStartEvent);
            foreach (var player in Players.Where(p => p.IsBot))
            {
                player.Player.NoteSeriesStart(seriesStartEvent);
            }
        }

        public IDisposable Subscribe(IObserver<GameSeriesEvent> observer) => _events.Subscribe(observer);

        public bool TryGiveGuess(int playerIndex, int count) => GameContext.TryGiveGuess(playerIndex, count);

        public bool TryPlayCard(int playerIndex, int cardIndex) => GameContext.TryPlayCard(playerIndex, cardIndex);

        public void StartNextRound()
        {
            // Currently trusting on RoundStarting arriving earlier than 
            // first events from the running game. Therefore 
            GameContext.PrepareNextRound();
            _events.Insert(new RoundStarted(GameContext.CurrentRoundSettings));
            GameContext.StartNextRound();
        }

        public bool HasMoreRounds => GameContext.HasMoreRounds;

        private Task GameEventHandler(object sender, GameEventArgs game)
        {
            OnNext(game.GameEvent);
            return Task.CompletedTask;
        }

        private void OnNext(GameEvent value)
        {
            var state = GameContext.State;
            var roundIsOver = value is TrickWon won && state.AllTricksPlayed;
            if (roundIsOver)
            {
                _events.Insert(new RoundEnded(GameContext.CurrentRoundSettings, RoundResult.From(state)));
            }

            HandleBotMoves(state);
        }

        private void HandleBotMoves(GameState state)
        {
            var turn = state.Turn;
            var nextTurn = turn.PlayerIndex;
            var playerAtTurn = Players[nextTurn];
            if (playerAtTurn.IsHuman) return;

            if (turn.NextEventType == typeof(GuessGiven))
            {
                GetGuess(nextTurn, playerAtTurn.Player);
            }
            else if (turn.NextEventType == typeof(CardPlayed))
            {
                GetMove(nextTurn, playerAtTurn.Player);
            }
        }

        private void GetMove(PlayerIndex nextTurn, IInvocablePlayer bot)
        {
            Thread.Sleep(Settings.BotDelayMs);
            while (true)
            {
                var card = bot.GetMove(GameContext.State);
                if (GameContext.TryPlayCard(nextTurn, card))
                    break;
            }
        }
        private void GetGuess(PlayerIndex nextTurn, IInvocablePlayer bot)
        {
            Thread.Sleep(Settings.BotDelayMs);
            while (true)
            {
                var count = bot.GetGuess(GameContext.State);
                if (GameContext.TryGiveGuess(nextTurn, count))
                    break;
            }
        }
    }
}