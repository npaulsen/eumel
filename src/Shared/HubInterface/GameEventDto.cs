namespace Eumel.Shared.HubInterface
{
    public abstract class BaseGameEventDto
    {
        public string GameId { get; set; }

        public BaseGameEventDto() { }

        protected BaseGameEventDto(string gameId)
        {
            GameId = gameId;
        }

    }

    public abstract class GameEventDto : BaseGameEventDto
    {
        public int RoundIndex { get; set; }
        public int PlayerIndex { get; set; }

        public GameEventDto() { }

        protected GameEventDto(string gameId, int roundIndex, int playerIndex)
            : base(gameId)
        {
            RoundIndex = roundIndex;
            PlayerIndex = playerIndex;
        }
    }
}