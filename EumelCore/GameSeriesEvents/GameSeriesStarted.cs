using System.Collections.Generic;
namespace EumelCore.GameSeriesEvents
{
    public class GameSeriesStarted : GameSeriesEvent
    {
        public readonly IReadOnlyList<string> PlayerNames;
        public readonly IReadOnlyList<EumelRoundSettings> PlannedRounds;

        public GameSeriesStarted(IReadOnlyList<string> playerNames, IReadOnlyList<EumelRoundSettings> plannedRounds)
        {
            PlayerNames = playerNames;
            PlannedRounds = plannedRounds;
        }
    }
}