using System;
using System.Collections.Generic;
using System.Linq;

namespace EumelCore
{

    public abstract class Player
    {
        public abstract int GetGuess(GameState state);
        public abstract Card GetMove(GameState state);
    }
}