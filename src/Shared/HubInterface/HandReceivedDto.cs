using System.Collections.Generic;

namespace Eumel.Shared.HubInterface
{
    public class HandReceivedDto
    {
        public int PlayerIndex { get; set; }
        public List<int> CardIndices { get; set; }

        public int NumberOfCards { get; set; }

        public HandReceivedDto() { }
        private HandReceivedDto(int playerIndex, List<int> cardIndices, int numberOfCards)
        {
            PlayerIndex = playerIndex;
            CardIndices = cardIndices;
        }
        public static HandReceivedDto ForKnownHand(int playerIndex, List<int> cardIndices) =>
            new HandReceivedDto(playerIndex, cardIndices, cardIndices.Count);

        public static HandReceivedDto ForSecretHand(int playerIndex, int numCards) =>
            new HandReceivedDto(playerIndex, null, numCards);
    }
}