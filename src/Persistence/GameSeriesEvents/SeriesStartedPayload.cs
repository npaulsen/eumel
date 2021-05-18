using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Persistance.GameEvents
{
    internal class SeriesStartedPayload : GameSeriesEventPayload
    {
        public List<PlayerInfo> Players { get; set; }

        public SeriesStartedPayload() { }
        public SeriesStartedPayload(GameSeriesStarted seriesStarted)
        {
            Players = seriesStarted.Players.ToList();
        }
    }
}