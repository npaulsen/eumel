using System;
using System.Linq;

namespace EumelCore
{
    public class EumelRound : IObservable<GameEvent>
    {
        private readonly PlayerCollection _players;
        private readonly GameCardDeck _allCards;

        private readonly EventCollection<GameEvent> _events;

        public readonly EumelRoundSettings Settings;

        public GameState State { get; private set; }

        public EumelRound(GameCardDeck cardStack, PlayerCollection players, EumelRoundSettings settings)
        {
            _players = players;
            _allCards = cardStack;
            _events = new EventCollection<GameEvent>();
            Settings = settings;
            State = GameState.Initial(players, settings);
        }

        public RoundResult Run()
        {
            InitPlayers();
            GiveCards();
            GetGuesses();
            PlayTricks();
            return GetResult();
        }

        private void InitPlayers()
        {
            foreach (var p in _players)
            {
                p.SetNewRound(this);
            }
        }

        private void GiveCards()
        {
            var hands = _allCards.DrawXTimesY(_players.PlayerCount, Settings.TricksToPlay);
            foreach (var(player, hand) in Enumerable.Zip(_players, hands))
            {
                Dispatch(new ReceivedHand(player, hand));
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
                var guess = player.GetGuess();
                var move = new MadeGuess(player, guess);
                if (IsValid(move))
                {
                    Dispatch(move);
                    break;
                }
            }
        }

        private bool IsValid(MadeGuess guess)
        {
            var isLastGuess = State.Players.Count(p => p.Guess.HasValue) == _players.PlayerCount - 1;
            if (!isLastGuess)
            {
                return true;
            }
            var totalGuessed = guess.Count + State.Players.Select(p => p.Guess).Where(g => g.HasValue).Sum();
            return totalGuessed != Settings.TricksToPlay;
        }

        private void PlayTrick()
        {
            foreach (var player in _players.AllStartingWith(State.TurnOfPlayerIndex))
            {
                GetMove(player);
            }
            Dispatch(new TrickWon(State.CurrentTrick.Winner));
        }

        private void GetMove(PlayerInfo player)
        {
            while (true)
            {
                var card = player.GetMove();
                var move = new PlayedCard(player, card);
                if (IsValid(move))
                {
                    Dispatch(move);
                    break;
                }
            }
        }

        private bool IsValid(PlayedCard move)
        {
            var card = move.Card;
            var playersHand = State.Players[move.Player.Index].Hand;
            var currentSuit = State.CurrentTrick.Suit;
            var switchesSuit = currentSuit.HasValue && card.Suit != currentSuit;
            if (switchesSuit && playersHand.MustFollow(currentSuit.Value))
            {
                return false;
            }
            return playersHand.Has(card);
        }

        public IDisposable Subscribe(IObserver<GameEvent> observer) => _events.Subscribe(observer);
    }
}