namespace Eumel.Core.GameSeriesEvents
{
    public class RoundEnded : GameSeriesEvent
    {
        public readonly EumelRoundSettings Settings;
        public readonly RoundResult Result;

        public RoundEnded(EumelRoundSettings settings, RoundResult result)
        {
            Settings = settings;
            Result = result;
        }
    }
}