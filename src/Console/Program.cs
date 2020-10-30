using System;
using System.Linq;
using Eumel.Core;
using Eumel.Core.Players;

namespace EumelConsole
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Simulation();

        }
        static void Simulation()
        {
            var players = new []
            {
                PlayerInfo.CreateCustom("Dumb2", new TrickBoy(6)),
                PlayerInfo.CreateBot("Random"),
                PlayerInfo.CreateCustom("Dumb2", new TrickBoy(6)),
                PlayerInfo.CreateCustom("Dumb2", new TrickBoy(6)),
                PlayerInfo.CreateCustom("Dumb2", new TrickBoy(6)),
                PlayerInfo.CreateCustom("Dumb2", new TrickBoy(6)),
            };
            for (int repeat = 0; repeat <= 70; repeat++)
            {
                var scoreTracker = new ScoreTracker();
                for (int games = 0; games < 1000; games++)
                {
                    var room = new GameRoom("the game", players, new GameRoomSettings(0));
                    room.Subscribe(scoreTracker);
                    while (room.HasMoreRounds)
                    {
                        room.StartNextRound();
                    }
                }
                Console.WriteLine("Total scores: " + string.Join(", ", Enumerable.Zip(players.Select(p => p.Name), scoreTracker.Scores)));
            }
        }
        static void PlayAgainstBots()
        {
            var players = new []
            {
                PlayerInfo.CreateBot("Fatz!"),
                PlayerInfo.CreateBot("Hans"),
                PlayerInfo.CreateCustom("YOU", new ConsolePlayer()),
            };
            var room = new GameRoom("the game", players, new GameRoomSettings(0));
            var logger = new ConsoleGameObserver();
            room.Subscribe(logger);
            room.GameContext.Subscribe(logger);
            while (room.HasMoreRounds)
            {
                room.StartNextRound();
            }
        }
    }
}