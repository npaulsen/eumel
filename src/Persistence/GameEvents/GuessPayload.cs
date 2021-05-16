using Eumel.Core;

namespace Eumel.Persistance.GameEvents
{
    public class GuessPayload : GameEventPayload
    {
        public int Guess { get; set; }

        public GuessPayload() {}

        public GuessPayload(GuessGiven gg) : base(gg)
        {
            Guess = gg.Count;
        }
    }
}