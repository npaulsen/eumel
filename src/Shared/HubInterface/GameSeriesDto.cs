using System.Collections.Generic;
using System.Linq;

namespace Eumel.Shared.HubInterface
{
    public class GameSeriesDto : BaseGameEventDto
    {
        public int MinCardRank { get; set; }
        public List<string> PlayerNames { get; set; }

        public List<GameRoundDto> PlannedRounds { get; set; }

        public GameSeriesDto() { }
        public GameSeriesDto(string gameId, 
            int minCardRank, 
            IEnumerable<string> playerNames, 
            IEnumerable<GameRoundDto> plannedRounds)
            : base(gameId)
        {
            MinCardRank = minCardRank;
            PlayerNames = playerNames.ToList();
            PlannedRounds = plannedRounds.ToList();
        }
    }
}