namespace Eumel.Core.GameSeriesEvents
{
    public record RoundStarted(string GameUuid, EumelRoundSettings Settings) 
        : GameSeriesEvent(GameUuid);
}