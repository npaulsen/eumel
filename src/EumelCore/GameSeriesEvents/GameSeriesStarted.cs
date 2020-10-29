using System.Collections.Generic;
namespace EumelCore.GameSeriesEvents
{
    public class GameSeriesStarted : GameSeriesEvent
    {
        public readonly IReadOnlyList<string> PlayerNames;
        public readonly IReadOnlyList<EumelRoundSettings> PlannedRounds;
        public readonly GameCardDeck Deck;

        public GameSeriesStarted(IReadOnlyList<string> playerNames, IReadOnlyList<EumelRoundSettings> plannedRounds, GameCardDeck deck)
        {
            PlayerNames = playerNames;
            PlannedRounds = plannedRounds;
            Deck = deck;
        }
    }
}