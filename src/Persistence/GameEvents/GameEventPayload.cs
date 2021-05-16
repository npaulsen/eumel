using Eumel.Core;

namespace Eumel.Persistance.GameEvents
{
    public abstract class GameEventPayload {
        public int PlayerIndex { get; set; }

        protected GameEventPayload() {}

        public GameEventPayload(GameEvent ge) {
            PlayerIndex = (int)ge.Player;
        }
    }
}