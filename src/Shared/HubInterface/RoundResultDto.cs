using System.Collections.Generic;
using System.Linq;

namespace Eumel.Shared.HubInterface
{
    public class RoundResultDto : BaseGameEventDto
    {
        public GameRoundDto GameRound { get; set; }
        public PlayerRoundResultDto[] PlayerResults { get; set; }

        public RoundResultDto() { }

        public RoundResultDto(string gameId,
            GameRoundDto gameRound,
            IEnumerable<PlayerRoundResultDto> playerResults)
            : base(gameId)
        {
            GameRound = gameRound;
            PlayerResults = playerResults.ToArray();
        }
    }
}