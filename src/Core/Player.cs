using System;
using System.Collections.Generic;
using System.Linq;

namespace Eumel.Core
{

    public interface IInvocablePlayer
    {
        int GetGuess(GameState state);
        Card GetMove(GameState state);
    }
}