using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eumel.Core.GameSeriesEvents;
using Eumel.Core.Players;

namespace Eumel.Core
{
    public class BotController
    {
        private readonly int _botDelayMs;
        private readonly IInvokablePlayer[] _bots;

        public BotController(IEnumerable<IInvokablePlayer> bots, EumelGameRoomDefinition def)
        {
            _botDelayMs = def.Settings.BotDelayMs;
            _bots = bots.ToArray();
            if (def.Players.Count != _bots.Length)
            {
                throw new ArgumentException($"'{nameof(bots)}' must have same length as players in game definition.");
            }
        }

        private bool HasBotFor(PlayerIndex playerIndex) => BotFor(playerIndex) is not null;
        private IInvokablePlayer BotFor(PlayerIndex playerIndex) => _bots[playerIndex.Value];

        internal void NotifySeriesStart(GameSeriesStarted seriesStartEvent)
        {
            foreach (var bot in _bots)
            {
                bot?.NoteSeriesStart(seriesStartEvent);
            }
        }

        internal Task OnGameEvent(object s, GameEventArgs e)
        {
            if (s is not GameEventHub context) throw new ArgumentException();
            HandleBotMoves(context);
            return Task.CompletedTask;
        }

        private void HandleBotMoves(GameEventHub ctx)
        {
            var turn = ctx.State.Turn;
            var nextTurn = turn.PlayerIndex;
            if (!HasBotFor(nextTurn)) return;

            var bot = BotFor(nextTurn);

            if (turn.NextEventType == typeof(GuessGiven))
            {
                GetGuess(ctx, nextTurn, bot);
            }
            else if (turn.NextEventType == typeof(CardPlayed))
            {
                GetMove(ctx, nextTurn, bot);
            }
        }

        private void GetMove(GameEventHub ctx, PlayerIndex nextTurn, IInvokablePlayer bot)
        {
            // TODO async
            Delay();
            while (true)
            {
                var card = bot.GetMove(ctx.State);
                if (ctx.TryPlayCard(nextTurn, card))
                    break;
            }
        }

        private void GetGuess(GameEventHub ctx, PlayerIndex nextTurn, IInvokablePlayer bot)
        {
            // TODO async
            Delay();
            while (true)
            {
                var count = bot.GetGuess(ctx.State);
                if (ctx.TryGiveGuess(nextTurn, count))
                    break;
            }
        }

        private void Delay() => Thread.Sleep(_botDelayMs);
    }
}