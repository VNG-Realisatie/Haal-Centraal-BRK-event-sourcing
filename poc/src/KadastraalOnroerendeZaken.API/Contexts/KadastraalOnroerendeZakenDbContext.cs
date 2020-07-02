using KadastraalOnroerendeZaken.API.Domain;
using KadastraalOnroerendeZaken.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KadastraalOnroerendeZaken.API.Contexts
{
    public class KadastraalOnroerendeZakenDbContext : DbContext
    {
        private readonly string connectionString;
        private readonly bool useLogger;

        public DbSet<KadastraalOnroerendeZaak> KadastraalOnroerendeZaken { get; set; }
        public DbSet<ZakelijkGerechtigde> ZakelijkGerechtigden { get; set; }
        public DbSet<LaatsteEvent> LaatsteEvents { get; set; }

        public KadastraalOnroerendeZakenDbContext(string connectionString, bool useLogger)
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
            modelBuilder.Entity<KadastraalOnroerendeZaak>(e =>
            {
                e.ToTable("KadastraalOnroerendeZaken").HasKey(k => k.Id);
                e.Property(k => k.Type).HasColumnName("Type");
                e.OwnsOne(k => k.Koopsom, k =>
                {
                    k.Property(ks => ks.Bedrag).HasColumnName("Koopsom");
                    k.Property(ks => ks.Koopjaar).HasColumnName("Koopjaar");
                    k.Property(ks => ks.IndicatieMetMeerObjectenVerkregen).HasColumnName("MetMeerObjectenVerkregen");
                });
                e.OwnsOne(k => k.KadastraleAanduiding, k =>
                {
                    k.Property(ka => ka.AppartementsrechtVolgnummer).HasColumnName("AppartementsrechtVolgnummer");
                    k.Property(ka => ka.KadastraleGemeenteCode).HasColumnName("KadastraleGemeenteCode");
                    k.Property(ka => ka.Perceelnummer).HasColumnName("Perceelnummer");
                    k.Property(ka => ka.Sectie).HasColumnName("Sectie");
                });
                e.HasMany(k => k.ZakelijkGerechtigden)
                    .WithOne()
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ZakelijkGerechtigde>(e =>
            {
                e.ToTable("ZakelijkGerechtigde").HasKey(z => z.Id);
                e.Property(z => z.Type).HasColumnName("Type");
                e.Property(z => z.Aanvangsdatum).HasColumnName("Aanvangsdatum");
            });

            modelBuilder.Entity<LaatsteEvent>(e =>
            {
                e.HasKey(x => x.KadastraalOnroerendeZaakIdentificatie);
            });
        }
    }
}
