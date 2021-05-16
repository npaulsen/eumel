using System;
using System.Net.Sockets;
using Eumel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eumel.Persistance
{
    public static class ServiceCollectionExtensions {
        public static void UseEumelPostgresPersistance(this IServiceCollection services, IConfiguration config) {
            services.AddDbContext<EumelGameContext>(
                options => {
                    
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    var connectionString = env == "Development" ?
                        config.GetConnectionString("EumelContext") 
                        : BuildConnectionStringFromPostgresUrl();

                    options.UseNpgsql(connectionString);
                },
                contextLifetime: ServiceLifetime.Transient, 
                optionsLifetime: ServiceLifetime.Singleton);
            services.AddDbContextFactory<EumelGameContext>(options =>
                options.UseNpgsql(config.GetConnectionString("EumelContext")));
            services.AddScoped<IGameRoomRepo, PersistenceGameRoomRepo>();
            services.AddSingleton<IGameEventPersister, GameEventPersister>();
            services.AddSingleton<IGameEventRepo, GameEventRepo>();
        }

        /// <summary>
        /// Uses the DATABASE_URL provided by heroku to generate a connection string.
        /// This is based on https://github.com/nbarbettini/little-aspnetcore-book/pull/49
        /// </summary>
        private static string BuildConnectionStringFromPostgresUrl()
        {
            string connectionString;
            // Use connection string provided at runtime by Heroku.
            var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrEmpty(connUrl))
            {
                throw new Exception("Unable to parse postgres connection string: environment variable <DATABASE_URL> not set.");
            }

            // Parse connection URL to connection string for Npgsql.
            connUrl = connUrl.Replace("postgres://", string.Empty);
            var pgUserPass = connUrl.Split("@")[0];
            var pgHostPortDb = connUrl.Split("@")[1];
            var pgHostPort = pgHostPortDb.Split("/")[0];
            var pgDb = pgHostPortDb.Split("/")[1];
            var pgUser = pgUserPass.Split(":")[0];
            var pgPass = pgUserPass.Split(":")[1];
            var pgHost = pgHostPort.Split(":")[0];
            var pgPort = pgHostPort.Split(":")[1];

            connectionString = $"Host={pgHost};Port={pgPort};Username={pgUser};Password={pgPass};Database={pgDb}";
            connectionString += ";Pooling=true;SSL Mode=Require;TrustServerCertificate=True;";
            return connectionString;
        }
    }
}