using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eumel.Persistance
{
    public static class EumelPersistence {
        public static void MigrateDatabase(IServiceScope scope)
        {
            scope.ServiceProvider.GetRequiredService<EumelGameContext>().Database.Migrate();
        }
    }
}