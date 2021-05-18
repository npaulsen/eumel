using Microsoft.EntityFrameworkCore;
using Eumel.Persistance.GameEvents;
using Eumel.Persistance.Games;

namespace Eumel.Persistance
{

    public class EumelGameContext : DbContext
    {
        public DbSet<PersistedEvent> Events { get; set; }
        public DbSet<PersistedSeriesEvent> SeriesEvents { get; set; }
        public DbSet<PersistedGameRound> Rounds { get; set; }
        public DbSet<PersistedEumelGame> Games { get; set; }
        public DbSet<PersistedPlayer> Players { get; set; }


        public EumelGameContext(DbContextOptions<EumelGameContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PersistedEumelGame>()
                .HasIndex(r => r.Name)
                .IsUnique();

            builder.Entity<PersistedEumelGame>()
                .HasMany(g => g.Players)
                .WithOne()
                .IsRequired();

            builder.Entity<PersistedEvent>()
                .HasIndex(e => e.RoundIndex);
            builder.Entity<PersistedEvent>()
                .HasIndex(e => e.GameUuid);
            builder.Entity<PersistedEvent>()
                .Property(e => e.GameUuid)
                    .IsRequired();

            builder.Entity<PersistedSeriesEvent>()
                .HasIndex(e => e.GameUuid);
        }
    }
}