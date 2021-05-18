using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using Xunit;

namespace Core.Tests
{
    public class GameEventHubTests
    {
        [Fact]
        public void ResumingTheFirstEventsLeadsToSameState()
        {
            var gameRoomDef = GenGameRoomDefinitionFor3Players();
            var originalHub = new GameEventHub(gameRoomDef, GameProgress.NotStarted);
            // var forwardedHub = new GameEventHub("test", plan, 3);
            var originalHubObserver = new TransparentObserver();
            PlayFirstTricksGuessesAndCards(originalHub);
            // First trick over!
            originalHub.MoveToNextRound();
            originalHub.StartCurrentRound();
            originalHub.TryGiveGuess(1, 3);
            originalHub.SubscribeWithPreviousEvents(originalHubObserver);

            var progress = new GameProgress(
                new GameSeriesEvent[] { new RoundStarted("", null), new RoundStarted("", null) }.ToImmutableList().WithValueSemantics(),
                originalHubObserver.ObservedEvents.ToImmutableList().WithValueSemantics());
            var resumingHub = new GameEventHub(gameRoomDef, progress);

            Assert.Equal(originalHub.State, resumingHub.State);
        }

        [Fact]
        public void ForwardingTheFirstEventsLeadsToSameRoundEvents()
        {
            var gameRoomDef = GenGameRoomDefinitionFor3Players();
            var originalHub = new GameEventHub(gameRoomDef, GameProgress.NotStarted);
            // var forwardedHub = new GameEventHub("test", plan, 3);
            var originalHubObserver = new TransparentObserver();
            PlayFirstTricksGuessesAndCards(originalHub);
            // First trick over!
            originalHub.MoveToNextRound();
            originalHub.StartCurrentRound();
            originalHub.TryGiveGuess(1, 3);
            originalHub.SubscribeWithPreviousEvents(originalHubObserver);

            var progress = new GameProgress(
                new GameSeriesEvent[] { new RoundStarted("", null), new RoundStarted("", null) }.ToImmutableList().WithValueSemantics(),
                originalHubObserver.ObservedEvents.ToImmutableList().WithValueSemantics());
            var resumingHub = new GameEventHub(gameRoomDef, progress);
            var resumingHubObserver = new TransparentObserver();
            resumingHub.SubscribeWithPreviousEvents(resumingHubObserver);

            Assert.Equal(originalHubObserver.ObservedEvents, resumingHubObserver.ObservedEvents);
        }

        private static EumelGameRoomDefinition GenGameRoomDefinitionFor3Players()
        {
            var players = Enumerable.Range(1, 3)
                            .Select(_ => new PlayerInfo("unnamed", PlayerType.Bot))
                            .ToImmutableList()
                            .WithValueSemantics();
            var plan = EumelGamePlan.For(3);
            var gameRoomDef = new EumelGameRoomDefinition("test", players);
            return gameRoomDef;
        }

        private static void PlayFirstTricksGuessesAndCards(GameEventHub originalHub)
        {
            originalHub.MoveToNextRound();
            originalHub.StartCurrentRound();
            originalHub.TryGiveGuess(0, 2);
            originalHub.TryGiveGuess(1, 3);
            originalHub.TryGiveGuess(2, 4);
            originalHub.TryPlayCard(0, ((KnownHand)originalHub.State.Players[0].Hand).First());
            originalHub.TryPlayCard(1, ((KnownHand)originalHub.State.Players[1].Hand).First());
            originalHub.TryPlayCard(2, ((KnownHand)originalHub.State.Players[2].Hand).First());
        }

        class TransparentObserver : IObserver<GameEvent>
        {
            private List<GameEvent> _observedEvents = new List<GameEvent>();
            public IEnumerable<GameEvent> ObservedEvents => _observedEvents;

            public void OnNext(GameEvent evt) => _observedEvents.Add(evt);

            public void OnCompleted() => throw new NotImplementedException();

            public void OnError(Exception error) => throw new NotImplementedException();
        }
    }
}