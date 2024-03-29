using System;

namespace Eumel.Core
{
    public class GameEventArgs : EventArgs
    {
        public readonly GameEvent GameEvent;

        public GameEventArgs(GameEvent gameEvent)
        {
            GameEvent = gameEvent;
        }
        public static implicit operator GameEventArgs(GameEvent gameEvent) => new(gameEvent);
    }
}