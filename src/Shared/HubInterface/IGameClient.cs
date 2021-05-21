using System.Threading.Tasks;

namespace Eumel.Shared.HubInterface
{
    public interface IGameClient
    {
        Task GameSeriesStarted(GameSeriesDto data);
        Task GameRoundStarted(RoundStartedDto data);
        Task GameRoundEnded(RoundResultDto data);

        Task CardPlayed(CardPlayedDto data);
        Task HandReceived(HandReceivedDto dto);
        Task GuessGiven(GuessGivenDto data);
        Task TrickWon(TrickWonDto data);

        Task PlayerUpdate(CurrentLobbyPlayersDto data);

    }
}