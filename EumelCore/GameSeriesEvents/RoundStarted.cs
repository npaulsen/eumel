namespace EumelCore.GameSeriesEvents
{
    public class RoundStarted : GameSeriesEvent
    {
        public readonly EumelRound Round;

        public RoundStarted(EumelRound round)
        {
            Round = round;
        }
    }
}