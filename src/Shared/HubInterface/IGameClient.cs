using System.Threading.Tasks;

namespace Eumel.Shared.HubInterface
{
    public interface IGameClient
    {
        Task Test(string msg);
        Task GameSeriesStarted(GameSeriesDto data);
        Task GameRoundStarted(GameRoundDto data);
        Task GameRoundEnded(RoundResultDto data);

        Task CardPlayed(CardPlayedDto data);
        Task HandReceived(HandReceivedDto dto);
        Task GuessGiven(GuessGivenDto data);
        Task TrickWon(TrickWonDto data);

    }
}