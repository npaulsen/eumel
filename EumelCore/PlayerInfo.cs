using System;

namespace EumelCore
{
    public class PlayerInfo
    {
        public readonly int Index;
        public readonly string Name;
        private readonly Player _interactivePlayer;

        public PlayerInfo(int index, string name, Player player)
        {
            Index = index;
            Name = name;
            _interactivePlayer = player;
        }

        internal int GetGuess() => _interactivePlayer.GetGuess();
        internal Card GetMove() => _interactivePlayer.GetMove();

        public override bool Equals(object obj) => obj is PlayerInfo player && Index == player.Index;

        public override int GetHashCode() => HashCode.Combine(Index);

        public override string ToString() => Name;

        internal void SetNewRound(EumelRound eumelRound)
        {
            _interactivePlayer.SetCurrentRound(eumelRound);
        }
    }
}