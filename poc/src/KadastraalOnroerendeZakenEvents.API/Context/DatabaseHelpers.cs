using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace KadastraalOnroerendeZakenEvents.API.Context
{
    public static class DatabaseHelpers
    {
        public static void EnsureDatabaseCreated(this IApplicationBuilder app, bool deleteIfExists)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (deleteIfExists)
            {
                dbContext.Database.EnsureDeleted();
            }
            dbContext.Database.EnsureCreated();
        }
    }
}
