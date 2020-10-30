using System.Collections.Generic;
using System.Linq;

namespace Eumel.Shared.HubInterface
{
    public class RoundResultDto
    {
        public GameRoundDto GameRound { get; set; }
        public PlayerRoundResultDto[] PlayerResults { get; set; }

        public RoundResultDto() { }

        public RoundResultDto(GameRoundDto gameRound, IEnumerable<PlayerRoundResultDto> playerResults)
        {
            GameRound = gameRound;
            PlayerResults = playerResults.ToArray();
        }
    }
}