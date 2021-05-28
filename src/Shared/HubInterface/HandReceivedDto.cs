using System.Collections.Generic;

namespace Eumel.Shared.HubInterface
{
    public class HandReceivedDto : GameEventDto
    {
        public List<int> CardIndices { get; set; }

        public int NumberOfCards { get; set; }

        public HandReceivedDto() { }
        private HandReceivedDto(string gameId, int roundIndex, int playerIndex, List<int> cardIndices, int numberOfCards)
            : base(gameId, roundIndex, playerIndex)
        {
            CardIndices = cardIndices;
            NumberOfCards = numberOfCards;
        }
        public static HandReceivedDto ForKnownHand(string gameId, int roundIndex, int playerIndex, List<int> cardIndices) =>
            new(gameId, roundIndex, playerIndex, cardIndices, cardIndices.Count);

        public static HandReceivedDto ForSecretHand(string gameId, int roundIndex, int playerIndex, int numCards) =>
            new(gameId, roundIndex, playerIndex, null, numCards);
    }
}