using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace KadastraalOnroerendeZaken.API.Contexts
{
    public static class DatabaseHelpers
    {
        public static void EnsureDatabaseCreated(this IApplicationBuilder app, bool deleteIfExists)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<KadastraalOnroerendeZakenDbContext>();

            if (deleteIfExists)
            {
                dbContext.Database.EnsureDeleted();
            }
            dbContext.Database.EnsureCreated();
        }
    }
}
