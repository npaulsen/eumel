using System;

namespace Eumel.Core
{
    public class GameContext : IObservable<GameEvent>
    {
        private readonly EumelGamePlan _plan;
        private readonly EventCollection<GameEvent> _events;

        public GameState State { get; private set; }
        public EumelRoundSettings CurrentRoundSettings { get; private set; }
        private int _nextRoundIndex = 0;
        private readonly int _numPlayers;

        public event EventHandler<GameEventArgs> OnGameEvent;

        public GameContext(EumelGamePlan plan, int numPlayers)
        {
            _numPlayers = numPlayers;
            _plan = plan;
            // TODO: join event types. Make GameSeriesStarted contents static data of the room
            _events = new EventCollection<GameEvent>();

        }

        public void PrepareNextRound()
        {
            if (_nextRoundIndex >= _plan.PlannedRounds.Count)
            {
                throw new InvalidOperationException("No more rounds");
            }
            CurrentRoundSettings = _plan.PlannedRounds[_nextRoundIndex];
            _nextRoundIndex += 1;
            State = GameState.Initial(_numPlayers, CurrentRoundSettings);
            _events.Clear();
        }

        public void StartNextRound()
        {
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

        public IDisposable Subscribe(IObserver<GameEvent> observer) => _events.Subscribe(observer);
    }
}