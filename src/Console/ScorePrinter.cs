using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core.GameSeriesEvents;

public class ScorePrinter : IObserver<GameSeriesEvent>
{
    public void OnNext(GameSeriesEvent ev)
    {
        if (ev is RoundEnded roundEnded)
        {
            List<string> columns = new List<string>
            {
                roundEnded.Settings.StartingPlayerIndex.ToString(),
                roundEnded.Settings.TricksToPlay.ToString(),
            };
            foreach (var playerResult in roundEnded.Result.PlayerResults)
            {
                columns.Add(playerResult.Guesses.ToString());
                columns.Add(playerResult.TricksWon.ToString());
                columns.Add(playerResult.Score.ToString());
            }
            System.Console.WriteLine(string.Join(";", columns));
        }
    }

    public void PrintHeader(Eumel.Core.PlayerInfo[] players)
    {
        List<string> columns = new List<string>
        {
            "StartingPlayerIndex",
            "Cards",
        };
        foreach (var playerName in players.Select(p => p.Name))
        {
            columns.Add("Guesses " + playerName);
            columns.Add("Won " + playerName);
            columns.Add("Score " + playerName);
        }
        System.Console.WriteLine(string.Join(";", columns));

    }

    public void OnError(Exception e) { }
    public void OnCompleted() { }
}