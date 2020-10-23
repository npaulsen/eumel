using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSignalRApp.Shared.HubInterface
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

    public class GameSeriesDto
    {
        public int MinCardRank { get; set; }
        public List<string> PlayerNames { get; set; }

        public GameSeriesDto() { }
        public GameSeriesDto(int minCardRank, List<string> playerNames)
        {
            MinCardRank = minCardRank;
            PlayerNames = playerNames;
        }
    }

    public class GameRoundDto
    {
        public int StartingPlayer { get; set; }
        public int TricksToPlay { get; set; }

        public GameRoundDto() { }

        public GameRoundDto(int startingPlayer, int tricksToPlay)
        {
            StartingPlayer = startingPlayer;
            TricksToPlay = tricksToPlay;
        }
    }

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
    public class PlayerRoundResultDto
    {
        public int Guesses { get; set; }
        public int TricksWon { get; set; }
        public int Score { get; set; }

        public PlayerRoundResultDto() { }

        public PlayerRoundResultDto(int guesses, int tricksWon, int score)
        {
            Guesses = guesses;
            TricksWon = tricksWon;
            Score = score;
        }
    }

    public class CardPlayedDto
    {
        public int PlayerIndex { get; set; }
        public int CardIndex { get; set; }

        public CardPlayedDto() { }
        public CardPlayedDto(int playerIndex, int cardIndex)
        {
            PlayerIndex = playerIndex;
            CardIndex = cardIndex;
        }
    }
    public class HandReceivedDto
    {
        public int PlayerIndex { get; set; }
        public List<int> CardIndices { get; set; }

        public HandReceivedDto() { }
        public HandReceivedDto(int playerIndex, List<int> cardIndices)
        {
            PlayerIndex = playerIndex;
            CardIndices = cardIndices;
        }
    }
    public class GuessGivenDto
    {
        public int PlayerIndex { get; set; }
        public int Count { get; set; }
        public GuessGivenDto() { }
        public GuessGivenDto(int playerIndex, int count)
        {
            PlayerIndex = playerIndex;
            Count = count;
        }
    }
    public class TrickWonDto
    {
        public int PlayerIndex { get; set; }

        public TrickWonDto() { }
        public TrickWonDto(int playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}