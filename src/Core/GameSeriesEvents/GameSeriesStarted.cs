using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Eumel.Core.GameSeriesEvents
{
    public record GameSeriesStarted : GameSeriesEvent
    {
        public readonly ImmutableListWithValueSemantics<PlayerInfo> Players;
        public readonly EumelGamePlan Plan;

        public GameSeriesStarted(string gameUuid, IEnumerable<PlayerInfo> players, EumelGamePlan gamePlan)
            : base(gameUuid)
        {
            Players = players.ToImmutableList().WithValueSemantics();
            Plan = gamePlan;
        }

        public GameSeriesStarted(EumelGameRoomDefinition roomDef)
            : this(roomDef.Name, roomDef.Players, roomDef.Plan)
        {}
    }
}