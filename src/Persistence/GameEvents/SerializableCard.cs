using Eumel.Core;

namespace Eumel.Persistance.GameEvents
{
    public class SerializableCard {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public static SerializableCard From(Card card) 
            => new SerializableCard { Suit = card.Suit, Rank = card.Rank };
    }
}