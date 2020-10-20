using System;
using System.Collections.Generic;
using System.Linq;
using EumelCore.GameSeriesEvents;

namespace EumelCore
{
    public class EumelGame : IObservable<GameSeriesEvent>
    {
        private readonly PlayerCollection _players;

        private readonly EumelGamePlan _plan;

        private readonly EventCollection<GameSeriesEvent> _events;

        public int NextRoundIndex => _events.OfType<RoundEnded>().Count();
        public int TotalRounds => _plan.PlannedRounds.Count;

        public bool HasMoreRounds => NextRoundIndex < TotalRounds;

        public EumelGame(PlayerCollection players)
        {
            _players = players;
            _plan = EumelGamePlan.For(players.PlayerCount);
            _events = new EventCollection<GameSeriesEvent>();
            _events.Insert(new GameSeriesStarted(
                playerNames: _players.Select(p => p.Name).ToList(),
                plannedRounds: _plan.PlannedRounds.ToList()));
        }

        public void PlayRound(IEnumerable<IObserver<GameEvent>> observers)
        {
            if (!HasMoreRounds)
            {
                throw new InvalidOperationException("No more rounds");
            }
            var plan = _plan.PlannedRounds[NextRoundIndex];
            var round = new EumelRound(_plan.Deck, _players, plan);
            _events.Insert(new RoundStarted(round));
            foreach (var observer in observers)
            {
                round.Subscribe(observer);
            }
            var result = round.Run();
            _events.Insert(new RoundEnded(plan, result));
        }

        public IDisposable Subscribe(IObserver<GameSeriesEvent> observer) =>
        _events.Subscribe(observer);
    }
}