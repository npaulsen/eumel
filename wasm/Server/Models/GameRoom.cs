using System;
using System.Collections.Generic;
using System.Linq;
//unnice
using BlazorSignalRApp.Shared.Rooms;
using EumelCore;
using Server.Models;

namespace Server.Models
{
    public class GameRoom : IObserver<GameEvent>
    {
        public readonly string Id;
        public readonly IReadOnlyList<PlayerInfo> Players;
        public readonly GameContext GameContext;

        public GameRoom(string id, IEnumerable<GamePlayerData> players)
        {
            Id = id;
            Players = players.Select(CreatePlayer).ToList();
            GameContext = new GameContext(Players.ToList());
            GameContext.Subscribe(this);
        }

        private static PlayerInfo CreatePlayer(GamePlayerData data) =>
        data.IsHuman? PlayerInfo.CreateHuman(data.Name) : PlayerInfo.CreateBot(data.Name);

        public void OnNext(GameEvent value)
        {
            var turn = GameContext.State.Turn;
            var nextTurn = turn.PlayerIndex;
            var bot = Players[nextTurn];
            if (bot.IsHuman) return;

            if (turn.NextEventType == typeof(GuessGiven))
            {
                GetGuess(nextTurn, bot.Player);
            }
            else if (turn.NextEventType == typeof(CardPlayed))
            {
                GetMove(nextTurn, bot.Player);
            }
        }

        private void GetMove(PlayerIndex nextTurn, IInvocablePlayer bot)
        {
            while (true)
            {
                var card = bot.GetMove(GameContext.State);
                if (GameContext.TryPlayCard(nextTurn, card))
                    break;
            }
        }
        private void GetGuess(PlayerIndex nextTurn, IInvocablePlayer bot)
        {
            while (true)
            {
                var count = bot.GetGuess(GameContext.State);
                if (GameContext.TryGiveGuess(nextTurn, count))
                    break;
            }
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }
    }
}