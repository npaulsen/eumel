using System;
using System.Collections.Generic;
using System.Linq;

namespace Eumel.Core
{
    /// <summary> Holds and manages the <see cref="GameState"/>. </summary>
    public class GameEventHub : IObservableWithHistory<GameEvent>
    {
        private readonly EumelGamePlan _plan;
        private readonly EventCollection<GameEvent> _events;

        public GameState State { get; private set; }
        public EumelRoundSettings CurrentRoundSettings { get; private set; }
        private GameEventContext _gameEventContext;
        private readonly int _numPlayers;

        public event EventHandler<GameEventArgs> OnGameEvent;

        public bool HasMoreRounds => _gameEventContext.RoundIndex + 1 < _plan.Rounds.Count;

        public GameEventHub(EumelGameRoomDefinition definition, GameProgress progress)
        {
            _gameEventContext = new(definition.Name, -1);
            _numPlayers = definition.Players.Count;
            _plan = definition.Plan;
            // TODO: join event types. Make GameSeriesStarted contents static data of the room
            _events = new EventCollection<GameEvent>();
            if (progress.LastRoundEvents.Any())
            {
                ForwardTo(progress);
            }
        }

        public void MoveToNextRound()
        {
            if (!HasMoreRounds)
            {
                throw new InvalidOperationException("No more rounds");
            }
            _gameEventContext = _gameEventContext with {RoundIndex =  _gameEventContext.RoundIndex + 1};
            CurrentRoundSettings = _plan.Rounds[_gameEventContext.RoundIndex];
            State = GameState.Initial(_numPlayers, CurrentRoundSettings);
            _events.Clear();
        }

        public void StartCurrentRound()
        {
            if (CurrentRoundSettings is null)
            {
                throw new InvalidOperationException();
            }
            if (State.Players.Any(p => p.Hand is not null))
            {
                throw new InvalidOperationException();
            }
            GiveCards();
        }

        private void ForwardTo(GameProgress progress)
        {
            // TODO: move directly to right round.
            while (_gameEventContext.RoundIndex < progress.CurrentRoundIndex)
            {
                MoveToNextRound();
            }
            
            var allPreviousEvents = progress.LastRoundEvents.ToList();
            foreach (var e in allPreviousEvents.Take(allPreviousEvents.Count - 1))
            {
                State = State.Dispatch(e);
                _events.Insert(e);
            }
            Dispatch(allPreviousEvents.Last());
        }

        public bool TryGiveGuess(int player, int guess)
        {
            var move = new GuessGiven(_gameEventContext, new PlayerIndex(player), guess);
            if (State.IsValid(move))
            {
                Dispatch(move);
                return true;
            }
            return false;
        }
        public bool TryPlayCard(int player, int cardIndex) => TryPlayCard(player, _plan.Deck[cardIndex]);

        public bool TryPlayCard(int player, Card card)
        {
            // TODO where to transform data
            var move = new CardPlayed(_gameEventContext, new PlayerIndex(player), card);
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
                Dispatch(new HandReceived(_gameEventContext, State.Turn.PlayerIndex, hand));
            }
        }

        private void Dispatch(GameEvent newEvent)
        {
            State = State.Dispatch(newEvent);
            _events.Insert(newEvent);
            OnGameEvent?.Invoke(this, newEvent);
            AfterInsert(newEvent);
        }

        private void AfterInsert(GameEvent value)
        {
            var trickComplete = State.CurrentTrick.Moves.Count == State.Players.Count;
            if (value is CardPlayed && trickComplete)
            {
                Dispatch(new TrickWon(_gameEventContext, State.CurrentTrick.PlayerWithHighestCard));
            }
        }

        public IDisposable Subscribe(IObserver<GameEvent> observer) 
            => _events.Subscribe(observer);
            
        public IDisposable SubscribeWithPreviousEvents(IObserver<GameEvent> observer)
            => _events.SubscribeWithPreviousEvents(observer);    
    }
}