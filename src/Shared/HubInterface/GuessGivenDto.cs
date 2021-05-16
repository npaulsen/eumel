namespace Eumel.Shared.HubInterface
{
    public class GuessGivenDto : GameEventDto
    {
        public int Count { get; set; }
        public GuessGivenDto() { }
        public GuessGivenDto(string gameId, int roundIndex, int playerIndex, int count)
            : base(gameId, roundIndex, playerIndex)
        {
            Count = count;
        }
    }
}