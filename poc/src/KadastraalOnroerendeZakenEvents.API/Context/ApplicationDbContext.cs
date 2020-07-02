using KadastraalOnroerendeZakenEvents.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KadastraalOnroerendeZakenEvents.API.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string connectionString;
        private readonly bool useLogger;

        public DbSet<KozTpo> KozTpos { get; set; }

        public ApplicationDbContext(string connectionString, bool useLogger)
        {
            this.connectionString = connectionString;
            this.useLogger = useLogger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);

            if (useLogger)
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                        .AddFilter((category, level) =>
                            category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                        .AddConsole();
                });

                optionsBuilder
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KozTpo>(e =>
            {
                e.ToTable("KozTpos")
                    .HasKey(k => k.Id);
                e.HasIndex(k => k.KozId);
            });
        }
    }
}
