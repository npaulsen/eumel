using System.Collections.Generic;
using System.Linq;

namespace Eumel.Shared.HubInterface
{
    public class GameSeriesDto
    {
        public int MinCardRank { get; set; }
        public List<string> PlayerNames { get; set; }

        public List<GameRoundDto> PlannedRounds { get; set; }

        public GameSeriesDto() { }
        public GameSeriesDto(int minCardRank, IEnumerable<string> playerNames, IEnumerable<GameRoundDto> plannedRounds)
        {
            MinCardRank = minCardRank;
            PlayerNames = playerNames.ToList();
            PlannedRounds = plannedRounds.ToList();
        }
    }
}