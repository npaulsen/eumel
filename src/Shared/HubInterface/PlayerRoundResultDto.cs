namespace Eumel.Shared.HubInterface
{
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
}