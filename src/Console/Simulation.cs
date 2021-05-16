using System;
using System.Collections.Immutable;
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
                Player.CreateCustom("TrickBoy1", new MaxTrickBoy()),
                Player.CreateCustom("O1", new Opportunist()),
                Player.CreateCustom("TrickBoy2", new MaxTrickBoy()),
                Player.CreateCustom("O1", new Opportunist()),
                Player.CreateCustom("O1", new Opportunist()),
                Player.CreateCustom("O1", new Opportunist()),
            };
            for (int repeat = 0; repeat <= 10; repeat++)
            {
                var scoreTracker = new ScoreTracker();
                // scoreTracker.PrintHeader(players);
                for (int games = 0; games < 1000; games++)
                {
                    var gameDef = new EumelGameRoomDefinition(
                        "the game", 
                        players.Select(p => p.Info).ToImmutableList().WithValueSemantics(), 
                        EumelGamePlan.For(players.Length), 
                        new GameRoomSettings(0)
                    );
                    var botController = new BotController(players.Select(p => p.Invocable), gameDef);
                    var room = new ActiveLobby(botController, gameDef, GameProgress.NotStarted);
                    room.SubscribeWithPreviousEvents(scoreTracker);
                    while (room.HasMoreRounds)
                    {
                        room.StartNextRound();
                    }
                }
                Console.WriteLine("Total scores: " + string.Join(", ", Enumerable.Zip(players.Select(p => p.Info.Name), scoreTracker.Scores)));
            }
        }
    }
}