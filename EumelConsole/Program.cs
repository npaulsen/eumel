using System;
using System.Collections.Generic;
using System.Linq;
using EumelCore;
using EumelCore.GameSeriesEvents;
using EumelCore.Players;

namespace EumelConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var players = new [] { "Goofy", "Fatz!" }
                .Select((name, index) => new PlayerInfo(index, name, new DumbPlayer())).ToList();
            System.Console.Write("Enter your name: ");
            var name = Console.ReadLine();
            players.Add(new PlayerInfo(2, name, new ConsolePlayer(players)));
            var game = new EumelGame(new PlayerCollection(players));
            game.Subscribe(new ConsoleGameObserver());
            while (game.HasMoreRounds)
            {
                Console.Write("(Hit Enter to start next round)");
                Console.ReadLine();
                game.PlayRound(new [] { new ConsoleGameObserver() });
            }
        }

        public class ConsoleGameObserver : IObserver<GameEvent>, IObserver<GameSeriesEvent>
        {
            private IReadOnlyList<string> _playerNames;
            private List<int> _totalScores;

            public void OnNext(GameEvent e)
            {
                var message = e
                switch
                {
                    MadeGuess guess => $"{e.Player.Name} made guess of <{guess.Count}>",
                    PlayedCard move => $"{e.Player.Name} played {move.Card} ({move.Card.Rank} of {move.Card.Suit})",
                    ReceivedHand received => $"{e.Player.Name} got {received.Hand.NumberOfCards} cards",
                    TrickWon trick => $"{e.Player.Name} won the trick",
                    _ => e.ToString(),
                };
                Render(message, ConsoleColor.DarkGreen);
            }
            public void OnNext(GameSeriesEvent e)
            {
                var message = e
                switch
                {
                    GameSeriesStarted start => $"a new game series of {start.PlannedRounds.Count} rounds was started",
                    RoundStarted roundStarted => $"a new round started",
                    RoundEnded roundEnded => $"round finished. {PointInfo(roundEnded.Result.PlayerResults)}",
                    _ => e.ToString(),
                };
                Render(message, ConsoleColor.DarkMagenta);
                if (e is GameSeriesStarted s)
                {
                    _playerNames = s.PlayerNames;
                    _totalScores = s.PlayerNames.Select(_ => 0).ToList();
                }
                else if (e is RoundEnded end)
                {
                    foreach (var(r, i) in end.Result.PlayerResults.Select((r, i) => (r, i)))
                    {
                        _totalScores[i] += r.Score;
                    }
                    Render("Total scores: " + string.Join(", ", Enumerable.Zip(_playerNames, _totalScores)), ConsoleColor.DarkMagenta);
                }
            }

            private static void Render(string message, ConsoleColor color)
            {
                var oldFc = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(" --- " + message + " --- ");
                Console.ForegroundColor = oldFc;
            }

            private string PointInfo(IEnumerable<RoundResult.PlayerRoundResult> playerResults)
            {
                var points = new List<string>();
                foreach (var(player, res) in Enumerable.Zip(_playerNames, playerResults))
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