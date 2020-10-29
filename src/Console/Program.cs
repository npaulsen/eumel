using System;
using Eumel.Core;
using Eumel.Core.Players;

namespace EumelConsole
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var players = new []
            {
                PlayerInfo.CreateBot("Fatz!"),
                PlayerInfo.CreateBot("Hans"),
                PlayerInfo.CreateCustom("YOU", new ConsolePlayer()),
            };
            var room = new GameRoom("the game", players);
            var logger = new ConsoleGameObserver();
            room.Subscribe(logger);
            room.GameContext.Subscribe(logger);
            while (room.HasMoreRounds)
            {
                Console.Write("(Hit Enter to start next round)");
                Console.ReadLine();
                room.StartNextRound();
            }
        }
    }
}