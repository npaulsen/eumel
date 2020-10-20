using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EumelCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eumeln
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var players = Enumerable.Range(0, 3)
                .Select(i => new PlayerInfo(i, $"Player {i}", new Data.Players.DumbPlayer()));
            var game = new EumelGame(new PlayerCollection(players));
            while (game.HasMoreRounds)
            {
                Console.Write("Hit Enter to start next round: ");
                // Console.ReadLine();
                game.PlayRound();
            }

            // CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}