using Eumel.Core;

namespace Eumel.Persistance.GameEvents
{
    public class TrickWonPayload : GameEventPayload
    {
        public TrickWonPayload() {}

        public TrickWonPayload(TrickWon tw) : base(tw)
        {
        }
    }
}