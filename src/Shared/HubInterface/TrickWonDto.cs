namespace Eumel.Shared.HubInterface
{
    public class TrickWonDto : GameEventDto
    {
        public TrickWonDto() { }
        public TrickWonDto(string gameId, int roundIndex, int playerIndex)
            : base(gameId, roundIndex, playerIndex)
        {
        }
    }
}