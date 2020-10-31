using System;
using System.Linq;
using Eumel.Core;
using Eumel.Core.Players;

namespace EumelConsole
{
    public class Simulation
    {
        public static void Run()
        {
            var players = new []
            {
                PlayerInfo.CreateCustom("TrickBoy1", new MaxTrickBoy()),
                PlayerInfo.CreateCustom("O1", new Opportunist(1)),
                PlayerInfo.CreateCustom("TrickBoy2", new MaxTrickBoy()),
                PlayerInfo.CreateCustom("O1", new Opportunist(1)),
                PlayerInfo.CreateCustom("O1", new Opportunist(1)),
                PlayerInfo.CreateCustom("O1", new Opportunist(1)),
            };
            for (int repeat = 0; repeat <= 10; repeat++)
            {
                var scoreTracker = new ScoreTracker();
                // scoreTracker.PrintHeader(players);
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
    }
}