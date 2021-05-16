using System.Collections.Generic;
using Eumel.Persistance.GameEvents;

namespace Eumel.Persistance.Games
{
    public class PersistedGameRound
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public int StartingPlayerIndex { get; set; }
        public int NumTricks { get; set; }
    }
}