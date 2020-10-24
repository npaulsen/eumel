using System;
using System.Linq;

namespace EumelCore
{
    public class EumelRound : IObservable<GameEvent>
    {
        public readonly EumelRoundSettings Settings;
        private readonly PlayerCollection _players;
        private readonly GameCardDeck _allCards;

        private readonly EventCollection<GameEvent> _events;

        public GameState State { get; private set; }

        public EumelRound(GameCardDeck cardStack, PlayerCollection players, EumelRoundSettings settings)
        {
            _players = players;
            _allCards = cardStack;
            _events = new EventCollection<GameEvent>();
            Settings = settings;
            State = GameState.Initial(players.PlayerCount, settings);
        }

        public RoundResult Run()
        {
            GiveCards();
            GetGuesses();
            PlayTricks();
            return GetResult();
        }

        private void GiveCards()
        {
            var hands = _allCards.DrawXTimesY(_players.PlayerCount, Settings.TricksToPlay);
            foreach (var hand in Enumerable.Zip(_players, hands, (player, hand) => new HandReceived(player.Index, hand)))
            {
                Dispatch(hand);
            }
        }

        private void Dispatch(GameEvent gameEvent)
        {
            State = State.Dispatch(gameEvent);
            _events.Insert(gameEvent);
        }

        private void GetGuesses()
        {
            foreach (var player in _players.AllStartingWith(Settings.StartingPlayerIndex))
            {
                GetGuess(player);
            }
        }

        private void PlayTricks()
        {
            while (State.TricksPlayed < Settings.TricksToPlay)
            {
                PlayTrick();
            }
        }

        private RoundResult GetResult() => RoundResult.From(State);

        private void GetGuess(PlayerInfo player)
        {
            while (true)
            {
                var guess = player.GetGuess(State);
                var move = new GuessGiven(player.Index, guess);
                if (State.IsValid(move))
                {
                    Dispatch(move);
                    break;
                }
            }
        }
        private void PlayTrick()
        {
            foreach (var player in _players.AllStartingWith(State.Turn.PlayerIndex))
            {
                GetMove(player);
            }
            Dispatch(new TrickWon(State.CurrentTrick.PlayerWithHighestCard));
        }

        private void GetMove(PlayerInfo player)
        {
            while (true)
            {
                var card = player.GetMove(State);
                var move = new CardPlayed(player.Index, card);
                if (State.IsValid(move))
                {
                    Dispatch(move);
                    break;
                }
            }
        }

        public IDisposable Subscribe(IObserver<GameEvent> observer) => _events.Subscribe(observer);
    }
}