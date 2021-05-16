using Eumel.Core;

namespace Eumel.Persistance.GameEvents
{
    public class CardPlayedPayload : GameEventPayload
    {
        public SerializableCard Card { get; set; }

        public CardPlayedPayload() {}

        public CardPlayedPayload(CardPlayed cp) : base(cp)
        {
            Card = SerializableCard.From(cp.Card);
        }
    }
}