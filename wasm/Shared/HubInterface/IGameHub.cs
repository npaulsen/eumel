using System.Threading.Tasks;

namespace BlazorSignalRApp.Shared.HubInterface
{
    public interface IGameHub
    {
        Task StartNextRound();
        Task MakeGuess(GuessGivenDto data);
        Task PlayCard(CardPlayedDto data);
        Task Join(JoinData data);
    }

    public class JoinData
    {
        public string Room { get; set; }
        public int PlayerIndex { get; set; }
    }
}