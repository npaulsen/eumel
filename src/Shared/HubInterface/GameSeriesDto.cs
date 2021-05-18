using System.Collections.Generic;
using System.Linq;

namespace Eumel.Shared.HubInterface
{
    public class GameSeriesDto : BaseGameEventDto
    {
        public int MinCardRank { get; set; }
        public List<PlayerDto> PlayerInfos { get; set; }

        public List<GameRoundDto> PlannedRounds { get; set; }

        public GameSeriesDto() { }
        public GameSeriesDto(string gameId,
            int minCardRank,
            IEnumerable<PlayerDto> playerInfos,
            IEnumerable<GameRoundDto> plannedRounds)
            : base(gameId)
        {
            MinCardRank = minCardRank;
            PlayerInfos = playerInfos.ToList();
            PlannedRounds = plannedRounds.ToList();
        }
    }
}