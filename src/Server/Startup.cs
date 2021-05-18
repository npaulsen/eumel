using System.Linq;
using Eumel.Core;
using Eumel.Core.Players;
using Eumel.Persistance;
using Eumel.Server.Hubs;
using Eumel.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Eumel.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            services.AddRazorPages();

            if (Configuration.IsPostgresPersistanceEnabled())
            {
                services.UseEumelPostgresPersistance(Configuration);
            }
            else
            {
                services.AddSingleton<IGameRoomRepo, InMemoryGameRoomRepo>();
                services.AddSingleton<IGameEventPersister, NoopGameEventPersister>();
                services.AddSingleton<IGameEventRepo, NoStorageGameEventRepo>();
            }

            services.AddSingleton<IPlayerFactory, PlayerFactory>();
            services.AddScoped<ConnectionManager>();
            // TODO make scoped/transient by dragging out dictionary singleton service
            services.AddSingleton<IActiveLobbyRepo, InMemoryLobbyManager>();
            services.AddSingleton<IClientToLobbyAssignmentStore, ClientToLobbyAssignmentStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                if (Configuration.IsPostgresPersistanceEnabled())
                {
                    InitializeDatabase(app);
                }
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<GameHub>("/gamehub");
                endpoints.MapFallbackToFile("index.html");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            EumelPersistence.MigrateDatabase(scope);
        }
    }

    public static class ConfigurationExtensions
    {
        public static bool IsPostgresPersistanceEnabled(this IConfiguration _) => true;
    }
}