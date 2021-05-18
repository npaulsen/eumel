using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Eumel.Core.GameSeriesEvents;
namespace Eumel.Core
{
    /// <summary> 
    /// Manages an active game series. 
    /// Controls access to the <see cref="GameEventHub"/>, 
    /// has a <see cref="BotController"/> and provides <see cref="GameSeriesEvent"/>s.
    /// </summary>
    public class ActiveLobby : IObservableWithHistory<GameSeriesEvent>
    {
        public readonly EumelGameRoomDefinition Room;
        // public readonly string Name;
        // public readonly IReadOnlyList<PlayerInfo> Players;
        public readonly GameEventHub GameContext;

        // public GameRoomSettings Settings { get; private set; }

        private readonly EventCollection<GameSeriesEvent> _events;

        private readonly BotController _botController;

        public bool HasMoreRounds => GameContext.HasMoreRounds;

        public ActiveLobby(BotController botController, EumelGameRoomDefinition room, GameProgress progress)
        {
            Room = room ?? throw new ArgumentException(nameof(room));
            _botController = botController ?? throw new ArgumentException(nameof(botController));
            _events = new EventCollection<GameSeriesEvent>();

            GameContext = new GameEventHub(room, progress);

            // TODO when / how is an active lobby disposed
            GameContext.OnGameEvent += async (s, e) => await GameEventHandler(s, e);
            GameContext.OnGameEvent += async (s, e) => await _botController.OnGameEvent(s, e);


            SetProgress(progress);
        }

        /// <summary>
        /// This will emit <see cref="GameSeriesStarted"/> in case no previous progress
        /// was restored.
        /// </summary>
        public void EnsureStarted()
        {
            if (!_events.Any())
            {
                AddSeriesStart(new GameSeriesStarted(Room));
            }
        }

        public bool TryGiveGuess(int playerIndex, int count) => GameContext.TryGiveGuess(playerIndex, count);

        public bool TryPlayCard(int playerIndex, int cardIndex) => GameContext.TryPlayCard(playerIndex, cardIndex);

        public void StartNextRound()
        {
            EnsureStarted();
            // Currently trusting on RoundStarting arriving earlier than 
            // first events from the running game.
            GameContext.MoveToNextRound();
            _events.Insert(new RoundStarted(Room.Name, GameContext.CurrentRoundSettings));
            GameContext.StartCurrentRound();
        }

        private void SetProgress(GameProgress progress)
        {
            var existingEvents = progress.SeriesEvents;
            if (existingEvents.Any())
            {
                ImportExistingEvents(existingEvents);
            }
        }

        private void ImportExistingEvents(IEnumerable<GameSeriesEvent> existingEvents)
        {
            if (existingEvents.First() is not GameSeriesStarted seriesStarted)
            {
                throw new Exception($"First event is not {nameof(GameSeriesStarted)}");
            }
            AddSeriesStart(seriesStarted);
            foreach (var evt in existingEvents.Skip(1))
            {
                _events.Insert(evt);
            }
        }

        private void AddSeriesStart(GameSeriesStarted seriesStarted)
        {
            _events.Insert(seriesStarted);
            _botController.NotifySeriesStart(seriesStarted);
        }

        private Task GameEventHandler(object sender, GameEventArgs game)
        {
            var state = GameContext.State;
            var roundIsOver = game.GameEvent is TrickWon && state.AllTricksPlayed;
            if (roundIsOver)
            {
                _events.Insert(new RoundEnded(Room.Name, GameContext.CurrentRoundSettings, RoundResult.From(state)));
            }
            return Task.CompletedTask;
        }

        public IDisposable Subscribe(IObserver<GameSeriesEvent> observer)
            => _events.Subscribe(observer);

        public IDisposable SubscribeWithPreviousEvents(IObserver<GameSeriesEvent> observer)
            => _events.SubscribeWithPreviousEvents(observer);

    }
}