namespace Eumel.Core.GameSeriesEvents
{
    public record RoundEnded(
        string GameUuid,
        EumelRoundSettings Settings,
        RoundResult Result)
        : GameSeriesEvent(GameUuid);
}