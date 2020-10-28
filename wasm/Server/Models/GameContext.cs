using System;
using System.Collections.Generic;
using System.Linq;
using EumelCore;
using EumelCore.GameSeriesEvents;
using EumelCore.Players;

namespace Server.Models
{
    public class GameEventArgs : EventArgs
    {
        public readonly GameEvent GameEvent;

        public GameEventArgs(GameEvent gameEvent)
        {
            GameEvent = gameEvent;
        }
        public static implicit operator GameEventArgs(GameEvent gameEvent) => new GameEventArgs(gameEvent);
    }

    public class GameContext : IObservable<GameSeriesEvent>, IObservable<GameEvent>
    {
        private readonly EumelGamePlan _plan;
        private readonly EventCollection<GameSeriesEvent> _seriesEvents;
        private readonly EventCollection<GameEvent> _events;

        public GameState State { get; private set; }
        private EumelRoundSettings CurrentRoundSettings;
        private int _nextRoundIndex = 0;
        private readonly int _numPlayers;

        public event EventHandler<GameEventArgs> OnGameEvent;

        public GameContext(List<Server.Models.PlayerInfo> players)
        {
            _numPlayers = players.Count;
            _plan = EumelGamePlan.For(_numPlayers);
            // TODO: join event types. Make GameSeriesStarted contents static data of the room
            _seriesEvents = new EventCollection<GameSeriesEvent>();
            _events = new EventCollection<GameEvent>();
            _seriesEvents.Insert(new GameSeriesStarted(
                playerNames: players.Select(p => p.Name).ToArray(),
                plannedRounds: _plan.PlannedRounds.ToList(),
                deck: _plan.Deck));
        }

        public void StartNextRound()
        {
            if (_nextRoundIndex >= _plan.PlannedRounds.Count)
            {
                throw new InvalidOperationException("No more rounds");
            }
            CurrentRoundSettings = _plan.PlannedRounds[_nextRoundIndex];
            _nextRoundIndex += 1;
            State = GameState.Initial(_numPlayers, CurrentRoundSettings);
            _events.Clear();
            _seriesEvents.Insert(new RoundStarted(CurrentRoundSettings));
            GiveCards();
        }

        internal bool TryGiveGuess(int player, int guess)
        {
            var move = new GuessGiven(new PlayerIndex(player), guess);
            if (State.IsValid(move))
            {
                Dispatch(move);
                return true;
            }
            return false;
        }
        internal bool TryPlayCard(int player, int cardIndex) => TryPlayCard(player, _plan.Deck[cardIndex]);

        internal bool TryPlayCard(int player, Card card)
        {
            // TODO where to transform data
            var move = new CardPlayed(new PlayerIndex(player), card);
            if (State.IsValid(move))
            {
                Dispatch(move);
                return true;
            }
            return false;
        }

        private void GiveCards()
        {
            var hands = _plan.Deck.DrawXTimesY(_numPlayers, CurrentRoundSettings.TricksToPlay);
            foreach (var hand in hands)
            {
                Dispatch(new HandReceived(State.Turn.PlayerIndex, hand));
            }
        }

        public void AfterInsert(GameEvent value)
        {
            if (value is TrickWon won && State.AllTricksPlayed)
            {
                Dispatch(new RoundEnded(CurrentRoundSettings, RoundResult.From(State)));
                return;
            }
            if (value is CardPlayed played)
            {
                var trickComplete = State.CurrentTrick.Moves.Count == State.Players.Count;
                if (trickComplete)
                {
                    Dispatch(new TrickWon(State.CurrentTrick.PlayerWithHighestCard));
                    return;
                }
            }
        }

        private void Dispatch(GameEvent newEvent)
        {
            System.Console.WriteLine("Dispatching" + newEvent);
            State = State.Dispatch(newEvent);
            System.Console.WriteLine("New State: " + State);
            _events.Insert(newEvent);
            OnGameEvent?.Invoke(this, newEvent);
            AfterInsert(newEvent);
        }
        private void Dispatch(GameSeriesEvent newEvent)
        {
            _seriesEvents.Insert(newEvent);
        }

        public IDisposable Subscribe(IObserver<GameSeriesEvent> observer) => _seriesEvents.Subscribe(observer);

        public IDisposable Subscribe(IObserver<GameEvent> observer) => _events.Subscribe(observer);
    }
}