namespace Eumel.Shared.HubInterface
{
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
}