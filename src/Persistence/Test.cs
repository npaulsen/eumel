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
        }
    }
}