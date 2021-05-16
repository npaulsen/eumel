using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core
{
    public record EumelGameRoomDefinition(
        string Name, 
        ImmutableListWithValueSemantics<PlayerInfo> Players,
        EumelGamePlan Plan,
        GameRoomSettings Settings){

        public EumelGameRoomDefinition(string name, IEnumerable<PlayerInfo> players)
        : this(
            name, 
            players.ToImmutableList().WithValueSemantics(), 
            EumelGamePlan.For(players.Count()), 
            GameRoomSettings.Default)
        { }
    }
}