using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eumel.Persistance
{
    public static class EumelPersistence
    {

        /// <summary>
        /// A helper to migrate db in prod environment without needing additional scripting steps.
        /// </summary>
        /// <param name="scope">Needed to obtain DB context</param>
        public static void MigrateDatabase(IServiceScope scope)
        {
            scope.ServiceProvider.GetRequiredService<EumelGameContext>().Database.Migrate();
        }
    }
}