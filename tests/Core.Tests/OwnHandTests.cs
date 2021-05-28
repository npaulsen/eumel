using System;
using System.Linq;
using System.Collections.Generic;
using Eumel.Core;
using Xunit;

namespace Core.Tests
{
    public class OwnHandTests
    {
        [Fact]
        public void OwnHand_WhenHandEmpty_CantPlayCard()
        {
            var sut = new KnownHand(Enumerable.Empty<Card>());
            var card = new Card(Suit.Hearts, Rank.Ten);

            var result = sut.CanPlay(card, TrickState.Initial);

            Assert.False(result);
        }

        [Fact]
        public void OwnHand_CardNotInHand_CantBePlayed()
        {
            var sut = new KnownHand(new[] { new Card(Suit.Hearts, Rank.Nine) });
            var card = new Card(Suit.Hearts, Rank.Ten);

            var result = sut.CanPlay(card, TrickState.Initial);

            Assert.False(result);
        }

        [Fact]
        public void OwnHand_Initially_AnyCardCanBePlayed()
        {
            var sut = new KnownHand(new[] { 
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Diamonds, Rank.King),
                new Card(Suit.Club, Rank.Ten) 
            });
            var card = new Card(Suit.Diamonds, Rank.King);

            var result = sut.CanPlay(card, TrickState.Initial);

            Assert.True(result);
        }

        [Fact]
        public void OwnHand_SuitToFollow_CanBePlayed()
        {
            var sut = new KnownHand(new[] { 
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Diamonds, Rank.King),
            });
            var card = new Card(Suit.Hearts, Rank.Nine);
            var stateWithHeartsPlayed = TrickState.Initial
                .Next(AsMove(new Card(Suit.Hearts, Rank.Five)));

            var result = sut.CanPlay(card, stateWithHeartsPlayed);

            Assert.True(result);
        }

        [Fact]
        public void OwnHand_SuitToFollowPresent_CantPlayAnother()
        {
            var sut = new KnownHand(new[] { 
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Diamonds, Rank.King),
            });
            var card = new Card(Suit.Diamonds, Rank.King);
            var stateWithHeartsPlayed = TrickState.Initial
                .Next(AsMove(new Card(Suit.Hearts, Rank.Five)));

            var result = sut.CanPlay(card, stateWithHeartsPlayed);

            Assert.False(result);
        }

        [Fact]
        public void OwnHand_SuitToFollowNotPresent_CanPlayAnother()
        {
            var sut = new KnownHand(new[] { 
                new Card(Suit.Club, Rank.Nine),
                new Card(Suit.Diamonds, Rank.King),
            });
            var card = new Card(Suit.Diamonds, Rank.King);
            var stateWithHeartsPlayed = TrickState.Initial
                .Next(AsMove(new Card(Suit.Hearts, Rank.Five)));

            var result = sut.CanPlay(card, stateWithHeartsPlayed);

            Assert.True(result);
        }

        private CardPlayed AsMove(Card c) => new CardPlayed(null, new PlayerIndex(1), c);

    }
}