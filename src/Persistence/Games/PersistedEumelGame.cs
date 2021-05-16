using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Persistance.Games
{
    public class PersistedEumelGame {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<PersistedPlayer> Players { get; set; }
        
        public List<PersistedGameRound> Rounds { get; set; }


        public static PersistedEumelGame CreateFrom(EumelGameRoomDefinition room)
        {
            if (room.Plan != EumelGamePlan.For(room.Players.Count)){
                throw new NotImplementedException($"can only persist simple variants of {nameof(EumelGamePlan)}");
            }

            return new PersistedEumelGame {
                Name = room.Name,
                Players = room.Players
                    .Select(pi => new PersistedPlayer { Name = pi.Name, Type = pi.Type })
                    .ToList(),
                // Rounds = room.Events.OfType<RoundStarted>().Select((rse, index) => new PersistedGameRound{
                //     Index = index,
                //     StartingPlayerIndex = rse.Settings.StartingPlayerIndex,
                //     NumTricks = rse.Settings.TricksToPlay,
                // }).ToList()
            };
        }
        public EumelGameRoomDefinition ToEumelGameRoomDef() 
            => new EumelGameRoomDefinition(Name, Players.Select(p => new PlayerInfo(p.Name, p.Type)));
    }

    public class PersistedPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}