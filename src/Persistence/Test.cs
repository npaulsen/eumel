using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Eumel.Persistance
{
    public class Test
    {
        public static void Main(string[] _)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<EumelGameContext>()
                .UseNpgsql(configuration.GetConnectionString("EumelContext"));
            using var ctx = new EumelGameContext(optionsBuilder.Options);
            System.Console.WriteLine(ctx.Games.Count());
            // var round = new PersistedGameRound();
            // var ctx = new GameEventContext("example", 14);
            // round.Events = new GameEvent[] {
            //     new HandReceived(ctx, new PlayerIndex(4), new KnownHand(new[] { new Card(Suit.Club, Rank.Four), new Card(Suit.Hearts, Rank.Ace)})),
            //     new GuessGiven(ctx, new PlayerIndex(3), 77),
            //     new CardPlayed(ctx, new PlayerIndex(3), new Card(Suit.Diamonds, Rank.King)),
            //     new TrickWon(ctx, new PlayerIndex(1))
            // }.Select(EventSerializer.Convert).ToList();
            // using (var context = new EumelGameContext(new Microsoft.EntityFrameworkCore.DbContextOptions<EumelGameContext>())) {

            //     context.Rounds.Add(round);
            //     context.SaveChanges();
            //     round.Events.Add(EventSerializer.Convert(new TrickWon(ctx, new PlayerIndex(77))));
            //     context.SaveChanges();  

            // }
            // System.Console.WriteLine("event added..");
            // // using (var context = new EumelGameContext()) {
            // //     var r = context.Rounds.First(r => r. == 2);
            // //     context.Entry(r).Collection(r => r.Events).Load();
            // //     System.Console.WriteLine(r + " " + r.Events.Count);
            // //     r.Events.Add(EventSerializer.Convert(new TrickWon(new PlayerIndex(9))));
            // //     context.SaveChanges();
            // // }
            // // System.Console.WriteLine("event added..");
        }
    }
}