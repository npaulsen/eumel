using System;
using Eumel.Core;
using Xunit;

namespace Core.Tests
{
    public class GameStateTests
    {
        [Fact]
        public void GameState_DispatchingWithUnknownHands_Succeeds()
        {
            var gameCtx = new GameEventContext("game", 12);
            var gamestate = GameState.Initial(3, new EumelRoundSettings(0, 1));
            gamestate = gamestate.Dispatch(new HandReceived(gameCtx, new PlayerIndex(0), new UnknownHand(1)));
            gamestate = gamestate.Dispatch(new HandReceived(gameCtx, new PlayerIndex(1), new UnknownHand(1)));
            gamestate = gamestate.Dispatch(new HandReceived(gameCtx, new PlayerIndex(2), new UnknownHand(1)));

            gamestate = gamestate.Dispatch(new GuessGiven(gameCtx, new PlayerIndex(0), 0));
            gamestate = gamestate.Dispatch(new GuessGiven(gameCtx, new PlayerIndex(1), 0));
            gamestate = gamestate.Dispatch(new GuessGiven(gameCtx, new PlayerIndex(2), 0));

            gamestate = gamestate.Dispatch(new CardPlayed(gameCtx, new PlayerIndex(0), new Card(Suit.Hearts, Rank.Ace)));
            System.Console.WriteLine(gamestate);
            Assert.Equal(0, gamestate.Players[0].Hand.NumberOfCards);
            Assert.Equal(1, gamestate.Players[1].Hand.NumberOfCards);
            Assert.True(gamestate.Turn.IsPlay);
            Assert.False(gamestate.Turn.IsRoundOver);
            Assert.Equal(new PlayerIndex(1), gamestate.Turn.PlayerIndex);
        }

        [Fact]
        public void TrickStateHasValueSemantics()
        {
            var gameCtx = new GameEventContext("game", 12);

            var ts1 = TrickState.Initial.Next(new CardPlayed(gameCtx, new PlayerIndex(3), new Card(Suit.Diamonds, Rank.Queen)));
            var ts2 = TrickState.Initial.Next(new CardPlayed(gameCtx, new PlayerIndex(3), new Card(Suit.Diamonds, Rank.Queen)));

            Assert.Equal(ts1, ts2);
        }
    }
}