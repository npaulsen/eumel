using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Eumel.Core;
using Eumel.Core.GameSeriesEvents;
using Eumel.Persistance;
using Eumel.Persistance.Games;
using System.Linq;

namespace Persistence
{
    public class IntegrationTest : IDisposable
    {
        private readonly EumelGameContext _context;

        public IntegrationTest()
        {
            // var serviceProvider = new ServiceCollection()

            //     .BuildServiceProvider();

            var configuration = new ConfigurationBuilder()
                 .AddEnvironmentVariables()
                 .Build();
            var optionsBuilder = new DbContextOptionsBuilder<EumelGameContext>()
                .UseNpgsql(configuration.GetConnectionString("EumelIntegrationTestContext"));

            _context = new EumelGameContext(optionsBuilder.Options);

            _context.Database.EnsureDeleted();
            _context.Database.Migrate();
        }

        [Fact]
        public void GameIsMappedAndBack()
        {
            _context.Games.Add(new PersistedEumelGame { });
            _context.SaveChanges();

            var gameCount = _context.Games.Count();
            Assert.Equal(1, gameCount);

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
