using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core.GameSeriesEvents;

namespace Eumel.Core
{

    public interface IInvokablePlayer
    {
        int GetGuess(GameState state);
        
        Card GetMove(GameState state);

        void NoteSeriesStart(GameSeriesStarted seriesStarted);
    }
}