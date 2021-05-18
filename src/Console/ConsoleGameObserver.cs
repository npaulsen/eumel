using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using static System.Console;

namespace EumelConsole
{
    partial class Program
    {
        public class ConsoleGameObserver : IObserver<GameEvent>, IObserver<GameSeriesEvent>
        {
            private IReadOnlyList<string> _playerNames;
            private List<int> _totalScores;
            public void OnNext(GameEvent e)
            {
                var player = _playerNames[e.Player];
                var message = e
                switch
                {
                    GuessGiven guess => $"{player} made guess of <{guess.Count}>",
                    CardPlayed move => $"{player} played {move.Card} ({move.Card.Rank} of {move.Card.Suit})",
                    HandReceived received => $"{player} got {received.Hand.NumberOfCards} cards",
                    TrickWon trick => $"{player} won the trick",
                    _ => e.ToString(),
                };
                Render(message, ConsoleColor.DarkGreen);
            }
            public void OnNext(GameSeriesEvent e)
            {
                var message = e
                switch
                {
                    GameSeriesStarted start => $"a new game series of {start.Plan.Rounds.Count} rounds was started",
                    RoundStarted roundStarted => $"a new round started",
                    RoundEnded roundEnded => $"round finished. {PointInfo(roundEnded.Result.PlayerResults)}",
                    _ => e.ToString(),
                };
                Render(message, ConsoleColor.DarkMagenta);
                if (e is GameSeriesStarted s)
                {
                    _playerNames = s.Players.Select(p => p.Name).ToList();
                    _totalScores = s.Players.Select(_ => 0).ToList();
                }
                else if (e is RoundEnded end)
                {
                    foreach (var (r, i) in end.Result.PlayerResults.Select((r, i) => (r, i)))
                    {
                        _totalScores[i] += r.Score;
                    }
                    Render("Total scores: " + string.Join(", ", Enumerable.Zip(_playerNames, _totalScores)), ConsoleColor.DarkMagenta);
                }
            }

            private static void Render(string message, ConsoleColor color)
            {
                ConsoleColor oldFc = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(" --- " + message + " --- ");
                Console.ForegroundColor = oldFc;
            }

            private string PointInfo(IEnumerable<PlayerRoundResult> playerResults)
            {
                var points = new List<string>();
                foreach (var (player, res) in Enumerable.Zip(_playerNames, playerResults))
                {
                    points.Add(player + " +" + res.Score);
                }
                return $"Points: {string.Join(", ", points)}";
            }
            public void OnCompleted() { }
            public void OnError(Exception e) { }
        }
    }
}