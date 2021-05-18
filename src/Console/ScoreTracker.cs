using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core.GameSeriesEvents;

namespace EumelConsole
{
    public class ScoreTracker : IObserver<GameSeriesEvent>
    {
        private int[] _scores;

        public IEnumerable<int> Scores => _scores;

        public ScoreTracker()
        {
            _scores = new int[0];
        }

        public void OnNext(GameSeriesEvent ev)
        {
            if (ev is RoundEnded roundEnded)
            {
                var res = roundEnded.Result.PlayerResults;
                if (_scores == null || _scores.Length == 0)
                {
                    _scores = new int[res.Count];
                }
                foreach (var (r, i) in res.Select((r, i) => (r, i)))
                {
                    _scores[i] += r.Score;
                }
            }
        }
        public void OnError(Exception e) { }
        public void OnCompleted() { }
    }
}