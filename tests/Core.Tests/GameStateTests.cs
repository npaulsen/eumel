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
            var gamestate = GameState.Initial(3, new EumelRoundSettings(0, 1));
            gamestate = gamestate.Dispatch(new HandReceived(new PlayerIndex(0), new UnknownHand(1)));
            gamestate = gamestate.Dispatch(new HandReceived(new PlayerIndex(1), new UnknownHand(1)));
            gamestate = gamestate.Dispatch(new HandReceived(new PlayerIndex(2), new UnknownHand(1)));

            gamestate = gamestate.Dispatch(new GuessGiven(new PlayerIndex(0), 0));
            gamestate = gamestate.Dispatch(new GuessGiven(new PlayerIndex(1), 0));
            gamestate = gamestate.Dispatch(new GuessGiven(new PlayerIndex(2), 0));

            gamestate = gamestate.Dispatch(new CardPlayed(new PlayerIndex(0), new Card(Suit.Hearts, Rank.Ace)));
            System.Console.WriteLine(gamestate);
            Assert.Equal(0, gamestate.Players[0].Hand.NumberOfCards);
            Assert.Equal(1, gamestate.Players[1].Hand.NumberOfCards);
            Assert.True(gamestate.Turn.IsPlay);
            Assert.False(gamestate.Turn.IsRoundOver);
            Assert.Equal(new PlayerIndex(1), gamestate.Turn.PlayerIndex);
        }
    }
}