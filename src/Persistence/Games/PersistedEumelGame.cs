using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Persistance.Games
{
    public class PersistedEumelGame
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public GameStatus Status { get; set; }

        public List<PersistedPlayer> Players { get; set; }


        public static PersistedEumelGame CreateFrom(EumelGameRoom room)
        {
            var def = room.Definition;
            if (def.Plan != EumelGamePlan.For(def.Players.Count))
            {
                throw new NotImplementedException($"can only persist simple variants of {nameof(EumelGamePlan)}");
            }

            return new PersistedEumelGame
            {
                Name = def.Name,
                Players = def.Players
                    .Select(pi => new PersistedPlayer { Name = pi.Name, Type = pi.Type })
                    .ToList(),
                Status = room.Status,
            };
        }
        public EumelGameRoom ToEumelGameRoom()
            => new(
                new(Name, Players.Select(p => new PlayerInfo(p.Name, p.Type))),
                Status);
    }

    public class PersistedPlayer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}