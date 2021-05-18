using System;
using System.Collections.Immutable;
using System.Linq;
using Eumel.Core;
using Eumel.Core.Players;

namespace EumelConsole
{
    partial class Program
    {
        static void Main(string[] args)
        {
            PlayAgainstBots();
            Simulation.Run();

        }
        static void PlayAgainstBots()
        {
            var players = new[]
            {
                Player.CreateBot("Fatz!"),
                Player.CreateBot("Hans"),
                Player.CreateCustom("YOU", new ConsolePlayer()),
            };

            var playerFactory = new PlayerFactory()
                .RegisterOrOverrideCreator(nameof(ConsolePlayer), () => new ConsolePlayer());

            var gameDef = new EumelGameRoomDefinition(
                "the game",
                players.Select(p => p.Info).ToImmutableList().WithValueSemantics(),
                EumelGamePlan.For(players.Length),
                new GameRoomSettings(0)
            );
            var botController = new BotController(players.Select(p => p.Invocable), gameDef);
            var lobby = new ActiveLobby(botController, gameDef, GameProgress.NotStarted);

            var logger = new ConsoleGameObserver();
            lobby.SubscribeWithPreviousEvents(logger);
            lobby.GameContext.SubscribeWithPreviousEvents(logger);
            while (lobby.HasMoreRounds)
            {
                lobby.StartNextRound();
            }
        }
    }
}