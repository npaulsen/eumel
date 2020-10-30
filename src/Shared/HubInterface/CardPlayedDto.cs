namespace Eumel.Shared.HubInterface
{
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
}