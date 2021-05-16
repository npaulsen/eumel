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

    public class RoundStartedDto : BaseGameEventDto
    {
        public int StartingPlayer { get; set; }
        public int TricksToPlay { get; set; }

        public RoundStartedDto() { }

        public RoundStartedDto(string gameId, int startingPlayer, int tricksToPlay)
            :base(gameId)
        {
            StartingPlayer = startingPlayer;
            TricksToPlay = tricksToPlay;
        }
    }
}