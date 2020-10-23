using System.Threading.Tasks;

namespace BlazorSignalRApp.Shared.HubInterface
{
    public interface IGameHub
    {
        Task StartNextRound();
        Task MakeGuess(GuessGivenDto data);
        Task PlayCard(CardPlayedDto data);
    }
}