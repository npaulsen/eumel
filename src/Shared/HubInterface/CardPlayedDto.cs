namespace Eumel.Shared.HubInterface
{
    public class CardPlayedDto : GameEventDto
    {
        public int CardIndex { get; set; }

        public CardPlayedDto() { }
        public CardPlayedDto(string gameId, int roundIndex, int playerIndex, int cardIndex)
            : base(gameId, roundIndex, playerIndex)
        {
            CardIndex = cardIndex;
        }
    }
}