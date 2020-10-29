namespace Eumel.Core.GameSeriesEvents
{
    public class RoundStarted : GameSeriesEvent
    {
        public readonly EumelRoundSettings Settings;

        public RoundStarted(EumelRoundSettings settings)
        {
            Settings = settings;
        }
    }
}