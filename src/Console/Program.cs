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
            Simulation.Run();

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