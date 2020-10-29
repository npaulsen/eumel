using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Eumel.Core
{
    public class PlayerCollection : IEnumerable<PlayerInfo>
    {
        private readonly IReadOnlyList<PlayerInfo> _players;

        public PlayerInfo this[int index] => _players[index];

        public PlayerCollection(IEnumerable<PlayerInfo> players)
        {
            _players = players.ToList();
        }

        public int PlayerCount => _players.Count;

        internal IEnumerable<PlayerInfo> AllStartingWith(int firstPlayerIndex) =>
        _players.Concat(_players).Skip(firstPlayerIndex).Take(_players.Count);

        public IEnumerator<PlayerInfo> GetEnumerator() => _players.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _players.GetEnumerator();
    }
}