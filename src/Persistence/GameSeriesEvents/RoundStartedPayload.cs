using Eumel.Core;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Persistance.GameEvents
{
    internal class RoundStartedPayload : GameSeriesEventPayload
    {
        public EumelRoundSettings Settings { get; set; }

        public RoundStartedPayload() { }
        public RoundStartedPayload(RoundStarted roundStarted)
        {
            Settings = roundStarted.Settings;
        }
    }
}