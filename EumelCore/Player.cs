using System.Collections.Generic;
using System.Linq;

namespace EumelCore
{

    public abstract class Player
    {
        protected EumelRound CurrentRound;

        public abstract int GetGuess();
        public abstract Card GetMove();

        public void SetCurrentRound(EumelRound round)
        {
            CurrentRound = round;
        }

        public void EndCurrentRound()
        {
            CurrentRound = null;
        }
    }
}