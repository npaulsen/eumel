namespace Eumel.Shared.HubInterface
{
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
}