namespace Eumel.Shared.HubInterface
{
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