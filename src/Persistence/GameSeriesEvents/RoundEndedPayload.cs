using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Persistance.GameEvents
{
    internal class RoundEndedPayload : GameSeriesEventPayload
    {
        public EumelRoundSettings Settings { get; set; }
        public List<PlayerRoundResult> PlayerResults { get; set; }

        public RoundEndedPayload() {}

        public RoundEndedPayload(RoundEnded roundEnded)
        {
            Settings = roundEnded.Settings;
            PlayerResults = roundEnded.Result.PlayerResults.ToList();
        }
    }
}