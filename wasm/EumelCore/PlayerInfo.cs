using System;

namespace EumelCore
{
    public class PlayerInfo
    {
        public readonly PlayerIndex Index;
        public readonly string Name;
        private readonly Player _interactivePlayer;

        public PlayerInfo(int index, string name, Player player)
        {
            Index = new PlayerIndex(index);
            Name = name;
            _interactivePlayer = player;
        }

        internal int GetGuess(GameState state) => _interactivePlayer.GetGuess(state);
        internal Card GetMove(GameState state) => _interactivePlayer.GetMove(state);

        public override bool Equals(object obj) => obj is PlayerInfo player && Index == player.Index;

        public override int GetHashCode() => HashCode.Combine(Index);

        public override string ToString() => Name;

    }
}