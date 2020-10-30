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
                PlayerInfo.CreateCustom("T1", new TrickBoy()),
                PlayerInfo.CreateCustom("T2", new TrickBoy()),
                PlayerInfo.CreateCustom("3", new TrickBoy()),
                PlayerInfo.CreateCustom("4", new TrickBoy()),
                PlayerInfo.CreateCustom("5", new TrickBoy()),
                PlayerInfo.CreateCustom("6", new TrickBoy()),
            };
            for (int repeat = 0; repeat <= 10; repeat++)
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