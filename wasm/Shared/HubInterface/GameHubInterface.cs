namespace BlazorSignalRApp.Shared.HubInterface
{
    public static class GameHubInterface
    {

        public static string HubUrl => "https://localhost:5001/gamehub";

        public static class Events
        {
            public static string Test => nameof(IGameClient.Test);

            public static string GameSeriesStarted => nameof(IGameClient.GameSeriesStarted);
            public static string GameRoundStarted => nameof(IGameClient.GameRoundStarted);
            public static string CardPlayed => nameof(IGameClient.CardPlayed);
            public static string CardsReceived => nameof(IGameClient.HandReceived);
            public static string GuessGiven => nameof(IGameClient.GuessGiven);
            public static string TrickWon => nameof(IGameClient.TrickWon);
        }
    }

}