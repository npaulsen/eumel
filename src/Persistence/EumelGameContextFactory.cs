using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace Eumel.Persistance
{
    /// <summary>
    /// This is used when applying dotnet ef tools for this project.
    /// </summary>
    public class EumelGameContextFactory : IDesignTimeDbContextFactory<EumelGameContext>
    {
        public EumelGameContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .AddEnvironmentVariables()
               .Build();
            var optionsBuilder = new DbContextOptionsBuilder<EumelGameContext>()
                .UseNpgsql(configuration.GetConnectionString("EumelContext"));

            return new EumelGameContext(optionsBuilder.Options);
        }
    }
}