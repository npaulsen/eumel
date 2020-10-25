using System;
using System.Linq;
using EumelCore;
using EumelCore.GameSeriesEvents;
using EumelCore.Players;

namespace Server.Hubs
{
    public class GameContext : IObservable<GameSeriesEvent>, IObservable<GameEvent>
    {
        private readonly EumelGamePlan _plan;
        private readonly EventCollection<GameSeriesEvent> _seriesEvents;
        private readonly EventCollection<GameEvent> _events;
        private readonly Player[] _bots;

        private GameState State;
        private EumelRoundSettings CurrentRoundSettings;
        private int _nextRoundIndex = 0;

        public GameContext(int numPlayers)
        {
            _bots =
                _bots = new Player[] { null, null }
                .Concat(Enumerable.Range(0, numPlayers - 2)
                    .Select(_ => new DumbPlayer(3000))
                ).ToArray();
            _plan = EumelGamePlan.For(numPlayers);
            _seriesEvents = new EventCollection<GameSeriesEvent>();
            _events = new EventCollection<GameEvent>();
            _seriesEvents.Insert(new GameSeriesStarted(
                playerNames: new [] { "Svea", "Niklas" }.Concat(Enumerable.Range(1, numPlayers - 2)
                    .Select(i => "Bot " + i)
                ).ToArray(),
                plannedRounds : _plan.PlannedRounds.ToList(),
                deck : _plan.Deck));
        }
        public void StartNextRound()
        {
            if (_nextRoundIndex >= _plan.PlannedRounds.Count)
            {
                throw new InvalidOperationException("No more rounds");
            }
            CurrentRoundSettings = _plan.PlannedRounds[_nextRoundIndex];
            _nextRoundIndex += 1;
            State = GameState.Initial(_bots.Length, CurrentRoundSettings);
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
        internal bool TryPlayCard(int player, int cardIndex)
        {
            // TODO where to transform data
            var move = new CardPlayed(new PlayerIndex(player), _plan.Deck[cardIndex]);
            if (State.IsValid(move))
            {
                Dispatch(move);
                return true;
            }
            return false;
        }

        private void GiveCards()
        {
            var hands = _plan.Deck.DrawXTimesY(_bots.Length, CurrentRoundSettings.TricksToPlay);
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
            AskBotsIfTheirTurn();
        }

        private void AskBotsIfTheirTurn()
        {
            // Probably nicer to have them listen for a specific event..
            var nextTurn = State.Turn.PlayerIndex;
            var bot = _bots[nextTurn];
            if (bot == null) return;

            if (State.Turn.NextEventType == typeof(GuessGiven))
            {
                GetGuess(nextTurn, bot);
            }
            else if (State.Turn.NextEventType == typeof(CardPlayed))
            {
                GetMove(nextTurn, bot);
            }
        }

        private void GetMove(PlayerIndex nextTurn, Player bot)
        {
            while (true)
            {
                var card = bot.GetMove(State);
                var move = new CardPlayed(nextTurn, card);
                if (State.IsValid(move))
                {
                    Dispatch(move);
                    break;
                }
            }
        }
        private void GetGuess(PlayerIndex nextTurn, Player bot)
        {
            while (true)
            {
                var count = bot.GetGuess(State);
                var move = new GuessGiven(nextTurn, count);
                if (State.IsValid(move))
                {
                    Dispatch(move);
                    break;
                }
            }
        }

        private void Dispatch(GameEvent newEvent)
        {
            System.Console.WriteLine("Dispatching" + newEvent);
            State = State.Dispatch(newEvent);
            System.Console.WriteLine("New State: " + State);
            _events.Insert(newEvent);
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