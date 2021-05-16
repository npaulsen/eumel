using System.Collections.Immutable;
using System.Linq;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core
{
    public record GameProgress(
        ImmutableListWithValueSemantics<GameSeriesEvent> SeriesEvents, 
        ImmutableListWithValueSemantics<GameEvent> LastRoundEvents)
    {
        public static GameProgress NotStarted => new(
            ImmutableListWithValueSemantics<GameSeriesEvent>.Empty,
            ImmutableListWithValueSemantics<GameEvent>.Empty);

        public int CurrentRoundIndex => SeriesEvents.OfType<RoundStarted>().Count() - 1;
    }
}